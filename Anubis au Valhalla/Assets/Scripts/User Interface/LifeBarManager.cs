using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarManager : MonoBehaviour
{
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
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        textPV.SetText(DamageManager.instance.vieActuelle + " / " + health);

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        textPV.SetText(DamageManager.instance.vieActuelle + " / " + DamageManager.instance.vieMax);

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
