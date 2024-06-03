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
    private Seeker seeker;
    private SeekerCollisionChecker collisionChecker;

    protected override void InstantiateObjects()
    {
        dummy = Instantiate(dumbPrefab);
        dummy.transform.position = GetRandomPosition();

        seeker = Instantiate(seekerPrefab);
        seeker.transform.position = GetRandomPosition();
        seeker.Target = dummy;

        collisionChecker.DummyObject = dummy;
        collisionChecker.Seeker = seeker;
    }

    protected override void UpdateValues()
    {
        seeker.Mass = mass;
        seeker.MaxForce = maxForce;
        seeker.MaxSpeed = maxSpeed;
        seeker.SeekWeight = seekWeight;
        seeker.StayInBoundsFutureTime = stayInBoundsFutureTime;
        seeker.StayInBoundsWeight = stayInBoundsWeight;
        seeker.SeekForce = seekForce;
        seeker.StayInBoundsForce = stayInBoundsForce;

        if (collisionChecker.AgentCircleCollision())
        { 
            dummy.transform.position = GetRandomPosition();
        }
    }

    protected override void UpdateAgentWallBounds()
    {
        seeker.WallBounds = WallBounds;
    }

    protected override void GetCollisionChecker()
    {
        collisionChecker = GetComponent<SeekerCollisionChecker>();
    }
}


