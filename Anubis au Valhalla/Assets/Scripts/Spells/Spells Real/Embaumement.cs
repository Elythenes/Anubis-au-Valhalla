using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Embaumement : MonoBehaviour
{
    public NewPowerManager manager;
    private Rigidbody2D rb;

    private void Start()
    {
        manager = GameObject.Find("NewPowerManager").GetComponent<NewPowerManager>();
       
        rb = gameObject.GetComponent<Rigidbody2D>();
        transform.localScale= new Vector2(manager.p2ThrustBandageSizes[manager.currentLevelPower2],manager.p2ThrustBandageSizes[manager.currentLevelPower2]);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        rb.velocity = transform.right * manager.p2ThrustBandageSpeed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            MonsterLifeManager monstre = col.GetComponentInParent<MonsterLifeManager>();
            monstre.TakeDamage(manager.p2ThrustBandageDamages[manager.currentLevelPower2], 0.5f);
            monstre.isMomified = true;
            monstre.MomifiedTime = 0.5f;
        }
    }
}
