using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //todo: set the enabled colors of the tuples during initalization
    //todo: make it so forces don't apply if the force check box is disabled
    [SerializeField]
    private Color enabledTupleColor, disabledTupleColor;
    [SerializeField]
    private GameObject canvasGameObject;
    private (Slider, Text) seekerStayInBoundsFutureTimeTuple, seekerMassTuple, seekerMaxForceTuple, seekerMaxSpeedTuple, seekerWeightTuple, seekerStayInBoundTuple;
    private (Slider, Text) fleerStayInBoundsFutureTimeTuple, fleerMassTuple, fleerMaxForceTuple, fleerMaxSpeedTuple, fleerWeightTuple, fleerStayInBoundTuple;
    private (Slider, Text) wandererAvoidTimeTuple, wandererStayInBoundsFutureTimeTuple, wandererWanderFutureTimeTuple, wandererMassTuple, wandererMaxForceTuple, wandererMaxSpeedTuple, wandererObstacleAvoidanceWeightTuple, wandererStayInBoundsWeightTuple, wandererWanderWeightTuple, wandererWanderCircleRadiusTuple, wandererWanderOffsetTuple, wandererWanderTimeTuple;
    private (Slider, Text) flockerAlignDistanceTuple, flockerAlignWeightTuple, flockerStayInBoundsFutureTimeTuple, flockerMassTuple, flockerMaxForceTuple, flockerMaxSpeedTuple, flockerSeparateDistanceTuple, flockerSeparateWeightTuple, flockerStayInBoundsWeightTuple;
    private (Toggle, Text) flockerAlignForceTuple, flockerSeparateForceTuple, flockerStayInBoundsForceTuple;
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
    [SerializeField, Range(0f, 3f)]
    private float defaultWandererAvoidTime;
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

    #region Flocker Values
    [Header("Flocker Values")]
    [SerializeField]
    private bool defaultFlockerAlignForce;
    [SerializeField]
    private bool defaultFlockerSeparateForce;
    [SerializeField]
    private bool defaultFlockerStayInBoundsForce;

    [SerializeField, Range(1f, 5f)]
    private float defaultFlockerAlignDistance;
    [SerializeField, Range(1, 10)]
    private int defaultFlockerAlignWeight;
    [SerializeField]
    private int flockerCount;
    [SerializeField, Range(1, 10)]
    private int defaultFlockerStayInBoundsWeight;
    [SerializeField, Range(1, 10)]
    private int defaultFlockerStayInBoundsFutureTime;
    [SerializeField, Range(1, 10)]
    private int defaultFlockerMass;
    [SerializeField, Range(1, 10)]
    private int defaultFlockerMaxForce;
    [SerializeField, Range(1, 10)]
    private int defaultFlockerMaxSpeed;
    [SerializeField, Range(1f, 5f)]
    private float defaultFlockerSeparateDistance;
    [SerializeField, Range(1, 10)]
    private int defaultFlockerSeparateWeight;
    public int FlockerCount { get { return flockerCount; } }
    #endregion

    private GameManager gameManager;
    private Button seekerButton, fleerButton;
    private GameObject seekPanel, fleePanel;

    private List<(GameObject, Button)> panelsAndButtons;
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        Transform contentTransform = null;
        switch (gameManager.SceneCheckerProperty)
        {
            case GameManager.SceneChecker.SeekerTest:
                contentTransform = canvasGameObject.transform.Find("Panel/Scroll Area/Content");
                seekerStayInBoundsFutureTimeTuple = GetStatTuple(contentTransform, parentName: "Future Time", initialValue: defaultSeekerFutureTime);
                seekerMassTuple = GetStatTuple(contentTransform, parentName: "Mass", initialValue: defaultSeekerMass);
                seekerMaxForceTuple = GetStatTuple(contentTransform, parentName: "Max Force", initialValue: defaultSeekerMaxForce);
                seekerMaxSpeedTuple = GetStatTuple(contentTransform, parentName: "Max Speed", initialValue: defaultSeekerMaxSpeed);
                seekerWeightTuple = GetStatTuple(contentTransform, parentName: "Seeker Weight", initialValue: defaultSeekerWeight);
                seekerStayInBoundTuple = GetStatTuple(contentTransform, parentName: "Stay in Bounds Weight", initialValue: defaultSeekerStayInBound);
                break;
            case GameManager.SceneChecker.FleerTest:
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
            case GameManager.SceneChecker.WandererTest:
            case GameManager.SceneChecker.ObstacleAvoidanceTest:
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

                if(gameManager.SceneCheckerProperty == GameManager.SceneChecker.ObstacleAvoidanceTest)
                {
                    wandererAvoidTimeTuple = GetStatTuple(transform: contentTransform, parentName: "Avoid Time", initialValue: defaultWandererAvoidTime, minValue: 0f, maxValue: 3f, intOnly: false);
                    wandererObstacleAvoidanceWeightTuple = GetStatTuple(transform: contentTransform, parentName: "Obstacle Avoidance Weight", initialValue: defaultWandererObstacleAvoidanceWeight);
                }
                break;
            case GameManager.SceneChecker.FlockingTest:
                contentTransform = canvasGameObject.transform.Find("Panel/Scroll Area/Content");
                flockerAlignDistanceTuple = GetStatTuple(transform: contentTransform, parentName: "Align Distance", initialValue: defaultFlockerAlignDistance, minValue: 1, maxValue: 5, intOnly: false);
                flockerAlignWeightTuple = GetStatTuple(transform: contentTransform, parentName: "Align Weight", initialValue: defaultFlockerAlignWeight);
                flockerMassTuple = GetStatTuple(transform: contentTransform, parentName: "Mass", initialValue: defaultFlockerMass);
                flockerMaxForceTuple = GetStatTuple(transform: contentTransform, parentName: "Max Force", initialValue: defaultFlockerMaxForce);
                flockerMaxSpeedTuple = GetStatTuple(transform: contentTransform, parentName: "Max Speed", initialValue: defaultFlockerMaxSpeed);
                flockerStayInBoundsFutureTimeTuple = GetStatTuple(transform: contentTransform, parentName: "Stay In Bounds Future Time", initialValue: defaultFlockerStayInBoundsFutureTime);
                flockerStayInBoundsWeightTuple = GetStatTuple(transform: contentTransform, parentName: "Stay in Bounds Weight", initialValue: defaultFlockerStayInBoundsWeight);
                flockerSeparateDistanceTuple = GetStatTuple(transform: contentTransform, parentName: "Separate Distance", initialValue: defaultFlockerSeparateDistance, minValue: 1, maxValue: 5, intOnly: false);
                flockerSeparateWeightTuple = GetStatTuple(transform: contentTransform, parentName: "Separate Weight", initialValue: defaultFlockerSeparateWeight);
                flockerAlignForceTuple = GetCheckBoxTuple(transform: contentTransform, parentName: "Align Force", initialValue: defaultFlockerAlignForce);
                flockerSeparateForceTuple = GetCheckBoxTuple(transform: contentTransform, parentName: "Separate Force", initialValue: defaultFlockerSeparateForce);
                flockerStayInBoundsForceTuple = GetCheckBoxTuple(transform: contentTransform, parentName: "Stay in Bounds Force", initialValue: defaultFlockerStayInBoundsForce);

                gameManager.flockerAlignForce = defaultFlockerAlignForce;
                gameManager.flockerSeparateForce = defaultFlockerSeparateForce;
                gameManager.flockerStayInBoundsForce = defaultFlockerStayInBoundsForce;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameManager.SceneCheckerProperty)
        {
            case GameManager.SceneChecker.SeekerTest:
                UpdateSeekerStats();
                break;

            case GameManager.SceneChecker.FleerTest:
                UpdateSeekerStats();
                UpdateFleerStats();
                break;

            case GameManager.SceneChecker.WandererTest:
            case GameManager.SceneChecker.ObstacleAvoidanceTest:
                UpdateWandererStats();
                break;

            case GameManager.SceneChecker.FlockingTest:
                UpdateFlockerStats();
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

    private (Toggle, Text) GetCheckBoxTuple(Transform transform, string parentName, bool initialValue)
    {
        Transform parent = transform.Find(parentName);
        Toggle checkBox = parent.Find("Toggle").GetComponent<Toggle>();
        checkBox.isOn = initialValue;
        Text text = parent.Find("Name").GetComponent<Text>();
        return (checkBox, text);
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
        gameManager.seekerMass = UpdateStats(seekerMassTuple);
        gameManager.seekerMaxForce = UpdateStats(seekerMaxForceTuple);
        gameManager.seekerMaxSpeed = UpdateStats(seekerMaxSpeedTuple);
        gameManager.seekerStayInBoundsWeight = UpdateStats(seekerStayInBoundTuple);
        gameManager.seekerWeight = UpdateStats(seekerWeightTuple); 
        gameManager.seekerStayInBoundsFutureTime = UpdateStats(seekerStayInBoundsFutureTimeTuple);
    }
    private void UpdateFleerStats()
    {
        gameManager.fleerMass = UpdateStats(fleerMassTuple);
        gameManager.fleerMaxForce = UpdateStats(fleerMaxForceTuple);
        gameManager.fleerMaxSpeed = UpdateStats(fleerMaxSpeedTuple);
        gameManager.fleerStayInBoundsWeight = UpdateStats(fleerStayInBoundTuple);
        gameManager.fleerWeight = UpdateStats(fleerWeightTuple); 
        gameManager.fleerStayInBoundsFutureTime = UpdateStats(fleerStayInBoundsFutureTimeTuple);
    }
    private void UpdateWandererStats()
    {
        gameManager.wandererMass = UpdateStats(wandererMassTuple);
        gameManager.wandererMaxForce = UpdateStats(wandererMaxForceTuple);
        gameManager.wandererMaxSpeed = UpdateStats(wandererMaxSpeedTuple);
        gameManager.wandererStayInBoundsWeight = UpdateStats(wandererStayInBoundsWeightTuple);
        gameManager.wandererWanderCircleRadius = UpdateStats(wandererWanderCircleRadiusTuple);
        gameManager.wandererWanderFutureTime = UpdateStats(wandererWanderFutureTimeTuple);
        gameManager.wandererWanderOffset = UpdateStats(wandererWanderOffsetTuple);
        gameManager.wandererWanderTime = UpdateStats(wandererWanderTimeTuple);
        gameManager.wandererWanderWeight = UpdateStats(wandererWanderWeightTuple); 
        gameManager.wandererStayInBoundsFutureTime = UpdateStats(wandererStayInBoundsFutureTimeTuple);

        if(gameManager.SceneCheckerProperty == GameManager.SceneChecker.ObstacleAvoidanceTest)
        {
            gameManager.wandererAvoidTime = UpdateStats(wandererAvoidTimeTuple);
            gameManager.wandererObstacleAvoidanceWeight = UpdateStats(wandererObstacleAvoidanceWeightTuple);
        }
    }
    private void UpdateFlockerStats()
    {
        gameManager.flockerAlignDistance = UpdateStats(flockerAlignDistanceTuple);
        gameManager.flockerAlignWeight = UpdateStats(flockerAlignWeightTuple);
        gameManager.flockerMass = UpdateStats(flockerMassTuple);
        gameManager.flockerMaxForce = UpdateStats(flockerMaxForceTuple);
        gameManager.flockerMaxSpeed = UpdateStats(flockerMaxSpeedTuple);
        gameManager.flockerStayInBoundsWeight = UpdateStats(flockerStayInBoundsWeightTuple);
        gameManager.flockerStayInBoundsFutureTime = UpdateStats(flockerStayInBoundsFutureTimeTuple);
        gameManager.flockerSeparateDistance = UpdateStats(flockerSeparateDistanceTuple);
        gameManager.flockerSeparateWeight = UpdateStats(flockerSeparateWeightTuple);
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

    #region CheckBox Methods
    public void ToggleFlockerAlignForce(bool value)
    {
        gameManager.flockerAlignForce = value;
        ToggleStatInteractability(flockerAlignDistanceTuple, value);
        ToggleStatInteractability(flockerAlignWeightTuple, value);
    }

    public void ToggleFlockerSeparateForce(bool value)
    {
        gameManager.flockerSeparateForce = value;
        ToggleStatInteractability(flockerSeparateDistanceTuple, value);
        ToggleStatInteractability(flockerSeparateWeightTuple, value);
    }

    public void ToggleFlockerStayInBounds(bool value)
    {
        gameManager.flockerStayInBoundsForce = value;
        ToggleStatInteractability(flockerStayInBoundsFutureTimeTuple, value);
        ToggleStatInteractability(flockerStayInBoundsWeightTuple, value);
    }

    private void ToggleStatInteractability((Slider, Text) tuple, bool value)
    {
        Color color = value ? enabledTupleColor : disabledTupleColor;
        //toggle the slider usability
        tuple.Item1.interactable = value;

        //change the color of the text
        tuple.Item2.color = color;
        tuple.Item1.transform.Find("../Name").GetComponent<Text>().color = color;
    }
    #endregion
}
