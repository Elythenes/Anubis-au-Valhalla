using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FlameArea : MonoBehaviour
{
    [Header("FlameArea")] 
    public SpellStaticAreaType sOFlameArea;
    public float tempsReloadHitFlameAreaTimer;
    public bool stopAttack;
    public bool startAttack;

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
                col.GetComponent<IA_Monstre1>().TakeDamage(sOFlameArea.puissanceAttaque);
                col.GetComponent<IA_Monstre1>().DamageText(sOFlameArea.puissanceAttaque);
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
            StartCoroutine(stopAttackTimer());
            tempsReloadHitFlameAreaTimer = 0;
        }
    }

    IEnumerator stopAttackTimer()
    {
        stopAttack = true;
        yield return new WaitForSeconds(0.1f);
        stopAttack = false;
    }
}


