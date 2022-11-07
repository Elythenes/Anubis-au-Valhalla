using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Akh : MonoBehaviour
{
    [Header("FlameArea")] 
    public SpellStaticAreaObject soAkh;
    public float tempsReloadHitFlameAreaTimer;
    public bool stopAttack;

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            stopAttack = false;
            for (int i = 0; i < soAkh.nombreOfDot; i++)
            {
                if (tempsReloadHitFlameAreaTimer <= soAkh.espacementDoT && stopAttack == false)
                {
                    tempsReloadHitFlameAreaTimer += Time.deltaTime;
                }

                if (tempsReloadHitFlameAreaTimer > soAkh.espacementDoT && col.gameObject.tag == "Monstre")
                {
                    Debug.Log("touché");
                    col.GetComponent<MonsterLifeManager>().DamageText(soAkh.puissanceAttaque + Mathf.RoundToInt(Mathf.Log(Souls.instance.soulBank + 1) *10));
                    col.GetComponent<MonsterLifeManager>().TakeDamage(soAkh.puissanceAttaque + Mathf.RoundToInt(Mathf.Log(Souls.instance.soulBank + 1) *10),soAkh.stagger);
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
