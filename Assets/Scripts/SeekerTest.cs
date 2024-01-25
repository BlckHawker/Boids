using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    Seeker seekerComponent;
    #endregion

    #region Agent Stats
    [SerializeField]
    [Range(1,10)]
    private float mass, maxSpeed, maxForce;
    #endregion
    #region Force Weight
    [Range(1, 10)]
    [SerializeField]
    private float seekerWeight;
    #endregion

    #region Screen Position
    [SerializeField]
    private Camera camera;
    #endregion

    void Start()
    {
        dummy = Instantiate(dumbPrefab);
        dummy.transform.position = GetRandomPosition();

        seeker = Instantiate(seekerPrefab).gameObject;
        seeker.transform.position = GetRandomPosition();
        seekerComponent = seeker.GetComponent<Seeker>();
        seekerComponent.Target = dummy;
        UpdateValues();


        collisionChecker.SetSceneChecker((int)sceneChecker);
        collisionChecker.SetDummyObject(dummy);
        collisionChecker.SetSeeker(seeker.GetComponent<Seeker>());
    }

    void Update()
    {
        if (collisionChecker.CircleCollision())
            dummy.transform.position = GetRandomPosition();

        UpdateValues();
    }

    private void UpdateValues()
    {
        seeker.GetComponent<Seeker>().Mass = mass;
        seeker.GetComponent<Seeker>().MaxForce = maxForce;
        seeker.GetComponent<Seeker>().SeekerWeight = seekerWeight;
        seeker.GetComponent<Seeker>().MaxSpeed = maxSpeed;
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 pos = camera.ScreenToWorldPoint(new Vector3(Rnd.Range(0, camera.pixelWidth), Rnd.Range(0, camera.pixelHeight)));
        pos.z = 0;
        return pos;
    }
}
