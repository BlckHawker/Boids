using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerCollisionChecker : CollisionChecker
{
    private Agent seeker;
    public Agent Seeker { set { seeker = value; } }
    private GameObject dummyObject;
    public GameObject DummyObject { set { dummyObject = value; dummyObjectRadius = dummyObject.transform.localScale.x / 2; } }
    private float dummyObjectRadius;
    private SeekerGameManager gameManager;

    protected override void GetGameManagerScript()
    { 
        gameManager = GetComponent<SeekerGameManager>();
    }

    public override bool AgentCircleCollision()
    {
        return CircleCollision(seeker.Position, seeker.Radius, dummyObject.transform.position, dummyObjectRadius);
    }
}
