using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class cooldownSpellBar2 : MonoBehaviour
{
    public static cooldownSpellBar2 instance;
    public Slider slider;
    public TextMeshProUGUI compteurCooldown;
    public TextMeshProUGUI compteurTempsRestant;
    public float countdownCooldown2;
    public GameObject currentPower;

    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Update()
    {
        {
            if (UiManager.instance.currentSpell2Holder is not null)
            {
                currentPower = UiManager.instance.currentSpell2Holder;
            }
            slider.maxValue = SpellManager.instance.cooldownSlot2;
            slider.value = SpellManager.instance.cooldownSlotTimer2;
            countdownCooldown2 -= Time.deltaTime;
            compteurCooldown.SetText(Mathf.RoundToInt(countdownCooldown2) + "");
            if (countdownCooldown2 <= 0)
            {
                compteurCooldown.enabled = false;
            }
            else
            {
                compteurCooldown.enabled = true;
            }

            if (currentPower != null)
            {
                if (currentPower.CompareTag("Feu"))
                {
                    compteurTempsRestant.enabled = true;
                    compteurTempsRestant.SetText(Mathf.RoundToInt(currentPower.GetComponent<PouvoirFeu>().secondesRestantes) + "");
                }

                if (currentPower.CompareTag("Plaie"))
                {
                    compteurTempsRestant.enabled = true;
                    compteurTempsRestant.SetText(Mathf.RoundToInt(currentPower.GetComponent<PouvoirPlaie>().secondesRestantes) + "");
                }
              
                if (currentPower.CompareTag("Eau"))
                {
                    compteurTempsRestant.enabled = true;
                    compteurTempsRestant.SetText(Mathf.RoundToInt(currentPower.GetComponent<PouvoirEau>().secondesRestantes) + "");
                }
              
                if (currentPower.CompareTag("Foudre"))
                {
                    compteurTempsRestant.enabled = true;
                    compteurTempsRestant.SetText(Mathf.RoundToInt(currentPower.GetComponent<PouvoirFoudre>().secondesRestantes) + "");
                }
                
                if (currentPower.CompareTag("Ame"))
                {
                    compteurTempsRestant.enabled = true;
                    compteurTempsRestant.SetText(Mathf.RoundToInt(currentPower.GetComponent<PouvoirAme>().secondesRestantes) + "");
                }
                
                if (currentPower.CompareTag("Malediction"))
                {
                    compteurTempsRestant.enabled = true;
                    compteurTempsRestant.SetText(Mathf.RoundToInt(currentPower.GetComponent<PouvoirMalediction>().secondesRestantes) + "");
                }
            }
            
        }
    }
    
    public void SetCooldownMax2()
    {
        countdownCooldown2 = SpellManager.instance.cooldownSlot2;
    }
}
