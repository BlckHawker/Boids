using System;
using UnityEngine;
using Rnd = UnityEngine.Random;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private enum SceneChecker
    {
        SeekerTest,
        FleerTest,
        ObstacleAvoidanceTest,
        WandererTest
    }

    [SerializeField]
    private SceneChecker sceneChecker;

    private CollisionChecker collisionChecker;
    private ObstacleManager obstacleManager;

    #region Prefabs
    [SerializeField]
    private GameObject dumbPrefab; //object that doesn't move

    [SerializeField]
    private Seeker seekerPrefab;

    [SerializeField]
    private Fleer fleerPrefab;

    [SerializeField]
    private Wanderer wandererPrefab;

    private GameObject dummy;
    private Agent seeker, fleer, wanderer;
    private Seeker seekerComponent;
    private Fleer fleerComponent;
    private Wanderer wandererComponent;
    #endregion

    #region Agent Stats
    [NonSerialized]
    public float seekerStayInBoundsFutureTime, seekerMass, seekerMaxForce, seekerMaxSpeed, seekerWeight, seekerStayInBoundsWeight;
    [NonSerialized]
    public float fleerStayInBoundsFutureTime, fleerMass, fleerMaxForce, fleerMaxSpeed, fleerWeight, fleerStayInBoundsWeight;
    [NonSerialized]
    public float wandererAvoidTime, wandererObstacleAvoidanceWeight, wandererStayInBoundsFutureTime, wandererWanderFutureTime, wandererMass, wandererMaxForce, wandererMaxSpeed, wandererStayInBoundsWeight, wandererWanderWeight, wandererWanderCircleRadius, wandererWanderOffset, wandererWanderTime;
    #endregion

    #region Screen Position
    [SerializeField]
    private Camera camera;
    #endregion

    void Start()
    {

        collisionChecker = GetComponent<CollisionChecker>();
        obstacleManager = GetComponent<ObstacleManager>();
        obstacleManager.obstacleList = new List<Obstacle>();
        switch (sceneChecker)
        {
            case SceneChecker.SeekerTest:
                dummy = Instantiate(dumbPrefab);
                dummy.transform.position = GetRandomPosition();

                seeker = Instantiate(seekerPrefab);
                seeker.transform.position = GetRandomPosition();
                seekerComponent = seeker.GetComponent<Seeker>();
                seekerComponent.Target = dummy;

                collisionChecker.SetSceneChecker((int)sceneChecker);
                collisionChecker.SetDummyObject(dummy);
                collisionChecker.SetSeeker(seekerComponent);
                break;

            case SceneChecker.FleerTest:
                fleer = Instantiate(fleerPrefab);
                seeker = Instantiate(seekerPrefab);

                seekerComponent = seeker.GetComponent<Seeker>();
                seekerComponent.Position = GetRandomPosition();
                seekerComponent.Target = fleer.gameObject;

                fleerComponent = fleer.GetComponent<Fleer>();
                fleerComponent.Position = GetRandomPosition();
                fleerComponent.Target = seeker.gameObject;
                break;

            case SceneChecker.WandererTest:
            case SceneChecker.ObstacleAvoidanceTest:
                wanderer = Instantiate(wandererPrefab);
                wandererComponent = wanderer.GetComponent<Wanderer>();
                break;
        }
        UpdateAgentWallBounds();
    }
    void Update()
    {
        UpdateAgentWallBounds();
        UpdateValues();
        switch (sceneChecker)
        {
            case SceneChecker.SeekerTest:
                if(collisionChecker.CircleCollision())
                    dummy.transform.position = GetRandomPosition();
                break;
            case SceneChecker.FleerTest:
                break;
            case SceneChecker.WandererTest:
                break;
        }
    }

    private void UpdateValues()
    {
        switch (sceneChecker)
        {
            case SceneChecker.SeekerTest:
                UpdateSeekerValues();
                break;
            case SceneChecker.FleerTest: 
                UpdateFleerValues(); 
                break;
            case SceneChecker.WandererTest: 
            case SceneChecker.ObstacleAvoidanceTest: 
                UpdateWandererValues(); 
                break;
        }
    }
    private void UpdateSeekerValues()
    {
        seekerComponent.Mass = seekerMass;
        seekerComponent.MaxForce = seekerMaxForce;
        seekerComponent.MaxSpeed = seekerMaxSpeed;
        seekerComponent.SeekWeight = seekerWeight;
        seekerComponent.StayInBoundsFutureTime = seekerStayInBoundsFutureTime;
        seekerComponent.StayInBoundsWeight = seekerStayInBoundsWeight;
    }
    private void UpdateFleerValues()
    {
        fleerComponent.Mass = fleerMass;
        fleerComponent.MaxForce = fleerMaxForce;
        fleerComponent.MaxSpeed = fleerMaxSpeed;
        fleerComponent.FleeWeight = fleerWeight;
        fleerComponent.StayInBoundsFutureTime = fleerStayInBoundsFutureTime;
        fleerComponent.StayInBoundsWeight = fleerStayInBoundsWeight;
    }
    private void UpdateWandererValues()
    {
        wandererComponent.Mass = wandererMass;
        wandererComponent.MaxForce = wandererMaxForce;
        wandererComponent.MaxSpeed = wandererMaxSpeed;
        wandererComponent.StayInBoundsFutureTime = wandererStayInBoundsFutureTime;
        wandererComponent.StayInBoundsWeight = wandererStayInBoundsWeight;
        wandererComponent.WanderCircleRadius = wandererWanderCircleRadius;
        wandererComponent.WanderFutureTime = wandererWanderFutureTime;
        wandererComponent.WanderWeight = wandererWanderWeight;
        wandererComponent.WanderOffset = wandererWanderOffset;
        wandererComponent.WanderTime = wandererWanderTime;

        if(sceneChecker == SceneChecker.ObstacleAvoidanceTest) 
        {
            wandererComponent.AvoidTime = wandererAvoidTime;
            wandererComponent.AvoidObstacleWeight = wandererObstacleAvoidanceWeight;
        }
    }
    private Vector3 GetRandomPosition()
    {
        Vector3 pos = camera.ScreenToWorldPoint(new Vector3(Rnd.Range(0, camera.pixelWidth), Rnd.Range(0, camera.pixelHeight)));
        pos.z = 0;
        return pos;
    }

    private void UpdateAgentWallBounds()
    {
        float height = 2f * camera.orthographicSize;
        float width = height * camera.aspect;
        Bounds wallBounds = new Bounds(Vector2.zero, new Vector2(width, height));

        switch (sceneChecker)
        {
            case SceneChecker.SeekerTest:
                seekerComponent.WallBounds = wallBounds;
                break;
            case SceneChecker.FleerTest:
                seekerComponent.WallBounds = wallBounds;
                fleerComponent.WallBounds = wallBounds;
                break;
            case SceneChecker.WandererTest:
                wandererComponent.WallBounds = wallBounds;
                break;
        }
    }
}
