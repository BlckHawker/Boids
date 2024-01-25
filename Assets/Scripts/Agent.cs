using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    #region Force Weights
    protected float seekerWeight;
    #endregion
    #region Bound Variables
    protected float futureTime, radius;
    public float Radius { get { return radius; } }
    protected Bounds agentBounds;
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

    public void SetPosition(Vector2 position) 
    {
        transform.position = position;
        this.position = position;
    }
    #endregion
}
