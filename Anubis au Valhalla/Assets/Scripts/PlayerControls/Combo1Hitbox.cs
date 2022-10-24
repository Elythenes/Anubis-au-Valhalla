using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Weapons;

public class Combo1Hitbox : MonoBehaviour
{
    [Range(0, 2)] public int comboNumber;
    public float stagger = 0.2f;

    private void Start()
    {
        transform.parent = CharacterController.instance.transform;
        Destroy(gameObject, AttaquesNormales.instance.dureeHitbox[comboNumber]);
       transform.localScale *= AttaquesNormales.instance.rangeAttaque[comboNumber];
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Monstre"))
        {
            col.gameObject.GetComponent<MonsterLifeManager>().DamageText(Mathf.RoundToInt(AttaquesNormales.instance.damage[comboNumber]));
            col.gameObject.GetComponent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(AttaquesNormales.instance.damage[comboNumber]), stagger);
        }
    }
}
