using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : Agent
{
    private GameObject target;
    public GameObject Target { set { target = value; } }
    protected override void CalcSteeringForces()
    {
        Vector2 totalForce = Vector2.zero;

        totalForce += Seek(target) * seekWeight;
        totalForce += StayInBounds() * stayInBoundsWeight;

        totalForce = Vector2.ClampMagnitude(totalForce, MaxForce);

        ApplyForce(totalForce);
    }
}
