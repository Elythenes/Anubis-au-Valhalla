using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class ElectricDash : MonoBehaviour
{
    public NewPowerManager manager;
    public CooldownPowerBar cooldown;
    public GameObject VFX;
    public VisualEffect realVFX;

    private void Start()
    {
        switch (CharacterController.instance.facing)
        {
            case CharacterController.LookingAt.Nord:
                VFX.transform.rotation = new Quaternion(0,0,90,0);
                break;
            case CharacterController.LookingAt.NordEst:
                VFX.transform.rotation = new Quaternion(0,0,45,0);
                break;
            case CharacterController.LookingAt.NordOuest:
                VFX.transform.rotation = new Quaternion(0,0,135,0);
                break;
            case CharacterController.LookingAt.Sud:
                VFX.transform.rotation = new Quaternion(0,0,-90,0);              
                break;
            case CharacterController.LookingAt.SudEst:
                VFX.transform.rotation = new Quaternion(0,0,-45,0);  
                break;
            case CharacterController.LookingAt.SudOuest:
                VFX.transform.rotation = new Quaternion(0,0,-135,0); 
                break;
            case CharacterController.LookingAt.Est:
                VFX.transform.rotation = new Quaternion(0,0,0,0);     
                break;
            case CharacterController.LookingAt.Ouest:
                VFX.transform.rotation = new Quaternion(0, 0, 180, 0);
                break;
        }
        
        realVFX.Play();
        cooldown = GameObject.Find("Spells - Consommable").GetComponent<CooldownPowerBar>();
        manager = GameObject.Find("NewPowerManager").GetComponent<NewPowerManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Monstre"))
        {
            if (!manager.p1DashContactStagger)
            {
                col.GetComponentInParent<MonsterLifeManager>().TakeDamage(manager.p1DashContactDamages[manager.currentLevelPower1 - 1] + (int)AnubisCurrentStats.instance.magicForce, manager.p1DashContactStaggers[manager.currentLevelPower1 - 1]);
            }
            else
            {
                col.GetComponentInParent<MonsterLifeManager>().TakeDamage(manager.p1DashContactDamages[manager.currentLevelPower1 - 1] + (int)AnubisCurrentStats.instance.magicForce, manager.p1DashContactStaggers[manager.currentLevelPower1 - 1]*1.5f);
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
}
