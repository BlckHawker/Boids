using System;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private enum SceneChecker
    {
        SeekerTest,
        FleerTest,
        WandererTest
    }

    [SerializeField]
    private SceneChecker sceneChecker;

    [SerializeField]
    CollisionChecker collisionChecker;

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
    public float seekerFutureTime, seekerMass, seekerMaxForce, seekerMaxSpeed, seekerWeight, seekerStayInBoundsWeight;
    [NonSerialized]
    public float fleerFutureTime, fleerMass, fleerMaxForce, fleerMaxSpeed, fleerWeight, fleerStayInBoundsWeight;
    [NonSerialized]
    public float wandererFutureTime, wandererMass, wandererMaxForce, wandererMaxSpeed, wandererStayInBoundsWeight, wandererWanderWeight, wandererWanderCircleRadius, wandererWanderOffset, wandererWanderTime;
    #endregion

    #region Screen Position
    [SerializeField]
    private Camera camera;
    #endregion

    void Start()
    {
        Bounds wallBounds = UpdateAgentWallBounds();
        if (sceneChecker == SceneChecker.SeekerTest)
        {
            dummy = Instantiate(dumbPrefab);
            dummy.transform.position = GetRandomPosition();

            seeker = Instantiate(seekerPrefab);
            seeker.transform.position = GetRandomPosition();
            seekerComponent = seeker.GetComponent<Seeker>();
            seekerComponent.Target = dummy;
            seekerComponent.WallBounds = wallBounds;

            collisionChecker.SetSceneChecker((int)sceneChecker);
            collisionChecker.SetDummyObject(dummy);
            collisionChecker.SetSeeker(seekerComponent);
        }

        else if (sceneChecker == SceneChecker.FleerTest)
        {
            fleer = Instantiate(fleerPrefab);
            seeker = Instantiate(seekerPrefab);

            seekerComponent = seeker.GetComponent<Seeker>();
            seekerComponent.Position = GetRandomPosition();
            seekerComponent.Target = fleer.gameObject;
            seekerComponent.WallBounds = wallBounds;

            fleerComponent = fleer.GetComponent<Fleer>();
            fleerComponent.Position = GetRandomPosition();
            fleerComponent.Target = seeker.gameObject;
            fleerComponent.WallBounds = wallBounds;
        }

        else if (sceneChecker == SceneChecker.WandererTest)
        {
            wanderer = Instantiate(wandererPrefab);
            wandererComponent = wanderer.GetComponent<Wanderer>();
            wandererComponent.WallBounds = wallBounds;
        }
    }
    void Update()
    {
        if (sceneChecker == SceneChecker.SeekerTest && collisionChecker.CircleCollision())
            dummy.transform.position = GetRandomPosition();

        UpdateValues();
    }

    private void UpdateValues()
    {
        if (sceneChecker == SceneChecker.SeekerTest)
        {
            UpdateSeekerValues();
        }

        else if (sceneChecker == SceneChecker.FleerTest)
        {
            UpdateSeekerValues();
            UpdateFleerValues();
        }

        else if(sceneChecker == SceneChecker.WandererTest) 
        {
            UpdateWandererValues();
        }
    }
    private void UpdateSeekerValues()
    {
        seekerComponent.FutureTime = seekerFutureTime;
        seekerComponent.Mass = seekerMass;
        seekerComponent.MaxForce = seekerMaxForce;
        seekerComponent.MaxSpeed = seekerMaxSpeed;
        seekerComponent.SeekWeight = seekerWeight;
        seekerComponent.StayInBoundsWeight = seekerStayInBoundsWeight;
    }
    private void UpdateFleerValues()
    {
        fleerComponent.FutureTime = fleerFutureTime;
        fleerComponent.Mass = fleerMass;
        fleerComponent.MaxForce = fleerMaxForce;
        fleerComponent.MaxSpeed = fleerMaxSpeed;
        fleerComponent.FleeWeight = fleerWeight;
        fleerComponent.StayInBoundsWeight = fleerStayInBoundsWeight;
    }
    private void UpdateWandererValues()
    {
        wandererComponent.FutureTime = wandererFutureTime;
        wandererComponent.Mass = wandererMass;
        wandererComponent.MaxForce = wandererMaxForce;
        wandererComponent.MaxSpeed = wandererMaxSpeed;
        wandererComponent.StayInBoundsWeight = wandererStayInBoundsWeight;
        wandererComponent.WanderWeight = wandererWanderWeight;
        wandererComponent.WanderCircleRadius = wandererWanderCircleRadius;
        wandererComponent.WanderOffset = wandererWanderOffset;
        wandererComponent.WanderTime = wandererWanderTime;
    }
    private Vector3 GetRandomPosition()
    {
        Vector3 pos = camera.ScreenToWorldPoint(new Vector3(Rnd.Range(0, camera.pixelWidth), Rnd.Range(0, camera.pixelHeight)));
        pos.z = 0;
        return pos;
    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            if (sceneChecker == SceneChecker.SeekerTest)
                Gizmos.DrawWireCube(seekerComponent.WallBounds.center, seekerComponent.WallBounds.size);
        }
    }

    private Bounds UpdateAgentWallBounds()
    {
        float height = 2f * camera.orthographicSize;
        float width = height * camera.aspect;
        return new Bounds(Vector2.zero, new Vector2(width, height));
    }
}
