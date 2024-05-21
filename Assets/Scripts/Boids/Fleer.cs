using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleer : Agent
{
    private GameObject target;
    public GameObject Target { set { target = value; } }
    protected override void CalcSteeringForces()
    {
        Vector2 totalForce = Vector2.zero;

        totalForce += Flee(target) * fleeWeight;
        totalForce += StayInBounds() * stayInBoundsWeight;

        totalForce = Vector2.ClampMagnitude(totalForce, MaxForce);
        ApplyForce(totalForce);
    }
}
