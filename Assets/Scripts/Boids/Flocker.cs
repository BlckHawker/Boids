using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocker : Agent
{
    protected override void CalcSteeringForces()
    {
        Vector2 totalForce = Vector2.zero;
        totalForce += stayInBoundsForce ? StayInBounds() * stayInBoundsWeight : Vector2.zero;
        totalForce += separateForce ? Separate() * separateWeight : Vector2.zero;
        totalForce += alignForce ? Align() * alignWeight : Vector2.zero;
        totalForce = Vector2.ClampMagnitude(totalForce, MaxForce);

        ApplyForce(totalForce);
    }
}
