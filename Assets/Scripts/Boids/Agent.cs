using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using rnd = UnityEngine.Random;
public abstract class Agent : MonoBehaviour
{
    #region Force Colors
    protected Color stayInBoundsColor = Color.magenta;
    protected Color wanderColor = Color.cyan;
    protected Color avoidObstacleColor = Color.red;
    protected Color separateColor = Color.green;
    protected Color alignColor = Color.yellow;
    #endregion

    #region Force Weights
    protected float seekWeight, fleeWeight, wanderWeight, stayInBoundsWeight, avoidObstacleWeight, separateWeight, alignWeight;
    #endregion

    #region Force Booleans
    protected bool avoidObstacleForce, alignForce, separateForce, stayInBoundsForce, wanderForce, seekForce, fleeForce;

    public bool SeekForce { set { seekForce = value; } }
    public bool FleeForce { set { fleeForce = value; } }
    public bool AvoidObstacleForce { set { avoidObstacleForce = value; } }
    public bool AlignForce { set { alignForce = value; } }
    public bool StayInBoundsForce { set { stayInBoundsForce = value; } }
    public bool WanderForce { set { wanderForce = value; } }
    public bool SeparateForce { set { separateForce = value; } }
    #endregion

    #region Setters
    public float AlignDistance { set { alignDistance = value; } }
    public float AlignWeight { set { alignWeight = value; } }
    public float SeparateDistance { set { separateDistance = value; } }
    public float SeparateWeight { set { separateWeight = value; } }
    public float AvoidObstacleWeight { set { avoidObstacleWeight = value;  } }
    public float FleeWeight { set { fleeWeight = value; } }
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

    #region Bound Variables
    protected float radius, stayInBoundsFutureTime, wanderFutureTime;
    protected Bounds agentBounds;
    public Bounds WallBounds { get; set; }
    #endregion
    #region Stat Variables
    protected float mass, maxSpeed;
    public float AvoidTime { set { avoidTime = value; } }
    public float Mass { set { mass = value; } }
    public float MaxForce { get; set; }
    public float MaxSpeed { set { maxSpeed = value; } }
    #endregion
    #region Other Variables
    #region Wander Variables
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
    protected Vector2 velocity, acceleration;
    private float separateDistance, alignDistance;
    private List<Obstacle> obstacleList;
    public List<Obstacle> ObstacleList { set { obstacleList = value; } }
    private List<Agent> flockingList;
    private List<Agent> importantSeparateAgentList, importantAlignAgentList;
    
   
    public List<Agent> FlockingList { set { flockingList = value; } }

    private List<Obstacle> importantObstacles = new List<Obstacle>(); // a list of obstacles that are "close enough"
    
    private float avoidTime; // how far ahead in the future should we care about avoiding obstacles
    [NonSerialized]
    public Vector2 Position;
    protected Vector2 direction { get { return velocity.normalized; } }
    #region Future Times
    private Vector2 wanderFuturePosition { get { return Position + velocity * wanderFutureTime; } }
    private Vector2 stayInBoundsFuturePosition { get { return Position + velocity * stayInBoundsFutureTime; } }
    private Vector2 avoidObstacleFuturePosition { get { return Position + velocity * avoidTime; } }

    private bool OutOfBounds 
    { 
        get {
            Vector2 futurePosition = stayInBoundsFuturePosition;

            float wallMinX = WallBounds.min.x;
            float wallMaxX = WallBounds.max.x;
            float wallMinY = WallBounds.min.y;
            float wallMaxY = WallBounds.max.y;

            bool left = futurePosition.x < wallMinX;
            bool right = futurePosition.x > wallMaxX;
            bool up = futurePosition.y > wallMaxY;
            bool down = futurePosition.y < wallMinY;

            return left || right || up || down;
        }
    }
    #endregion
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        importantObstacles = new List<Obstacle>();
        importantSeparateAgentList = new List<Agent>();
        importantAlignAgentList = new List<Agent>();
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

    #region Agent Force Methods
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
        Vector2 futurePosition = wanderFuturePosition;

        //Find the spot on that circle using that angle of rotation
        float xPos = Mathf.Cos(wanderAngle * Mathf.Deg2Rad) * wanderCircleRadius + futurePosition.x;
        float yPos = Mathf.Sin(wanderAngle * Mathf.Deg2Rad) * wanderCircleRadius + futurePosition.y;

        wanderPosition = new Vector3(xPos, yPos, 0);
        return Seek(wanderPosition);
    }
    protected Vector2 StayInBounds()
    {
        if (OutOfBounds)
        {
            return Seek(Vector2.zero);
        }

        return Vector2.zero;
    }

    protected Vector2 AvoidObstacles()
    {
        Vector2 steeringForce = Vector2.zero;
        importantObstacles.Clear();
        foreach (Obstacle obstacle in obstacleList)
        {
            Vector2 obstacleToAgent = (Vector2)obstacle.transform.position - Position;
            float forwardDot = Vector2.Dot(direction, obstacleToAgent);
            float rightDot = Vector2.Dot(transform.right, obstacleToAgent);

            //if the dot prduct is negative, then the object is behind, and ignore it
            if (forwardDot < 0)
            {
                continue;
            }

            //if the object is too far in the future, ignore it
            if (forwardDot > avoidObstacleFuturePosition.magnitude + radius)
            {
                continue;
            }

            //if the object is too far right, ignore it
            if (rightDot > radius + obstacle.Radius)
            {
                continue;
            }

            //if the object is too far left, ignore it
            if (rightDot < -(radius + obstacle.Radius))
            {
                continue;
            }

            //for debugging purposes, add this obstacle to the list of important obstcles
            importantObstacles.Add(obstacle);


            Vector2 desiredVelocity = Vector2.zero;

            //if the dot product is negative, the object is to the left and the vehicle has to move to the right (scale the weight based on distance)
            if (rightDot < 0)
            {
                desiredVelocity = transform.right * maxSpeed * (1f / obstacleToAgent.magnitude);
            }

            //if the dot product is positive, the object is to the right and the vehicle has to move to the left
            else if (rightDot > 0)
            {
                desiredVelocity = -transform.right * maxSpeed * (1f / obstacleToAgent.magnitude);
            }

            steeringForce += desiredVelocity.magnitude != 0 ? desiredVelocity - velocity : Vector2.zero;
        }

        return steeringForce;
    }
    protected Vector2 Separate()
    {
        Vector2 steeringForce = Vector2.zero;
        importantSeparateAgentList.Clear();
        foreach (Agent neighbor in flockingList) 
        {
            //be sure you are not seperating from yourself
            if (neighbor == this)
                continue;

            Vector2 neighborToAgent = Position - neighbor.Position;

            //make sure the distance is "close enough" to be considered a problem
            float distance = neighborToAgent.magnitude;
            if (distance > separateDistance)
                continue;

            importantSeparateAgentList.Add(neighbor);

            //steer clear from any obstacles that are too close
            Vector2 desiredVelocity = (Position - neighbor.Position).normalized * maxSpeed * (1f / distance);
            steeringForce += desiredVelocity - velocity;
        }

        //divide by the average
        return importantSeparateAgentList.Count == 0 ? Vector2.zero : steeringForce / importantSeparateAgentList.Count;
    }
    protected Vector2 Align()
    {
        Vector2 desiredVelocity = Vector2.zero;
        importantAlignAgentList.Clear();

        //find all neighbors that are "close enough" to align to
        foreach (Agent neighbor in flockingList)
        {
            //don't include yourself
            if(neighbor == this)
                continue;

            //only look at obstacles that are "close enough"
            Vector2 neighborToAgent = Position - neighbor.Position;
            float distance = neighborToAgent.magnitude;

            if (distance > alignDistance)
                continue;

            importantAlignAgentList.Add(neighbor);
        }

        if (importantAlignAgentList.Count == 0)
            return Vector2.zero;

        //find the average of all of the agent's velocities that are in range (excluding yourself)
        foreach (Agent neighbor in importantAlignAgentList)
        {
            desiredVelocity += neighbor.velocity;
        }

        desiredVelocity /= flockingList.Count;
            
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        return desiredVelocity - velocity;
    }
    protected void ApplyForce(Vector2 force)
    {
        acceleration += force / mass;
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            //draw stay in bounds future position
            if (stayInBoundsForce)
            {
                Gizmos.color = stayInBoundsColor;
                Gizmos.DrawLine(Position, stayInBoundsFuturePosition);
                Gizmos.DrawWireSphere(stayInBoundsFuturePosition, Radius);

                //draw stay in bounds
                Gizmos.DrawWireCube(Vector2.zero, WallBounds.size);
            }

            //draw wander future position / target + wander circle + positon the player is seeking
            if (wanderForce)
            {
                Gizmos.color = wanderColor;
                Gizmos.DrawLine(Position, wanderFuturePosition);
                Gizmos.DrawWireSphere(wanderFuturePosition, wanderCircleRadius);
                Gizmos.DrawLine(wanderFuturePosition, wanderPosition);
            }

            //draw box area to avoid obstacles
            if (avoidObstacleForce)
            {
                Gizmos.color = avoidObstacleColor;
                Vector3 avoidBoxSize = new Vector3(radius * 2f, avoidObstacleFuturePosition.magnitude + radius, radius * 2f);
                Vector3 boxCenter = new Vector3(0, (avoidObstacleFuturePosition.magnitude + radius) / 2f, 0);
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawWireCube(boxCenter, avoidBoxSize);
                Gizmos.matrix = Matrix4x4.identity;

                //draw lines to important obstacles
                foreach (Obstacle obstacle in importantObstacles)
                {
                    Gizmos.DrawLine(Position, obstacle.transform.position);
                }
            }

            //draw the closness for seperation
            if (separateForce)
            {
                Gizmos.color = separateColor;
                Gizmos.DrawWireSphere(Position, separateDistance);
                //draw a line to agents that are too close
                foreach (Agent agent in importantSeparateAgentList)
                {
                    Gizmos.DrawLine(Position, agent.Position);
                }
            }

            //draw the closness for align
            if (alignForce)
            {
                Gizmos.color = alignColor;
                Gizmos.DrawWireSphere(Position, alignDistance);
                //draw a line to agents that are too close
                foreach (Agent agent in importantAlignAgentList)
                {
                    Gizmos.DrawLine(Position, agent.Position);
                }
            }
        }
    }
}
