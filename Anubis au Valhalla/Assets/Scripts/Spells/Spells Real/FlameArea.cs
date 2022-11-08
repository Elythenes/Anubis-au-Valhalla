using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FlameArea : MonoBehaviour
{
    [Header("FlameArea")] 
    public SpellStaticAreaObject sOFlameArea;
    public float tempsReloadHitFlameAreaTimer;
    public bool stopAttack;

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            stopAttack = false;
         for (int i = 0; i < sOFlameArea.nombreOfDot; i++)
         {
            if (tempsReloadHitFlameAreaTimer <= sOFlameArea.espacementDoT && stopAttack == false)
            {
                tempsReloadHitFlameAreaTimer += Time.deltaTime;
            }

            if (tempsReloadHitFlameAreaTimer > sOFlameArea.espacementDoT && col.gameObject.tag == "Monstre")
            {
                Debug.Log("touché");
                col.GetComponent<MonsterLifeManager>().DamageText(sOFlameArea.puissanceAttaque);
                col.GetComponent<MonsterLifeManager>().TakeDamage(sOFlameArea.puissanceAttaque,sOFlameArea.stagger);
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


