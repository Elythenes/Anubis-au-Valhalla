using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FlameArea : MonoBehaviour
{
    public NewPowerManager manager;
    public float tempsReloadHitFlameAreaTimer;
    public bool stopAttack;
    public bool isMiniFlame;
    public GameObject childVFX;

    private void Start()
    {
        manager = GameObject.Find("NewPowerManager").GetComponent<NewPowerManager>();
        if (isMiniFlame)
        {
            Destroy(gameObject,manager.p2DashTrailDurations[manager.currentLevelPower2 - 1]/2);
        }
        else
        {
            Destroy(gameObject,manager.p2DashTrailDurations[manager.currentLevelPower2 - 1]);
        }
        transform.localScale = Vector3.one *manager.p2DashTrailSize/1.5f;
        childVFX.transform.localScale = Vector3.one *manager.p2DashTrailSize;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            stopAttack = false;
         for (int i = 0; i < 5; i++)
         {
            if (tempsReloadHitFlameAreaTimer <= manager.p2DashTrailEspacementDoT && stopAttack == false)
            {
                tempsReloadHitFlameAreaTimer += Time.deltaTime;
            }

            if (tempsReloadHitFlameAreaTimer > manager.p2DashTrailEspacementDoT && col.gameObject.tag == "Monstre")
            {
                if (manager.p2DashTrailInfection && !isMiniFlame)
                {
                    col.GetComponentInParent<MonsterLifeManager>().flameManager = manager;
                    col.GetComponentInParent<MonsterLifeManager>().FlameInfected = true;
                }

                if (!manager.p2DashTrailMiniStagger)
                {
                    col.GetComponentInParent<MonsterLifeManager>().TakeDotDamage(manager.p2DashTrailDamagesPerTick[manager.currentLevelPower2 - 1] + (int)AnubisCurrentStats.instance.magicForce/10,0f);
                }
                else if(!isMiniFlame)
                {
                    col.GetComponentInParent<MonsterLifeManager>().TakeDotDamage(manager.p2DashTrailDamagesPerTick[manager.currentLevelPower2 -1] + (int)AnubisCurrentStats.instance.magicForce/10,0.1f);
                }
                else if(isMiniFlame)
                {
                    col.GetComponentInParent<MonsterLifeManager>().TakeDotDamage(manager.p2DashTrailDamagesPerTick[manager.currentLevelPower2 - 1] + (int)AnubisCurrentStats.instance.magicForce/10,0f);
                }
               
                tempsReloadHitFlameAreaTimer = 0;
            }
         }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
          stopAttack = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            tempsReloadHitFlameAreaTimer = 0;
        }
    }
}


