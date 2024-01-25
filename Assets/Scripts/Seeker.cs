using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Seeker : Agent
{
    private GameObject target;
    public GameObject Target { set { target = value; } }
    public float SeekerWeight { set { seekerWeight = value; } }

    protected override void CalcSteeringForces()
    {
        Vector2 force = Seek(target) * seekerWeight;

        Vector2.ClampMagnitude(force, MaxForce);

        ApplyForce(force);
    }
}
