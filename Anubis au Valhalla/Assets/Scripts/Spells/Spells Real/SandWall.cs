using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SandWall : MonoBehaviour
{
    private BoxCollider2D collider;
    private Rigidbody2D rb;
    public SpellSpawnEntityObject soSandWall;
    public float timerStep2Time;
    public float speed;
    public float tempsReloadHitFlameAreaTimer;
    public bool stopAttack;
    public bool activeHitbox;


    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        timerStep2Time += Time.deltaTime;

        if (timerStep2Time >= soSandWall.timerStep2)
        { 
            activeHitbox = true;
            collider.isTrigger = true;
            transform.Translate(transform.right*speed,Space.World);
        }
    }
    
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre" && activeHitbox)
        {
            stopAttack = false;
            for (int i = 0; i < soSandWall.nombreOfDot; i++)
            {
                if (tempsReloadHitFlameAreaTimer <= soSandWall.espacementDoT && stopAttack == false)
                {
                    tempsReloadHitFlameAreaTimer += Time.deltaTime;
                }

                if (tempsReloadHitFlameAreaTimer > soSandWall.espacementDoT && col.gameObject.tag == "Monstre")
                {
                    Debug.Log("touché");
                    col.GetComponentInParent<MonsterLifeManager>().DamageText(soSandWall.puissanceAttaque);
                    col.GetComponentInParent<MonsterLifeManager>().TakeDamage(soSandWall.puissanceAttaque,soSandWall.stagger);
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
