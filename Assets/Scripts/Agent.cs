using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    #region Force Weights
    protected float seekerWeight, stayInBoundsWeight;
    #endregion
    #region Bound Variables
    protected float futureTime, radius;
    public float FutureTime { set { futureTime = value; } }
    public float Radius { get { return radius; } }
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
    protected Vector2 position, velocity, acceleration;
    public Vector2 Position { get { return position; } }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        radius = transform.localScale.x / 2;
        agentBounds = new Bounds(transform.position, new(radius, radius));
        transform.position = position;
        velocity = Vector2.zero;
        acceleration = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //reset acceleration
        acceleration = Vector2.zero;
        CalcSteeringForces();

        //add acceleration to velocity
        velocity += acceleration * Time.deltaTime;

        Vector2.ClampMagnitude(velocity, maxSpeed);

        //get the new direction
        RotateVehicle();

        //udpate vehicle position
        position += velocity * Time.deltaTime;

        transform.position = position;
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
        Vector2 desiredVelocity = targetPos - position;

        //scale desired velocity to max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        //find the steering force
        Vector2 seekingForce = desiredVelocity - velocity;

        return seekingForce;
    }

    protected Vector2 StayInBounds()
    {
        float wallMinX = WallBounds.min.x;
        float wallMaxX = WallBounds.max.x;
        float wallMinY = WallBounds.min.y;
        float wallMaxY = WallBounds.max.y;

        Vector2 futurePosition = position + velocity * futureTime;

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
    protected Vector2 Seek(GameObject target)
    {
        return Seek(target.transform.position);
    }
    #endregion
    #region Weight Setter Methods
    public void SetSeekerForce(float weight)
    {
        seekerWeight = weight;
    }
    #endregion
    public void SetPosition(Vector2 position)
    {
        transform.position = position;
        this.position = position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        
        //draw future position
        Gizmos.DrawLine(position, position + velocity * futureTime);
        Gizmos.DrawWireSphere(position + velocity * futureTime, Radius);
    }
}
