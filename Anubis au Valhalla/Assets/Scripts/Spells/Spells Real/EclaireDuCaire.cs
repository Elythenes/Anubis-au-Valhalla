using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EclaireDuCaire : MonoBehaviour
{
    [Header("FlameArea")] 
    public PouvoirFoudreObject sOPouvoirFoudre;
    public float tempsReloadHitFlameAreaTimer;
    public bool stopAttack;

    private void Start()
    {
        Destroy(gameObject,sOPouvoirFoudre.attaqueNormaleDuration);
        transform.localScale = sOPouvoirFoudre.attaqueNormaleScale;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            stopAttack = false;
            for (int i = 0; i < sOPouvoirFoudre.nombreOfDot; i++)
            {
                if (tempsReloadHitFlameAreaTimer <= sOPouvoirFoudre.espacementDoT && stopAttack == false)
                {
                    tempsReloadHitFlameAreaTimer += Time.deltaTime;
                }

                if (tempsReloadHitFlameAreaTimer > sOPouvoirFoudre.espacementDoT && col.gameObject.tag == "Monstre")
                {
                    Debug.Log("touché");
                    col.GetComponentInParent<MonsterLifeManager>().TakeDamage(sOPouvoirFoudre.attaqueNormaleDamage,sOPouvoirFoudre.stagger);
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
