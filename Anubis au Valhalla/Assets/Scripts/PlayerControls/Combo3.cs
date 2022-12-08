using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DG.Tweening;
using Pathfinding;
using UnityEngine;
using Weapons;
using Debug = UnityEngine.Debug;

public class Combo3 : MonoBehaviour
{
    [Range(0, 2)] public int comboNumber;
    public float stagger = 0.2f;
    private bool isShaking;
    private bool isWaiting;
    public GameObject bloodEffect;


    public virtual void Start()
    {
        Destroy(gameObject, AttaquesNormales.instance.dureeHitbox[3]);
       transform.localScale *= AttaquesNormales.instance.rangeAttaque[3];
    }

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Monstre"))
        {
            float angle = Mathf.Atan2(transform.position.y - col.transform.position.y,transform.position.x - col.transform.position.x ) * Mathf.Rad2Deg;
            GameObject effetSang = Instantiate(bloodEffect, col.transform.position, Quaternion.identity);
            effetSang.transform.rotation = Quaternion.Euler(0,0,angle);
            if (col.GetComponent<PuppetHealth>())
            {
                col.gameObject.GetComponent<MonsterLifeManager>().DamageText(Mathf.RoundToInt(AttaquesNormales.instance.damage[comboNumber]));
                col.gameObject.GetComponent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(AttaquesNormales.instance.damage[comboNumber]), stagger);
                return;
            }
            Vector3 angleKnockback = col.transform.position - transform.position;
            Vector3 angleNormalized = angleKnockback.normalized;
            col.gameObject.GetComponent<AIPath>().canMove = false;
            col.gameObject.GetComponentInParent<MonsterLifeManager>().DamageText(Mathf.RoundToInt(AttaquesNormales.instance.damage[comboNumber]));
            col.gameObject.GetComponentInParent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(AttaquesNormales.instance.damage[comboNumber]), stagger);
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(angleNormalized*AttaquesNormales.instance.forceKnockback[comboNumber],ForceMode2D.Impulse);
            col.GetComponentInParent<MonsterLifeManager>().ai.Move(angleNormalized*AttaquesNormales.instance.forceKnockback[comboNumber]);
        }
    }
}
