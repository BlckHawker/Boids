using System;
using UnityEngine;

public class SeekerGameManager : GameManager
{
    #region Prefabs
    [SerializeField]
    private GameObject dumbPrefab; //object that doesn't move
    [SerializeField]
    private Seeker seekerPrefab;
    #endregion

    #region Seeker Stats
    [NonSerialized]
    public float stayInBoundsFutureTime, mass, maxForce, maxSpeed, seekWeight, stayInBoundsWeight;
    [NonSerialized]
    public bool seekForce, stayInBoundsForce;
    #endregion

    private GameObject dummy;
    private Agent seeker;
    private Seeker seekerComponent;
    private SeekerCollisionChecker collisionChecker;

    protected override void InstantiateObjects()
    {
        dummy = Instantiate(dumbPrefab);
        dummy.transform.position = GetRandomPosition();

        seeker = Instantiate(seekerPrefab);
        seeker.transform.position = GetRandomPosition();
        seekerComponent = seeker.GetComponent<Seeker>();
        seekerComponent.Target = dummy;

        collisionChecker.DummyObject = dummy;
        collisionChecker.Seeker = seekerComponent;
    }

    protected override void UpdateValues()
    {
        seekerComponent.Mass = mass;
        seekerComponent.MaxForce = maxForce;
        seekerComponent.MaxSpeed = maxSpeed;
        seekerComponent.SeekWeight = seekWeight;
        seekerComponent.StayInBoundsFutureTime = stayInBoundsFutureTime;
        seekerComponent.StayInBoundsWeight = stayInBoundsWeight;
        seekerComponent.SeekForce = seekForce;
        seekerComponent.StayInBoundsForce = stayInBoundsForce;

        if (collisionChecker.AgentCircleCollision())
        { 
            dummy.transform.position = GetRandomPosition();
        }
    }

    protected override void UpdateAgentWallBounds()
    {
        seekerComponent.WallBounds = WallBounds;
    }

    protected override void GetCollisionChecker()
    {
        collisionChecker = GetComponent<SeekerCollisionChecker>();
    }
}


