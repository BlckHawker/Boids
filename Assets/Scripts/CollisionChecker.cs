using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    private enum SceneChecker
    {
        SeekerTest
    }
    private SceneChecker sceneChecker;
    private Agent seeker;
    private GameObject dummyObject; //object that doesn't move
    private float dummyObjectRadius;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Checks to see if two objects are colliding using circle colision
    /// </summary>
    /// <returns>if the objects are colliding</returns>
    public bool CircleCollision()
    {
        switch (sceneChecker)
        {
            case SceneChecker.SeekerTest:
                return CircleCollision(seeker.Position, seeker.Radius, dummyObject.transform.position, dummyObjectRadius);
        }

        return false;
    }

    public bool CircleCollision(Vector2 objPosition1, float objRadius1, Vector2 objPosition2, float objRadius2)
    {
        float dististanceSquared = Mathf.Pow(objPosition1.x - objPosition2.x, 2) +
                                   Mathf.Pow(objPosition1.y - objPosition2.y, 2);

        float radiusSum = Mathf.Pow(objRadius1 + objRadius2, 2);

        bool colliding = dististanceSquared < radiusSum;

        //Debug.Log($"Distance: {dististanceSquared} | Radius Sum: {radiusSum} | Colliding {colliding}");
        return colliding;
    }


    #region Setter Methods
    public void SetSceneChecker(int sceneChecker)
    {
        this.sceneChecker = (SceneChecker)sceneChecker;
    }

    public void SetSeeker(Agent seeker)
    {
        this.seeker = seeker;
    }

    public void SetDummyObject(GameObject dummyObject)
    {
        this.dummyObject = dummyObject;
        dummyObjectRadius = dummyObject.transform.localScale.x / 2;
    }

    #endregion

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        switch (sceneChecker)
        {
            case SceneChecker.SeekerTest:
                //Gizmos.DrawWireSphere(seeker.Position, seeker.Radius);
                //Gizmos.DrawWireSphere(dummyObject.transform.position, dummyObjectRadius);
                break;
        }
    }
}
