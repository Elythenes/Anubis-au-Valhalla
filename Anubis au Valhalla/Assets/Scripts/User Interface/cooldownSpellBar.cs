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
    public GameObject currentPower;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Update()
    {
            currentPower = UiManager.instance.currentSpell1Holder;
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

            if (currentPower.CompareTag("Feu"))
            {
                compteurTempsRestant.enabled = true;
                compteurTempsRestant.SetText(Mathf.RoundToInt(currentPower.GetComponent<PouvoirFeu>().secondesRestantes) + "");
            }
            /*else
            {
                compteurTempsRestant.enabled = false;
            }*/
            
            if (currentPower.CompareTag("Plaie"))
            {
                compteurTempsRestant.enabled = true;
                compteurTempsRestant.SetText(Mathf.RoundToInt(currentPower.GetComponent<PouvoirPlaie>().secondesRestantes) + "");
            }
            /*else
            {
                compteurTempsRestant.enabled = false;
            }*/
            if (currentPower.CompareTag("Eau"))
            {
                compteurTempsRestant.enabled = true;
                compteurTempsRestant.SetText(Mathf.RoundToInt(currentPower.GetComponent<PouvoirEau>().secondesRestantes) + "");
            }
            /*else
            {
                compteurTempsRestant.enabled = false;
            }*/
    }


    public void SetCooldownMax1()
    {
        countdownCooldown1 = SpellManager.instance.cooldownSlot1;
    }
}
