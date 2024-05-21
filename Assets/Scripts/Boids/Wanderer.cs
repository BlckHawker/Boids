using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Wanderer : Agent
{
    protected override void CalcSteeringForces()
    {
        Vector2 totalForce = Vector2.zero;

        totalForce += stayInBoundsForce ? StayInBounds() * stayInBoundsWeight : Vector2.zero;
        totalForce += wanderForce ? Wander() * wanderWeight : Vector2.zero;
        totalForce += avoidObstacleForce ? AvoidObstacles() * avoidObstacleWeight : Vector2.zero;
        totalForce = Vector2.ClampMagnitude(totalForce, MaxForce);
        ApplyForce(totalForce);
    }
}
