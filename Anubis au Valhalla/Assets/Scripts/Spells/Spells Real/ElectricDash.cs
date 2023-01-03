using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricDash : MonoBehaviour
{
    public NewPowerManager manager;

    private void Start()
    {
        manager = GameObject.Find("NewPowerManager").GetComponent<NewPowerManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Monstre"))
        {
            col.GetComponentInParent<MonsterLifeManager>().TakeDamage(manager.p1DashContactDamages[manager.currentLevelPower1], manager.p1DashContactStaggers[manager.currentLevelPower1]);
        }

        if (manager.p1DashContactPowerExtend)
        {
            if (col.GetComponentInParent<MonsterLifeManager>().vieActuelle <= 0)
            {
                // Allonger la durée restante du spell
            }
        }
    }
}
