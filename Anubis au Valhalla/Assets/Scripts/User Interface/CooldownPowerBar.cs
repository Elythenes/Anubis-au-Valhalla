using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownPowerBar : MonoBehaviour
{
    public static CooldownPowerBar Instance;

    public Slider sliderPower1;
    public Slider sliderPower2;

    public bool p1OnCd;
    public bool p2OnCd;
    
    public float sliderP1Max;
    public float sliderP2Max;
    
    public TextMeshProUGUI compteurCurrentDurationPower1;
    [HideInInspector] public float countdownDurationPower1;
    [HideInInspector] public float durationBeforeCooldownPower1;
    //[HideInInspector] public float countdownCooldownPower1;

    public TextMeshProUGUI compteurCurrentDurationPower2;
    [HideInInspector] public float countdownDurationPower2;
    [HideInInspector] public float durationBeforeCooldownPower2;
    //[HideInInspector] public float countdownCooldownPower2;
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        sliderPower1.maxValue = 0;
        compteurCurrentDurationPower1.enabled = true;
        
        sliderPower2.maxValue = 0;
        compteurCurrentDurationPower2.enabled = true;
    }
    
    void Update()
    {
        CheckSeconds();
        compteurCurrentDurationPower1.SetText(Mathf.RoundToInt(countdownDurationPower1) + "");
        compteurCurrentDurationPower2.SetText(Mathf.RoundToInt(countdownDurationPower2) + "");
    }

    void CheckSeconds()
    {
        if (p1OnCd)
        {
            sliderPower1.maxValue = sliderP1Max;
            sliderPower1.value = sliderP1Max - NewPowerManager.Instance.currentCooldownPower1;
            countdownDurationPower1 = durationBeforeCooldownPower1 + NewPowerManager.Instance.currentCooldownPower1 * NewPowerManager.Instance.durationPower1 / NewPowerManager.Instance.cooldownPower1; 
        }
        else
        {
            sliderPower1.value = 0;
            countdownDurationPower1 = NewPowerManager.Instance.durationPower1 - NewPowerManager.Instance.currentDurationPower1;
        }
        
        if (p2OnCd)
        {
            sliderPower2.maxValue = sliderP2Max;
            sliderPower2.value = sliderP2Max - NewPowerManager.Instance.currentCooldownPower2;
            countdownDurationPower2 = durationBeforeCooldownPower2 + NewPowerManager.Instance.currentCooldownPower2 * NewPowerManager.Instance.durationPower2 / NewPowerManager.Instance.cooldownPower2; 
        }
        else
        {
            sliderPower2.value = 0;
            countdownDurationPower2 = NewPowerManager.Instance.durationPower2 - NewPowerManager.Instance.currentDurationPower2;
        }
    }
}
