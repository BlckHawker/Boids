using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    private enum SceneChecker
    {
        SeekerTest,
        FleerTest,
        WandererTest,
    }

    [SerializeField]
    private SceneChecker sceneChecker;
    [SerializeField]
    private GameObject canvasGameObject;
    private (Slider, Text) seekerFutureTimeTuple, seekerMassTuple, seekerMaxForceTuple, seekerMaxSpeedTuple, seekerWeightTuple, seekerStayInBoundTuple;
    private (Slider, Text) fleerFutureTimeTuple, fleerMassTuple, fleerMaxForceTuple, fleerMaxSpeedTuple, fleerWeightTuple, fleerStayInBoundTuple;
    private (Slider, Text) wandererFutureTimeTuple, wandererMassTuple, wandererMaxForceTuple, wandererMaxSpeedTuple, wandererStayInBoundsWeightTuple, wandererWanderWeightTuple, wandererWanderCircleRadiusTuple, wandererWanderOffsetTuple, wandererWanderTimeTuple;

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
    private int defaultWandererFutureTime;
    [SerializeField, Range(1, 10)]
    private int defaultWandererMass;
    [SerializeField, Range(1, 10)]
    private int defaultWandererMaxForce;
    [SerializeField, Range(1, 10)]
    private int defaultWandererMaxSpeed;
    [SerializeField, Range(1, 10)]
    private int defaultWandererStayInBoundsWeight;
    [SerializeField, Range(1, 10)]
    private int defaultWandererWanderWeight;
    [SerializeField, Range(1, 20)]
    private int defaultWandererWanderCircleRadius;
    [SerializeField, Range(1, 100)]
    private int defaultWandererWanderOffset;
    [SerializeField, Range(1, 10)]
    private int defaultWandererWanderTime;
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
                contentTransform = canvasGameObject.transform.Find("Panel").Find("Scroll Area").Find("Content");
                seekerFutureTimeTuple = GetStatTuple(contentTransform, "Future Time", defaultSeekerFutureTime);
                seekerMassTuple = GetStatTuple(contentTransform, "Mass", defaultSeekerMass);
                seekerMaxForceTuple = GetStatTuple(contentTransform, "Max Force", defaultSeekerMaxForce);
                seekerMaxSpeedTuple = GetStatTuple(contentTransform, "Max Speed", defaultSeekerMaxSpeed);
                seekerWeightTuple = GetStatTuple(contentTransform, "Seeker Weight", defaultSeekerWeight);
                seekerStayInBoundTuple = GetStatTuple(contentTransform, "Stay in Bounds Weight", defaultSeekerStayInBound);
                break;

            case SceneChecker.FleerTest:
                Transform statPanel = canvasGameObject.transform.Find("Stat Panel");
                fleePanel = statPanel.Find("Flee Panel").gameObject;
                seekPanel = statPanel.Find("Seek Panel").gameObject;

                Transform fleePanelContent = fleePanel.transform.Find("Scroll Area").Find("Content");
                Transform seekPanelContent = seekPanel.transform.Find("Scroll Area").Find("Content");

                seekerButton = statPanel.Find("Seeker Button").GetComponent<Button>();
                fleerButton = statPanel.Find("Flee Button").GetComponent<Button>();
                panelsAndButtons = new List<(GameObject, Button)>() { (seekPanel.gameObject, seekerButton), (fleePanel.gameObject, fleerButton) };

                seekerFutureTimeTuple = GetStatTuple(seekPanelContent, "Future Time", defaultSeekerFutureTime);
                seekerMassTuple = GetStatTuple(seekPanelContent, "Mass", defaultSeekerMass);
                seekerMaxForceTuple = GetStatTuple(seekPanelContent, "Max Force", defaultSeekerMaxForce);
                seekerMaxSpeedTuple = GetStatTuple(seekPanelContent, "Max Speed", defaultSeekerMaxSpeed);
                seekerWeightTuple = GetStatTuple(seekPanelContent, "Seeker Weight", defaultSeekerWeight);
                seekerStayInBoundTuple = GetStatTuple(seekPanelContent, "Stay in Bounds Weight", defaultSeekerStayInBound);

                fleerFutureTimeTuple = GetStatTuple(fleePanelContent, "Future Time", defaultFleerWeight);
                fleerMassTuple = GetStatTuple(fleePanelContent, "Mass", defaultFleerMass);
                fleerMaxForceTuple = GetStatTuple(fleePanelContent, "Max Force", defaultFleerMaxForce);
                fleerMaxSpeedTuple = GetStatTuple(fleePanelContent, "Max Speed", defaultFleerMaxSpeed);
                fleerWeightTuple = GetStatTuple(fleePanelContent, "Flee Weight", defaultFleerWeight);
                fleerStayInBoundTuple = GetStatTuple(fleePanelContent, "Stay in Bounds Weight", defaultFleerStayInBound);
                ShowSeekPanel();
                break;
            case SceneChecker.WandererTest:
                contentTransform = canvasGameObject.transform.Find("Panel").Find("Scroll Area").Find("Content");

                wandererFutureTimeTuple = GetStatTuple(contentTransform, "Future Time", defaultWandererFutureTime);
                wandererMassTuple = GetStatTuple(contentTransform, "Mass", defaultWandererMass);
                wandererMaxForceTuple = GetStatTuple(contentTransform, "Max Force", defaultWandererMaxForce);
                wandererMaxSpeedTuple = GetStatTuple(contentTransform, "Max Speed", defaultWandererMaxSpeed);
                wandererStayInBoundsWeightTuple = GetStatTuple(contentTransform, "Stay in Bounds Weight", defaultWandererStayInBoundsWeight);
                wandererWanderWeightTuple = GetStatTuple(contentTransform, "Wander Weight", defaultWandererWanderWeight);
                wandererWanderCircleRadiusTuple = GetStatTuple(contentTransform, "Wander Circle Radius", defaultWandererWanderCircleRadius);
                wandererWanderOffsetTuple = GetStatTuple(contentTransform, "Wander Offset", defaultWandererWanderOffset);
                wandererWanderTimeTuple = GetStatTuple(contentTransform, "Wander Time", defaultWandererWanderTime);

                //manually setting the ranges for certain sliders
                UpdateSliderRange(wandererWanderCircleRadiusTuple.Item1, 1, 5);
                UpdateSliderRange(wandererWanderOffsetTuple.Item1, 1, 100);


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
                UpdateWandererStats();
                break;
        }
    }
    private (Slider, Text) GetStatTuple(Transform transform, string parentName, float initialValue)
    {
        Transform parent = transform.Find(parentName);
        Slider slider = parent.Find("Slider").GetComponent<Slider>();
        slider.minValue = 1;
        slider.maxValue = 10;
        slider.wholeNumbers = true;
        slider.value = initialValue;
        Text text = parent.Find("Stat").GetComponent<Text>();
        return (slider, text);
    }

    private void UpdateSliderRange(Slider slider, int min, int max)
    {
        slider.minValue = min;
        slider.maxValue = max;
    }
    private float UpdateStats((Slider, Text) tuple)
    {
        tuple.Item2.text = tuple.Item1.value.ToString();
        return tuple.Item1.value;
    }

    
    private void UpdateSeekerStats()
    {
        gameManagerScript.seekerFutureTime = UpdateStats(seekerFutureTimeTuple);
        gameManagerScript.seekerMass = UpdateStats(seekerMassTuple);
        gameManagerScript.seekerMaxForce = UpdateStats(seekerMaxForceTuple);
        gameManagerScript.seekerMaxSpeed = UpdateStats(seekerMaxSpeedTuple);
        gameManagerScript.seekerWeight = UpdateStats(seekerWeightTuple);
        gameManagerScript.seekerStayInBoundsWeight = UpdateStats(seekerStayInBoundTuple);
    }
    private void UpdateFleerStats()
    {
        gameManagerScript.fleerFutureTime = UpdateStats(fleerFutureTimeTuple);
        gameManagerScript.fleerMass = UpdateStats(fleerMassTuple);
        gameManagerScript.fleerMaxForce = UpdateStats(fleerMaxForceTuple);
        gameManagerScript.fleerMaxSpeed = UpdateStats(fleerMaxSpeedTuple);
        gameManagerScript.fleerWeight = UpdateStats(fleerWeightTuple);
        gameManagerScript.fleerStayInBoundsWeight = UpdateStats(fleerStayInBoundTuple);
    }
    private void UpdateWandererStats()
    {
        gameManagerScript.wandererFutureTime = UpdateStats(wandererFutureTimeTuple);
        gameManagerScript.wandererMass = UpdateStats(wandererMassTuple);
        gameManagerScript.wandererMaxForce = UpdateStats(wandererMaxForceTuple);
        gameManagerScript.wandererMaxSpeed = UpdateStats(wandererMaxSpeedTuple);
        gameManagerScript.wandererStayInBoundsWeight = UpdateStats(wandererStayInBoundsWeightTuple);
        gameManagerScript.wandererWanderWeight = UpdateStats(wandererWanderWeightTuple);
        gameManagerScript.wandererWanderCircleRadius = UpdateStats(wandererWanderCircleRadiusTuple);
        gameManagerScript.wandererWanderOffset = UpdateStats(wandererWanderOffsetTuple);
        gameManagerScript.wandererWanderTime = UpdateStats(wandererWanderTimeTuple);
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
