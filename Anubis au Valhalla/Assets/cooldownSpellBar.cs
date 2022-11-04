using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cooldownSpellBar : MonoBehaviour
{
    public static cooldownSpellBar instance;
    public bool Slot1;
    public Slider slider;
    public Image fill;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Update()
    {
        if (Slot1)
        {
            slider.maxValue = SpellManager.instance.cooldownSlot1;
            slider.value = SpellManager.instance.cooldownSlotTimer1;
        }
        else
        {
            slider.maxValue = SpellManager.instance.cooldownSlot2;
            slider.value = SpellManager.instance.cooldownSlotTimer2;
        }
    }
}
