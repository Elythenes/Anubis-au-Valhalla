using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Pathfinding;
using UnityEngine;
using Weapons;

public class Combo1Hitbox : MonoBehaviour
{
    [Range(0, 2)] public int comboNumber;
    public float stagger = 0.2f;
    public GameObject camera;
    

    private void Start()
    {
        camera = GameObject.Find("CameraHolder");
        transform.parent = CharacterController.instance.transform;
        Destroy(gameObject, AttaquesNormales.instance.dureeHitbox[comboNumber]);
       transform.localScale *= AttaquesNormales.instance.rangeAttaque[comboNumber];
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Monstre"))
        {
            camera.transform.DOShakePosition(0.15f, 1.5f);
            Vector3 angleKnockback = col.transform.position - transform.parent.position;
            Vector3 angleNormalized = angleKnockback.normalized;
            col.gameObject.GetComponent<AIPath>().canMove = false;
            col.gameObject.GetComponent<MonsterLifeManager>().DamageText(Mathf.RoundToInt(AttaquesNormales.instance.damage[comboNumber]));
            col.gameObject.GetComponent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(AttaquesNormales.instance.damage[comboNumber]), stagger);
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(angleNormalized*AttaquesNormales.instance.forceKnockback[comboNumber],ForceMode2D.Impulse);
        }
    }
    

    
}
