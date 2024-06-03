using System;
using UnityEngine;
using Rnd = UnityEngine.Random;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using System.Security.Authentication.ExtendedProtection;

public abstract class GameManager : MonoBehaviour
{
    private ObstacleManager obstacleManager;

    public Vector2 CameraWdithHeight { get {
            Camera c = Camera.main;
            float height = 2f * c.orthographicSize;
            float width = height * c.aspect;
            return new Vector2(width, height);
        } }
    public Bounds WallBounds { get { return new Bounds(Vector2.zero, CameraWdithHeight); } }

    #region Prefabs

    //[SerializeField]
    //private Fleer fleerPrefab;

    //[SerializeField]
    //private Wanderer wandererPrefab;

    //[SerializeField]
    //private Flocker flockerPrefab;

    //private List<Flocker> flockers;

    private Agent wanderer;
    private Wanderer wandererComponent;
    #endregion

    #region Agent Stats
    
    [NonSerialized]
    public float wandererAvoidTime, wandererObstacleAvoidanceWeight, wandererStayInBoundsFutureTime, wandererWanderFutureTime, wandererMass, wandererMaxForce, wandererMaxSpeed, wandererStayInBoundsWeight, wandererWanderWeight, wandererWanderCircleRadius, wandererWanderOffset, wandererWanderTime;
    [NonSerialized]
    public float flockerAlignDistance, flockerAlignWeight, flockerSeparateWeight, flockerSeparateDistance, flockerMass, flockerMaxForce, flockerMaxSpeed, flockerStayInBoundsFutureTime, flockerStayInBoundsWeight;
    [NonSerialized]
    public bool flockerAlignForce, flockerSeparateForce, flockerStayInBoundsForce;
    #endregion

    #region Screen Position

    #endregion

    void Start()
    {

        GetCollisionChecker();
        obstacleManager = GetComponent<ObstacleManager>();
        obstacleManager.ObstacleList = new List<Obstacle>();
        //switch (sceneChecker)
        //{
        //    case SceneChecker.SeekerTest:
        //        break;

        //    case SceneChecker.FleerTest:
        //        break;

        //    case SceneChecker.WandererTest:
        //    case SceneChecker.ObstacleAvoidanceTest:
        //        //wanderer = Instantiate(wandererPrefab);
        //        //wandererComponent = wanderer.GetComponent<Wanderer>();
        //        break;

        //    case SceneChecker.FlockingTest:
        //        //for (int i = 0; i < uiManager.FlockerCount; i++)
        //        //{
        //        //    Flocker flocker = Instantiate(flockerPrefab);
        //        //    flocker.Position = GetRandomPosition();
        //        //    flockers.Add(flocker);
        //        //}
                
        //        //foreach (Flocker flocker in flockers)
        //        //{
        //        //    flocker.FlockingList = flockers.Select(f => (Agent)f).ToList();
        //        //}
        //    break;
        //}
        InstantiateObjects();
        UpdateAgentWallBounds();
    }
    void Update()
    {
        UpdateAgentWallBounds();
        UpdateValues();
    }



    //private void UpdateValues()
    //{
    //    switch (sceneChecker)
    //    {
    //        case SceneChecker.SeekerTest:
    //            UpdateSeekerValues();
    //            break;
    //        case SceneChecker.FleerTest: 
    //            UpdateFleerValues(); 
    //            break;
    //        case SceneChecker.WandererTest: 
    //        case SceneChecker.ObstacleAvoidanceTest:
    //            UpdateWandererValues(); 
    //            break;
    //        case SceneChecker.FlockingTest:
    //            UpdateFlockerValues();
    //        break;
    //    }
    //}

    private void UpdateWandererValues()
    {
        //wandererComponent.Mass = wandererMass;
        //wandererComponent.MaxForce = wandererMaxForce;
        //wandererComponent.MaxSpeed = wandererMaxSpeed;
        //wandererComponent.StayInBoundsFutureTime = wandererStayInBoundsFutureTime;
        //wandererComponent.StayInBoundsWeight = wandererStayInBoundsWeight;
        //wandererComponent.WanderCircleRadius = wandererWanderCircleRadius;
        //wandererComponent.WanderFutureTime = wandererWanderFutureTime;
        //wandererComponent.WanderWeight = wandererWanderWeight;
        //wandererComponent.WanderOffset = wandererWanderOffset;
        //wandererComponent.WanderTime = wandererWanderTime;

        //if(sceneChecker == SceneChecker.ObstacleAvoidanceTest) 
        //{
        //    wandererComponent.AvoidTime = wandererAvoidTime;
        //    wandererComponent.AvoidObstacleWeight = wandererObstacleAvoidanceWeight;
        //    wandererComponent.ObstacleList = obstacleManager.ObstacleList;
        //}
    }

    private void UpdateFlockerValues()
    {
        //foreach (Flocker flocker in flockers)
        //{
        //    flocker.Mass = flockerMass;
        //    flocker.SeparateDistance = flockerSeparateDistance;
        //    flocker.SeparateWeight = flockerSeparateWeight;
        //    flocker.MaxForce = flockerMaxForce;
        //    flocker.MaxSpeed = flockerMaxSpeed;
        //    flocker.StayInBoundsFutureTime = flockerStayInBoundsFutureTime;
        //    flocker.StayInBoundsWeight = flockerStayInBoundsWeight;
        //    flocker.AlignDistance = flockerAlignDistance;
        //    flocker.AlignWeight = flockerAlignWeight;
        //    flocker.StayInBoundsForce = flockerStayInBoundsForce;
        //    flocker.SeparateForce = flockerSeparateForce;
        //    flocker.AlignForce = flockerAlignForce;
        //}
    }
    protected Vector3 GetRandomPosition()
    {
        Camera c = Camera.main;
        Vector3 pos = c.ScreenToWorldPoint(new Vector3(Rnd.Range(0, c.pixelWidth), Rnd.Range(0, c.pixelHeight)));
        pos.z = 0;
        return pos;
    }

    protected abstract void GetCollisionChecker();
    protected abstract void InstantiateObjects();

    protected abstract void UpdateValues();

    protected abstract void UpdateAgentWallBounds();

    //private void UpdateAgentWallBounds()
    //{
    //    Bounds wallBounds = new Bounds(Vector2.zero, CameraWdithHeight);

    //    switch (sceneChecker)
    //    {
    //        case SceneChecker.SeekerTest:
    //            seekerComponent.WallBounds = wallBounds;
    //            break;
    //        case SceneChecker.FleerTest:
    //            seekerComponent.WallBounds = wallBounds;
    //            fleerComponent.WallBounds = wallBounds;
    //            break;
    //        case SceneChecker.WandererTest:
    //        case SceneChecker.ObstacleAvoidanceTest: 
    //            wandererComponent.WallBounds = wallBounds;
    //            break;
    //        case SceneChecker.FlockingTest:
    //            foreach (Agent flocker in flockers)
    //            {
    //                flocker.WallBounds = wallBounds;
    //            }
    //        break;
    //    }
    //}


}
