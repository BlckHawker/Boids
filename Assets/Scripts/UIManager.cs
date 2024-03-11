using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    private enum SceneChecker
    {
        SeekerTest,
        FleerTest
    }

    [SerializeField]
    private SceneChecker sceneChecker;
    [SerializeField]
    private GameObject canvasGameObject;
    private (Slider, TextMeshProUGUI) seekerFutureTimeTuple, seekerMassTuple, seekerMaxForceTuple, seekerMaxSpeedTuple, seekerWeightTuple, seekerStayInBoundTuple;
    private (Slider, TextMeshProUGUI) fleerFutureTimeTuple, fleerMassTuple, fleerMaxForceTuple, fleerMaxSpeedTuple, fleerWeightTuple, fleerStayInBoundTuple;


    #region Seeker Values
    [Header("Seeker Values")]
    [SerializeField, Range(1, 10)]
    private int defaultSeekerFutureTime;

    [SerializeField, Range(1, 10)]
    private int defaultSeekerMass;

    [SerializeField, Range(1, 10)]
    private int defaultSeekerMaxForce;

    [SerializeField, Range(1, 10)] 
    private int defaultSeekerMaxSpeed;

    [SerializeField, Range(1, 10)] 
    private int defaultSeekerWeight;

    [SerializeField, Range(1, 10)] 
    private int defaultSeekerStayInBound;
    #endregion

    #region Fleer Values
    [Header("Fleer Values")]
    [SerializeField, Range(1, 10)]
    private int defaultFleerFutureTime;

    [SerializeField, Range(1, 10)]
    private int defaultFleerMass;

    [SerializeField, Range(1, 10)]
    private int defaultFleerMaxForce;

    [SerializeField, Range(1, 10)]
    private int defaultFleerMaxSpeed;

    [SerializeField, Range(1, 10)]
    private int defaultFleerWeight;

    [SerializeField, Range(1, 10)]
    private int defaultFleerStayInBound;
    #endregion

    private GameManager gameManagerScript;
    private Button seekerButton, fleerButton;
    private GameObject seekPanel, fleePanel;

    private List<(GameObject, Button)> panelsAndButtons;
    void Start()
    {
        gameManagerScript = GetComponent<GameManager>();
        switch (sceneChecker)
        {
            case SceneChecker.SeekerTest:
                Transform panelTransform = canvasGameObject.transform.Find("Panel");
                seekerFutureTimeTuple = GetStatTuple(panelTransform, "Future Time", defaultSeekerFutureTime);
                seekerMassTuple = GetStatTuple(panelTransform, "Mass", defaultSeekerMass);
                seekerMaxForceTuple = GetStatTuple(panelTransform, "Max Force", defaultSeekerMaxForce);
                seekerMaxSpeedTuple = GetStatTuple(panelTransform, "Max Speed", defaultSeekerMaxSpeed);
                seekerWeightTuple = GetStatTuple(panelTransform, "Seeker Weight", defaultSeekerWeight);
                seekerStayInBoundTuple = GetStatTuple(panelTransform, "Stay in Bounds Weight", defaultSeekerStayInBound);
                break;

            case SceneChecker.FleerTest:
                Transform statPanel = canvasGameObject.transform.Find("Stat Panel");
                fleePanel = statPanel.Find("Flee Panel").gameObject;
                seekPanel = statPanel.Find("Seek Panel").gameObject;

                seekerButton = statPanel.Find("Seeker Button").GetComponent<Button>();
                fleerButton = statPanel.Find("Flee Button").GetComponent<Button>();
                panelsAndButtons = new List<(GameObject, Button)>() { (seekPanel.gameObject, seekerButton), (fleePanel.gameObject, fleerButton) };

                seekerFutureTimeTuple = GetStatTuple(seekPanel.transform, "Future Time", defaultSeekerFutureTime);
                seekerMassTuple = GetStatTuple(seekPanel.transform, "Mass", defaultSeekerMass);
                seekerMaxForceTuple = GetStatTuple(seekPanel.transform, "Max Force", defaultSeekerMaxForce);
                seekerMaxSpeedTuple = GetStatTuple(seekPanel.transform, "Max Speed", defaultSeekerMaxSpeed);
                seekerWeightTuple = GetStatTuple(seekPanel.transform, "Seeker Weight", defaultSeekerWeight);
                seekerStayInBoundTuple = GetStatTuple(seekPanel.transform, "Stay in Bounds Weight", defaultSeekerStayInBound);

                fleerFutureTimeTuple = GetStatTuple(fleePanel.transform, "Future Time", defaultFleerWeight);
                fleerMassTuple = GetStatTuple(fleePanel.transform, "Mass", defaultFleerMass);
                fleerMaxForceTuple = GetStatTuple(fleePanel.transform, "Max Force", defaultFleerMaxForce);
                fleerMaxSpeedTuple = GetStatTuple(fleePanel.transform, "Max Speed", defaultFleerMaxSpeed);
                fleerWeightTuple = GetStatTuple(fleePanel.transform, "Flee Weight", defaultFleerWeight);
                fleerStayInBoundTuple = GetStatTuple(fleePanel.transform, "Stay in Bounds Weight", defaultFleerStayInBound);
                ShowSeekPanel();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneChecker == SceneChecker.SeekerTest)
        {
            UpdateSeekerStats();
        }

        else if (sceneChecker == SceneChecker.FleerTest)
        {
            UpdateSeekerStats();
            UpdateFleerStats();
        }
    }
    private (Slider,TextMeshProUGUI) GetStatTuple(Transform transform, string parentName, float initialValue)
    {
        Transform parent = transform.Find(parentName);
        Slider slider = parent.Find("Slider").GetComponent<Slider>();
        slider.minValue = 1;
        slider.maxValue = 10;
        slider.wholeNumbers = true;
        slider.value = initialValue;
        TextMeshProUGUI text = parent.Find("Stat").GetComponent<TextMeshProUGUI>();
        return (slider, text);
    }
    private float UpdateStats((Slider, TextMeshProUGUI) tuple)
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
        gameManagerScript.fleerMaxSpeed = UpdateStats(fleerMaxForceTuple);
        gameManagerScript.fleerWeight = UpdateStats(fleerWeightTuple);
        gameManagerScript.fleerStayInBoundsWeight = UpdateStats(fleerStayInBoundTuple);
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
