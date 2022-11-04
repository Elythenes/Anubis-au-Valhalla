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
    public float countdownCooldown2;

    
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
        }
    }
    
    public void SetCooldownMax2()
    {
        countdownCooldown2 = SpellManager.instance.cooldownSlot2;
    }
}
