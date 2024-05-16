using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    float canvasWidth, canvasHeight;
    ObstacleManager obstacleManager;
    GameManager gameManager;
    private void Start()
    {
        obstacleManager = GetComponent<ObstacleManager>();
        gameManager = GetComponent<GameManager>();
    }
    public void CreateCircle(InputAction.CallbackContext context)
    {
        //Only want to call when click is pressed
        if (context.started || context.canceled || gameManager.SceneCheckerProperty != GameManager.SceneChecker.ObstacleAvoidanceTest)
            return;

        Debug.Log("Click");

        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector2 CameraRatio = gameManager.CameraWdithHeight;
        Vector2 screenRatio = Camera.main.WorldToScreenPoint(new Vector2(CameraRatio.x / 2, CameraRatio.y / 2));
        Debug.Log(screenRatio);
        Vector2 mouseRatio = new Vector2(mousePos.x / screenRatio.x, mousePos.y / screenRatio.y);

        //don't do anything if the mouse click was on top of canvas
        if (mouseRatio.x <= canvasWidth && mouseRatio.y >= canvasHeight)
            return;


        //convert from screen to world view
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        //make sure the object is in front of the camera
        mousePos.z = 0;

        obstacleManager.CreateObstacle(mousePos);
    }
}
