using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleer : Agent
{
    private GameObject target;
    public GameObject Target { set { target = value; } }
    public float FleeWeight { set { fleerWeight = value; } }
    public float StayInBoundsWeight { set { stayInBoundsWeight = value; } }

    protected override void CalcSteeringForces()
    {
        Vector2 totalForce = Vector2.zero;

        totalForce += Flee(target) * fleerWeight;
        totalForce += StayInBounds() * stayInBoundsWeight;

        Vector2.ClampMagnitude(totalForce, MaxForce);
        ApplyForce(totalForce);
    }
}
