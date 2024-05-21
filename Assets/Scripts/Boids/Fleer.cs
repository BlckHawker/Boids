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

        totalForce += fleeForce ? Flee(target) * fleeWeight : Vector2.zero;
        totalForce += stayInBoundsForce ? StayInBounds() * stayInBoundsWeight : Vector2.zero;

        totalForce = Vector2.ClampMagnitude(totalForce, MaxForce);
        ApplyForce(totalForce);
    }
}
