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
    public TextMeshProUGUI compteurTempsRestant;
    public float countdownCooldown1;
    public GameObject ankhShield;
    public AnkhShield ankhSheildData;


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

            if (SpellManager.instance.prefabA == ankhShield)
            {
                Debug.Log("oui");
                compteurTempsRestant.enabled = true;
                compteurTempsRestant.SetText(Mathf.RoundToInt(ankhSheildData.secondesRestantes) + "");
            }
            else
            {
                compteurTempsRestant.enabled = false;
            }
    }


    public void SetCooldownMax1()
    {
        countdownCooldown1 = SpellManager.instance.cooldownSlot1;
    }
}
