using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FleerUIManager : UIManager
{
    private FleerGameManager gameManager;

    #region Seeker Tuples
    private (Slider, Text) seekerStayInBoundsFutureTimeTuple, seekerMassTuple, seekerMaxForceTuple, seekerMaxSpeedTuple, seekerSeekWeightTuple, seekerStayInBoundsTuple;
    #endregion
    
    #region Fleer Tuples
    private (Slider, Text) fleerStayInBoundsFutureTimeTuple, fleerMassTuple, fleerMaxForceTuple, fleerMaxSpeedTuple, fleerFleeWeightTuple, fleerStayInBoundsTuple;
    #endregion
    
    private GameObject seekPanel, fleePanel;
    private Button seekerButton, fleerButton;

    #region Seeker Values
    [Header("Seeker Values")]
    private bool seekerDefaultStayInBoundsForce;
    [SerializeField]
    private bool seekerDefaultSeekForce;
    [SerializeField, Range(1, 10)]
    private int seekerDefaultStayInBoundsFutureTime;
    [SerializeField, Range(1, 10)]
    private int seekerDefaultMass, seekerDefaultMaxForce, seekerDefaultMaxSpeed, seekerDefaultSeekWeight, seekerDefaultStayInBoundsWeight;
    #endregion

    #region Fleer Values
    [Header("Fleer Values")]
    private bool defaultFleerFleeForce, defaultFleerStayInBoundsForce;
    [SerializeField, Range(1, 10)]
    private int defaultFleerFutureTime;

    [SerializeField, Range(1, 10)]
    private int defaultFleerMass, defaultFleerMaxForce, defaultFleerMaxSpeed, defaultFleerWeight, defaultFleerStayInBound;
    #endregion
    protected override void GetDefaultValues()
    {
        Transform statPanel = canvasGameObject.transform.Find("Stat Panel");
        fleePanel = statPanel.Find("Fleer Panel").gameObject;
        seekPanel = statPanel.Find("Seeker Panel").gameObject;

        Transform fleePanelContent = fleePanel.transform.Find("Scroll Area/Content");
        Transform seekPanelContent = seekPanel.transform.Find("Scroll Area/Content");

        seekerButton = statPanel.Find("Seeker Button").GetComponent<Button>();
        fleerButton = statPanel.Find("Fleer Button").GetComponent<Button>();
        panelsAndButtons = new List<(GameObject, Button)>() { (seekPanel, seekerButton), (fleePanel, fleerButton) };

        seekerMassTuple = GetStatTuple(seekPanelContent, parentName: "Mass", initialValue: seekerDefaultMass);
        seekerMaxForceTuple = GetStatTuple(seekPanelContent, parentName: "Max Force", initialValue: seekerDefaultMaxForce);
        seekerMaxSpeedTuple = GetStatTuple(seekPanelContent, parentName: "Max Speed", initialValue: seekerDefaultMaxSpeed);
        seekerStayInBoundsTuple = GetStatTuple(seekPanelContent, parentName: "Stay in Bounds Weight", initialValue: seekerDefaultStayInBoundsWeight);
        seekerSeekWeightTuple = GetStatTuple(seekPanelContent, parentName: "Seek Weight", initialValue: seekerDefaultSeekWeight);
        seekerStayInBoundsFutureTimeTuple = GetStatTuple(seekPanelContent, parentName: "Stay in Bounds Future Time", initialValue: seekerDefaultStayInBoundsFutureTime);
        GetCheckBoxTuple(seekPanelContent, "Stay in Bounds Force", seekerDefaultStayInBoundsForce);
        GetCheckBoxTuple(seekPanelContent, "Seek Force", seekerDefaultSeekForce);
        gameManager.seekerSeekForce = seekerDefaultSeekForce;
        gameManager.seekerStayInBoundsForce = seekerDefaultStayInBoundsForce;

        fleerMassTuple = GetStatTuple(fleePanelContent, parentName: "Mass", initialValue: defaultFleerMass);
        fleerMaxForceTuple = GetStatTuple(fleePanelContent, parentName: "Max Force", initialValue: defaultFleerMaxForce);
        fleerMaxSpeedTuple = GetStatTuple(fleePanelContent, parentName: "Max Speed", initialValue: defaultFleerMaxSpeed);
        fleerStayInBoundsTuple = GetStatTuple(fleePanelContent, parentName: "Stay in Bounds Weight", initialValue: defaultFleerStayInBound);
        fleerFleeWeightTuple = GetStatTuple(fleePanelContent, parentName: "Flee Weight", initialValue: defaultFleerWeight);
        fleerStayInBoundsFutureTimeTuple = GetStatTuple(fleePanelContent, parentName: "Stay in Bounds Future Time", initialValue: defaultFleerWeight);
        GetCheckBoxTuple(fleePanelContent, "Flee Force", defaultFleerFleeForce);
        GetCheckBoxTuple(fleePanelContent, "Stay in Bounds Force", defaultFleerStayInBoundsForce);
        ShowSeekPanel();
    }

    protected override void UpdateAgentStats()
    {
        #region Seeker
        gameManager.seekerMass = UpdateStats(seekerMassTuple);
        gameManager.seekerMaxForce = UpdateStats(seekerMaxForceTuple);
        gameManager.seekerMaxSpeed = UpdateStats(seekerMaxSpeedTuple);
        gameManager.seekerStayInBoundsWeight = UpdateStats(seekerStayInBoundsTuple);
        gameManager.seekerSeekWeight = UpdateStats(seekerSeekWeightTuple);
        gameManager.seekerStayInBoundsFutureTime = UpdateStats(seekerStayInBoundsFutureTimeTuple);
        #endregion

        #region Fleer
        gameManager.fleerMass = UpdateStats(fleerMassTuple);
        gameManager.fleerMaxForce = UpdateStats(fleerMaxForceTuple);
        gameManager.fleerMaxSpeed = UpdateStats(fleerMaxSpeedTuple);
        gameManager.fleerStayInBoundsWeight = UpdateStats(fleerStayInBoundsTuple);
        gameManager.fleerWeight = UpdateStats(fleerFleeWeightTuple);
        gameManager.fleerStayInBoundsFutureTime = UpdateStats(fleerStayInBoundsFutureTimeTuple);
        #endregion
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

    protected override void GetGameManager()
    {
        gameManager = GetComponent<FleerGameManager>();
    }

    public void ToggleSeekerSeekForce(bool value)
    {
        gameManager.seekerSeekForce = value;
        ToggleStatInteractability(seekerSeekWeightTuple, value);
    }

    public void ToggleSeekerStayInBoundsForce(bool value)
    {
        gameManager.seekerStayInBoundsForce = value;
        ToggleStatInteractability(seekerStayInBoundsFutureTimeTuple, value);
        ToggleStatInteractability(seekerStayInBoundsTuple, value);
    }

    public void ToggleFleerFleeForce(bool value)
    {
        gameManager.fleerFleeForce = value;
        ToggleStatInteractability(fleerFleeWeightTuple, value);
    }

    public void ToggleFleerStayInBoundsForce(bool value)
    {
        
        gameManager.fleerStayInBoundsForce = value;
        ToggleStatInteractability(fleerStayInBoundsFutureTimeTuple, value);
        ToggleStatInteractability(fleerStayInBoundsTuple, value);
    }
}