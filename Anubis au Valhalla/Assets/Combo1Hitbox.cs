using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

public class Combo1Hitbox : MonoBehaviour
{

    private void Start()
    {
        transform.parent = CharacterController.instance.transform;
        Destroy(gameObject, AttaquesNormales.instance.dureeHitbox1);
       transform.localScale *= AttaquesNormales.instance.rangeAttaque1;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Monstre"))
        {
            col.gameObject.GetComponent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(AttaquesNormales.instance.damageC1));
            col.gameObject.GetComponent<MonsterLifeManager>().DamageText(Mathf.RoundToInt(AttaquesNormales.instance.damageC1));
        }
    }
}
