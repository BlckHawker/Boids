using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocker : Agent
{
    protected override void CalcSteeringForces()
    {
        Vector2 totalForce = Vector2.zero;

        totalForce += StayInBounds() * stayInBoundsWeight;
        totalForce += Separate() * separateWeight;
        totalForce += Align() * alignWeight;
        totalForce = Vector2.ClampMagnitude(totalForce, MaxForce);

        ApplyForce(totalForce);
    }
}
