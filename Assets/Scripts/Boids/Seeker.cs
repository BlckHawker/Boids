using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : Agent
{
    private GameObject target;
    public GameObject Target { set { target = value; } }
    public float SeekWeight { set { seekerWeight = value; } }
    public float StayInBoundsWeight { set { stayInBoundsWeight = value; } }


    protected override void CalcSteeringForces()
    {
        Vector2 totalForce = Vector2.zero;

        totalForce += Seek(target) * seekerWeight;
        totalForce += StayInBounds() * stayInBoundsWeight;

        Vector2.ClampMagnitude(totalForce, MaxForce);

        ApplyForce(totalForce);
    }
}
