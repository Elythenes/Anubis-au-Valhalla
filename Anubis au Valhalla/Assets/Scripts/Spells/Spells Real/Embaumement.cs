using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Embaumement : MonoBehaviour
{
    public NewPowerManager manager;
    private Rigidbody2D rb;
    public int touchedEnemys;

    private void Start()
    {
        manager = GameObject.Find("NewPowerManager").GetComponent<NewPowerManager>();
       
        rb = gameObject.GetComponent<Rigidbody2D>();
        transform.localScale += new Vector3(1,1,0) * manager.p2ThrustBandageSizes[manager.currentLevelPower2];
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        rb.velocity = transform.right * manager.p2ThrustBandageSpeed;
        if (touchedEnemys >= manager.p2ThrustBandageMaxHit)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            touchedEnemys += 1;
            MonsterLifeManager monstre = col.GetComponentInParent<MonsterLifeManager>();
            monstre.TakeDamage(manager.p2ThrustBandageDamages[manager.currentLevelPower2], 3);
            monstre.isMomified = true;
            if (!manager.p2ThrustBandageStunUp)
            {
                monstre.MomifiedTime = 3f;
            }
            else
            {
                monstre.MomifiedTime = 5f;
            }
            
           
          
        }
    }
}
