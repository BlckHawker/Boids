using System;
using UnityEngine;
using Rnd = UnityEngine.Random;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public enum SceneChecker
    {
        SeekerTest,
        FleerTest,
        FlockingTest,
        ObstacleAvoidanceTest,
        WandererTest
    }

    UIManager uiManager;

    [SerializeField]
    private SceneChecker sceneChecker;
    public SceneChecker SceneCheckerProperty { get { return sceneChecker; } }

    private CollisionChecker collisionChecker;
    private ObstacleManager obstacleManager;

    public Vector2 CameraWdithHeight { get {
            float height = 2f * camera.orthographicSize;
            float width = height * camera.aspect;
            return new Vector2(width, height);
        } }

    #region Prefabs
    [SerializeField]
    private GameObject dumbPrefab; //object that doesn't move

    [SerializeField]
    private Seeker seekerPrefab;

    [SerializeField]
    private Fleer fleerPrefab;

    [SerializeField]
    private Wanderer wandererPrefab;

    [SerializeField]
    private Flocker flockerPrefab;

    private List<Flocker> flockers;

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
    [NonSerialized]
    public float flockerAlignDistance, flockerAlignWeight, flockerSeparateWeight, flockerSeparateDistance, flockerMass, flockerMaxForce, flockerMaxSpeed, flockerStayInBoundsFutureTime, flockerStayInBoundsWeight;
    #endregion

    #region Screen Position
    [SerializeField]
    private Camera camera;
    #endregion

    void Start()
    {

        collisionChecker = GetComponent<CollisionChecker>();
        obstacleManager = GetComponent<ObstacleManager>();
        uiManager = GetComponent<UIManager>();
        obstacleManager.ObstacleList = new List<Obstacle>();
        flockers = new List<Flocker>();
        switch (sceneChecker)
        {
            case SceneChecker.SeekerTest:
                dummy = Instantiate(dumbPrefab);
                dummy.transform.position = GetRandomPosition();

                seeker = Instantiate(seekerPrefab);
                seeker.transform.position = GetRandomPosition();
                seekerComponent = seeker.GetComponent<Seeker>();
                seekerComponent.Target = dummy;

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

            case SceneChecker.FlockingTest:
                for (int i = 0; i < uiManager.FlockerCount; i++)
                {
                    Flocker flocker = Instantiate(flockerPrefab);
                    flocker.Position = GetRandomPosition();
                    flockers.Add(flocker);
                }
                
                foreach (Flocker flocker in flockers)
                {
                    flocker.FlockingList = flockers.Select(f => (Agent)f).ToList();
                }
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
            case SceneChecker.FlockingTest:
                UpdateFlockerValues();
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
            wandererComponent.ObstacleList = obstacleManager.ObstacleList;
        }
    }

    private void UpdateFlockerValues()
    {
        foreach (Flocker flocker in flockers)
        {
            flocker.Mass = flockerMass;
            flocker.SeparateDistance = flockerSeparateDistance;
            flocker.SeparateWeight = flockerSeparateWeight;
            flocker.MaxForce = flockerMaxForce;
            flocker.MaxSpeed = flockerMaxSpeed;
            flocker.StayInBoundsFutureTime = flockerStayInBoundsFutureTime;
            flocker.StayInBoundsWeight = flockerStayInBoundsWeight;
            flocker.AlignDistance = flockerAlignDistance;
            flocker.AlignWeight = flockerAlignWeight;
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
        Bounds wallBounds = new Bounds(Vector2.zero, CameraWdithHeight);

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
            case SceneChecker.ObstacleAvoidanceTest: 
                wandererComponent.WallBounds = wallBounds;
                break;
        }
    }
}
