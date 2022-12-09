using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FlameArea : MonoBehaviour
{
    [Header("FlameArea")] 
    public PouvoirFeuObject sOPouvoirFeu;
    public float tempsReloadHitFlameAreaTimer;
    public bool stopAttack;

    private void Start()
    {
        Destroy(gameObject,sOPouvoirFeu.hitboxDashDuration);
        transform.localScale = sOPouvoirFeu.zoneScale;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            stopAttack = false;
         for (int i = 0; i < sOPouvoirFeu.nombreOfDot; i++)
         {
            if (tempsReloadHitFlameAreaTimer <= sOPouvoirFeu.espacementDoT && stopAttack == false)
            {
                tempsReloadHitFlameAreaTimer += Time.deltaTime;
            }

            if (tempsReloadHitFlameAreaTimer > sOPouvoirFeu.espacementDoT && col.gameObject.tag == "Monstre")
            {
                Debug.Log("touché");
                col.GetComponentInParent<MonsterLifeManager>().TakeDamage(sOPouvoirFeu.dashPuissanceAttaque,sOPouvoirFeu.stagger);
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


