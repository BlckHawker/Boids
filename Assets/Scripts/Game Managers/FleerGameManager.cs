using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleerGameManager : GameManager
{
    [SerializeField]
    private Fleer fleerPrefab;
    [SerializeField]
    private Seeker seekerPrefab;

    private Fleer fleer;
    private Seeker seeker;

    #region Agent Stats
    [NonSerialized]
    public float seekerMass, seekerMaxForce, seekerMaxSpeed, seekerSeekWeight, seekerStayInBoundsFutureTime, seekerStayInBoundsWeight;
    [NonSerialized]
    public bool seekerSeekForce, seekerStayInBoundsForce;
    [NonSerialized]
    public float fleerStayInBoundsFutureTime, fleerMass, fleerMaxForce, fleerMaxSpeed, fleerWeight, fleerStayInBoundsWeight;
    [NonSerialized]
    public bool fleerFleeForce, fleerStayInBoundsForce;
    #endregion



    protected override void GetCollisionChecker()
    {
        
    }

    protected override void InstantiateObjects()
    {
        fleer = Instantiate(fleerPrefab);
        seeker = Instantiate(seekerPrefab);
        seeker.Position = GetRandomPosition();
        seeker.Target = fleer.gameObject;

        fleer.Position = GetRandomPosition();
        fleer.Target = seeker.gameObject;
    }

    protected override void UpdateAgentWallBounds()
    {
        Bounds b = WallBounds;
        seeker.WallBounds = b;
        fleer.WallBounds = b;
    }

    protected override void UpdateValues()
    {
        seeker.Mass = seekerMass;
        seeker.MaxForce = seekerMaxForce;
        seeker.MaxSpeed = seekerMaxSpeed;
        seeker.SeekWeight = seekerSeekWeight;
        seeker.StayInBoundsFutureTime = seekerStayInBoundsFutureTime;
        seeker.StayInBoundsWeight = seekerStayInBoundsWeight;
        seeker.SeekForce = seekerSeekForce;
        seeker.StayInBoundsForce = seekerStayInBoundsForce;

        fleer.Mass = fleerMass;
        fleer.MaxForce = fleerMaxForce;
        fleer.MaxSpeed = fleerMaxSpeed;
        fleer.FleeWeight = fleerWeight;
        fleer.StayInBoundsFutureTime = fleerStayInBoundsFutureTime;
        fleer.StayInBoundsWeight = fleerStayInBoundsWeight;
        fleer.FleeForce = fleerFleeForce;
        fleer.StayInBoundsForce = fleerStayInBoundsForce;
    }
}
