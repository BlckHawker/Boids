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
        totalForce = Vector2.ClampMagnitude(totalForce, MaxForce);
        ApplyForce(totalForce);

        Debug.Log($"Mass: {mass}\nMax Force: {MaxForce}\nMax Speed: {maxSpeed}\nStay In Bounds Future Time: {stayInBoundsFutureTime}\nStay In Bounds Weight: {stayInBoundsWeight}\nWander Weight: {wanderWeight}\nWander circle Radius: {wanderCircleRadius}\nWander Offset: {wanderOffset}\nWander Time: {wanderTime}\nWander Future Time: {wanderFutureTime}");
    }
}
