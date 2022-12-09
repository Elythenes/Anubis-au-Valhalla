using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RayonEau : MonoBehaviour
{
    public PouvoirEauObject sOPouvoirEau;
    public float tempsReloadHitTimer;
    public bool stopAttack;

    private void Start()
    {
        Destroy(gameObject,CharacterController.instance.dashDuration - CharacterController.instance.timerDash);
        transform.localScale = sOPouvoirEau.rayonScale;
    }

    private void Update()
    {
        transform.localScale += new Vector3(1.5f, 0, 0);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            stopAttack = false;
            for (int i = 0; i < sOPouvoirEau.nbOfDotRayon; i++)
            {
                if (tempsReloadHitTimer <= sOPouvoirEau.espacementDoTRayon && stopAttack == false)
                {
                    tempsReloadHitTimer += Time.deltaTime;
                }

                if (tempsReloadHitTimer > sOPouvoirEau.espacementDoTRayon && col.gameObject.tag == "Monstre")
                {
                    Debug.Log("touché");
                    col.GetComponentInParent<MonsterLifeManager>().TakeDamage(sOPouvoirEau.rayonDamage + (AnubisCurrentStats.instance.vieActuelle /25),sOPouvoirEau.staggerRayon);
                    tempsReloadHitTimer = 0;
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
            
            tempsReloadHitTimer = 0;
        }
    }
}
