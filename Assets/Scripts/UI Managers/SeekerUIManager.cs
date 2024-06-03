using System;
using UnityEngine;
using UnityEngine.UI;

public class SeekerUIManager : UIManager
{
    private (Slider, Text) stayInBoundsFutureTimeTuple, massTuple, maxForceTuple, maxSpeedTuple, seekWeightTuple, stayInBoundsTuple;
    private (Toggle, Text) stayInBoundsForceTuple, seekForceTuple;

    private SeekerGameManager gameManager;

    [Header("Seeker Values")]
    [SerializeField]
    private bool defaultStayInBoundsForce;
    [SerializeField]
    private bool defaultSeekForce;
    [SerializeField, Range(1, 10)]
    private int defaultStayInBoundsFutureTime;
    [SerializeField, Range(1, 10)]
    private int defaultMass, defaultMaxForce, defaultMaxSpeed, defaultSeekWeight, defaultStayInBound;
    protected override void GetGameManager()
    {
        gameManager = GetComponent<SeekerGameManager>();
    }

    protected override void GetDefaultValues()
    {
        #region Stats
        Transform contentTransform = canvasGameObject.transform.Find("Panel/Scroll Area/Content");
        stayInBoundsFutureTimeTuple = GetStatTuple(contentTransform, parentName: "Stay In Bounds Future Time", initialValue: defaultStayInBoundsFutureTime);
        massTuple = GetStatTuple(contentTransform, parentName: "Mass", initialValue: defaultMass);
        maxForceTuple = GetStatTuple(contentTransform, parentName: "Max Force", initialValue: defaultMaxForce);
        maxSpeedTuple = GetStatTuple(contentTransform, parentName: "Max Speed", initialValue: defaultMaxSpeed);
        #endregion

        #region Seek Force
        seekWeightTuple = GetStatTuple(contentTransform, parentName: "Seek Weight", initialValue: defaultSeekWeight);
        GetCheckBoxTuple(contentTransform, "Seek Force", defaultSeekForce);
        gameManager.seekForce = defaultSeekForce;
        #endregion

        #region Stay in Bounds Force
        stayInBoundsTuple = GetStatTuple(contentTransform, parentName: "Stay In Bounds Weight", initialValue: defaultStayInBound);
        GetCheckBoxTuple(contentTransform, parentName: "Stay In Bounds Force", initialValue: defaultStayInBoundsForce);
        gameManager.seekForce = defaultStayInBoundsForce;
        #endregion
    }

    

    protected override void UpdateAgentStats()
    {
        gameManager.mass = UpdateStats(massTuple);
        gameManager.maxForce = UpdateStats(maxForceTuple);
        gameManager.maxSpeed = UpdateStats(maxSpeedTuple);
        gameManager.stayInBoundsWeight = UpdateStats(stayInBoundsTuple);
        gameManager.seekWeight = UpdateStats(seekWeightTuple);
        gameManager.stayInBoundsFutureTime = UpdateStats(stayInBoundsFutureTimeTuple);
    }

    public void ToggleSeekForce(bool value)
    {
        gameManager.seekForce = value;
        ToggleStatInteractability(seekWeightTuple, value);
    }

    public void ToggleStayInBoundsForce(bool value)
    {
        gameManager.stayInBoundsForce = value;
        ToggleStatInteractability(stayInBoundsFutureTimeTuple, value);
        ToggleStatInteractability(stayInBoundsTuple, value);
    }
}
