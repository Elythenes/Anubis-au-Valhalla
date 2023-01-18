using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarManager : MonoBehaviour
{
    public AnubisCurrentStats manager;
    public static LifeBarManager instance;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI textPV;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        manager = GameObject.Find("StatManager").GetComponent<AnubisCurrentStats>();
    }

    private void Update()
    {
        slider.value = manager.vieActuelle;
        slider.maxValue = manager.vieMax;
        textPV.SetText(AnubisCurrentStats.instance.vieActuelle + " / " + AnubisCurrentStats.instance.vieMax);
        fill.color = gradient.Evaluate(1f);
    }

   
}
