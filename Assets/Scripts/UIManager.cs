using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ShaderData;

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
    private (Slider, TextMeshProUGUI) futureTimeTuple, massTuple, maxForceTuple, maxSpeedTuple, seekerWeightTuple, stayInBoundTuple;
    [SerializeField]
    [Range(1, 10)]
    private int defaultFutureTime, defaultMass, defaultMaxForce, defaultMaxSpeed, defaultSeekerWeight, defaultStayInBound;
    private GameManager gameManagerScript;

    void Start()
    {
        if(sceneChecker == SceneChecker.SeekerTest)
        {
            Transform panelTransform = canvasGameObject.transform.Find("Panel");
            futureTimeTuple = GetStatTuple(panelTransform, "Future Time", defaultFutureTime);
            massTuple = GetStatTuple(panelTransform, "Mass", defaultMass);
            maxForceTuple = GetStatTuple(panelTransform, "Max Force", defaultMaxForce);
            maxSpeedTuple = GetStatTuple(panelTransform, "Max Speed", defaultMaxSpeed);
            seekerWeightTuple = GetStatTuple(panelTransform, "Seeker Weight", defaultSeekerWeight);
            stayInBoundTuple = GetStatTuple(panelTransform, "Stay in Bounds Weight", defaultStayInBound);
            gameManagerScript = GetComponent<GameManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneChecker == SceneChecker.SeekerTest)
        {
            gameManagerScript.futureTime = UpdateStats(futureTimeTuple);
            gameManagerScript.mass = UpdateStats(massTuple);
            gameManagerScript.maxForce = UpdateStats(maxForceTuple);
            gameManagerScript.maxSpeed = UpdateStats(maxSpeedTuple);
            gameManagerScript.seekerWeight = UpdateStats(seekerWeightTuple);
            gameManagerScript.stayInBoundsWeight = UpdateStats(stayInBoundTuple);
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
}
