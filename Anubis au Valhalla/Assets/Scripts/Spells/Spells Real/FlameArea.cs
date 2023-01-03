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

    private void Start()
    {
        manager = GameObject.Find("NewPowerManager").GetComponent<NewPowerManager>();
        Destroy(gameObject,manager.p2DashTrailDurations[manager.currentLevelPower2]);
        transform.localScale = new Vector2(manager.p2DashTrailSize,manager.p2DashTrailSize);
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
                col.GetComponentInParent<MonsterLifeManager>().TakeDamage(manager.p2DashTrailDamagesPerTick[manager.currentLevelPower2],0.5f);
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


