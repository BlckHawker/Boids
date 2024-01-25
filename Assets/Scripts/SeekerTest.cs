using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rnd = UnityEngine.Random;

/// <summary>
/// Seeker and dummy object spawn at a random position on screen.
/// Seeker moves towards dummy object. Once both collide, dummy 
/// object will move to another random position.
/// </summary>
public class SeekerTest : MonoBehaviour
{
    private enum SceneChecker
    {
        SeekerTest
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

    private GameObject dummy, seeker;
    private Seeker seekerComponent;
    #endregion

    #region Agent Stats
    [SerializeField]
    [Range(0f, 10f)]
    private float futureTime;
    [SerializeField]
    [Range(1f,10f)]
    private float mass, maxForce, maxSpeed;
    #endregion
    #region Force Weight
    [Range(1, 10)]
    [SerializeField]
    private float seekerWeight, stayInBoundsWeight;
    #endregion

    #region Screen Position
    [SerializeField]
    private Camera camera;
    #endregion

    void Start()
    {
        float height = 2f * camera.orthographicSize;
        float width = height * camera.aspect;

        dummy = Instantiate(dumbPrefab);
        dummy.transform.position = GetRandomPosition();

        seeker = Instantiate(seekerPrefab).gameObject;
        seeker.transform.position = GetRandomPosition();
        seekerComponent = seeker.GetComponent<Seeker>();
        seekerComponent.Target = dummy;
        seekerComponent.WallBounds = new Bounds(Vector2.zero, new Vector2(width, height));
        UpdateValues();
        
        collisionChecker.SetSceneChecker((int)sceneChecker);
        collisionChecker.SetDummyObject(dummy);
        collisionChecker.SetSeeker(seekerComponent);
    }

    void Update()
    {
        if (collisionChecker.CircleCollision())
            dummy.transform.position = GetRandomPosition();
        UpdateValues();
    }

    private void UpdateValues()
    {
        seekerComponent.Mass = mass;
        seekerComponent.MaxForce = maxForce;
        seekerComponent.SeekerWeight = seekerWeight;
        seekerComponent.MaxSpeed = maxSpeed;
        seekerComponent.StayInBoundsWeight = stayInBoundsWeight;
        seekerComponent.FutureTime = futureTime;
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
            Gizmos.DrawWireCube(seekerComponent.WallBounds.center, seekerComponent.WallBounds.size);
        }
    }
}
