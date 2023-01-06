using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricDash : MonoBehaviour
{
    public NewPowerManager manager;
    public CooldownPowerBar cooldown;

    private void Start()
    {
        cooldown = GameObject.Find("Spells - Consommable").GetComponent<CooldownPowerBar>();
        manager = GameObject.Find("NewPowerManager").GetComponent<NewPowerManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Monstre"))
        {
            if (!manager.p1DashContactStagger)
            {
                col.GetComponentInParent<MonsterLifeManager>().TakeDamage(manager.p1DashContactDamages[manager.currentLevelPower1], manager.p1DashContactStaggers[manager.currentLevelPower1]);
            }
            else
            {
                col.GetComponentInParent<MonsterLifeManager>().TakeDamage(manager.p1DashContactDamages[manager.currentLevelPower1], manager.p1DashContactStaggers[manager.currentLevelPower1]*1.5f);
            }
           
        }

        if (manager.p1DashContactPowerExtend)
        {
            if (col.GetComponentInParent<MonsterLifeManager>().vieActuelle <= 0)
            {
                cooldown.countdownDurationPower1 += 2;
            }
        }
    }
}
