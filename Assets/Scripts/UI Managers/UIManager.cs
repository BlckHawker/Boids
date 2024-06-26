using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIManager : MonoBehaviour
{
    [SerializeField]
    protected Color enabledTupleColor, disabledTupleColor;
    [SerializeField]
    protected GameObject canvasGameObject;
    
    
    private (Slider, Text) wandererAvoidTimeTuple, wandererStayInBoundsFutureTimeTuple, wandererWanderFutureTimeTuple, wandererMassTuple, wandererMaxForceTuple, wandererMaxSpeedTuple, wandererObstacleAvoidanceWeightTuple, wandererStayInBoundsWeightTuple, wandererWanderWeightTuple, wandererWanderCircleRadiusTuple, wandererWanderOffsetTuple, wandererWanderTimeTuple;
    private (Slider, Text) flockerAlignDistanceTuple, flockerAlignWeightTuple, flockerStayInBoundsFutureTimeTuple, flockerMassTuple, flockerMaxForceTuple, flockerMaxSpeedTuple, flockerSeparateDistanceTuple, flockerSeparateWeightTuple, flockerStayInBoundsWeightTuple;
    private (Toggle, Text) flockerAlignForceTuple, flockerSeparateForceTuple, flockerStayInBoundsForceTuple;
    #region Seeker Values
   
    #endregion

    #region Fleer Values
    
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

    protected List<(GameObject, Button)> panelsAndButtons;
    void Start()
    {
        GetGameManager();
        GetDefaultValues();

        //switch (gameManager.SceneCheckerProperty)
        //{
        //    case GameManager.SceneChecker.SeekerTest:
                
        //        break;
            //case GameManager.SceneChecker.FleerTest:

            //    break;
            //case GameManager.SceneChecker.WandererTest:
            //case GameManager.SceneChecker.ObstacleAvoidanceTest:
            //    contentTransform = canvasGameObject.transform.Find("Panel/Scroll Area/Content");
            //    wandererMassTuple = GetStatTuple(transform: contentTransform, parentName: "Mass", initialValue: defaultWandererMass);
            //    wandererMaxForceTuple = GetStatTuple(transform: contentTransform, parentName: "Max Force", initialValue: defaultWandererMaxForce);
            //    wandererMaxSpeedTuple = GetStatTuple(transform: contentTransform, parentName: "Max Speed", initialValue: defaultWandererMaxSpeed);
            //    wandererStayInBoundsFutureTimeTuple = GetStatTuple(transform: contentTransform, parentName: "Stay In Bounds Future Time", initialValue: defaultWandererStayInBoundsFutureTime);
            //    wandererStayInBoundsWeightTuple = GetStatTuple(transform: contentTransform, parentName: "Stay in Bounds Weight", initialValue: defaultWandererStayInBoundsWeight);
            //    wandererWanderCircleRadiusTuple = GetStatTuple(transform: contentTransform, parentName: "Wander Circle Radius", initialValue: defaultWandererWanderCircleRadius, maxValue: 5);
            //    wandererWanderOffsetTuple = GetStatTuple(transform: contentTransform, parentName: "Wander Offset", initialValue: defaultWandererWanderOffset, maxValue: 100);
            //    wandererWanderTimeTuple = GetStatTuple(transform: contentTransform, parentName: "Wander Time", initialValue: defaultWandererWanderTime, intOnly: false);
            //    wandererWanderWeightTuple = GetStatTuple(transform: contentTransform, parentName: "Wander Weight", initialValue: defaultWandererWanderWeight); 
            //    wandererWanderFutureTimeTuple = GetStatTuple(transform: contentTransform, parentName: "Wander Future Time", initialValue: defaultWandererWanderFutureTime);

            //    if(gameManager.SceneCheckerProperty == GameManager.SceneChecker.ObstacleAvoidanceTest)
            //    {
            //        wandererAvoidTimeTuple = GetStatTuple(transform: contentTransform, parentName: "Avoid Time", initialValue: defaultWandererAvoidTime, minValue: 0f, maxValue: 3f, intOnly: false);
            //        wandererObstacleAvoidanceWeightTuple = GetStatTuple(transform: contentTransform, parentName: "Obstacle Avoidance Weight", initialValue: defaultWandererObstacleAvoidanceWeight);
            //    }
            //    break;
            //case GameManager.SceneChecker.FlockingTest:
            //    contentTransform = canvasGameObject.transform.Find("Panel/Scroll Area/Content");
            //    flockerAlignDistanceTuple = GetStatTuple(transform: contentTransform, parentName: "Align Distance", initialValue: defaultFlockerAlignDistance, minValue: 1, maxValue: 5, intOnly: false);
            //    flockerAlignWeightTuple = GetStatTuple(transform: contentTransform, parentName: "Align Weight", initialValue: defaultFlockerAlignWeight);
            //    flockerMassTuple = GetStatTuple(transform: contentTransform, parentName: "Mass", initialValue: defaultFlockerMass);
            //    flockerMaxForceTuple = GetStatTuple(transform: contentTransform, parentName: "Max Force", initialValue: defaultFlockerMaxForce);
            //    flockerMaxSpeedTuple = GetStatTuple(transform: contentTransform, parentName: "Max Speed", initialValue: defaultFlockerMaxSpeed);
            //    flockerStayInBoundsFutureTimeTuple = GetStatTuple(transform: contentTransform, parentName: "Stay In Bounds Future Time", initialValue: defaultFlockerStayInBoundsFutureTime);
            //    flockerStayInBoundsWeightTuple = GetStatTuple(transform: contentTransform, parentName: "Stay in Bounds Weight", initialValue: defaultFlockerStayInBoundsWeight);
            //    flockerSeparateDistanceTuple = GetStatTuple(transform: contentTransform, parentName: "Separate Distance", initialValue: defaultFlockerSeparateDistance, minValue: 1, maxValue: 5, intOnly: false);
            //    flockerSeparateWeightTuple = GetStatTuple(transform: contentTransform, parentName: "Separate Weight", initialValue: defaultFlockerSeparateWeight);
            //    flockerAlignForceTuple = GetCheckBoxTuple(transform: contentTransform, parentName: "Align Force", initialValue: defaultFlockerAlignForce);
            //    flockerSeparateForceTuple = GetCheckBoxTuple(transform: contentTransform, parentName: "Separate Force", initialValue: defaultFlockerSeparateForce);
            //    flockerStayInBoundsForceTuple = GetCheckBoxTuple(transform: contentTransform, parentName: "Stay in Bounds Force", initialValue: defaultFlockerStayInBoundsForce);

            //    gameManager.flockerAlignForce = defaultFlockerAlignForce;
            //    gameManager.flockerSeparateForce = defaultFlockerSeparateForce;
            //    gameManager.flockerStayInBoundsForce = defaultFlockerStayInBoundsForce;
            //    break;
        //}
    }

    void Update()
    {
        UpdateAgentStats();
        //switch (gameManager.SceneCheckerProperty)
        //{
        //    case GameManager.SceneChecker.SeekerTest:
        //        UpdateSeekerStats();
        //        break;

        //    case GameManager.SceneChecker.FleerTest:
        //        UpdateSeekerStats();
        //        UpdateFleerStats();
        //        break;

        //    case GameManager.SceneChecker.WandererTest:
        //    case GameManager.SceneChecker.ObstacleAvoidanceTest:
        //        UpdateWandererStats();
        //        break;

        //    case GameManager.SceneChecker.FlockingTest:
        //        UpdateFlockerStats();
        //        break;
        //}
    }

    protected abstract void GetDefaultValues();
    protected abstract void UpdateAgentStats();

    protected (Slider, Text) GetStatTuple(Transform transform, string parentName, float initialValue, float minValue = 1, float maxValue = 10, bool intOnly = true)
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

    protected void GetCheckBoxTuple(Transform transform, string parentName, bool initialValue)
    {
        Transform parent = transform.Find(parentName);
        Toggle checkBox = parent.Find("Toggle").GetComponent<Toggle>();
        checkBox.isOn = initialValue;
    }
    protected float UpdateStats((Slider, Text) tuple)
    {
        float value = tuple.Item1.value;
        //if ther are decimal places, cut off after the first two
        string strValue = value.ToString().Split(".").Length > 1 ? string.Format("{0:0.##}", value) : value.ToString();
        tuple.Item2.text = strValue;
        return tuple.Item1.value;
    }

    private void UpdateSeekerStats()
    {
        
    }

    protected abstract void GetGameManager();
    //private void UpdateFleerStats()
    //{
    //    gameManager.fleerMass = UpdateStats(fleerMassTuple);
    //    gameManager.fleerMaxForce = UpdateStats(fleerMaxForceTuple);
    //    gameManager.fleerMaxSpeed = UpdateStats(fleerMaxSpeedTuple);
    //    gameManager.fleerStayInBoundsWeight = UpdateStats(fleerStayInBoundTuple);
    //    gameManager.fleerWeight = UpdateStats(fleerWeightTuple); 
    //    gameManager.fleerStayInBoundsFutureTime = UpdateStats(fleerStayInBoundsFutureTimeTuple);
    //}
    //private void UpdateWandererStats()
    //{
    //    gameManager.wandererMass = UpdateStats(wandererMassTuple);
    //    gameManager.wandererMaxForce = UpdateStats(wandererMaxForceTuple);
    //    gameManager.wandererMaxSpeed = UpdateStats(wandererMaxSpeedTuple);
    //    gameManager.wandererStayInBoundsWeight = UpdateStats(wandererStayInBoundsWeightTuple);
    //    gameManager.wandererWanderCircleRadius = UpdateStats(wandererWanderCircleRadiusTuple);
    //    gameManager.wandererWanderFutureTime = UpdateStats(wandererWanderFutureTimeTuple);
    //    gameManager.wandererWanderOffset = UpdateStats(wandererWanderOffsetTuple);
    //    gameManager.wandererWanderTime = UpdateStats(wandererWanderTimeTuple);
    //    gameManager.wandererWanderWeight = UpdateStats(wandererWanderWeightTuple); 
    //    gameManager.wandererStayInBoundsFutureTime = UpdateStats(wandererStayInBoundsFutureTimeTuple);

    //    if(gameManager.SceneCheckerProperty == GameManager.SceneChecker.ObstacleAvoidanceTest)
    //    {
    //        gameManager.wandererAvoidTime = UpdateStats(wandererAvoidTimeTuple);
    //        gameManager.wandererObstacleAvoidanceWeight = UpdateStats(wandererObstacleAvoidanceWeightTuple);
    //    }
    //}
    //private void UpdateFlockerStats()
    //{
    //    gameManager.flockerAlignDistance = UpdateStats(flockerAlignDistanceTuple);
    //    gameManager.flockerAlignWeight = UpdateStats(flockerAlignWeightTuple);
    //    gameManager.flockerMass = UpdateStats(flockerMassTuple);
    //    gameManager.flockerMaxForce = UpdateStats(flockerMaxForceTuple);
    //    gameManager.flockerMaxSpeed = UpdateStats(flockerMaxSpeedTuple);
    //    gameManager.flockerStayInBoundsWeight = UpdateStats(flockerStayInBoundsWeightTuple);
    //    gameManager.flockerStayInBoundsFutureTime = UpdateStats(flockerStayInBoundsFutureTimeTuple);
    //    gameManager.flockerSeparateDistance = UpdateStats(flockerSeparateDistanceTuple);
    //    gameManager.flockerSeparateWeight = UpdateStats(flockerSeparateWeightTuple);
    //}

    protected void HideAllPanels()
    {
        foreach ((GameObject, Button) pb in panelsAndButtons)
        {
            pb.Item1.SetActive(false);
            pb.Item2.interactable = true;
        }
    }

    #region CheckBox Methods
    #region Seeker Methods



    #endregion
    #region Flocker Methods
    //public void ToggleFlockerAlignForce(bool value)
    //{
    //    gameManager.flockerAlignForce = value;
    //    ToggleStatInteractability(flockerAlignDistanceTuple, value);
    //    ToggleStatInteractability(flockerAlignWeightTuple, value);
    //}

    //public void ToggleFlockerSeparateForce(bool value)
    //{
    //    gameManager.flockerSeparateForce = value;
    //    ToggleStatInteractability(flockerSeparateDistanceTuple, value);
    //    ToggleStatInteractability(flockerSeparateWeightTuple, value);
    //}

    //public void ToggleFlockerStayInBounds(bool value)
    //{
    //    gameManager.flockerStayInBoundsForce = value;
    //    ToggleStatInteractability(flockerStayInBoundsFutureTimeTuple, value);
    //    ToggleStatInteractability(flockerStayInBoundsWeightTuple, value);
    //}
    #endregion

    protected void ToggleStatInteractability((Slider, Text) tuple, bool value)
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
