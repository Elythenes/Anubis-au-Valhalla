using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarMonstre : MonoBehaviour
{
    public static HealthBarMonstre instance;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public MonsterLifeManager ia;
    public GameObject monstreassocie;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        SetMaxHealth(ia.vieMax);
    }

    private void Update()
    {
        transform.position = monstreassocie.transform.position - new Vector3(0,2,0);
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate((slider.normalizedValue));
    }
}
