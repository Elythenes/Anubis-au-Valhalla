using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class cooldownSpellBar : MonoBehaviour
{
    public static cooldownSpellBar instance;
    public Slider slider;
    public TextMeshProUGUI compteurCooldown;
    public float countdownCooldown1;
   

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Update()
    {
            slider.maxValue = SpellManager.instance.cooldownSlot1;
            slider.value = SpellManager.instance.cooldownSlotTimer1;
            countdownCooldown1 -= Time.deltaTime;
            compteurCooldown.SetText(Mathf.RoundToInt(countdownCooldown1) + "");
            if (countdownCooldown1 <= 0)
            {
                compteurCooldown.enabled = false;
            }
            else
            {
                compteurCooldown.enabled = true;
            }
    }


    public void SetCooldownMax1()
    {
        countdownCooldown1 = SpellManager.instance.cooldownSlot1;
    }
}
