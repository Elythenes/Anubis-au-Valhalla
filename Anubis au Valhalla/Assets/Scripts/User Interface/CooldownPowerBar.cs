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
    //

    public bool activeSlider1;
    //

    public float sliderP1Max;
    
    public TextMeshProUGUI compteurCurrentDurationPower1;
    [HideInInspector] public float countdownDurationPower1;
    [HideInInspector] public float countdownCooldownPower1;

    public TextMeshProUGUI compteurCurrentDurationPower2;
    [HideInInspector] public float countdownDurationPower2;
    [HideInInspector] public float countdownCooldownPower2;
    
    
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
    }
    
    void Update()
    {
        CheckSeconds();
        compteurCurrentDurationPower1.SetText(Mathf.RoundToInt(countdownDurationPower1) + "");
    }

    void CheckSeconds()
    {
        if (p1OnCd)
        {
            sliderPower1.maxValue = sliderP1Max;
            sliderPower1.value = sliderP1Max - NewPowerManager.Instance.currentCooldownPower1;
            countdownDurationPower1 = NewPowerManager.Instance.durationPower1 - NewPowerManager.Instance.currentCooldownPower1 / NewPowerManager.Instance.cooldownPower1; //ici, trouver la formule pour afficher le bon temps restants avant d'arriver au max = 5
        }
        else
        {
            sliderPower1.value = 0;
            countdownDurationPower1 = NewPowerManager.Instance.durationPower1 - NewPowerManager.Instance.currentDurationPower1;
        }
        
    }

    void ShowSlidebar()
    {

        //Power2
        /*sliderPower2.maxValue = sliderP2MaxValue;
        compteurCurrentCooldownPower2.SetText(Mathf.RoundToInt(countdownCooldownPower2) + "");
        sliderPower2.value = countdownCooldownPower2;
        
        if (countdownCooldownPower2 <= 0)
        {
            compteurCurrentCooldownPower2.enabled = false;
        }
        else
        {
            compteurCurrentCooldownPower2.enabled = true;
        }

        compteurCurrentDurationPower2.enabled = true;
        compteurCurrentDurationPower2.SetText(Mathf.RoundToInt(countdownDurationPower2) + "");
        */
        
    }
}
