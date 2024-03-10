using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private GameObject canvasGameObject;
    private (Slider, TextMeshProUGUI) futureTimeTuple, massTuple, maxForceTuple, maxSpeedTuple, seekerWeightTuple, stayInBoundTuple;
    private SeekerTest gameManagerScript;
    void Start()
    {
        Transform panelTransform = canvasGameObject.transform.Find("Panel");
        futureTimeTuple = GetStatTuple(panelTransform, "Future Time");
        massTuple = GetStatTuple(panelTransform, "Mass");
        maxForceTuple = GetStatTuple(panelTransform, "Max Force");
        maxSpeedTuple = GetStatTuple(panelTransform, "Max Speed");
        seekerWeightTuple = GetStatTuple(panelTransform, "Seeker Weight");
        stayInBoundTuple = GetStatTuple(panelTransform, "Stay in Bounds Weight");

        gameManagerScript = GetComponent<SeekerTest>();
        //Debug.Log("a");
    }

    // Update is called once per frame
    void Update()
    {
        gameManagerScript.futureTime = UpdateStats(futureTimeTuple);
        gameManagerScript.mass = UpdateStats(massTuple);
        gameManagerScript.maxForce = UpdateStats(maxForceTuple);
        gameManagerScript.maxSpeed = UpdateStats(maxSpeedTuple);
        gameManagerScript.seekerWeight = UpdateStats(seekerWeightTuple);
        gameManagerScript.stayInBoundsWeight = UpdateStats(stayInBoundTuple);

        Debug.Log("Future Time: " + gameManagerScript.futureTime);
    }

    private (Slider,TextMeshProUGUI) GetStatTuple(Transform transform, string parentName)
    {
        Transform parent = transform.Find(parentName);
        Slider slider = parent.Find("Slider").GetComponent<Slider>();
        slider.minValue = 1;
        slider.maxValue = 10;
        slider.wholeNumbers = true;
        TextMeshProUGUI text = parent.Find("Stat").GetComponent<TextMeshProUGUI>();
        return (slider, text);
    }

    private float UpdateStats((Slider, TextMeshProUGUI) tuple)
    {

        tuple.Item2.text = tuple.Item1.value.ToString();
        return tuple.Item1.value;
    }
}
