using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rnd = UnityEngine.Random;
public abstract class Agent : MonoBehaviour
{
    #region Force Weights
    protected float seekWeight, fleerWeight, wanderWeight, stayInBoundsWeight;

    #region Wander Weight
    protected float wanderCircleRadius;
    //the time in which to change angles/directions
    protected float wanderTime;
    //the range in which to change angles/directions
    protected float wanderOffset;
    //the angle/direction to wander to
    private float wanderAngle;
    private float wanderCurrentTime = 0;
    private Vector2 wanderPosition;
    #endregion

    #region Setters
    public float FleeWeight { set { fleerWeight = value; } }
    public float Radius { get { return radius; } }
    public float SeekWeight { set { seekWeight = value; } }
    public float StayInBoundsFutureTime { set { stayInBoundsFutureTime = value; } }
    public float StayInBoundsWeight { set { stayInBoundsWeight = value; } }
    public float WanderCircleRadius { set { wanderCircleRadius = value; } }
    public float WanderFutureTime { set { wanderFutureTime = value; } }
    public float WanderOffset { set { wanderOffset = value; } }
    public float WanderTime { set { wanderTime = value; } }
    public float WanderWeight { set { wanderWeight = value; } }
    #endregion

    #endregion
    #region Bound Variables
    protected float radius, stayInBoundsFutureTime, wanderFutureTime;
    protected Bounds agentBounds;
    public Bounds WallBounds { get; set; }
    #endregion
    #region Stat Variables
    protected float mass, maxSpeed;
    public float Mass { set { mass = value; } }
    public float MaxForce { get; set; }
    public float MaxSpeed { set { maxSpeed = value; } }
    #endregion
    #region Other Variables
    protected Vector2 velocity, acceleration;
    [NonSerialized]
    public Vector2 Position;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        radius = transform.localScale.x / 2;
        agentBounds = new Bounds(transform.position, new(radius, radius));
        transform.position = Position;
        velocity = Vector2.zero;
        acceleration = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //if mass is 0, don't run update
        if (mass == 0)
        {
            Debug.Log("Mass is 0 for " + gameObject.name);
            return;
        }


        //reset acceleration
        acceleration = Vector2.zero;
        CalcSteeringForces();

        //add acceleration to velocity
        velocity += acceleration * Time.deltaTime;

        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);

        //get the new direction
        RotateVehicle();

        //udpate vehicle position
        Position += velocity * Time.deltaTime;

        transform.position = Position;
    }

    private void RotateVehicle()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.back, velocity);
    }

    #region Agent Weight Methods
    protected abstract void CalcSteeringForces();

    protected Vector2 Seek(Vector2 targetPos)
    {
        //calculate desired veolcity
        Vector2 desiredVelocity = targetPos - Position;

        //scale desired velocity to max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        //find the steering force
        Vector2 seekingForce = desiredVelocity - velocity;

        return seekingForce;
    }
    protected Vector2 Seek(GameObject target)
    {
        return Seek(target.transform.position);
    }
    protected Vector2 Seek(Agent agent)
    {
        return Seek(agent.Position);
    }
    protected Vector2 Flee(Vector2 targetPos)
    {
        //calculate desired veolcity
        Vector2 desiredVelocity = Position - targetPos;

        //scale desired velocity to max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        //find the steering force
        Vector2 fleeingForce = desiredVelocity - velocity;

        return fleeingForce;
    }
    protected Vector2 Flee(GameObject target)
    {
        return Flee(target.transform.position);
    }
    protected Vector2 Flee(Agent agent)
    {
        return Flee(agent.Position);
    }
    protected Vector2 Wander()
    {
        wanderCurrentTime -= Time.deltaTime;
        //find random angle in intervals
        if (wanderCurrentTime <= 0)
        {
            wanderAngle += rnd.Range(-wanderOffset, wanderOffset);
            wanderCurrentTime = wanderTime;
        }
        //Prjoect a circle a distance ahead of you
        Vector2 futurePosition = Position + velocity * wanderFutureTime;

        //Find the spot on that circle using that angle of rotation
        float xPos = Mathf.Cos(wanderAngle * Mathf.Deg2Rad) * wanderCircleRadius + futurePosition.x;
        float yPos = Mathf.Sin(wanderAngle * Mathf.Deg2Rad) * wanderCircleRadius + futurePosition.y;

        wanderPosition = new Vector3(xPos, yPos, 0);
        return Seek(wanderPosition);
    }
    protected Vector2 StayInBounds()
    {
        float wallMinX = WallBounds.min.x;
        float wallMaxX = WallBounds.max.x;
        float wallMinY = WallBounds.min.y;
        float wallMaxY = WallBounds.max.y;

        Vector2 futurePosition = Position + velocity * stayInBoundsFutureTime;

        bool left = futurePosition.x < wallMinX;
        bool right = futurePosition.x > wallMaxX;
        bool up = futurePosition.y > wallMaxY;
        bool down = futurePosition.y < wallMinY;

        if (left || right || up || down)
            return Seek(Vector2.zero);
        
        return Vector2.zero;
    }
    protected void ApplyForce(Vector2 force)
    {
        acceleration += force / mass;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector2.zero, WallBounds.size);
        
        Gizmos.color = Color.magenta;

        Vector2 stayInBoundsFuturePos = Position + velocity * stayInBoundsFutureTime;

        //draw future position
        Gizmos.DrawLine(Position, stayInBoundsFuturePos);
        Gizmos.DrawWireSphere(stayInBoundsFuturePos, Radius);


        Gizmos.color = Color.cyan;
        if (wanderCircleRadius > 0)
        {
            //draw wander circle + positon the player is seeking
            Vector2 futurePosition = Position + velocity * wanderFutureTime;
            Gizmos.DrawLine(Position, futurePosition);
            Gizmos.DrawWireSphere(futurePosition, wanderCircleRadius);
            Gizmos.DrawLine(futurePosition, wanderPosition);
        }
    }
}
