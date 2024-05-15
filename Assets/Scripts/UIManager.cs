using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private enum SceneChecker
    {
        SeekerTest,
        FleerTest,
        ObstacleAvoidanceTest,
        WandererTest,
    }

    [SerializeField]
    private SceneChecker sceneChecker;
    [SerializeField]
    private GameObject canvasGameObject;
    private (Slider, Text) seekerStayInBoundsFutureTimeTuple, seekerMassTuple, seekerMaxForceTuple, seekerMaxSpeedTuple, seekerWeightTuple, seekerStayInBoundTuple;
    private (Slider, Text) fleerStayInBoundsFutureTimeTuple, fleerMassTuple, fleerMaxForceTuple, fleerMaxSpeedTuple, fleerWeightTuple, fleerStayInBoundTuple;
    private (Slider, Text) wandererStayInBoundsFutureTimeTuple, wandererWanderFutureTimeTuple, wandererMassTuple, wandererMaxForceTuple, wandererMaxSpeedTuple, wandererObstacleAvoidanceWeightTuple, wandererStayInBoundsWeightTuple, wandererWanderWeightTuple, wandererWanderCircleRadiusTuple, wandererWanderOffsetTuple, wandererWanderTimeTuple;

    #region Seeker Values
    [Header("Seeker Values")]
    [SerializeField, Range(1, 10)]
    private int defaultSeekerFutureTime;

    [SerializeField, Range(1, 10)]
    private int defaultSeekerMass, defaultSeekerMaxForce, defaultSeekerMaxSpeed, defaultSeekerWeight, defaultSeekerStayInBound;
    #endregion

    #region Fleer Values
    [Header("Fleer Values")]
    [SerializeField, Range(1, 10)]
    private int defaultFleerFutureTime;

    [SerializeField, Range(1, 10)]
    private int defaultFleerMass, defaultFleerMaxForce, defaultFleerMaxSpeed, defaultFleerWeight, defaultFleerStayInBound;
    #endregion

    #region Wanderer Values
    [Header("Wanderer Values")]
    [SerializeField, Range(1, 10)]
    private int defaultWandererMass;
    [SerializeField, Range(1, 10)]
    private int defaultWandererMaxForce;
    [SerializeField, Range(1, 10)]
    private int defaultWandererMaxSpeed;
    [SerializeField, Range(1, 10)]
    private int defaultWandererObstacleAvoidanceWeight;
    [SerializeField, Range(1, 10)]
    private int defaultWandererStayInBoundsFutureTime;
    [SerializeField, Range(1, 10)]
    private int defaultWandererStayInBoundsWeight;
    [SerializeField, Range(1, 10)]
    private int defaultWandererWanderWeight;
    [SerializeField, Range(1, 5)]
    private int defaultWandererWanderCircleRadius;
    [SerializeField, Range(1, 100)]
    private int defaultWandererWanderOffset;
    [SerializeField, Range(0f, 3f)]
    private float defaultWandererWanderTime;
    [SerializeField, Range(1, 10)]
    private int defaultWandererWanderFutureTime;
    #endregion

    private GameManager gameManagerScript;
    private Button seekerButton, fleerButton;
    private GameObject seekPanel, fleePanel;

    private List<(GameObject, Button)> panelsAndButtons;
    void Start()
    {
        gameManagerScript = GetComponent<GameManager>();
        Transform contentTransform = null;
        switch (sceneChecker)
        {
            case SceneChecker.SeekerTest:
                contentTransform = canvasGameObject.transform.Find("Panel/Scroll Area/Content");
                seekerStayInBoundsFutureTimeTuple = GetStatTuple(contentTransform, parentName: "Future Time", initialValue: defaultSeekerFutureTime);
                seekerMassTuple = GetStatTuple(contentTransform, parentName: "Mass", initialValue: defaultSeekerMass);
                seekerMaxForceTuple = GetStatTuple(contentTransform, parentName: "Max Force", initialValue: defaultSeekerMaxForce);
                seekerMaxSpeedTuple = GetStatTuple(contentTransform, parentName: "Max Speed", initialValue: defaultSeekerMaxSpeed);
                seekerWeightTuple = GetStatTuple(contentTransform, parentName: "Seeker Weight", initialValue: defaultSeekerWeight);
                seekerStayInBoundTuple = GetStatTuple(contentTransform, parentName: "Stay in Bounds Weight", initialValue: defaultSeekerStayInBound);
                break;

            case SceneChecker.FleerTest:
                Transform statPanel = canvasGameObject.transform.Find("Stat Panel");
                fleePanel = statPanel.Find("Flee Panel").gameObject;
                seekPanel = statPanel.Find("Seek Panel").gameObject;

                Transform fleePanelContent = fleePanel.transform.Find("Scroll Area/Content");
                Transform seekPanelContent = seekPanel.transform.Find("Scroll Area/Content");

                seekerButton = statPanel.Find("Seeker Button").GetComponent<Button>();
                fleerButton = statPanel.Find("Flee Button").GetComponent<Button>();
                panelsAndButtons = new List<(GameObject, Button)>() { (seekPanel, seekerButton), (fleePanel, fleerButton) };


                seekerMassTuple = GetStatTuple(seekPanelContent, parentName: "Mass", initialValue: defaultSeekerMass);
                seekerMaxForceTuple = GetStatTuple(seekPanelContent, parentName: "Max Force", initialValue: defaultSeekerMaxForce);
                seekerMaxSpeedTuple = GetStatTuple(seekPanelContent, parentName: "Max Speed", initialValue: defaultSeekerMaxSpeed);
                seekerStayInBoundTuple = GetStatTuple(seekPanelContent, parentName: "Stay in Bounds Weight", initialValue: defaultSeekerStayInBound);
                seekerWeightTuple = GetStatTuple(seekPanelContent, parentName: "Seeker Weight", initialValue: defaultSeekerWeight); 
                seekerStayInBoundsFutureTimeTuple = GetStatTuple(seekPanelContent, parentName: "Future Time", initialValue: defaultSeekerFutureTime);

                fleerMassTuple = GetStatTuple(fleePanelContent, parentName: "Mass", initialValue: defaultFleerMass);
                fleerMaxForceTuple = GetStatTuple(fleePanelContent, parentName: "Max Force", initialValue: defaultFleerMaxForce);
                fleerMaxSpeedTuple = GetStatTuple(fleePanelContent, parentName: "Max Speed", initialValue: defaultFleerMaxSpeed);
                fleerStayInBoundTuple = GetStatTuple(fleePanelContent, parentName: "Stay in Bounds Weight", initialValue: defaultFleerStayInBound);
                fleerWeightTuple = GetStatTuple(fleePanelContent, parentName: "Flee Weight", initialValue: defaultFleerWeight); 
                fleerStayInBoundsFutureTimeTuple = GetStatTuple(fleePanelContent, parentName: "Future Time", initialValue: defaultFleerWeight);
                ShowSeekPanel();
                break;
            case SceneChecker.WandererTest:
            case SceneChecker.ObstacleAvoidanceTest:
                contentTransform = canvasGameObject.transform.Find("Panel/Scroll Area/Content");
                wandererMassTuple = GetStatTuple(transform: contentTransform, parentName: "Mass", initialValue: defaultWandererMass);
                wandererMaxForceTuple = GetStatTuple(transform: contentTransform, parentName: "Max Force", initialValue: defaultWandererMaxForce);
                wandererMaxSpeedTuple = GetStatTuple(transform: contentTransform, parentName: "Max Speed", initialValue: defaultWandererMaxSpeed);
                wandererStayInBoundsFutureTimeTuple = GetStatTuple(transform: contentTransform, parentName: "Stay In Bounds Future Time", initialValue: defaultWandererStayInBoundsFutureTime);
                wandererStayInBoundsWeightTuple = GetStatTuple(transform: contentTransform, parentName: "Stay in Bounds Weight", initialValue: defaultWandererStayInBoundsWeight);
                wandererWanderCircleRadiusTuple = GetStatTuple(transform: contentTransform, parentName: "Wander Circle Radius", initialValue: defaultWandererWanderCircleRadius, maxValue: 5);
                wandererWanderOffsetTuple = GetStatTuple(transform: contentTransform, parentName: "Wander Offset", initialValue: defaultWandererWanderOffset, maxValue: 100);
                wandererWanderTimeTuple = GetStatTuple(transform: contentTransform, parentName: "Wander Time", initialValue: defaultWandererWanderTime, intOnly: false);
                wandererWanderWeightTuple = GetStatTuple(transform: contentTransform, parentName: "Wander Weight", initialValue: defaultWandererWanderWeight); 
                wandererWanderFutureTimeTuple = GetStatTuple(transform: contentTransform, parentName: "Wander Future Time", initialValue: defaultWandererWanderFutureTime);

                if(sceneChecker == SceneChecker.ObstacleAvoidanceTest)
                {
                    wandererObstacleAvoidanceWeightTuple = GetStatTuple(transform: contentTransform, parentName: "Obstacle Avoidance Weight", initialValue: defaultWandererObstacleAvoidanceWeight);
                }
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (sceneChecker)
        {
            case SceneChecker.SeekerTest:
                UpdateSeekerStats();
                break;

            case SceneChecker.FleerTest:
                UpdateSeekerStats();
                UpdateFleerStats();
                break;

            case SceneChecker.WandererTest:
            case SceneChecker.ObstacleAvoidanceTest:
                UpdateWandererStats();
                break;
        }
    }
    private (Slider, Text) GetStatTuple(Transform transform, string parentName, float initialValue, float minValue = 1, float maxValue = 10, bool intOnly = true)
    {
        Transform parent = transform.Find(parentName);
        Slider slider = parent.Find("Slider").GetComponent<Slider>();
        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.wholeNumbers = intOnly;
        slider.value = initialValue;
        Text text = parent.Find("Stat").GetComponent<Text>();
        return (slider, text);
    }

    private void UpdateSliderRange(Slider slider, int min, int max, bool intOnly, float initalValue)
    {
        slider.minValue = min;
        slider.maxValue = max;
        slider.wholeNumbers = intOnly;
        slider.value = initalValue;
    }
    private float UpdateStats((Slider, Text) tuple)
    {
        float value = tuple.Item1.value;
        //if ther are decimal places, cut off after the first two
        string strValue = value.ToString().Split(".").Length > 1 ? string.Format("{0:0.##}", value) : value.ToString();
        tuple.Item2.text = strValue;
        return tuple.Item1.value;
    }

    
    private void UpdateSeekerStats()
    {
        gameManagerScript.seekerMass = UpdateStats(seekerMassTuple);
        gameManagerScript.seekerMaxForce = UpdateStats(seekerMaxForceTuple);
        gameManagerScript.seekerMaxSpeed = UpdateStats(seekerMaxSpeedTuple);
        gameManagerScript.seekerStayInBoundsWeight = UpdateStats(seekerStayInBoundTuple);
        gameManagerScript.seekerWeight = UpdateStats(seekerWeightTuple); 
        gameManagerScript.seekerStayInBoundsFutureTime = UpdateStats(seekerStayInBoundsFutureTimeTuple);
    }
    private void UpdateFleerStats()
    {
        gameManagerScript.fleerMass = UpdateStats(fleerMassTuple);
        gameManagerScript.fleerMaxForce = UpdateStats(fleerMaxForceTuple);
        gameManagerScript.fleerMaxSpeed = UpdateStats(fleerMaxSpeedTuple);
        gameManagerScript.fleerStayInBoundsWeight = UpdateStats(fleerStayInBoundTuple);
        gameManagerScript.fleerWeight = UpdateStats(fleerWeightTuple); 
        gameManagerScript.fleerStayInBoundsFutureTime = UpdateStats(fleerStayInBoundsFutureTimeTuple);
    }
    private void UpdateWandererStats()
    {
        gameManagerScript.wandererMass = UpdateStats(wandererMassTuple);
        gameManagerScript.wandererMaxForce = UpdateStats(wandererMaxForceTuple);
        gameManagerScript.wandererMaxSpeed = UpdateStats(wandererMaxSpeedTuple);
        gameManagerScript.wandererStayInBoundsWeight = UpdateStats(wandererStayInBoundsWeightTuple);
        gameManagerScript.wandererWanderCircleRadius = UpdateStats(wandererWanderCircleRadiusTuple);
        gameManagerScript.wandererWanderFutureTime = UpdateStats(wandererWanderFutureTimeTuple);
        gameManagerScript.wandererWanderOffset = UpdateStats(wandererWanderOffsetTuple);
        gameManagerScript.wandererWanderTime = UpdateStats(wandererWanderTimeTuple);
        gameManagerScript.wandererWanderWeight = UpdateStats(wandererWanderWeightTuple); 
        gameManagerScript.wandererStayInBoundsFutureTime = UpdateStats(wandererStayInBoundsFutureTimeTuple);

        if(sceneChecker == SceneChecker.ObstacleAvoidanceTest)
        {
            gameManagerScript.wandererObstacleAvoidanceWeight = UpdateStats(wandererObstacleAvoidanceWeightTuple);
        }
    }
    public void ShowSeekPanel()
    {
        HideAllPanels();
        seekPanel.SetActive(true);
        seekerButton.interactable = false;
    }
    public void ShowFleePanel()
    {
        HideAllPanels();
        fleePanel.SetActive(true);
        fleerButton.interactable = false;
    }
    private void HideAllPanels()
    {
        foreach ((GameObject, Button) pb in panelsAndButtons)
        { 
            pb.Item1.SetActive(false);
            pb.Item2.interactable = true;
        }
    }
}
