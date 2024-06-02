using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class CollisionChecker : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        GetGameManagerScript();
    }

    protected abstract void GetGameManagerScript();

    public abstract bool AgentCircleCollision();

    public bool CircleCollision(Vector2 objPosition1, float objRadius1, Vector2 objPosition2, float objRadius2)
    {
        float distance = Vector2.Distance(objPosition1, objPosition2);

        float radiusSum = objRadius1 + objRadius2;

        bool colliding = distance < radiusSum;

        //Debug.Log($"Distance: {dististanceSquared} | Radius Sum: {radiusSum} | Colliding {colliding}");
        return colliding;
    }
}
