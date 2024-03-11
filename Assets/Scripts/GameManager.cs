using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class GameManager : MonoBehaviour
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
    [NonSerialized]
    public float futureTime, mass, maxForce, maxSpeed;
    #endregion
    #region Force Weight
    [NonSerialized]
    public float seekerWeight, stayInBoundsWeight;
    #endregion

    #region Screen Position
    [SerializeField]
    private Camera camera;
    #endregion

    void Start()
    {
        float height = 2f * camera.orthographicSize;
        float width = height * camera.aspect;
        if (sceneChecker == SceneChecker.SeekerTest)
        {
            dummy = Instantiate(dumbPrefab);
            dummy.transform.position = GetRandomPosition();

            seeker = Instantiate(seekerPrefab).gameObject;
            seeker.transform.position = GetRandomPosition();
            seekerComponent = seeker.GetComponent<Seeker>();
            seekerComponent.Target = dummy;
            seekerComponent.WallBounds = new Bounds(Vector2.zero, new Vector2(width, height));

            collisionChecker.SetSceneChecker((int)sceneChecker);
            collisionChecker.SetDummyObject(dummy);
            collisionChecker.SetSeeker(seekerComponent);
        }
    }

    void Update()
    {
        if (collisionChecker.CircleCollision())
            dummy.transform.position = GetRandomPosition();

        UpdateValues();
    }

    private void UpdateValues()
    {
        if (sceneChecker == SceneChecker.SeekerTest)
        {
            seekerComponent.FutureTime = futureTime;
            seekerComponent.Mass = mass;
            seekerComponent.MaxForce = maxForce;
            seekerComponent.MaxSpeed = maxSpeed;
            seekerComponent.SeekerWeight = seekerWeight;
            seekerComponent.StayInBoundsWeight = stayInBoundsWeight;
        }
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
}
