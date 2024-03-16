using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Wanderer : Agent
{
    protected override void CalcSteeringForces()
    {
        Vector2 totalForce = Vector2.zero;

        totalForce += StayInBounds() * stayInBoundsWeight;
        totalForce += Wander() * wanderWeight;
        Vector2.ClampMagnitude(totalForce, MaxForce);
        ApplyForce(totalForce);
    }
}
