using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    private Agent seeker;
    private GameObject dummyObject; //object that doesn't move
    private float dummyObjectRadius;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    /// <summary>
    /// Checks to see if two objects are colliding using circle colision
    /// </summary>
    /// <returns>if the objects are colliding</returns>
    public bool CircleCollision()
    {
        switch (gameManager.SceneCheckerProperty)
        {
            case GameManager.SceneChecker.SeekerTest:
                return CircleCollision(seeker.Position, seeker.Radius, dummyObject.transform.position, dummyObjectRadius);
        }

        return false;
    }

    public bool CircleCollision(Vector2 objPosition1, float objRadius1, Vector2 objPosition2, float objRadius2)
    {
        float distance = Vector2.Distance(objPosition1, objPosition2);

        float radiusSum = objRadius1 + objRadius2;

        bool colliding = distance < radiusSum;

        //Debug.Log($"Distance: {dististanceSquared} | Radius Sum: {radiusSum} | Colliding {colliding}");
        return colliding;
    }


    #region Setter Methods

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
        if(Application.isPlaying) 
        {
            switch (gameManager.SceneCheckerProperty)
            {
                case GameManager.SceneChecker.SeekerTest:
                    //Gizmos.DrawWireSphere(seeker.Position, seeker.Radius);
                    //Gizmos.DrawWireSphere(dummyObject.transform.position, dummyObjectRadius);
                    break;
            }
        }
    }
}
