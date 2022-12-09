using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlumeMaat : MonoBehaviour
{
    
    public PouvoirMaledictionObject soMalediction;

    private void Start()
    {
        Destroy(gameObject,soMalediction.plumeHitboxDuration);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            if (col.GetComponentInParent<MonsterLifeManager>().vieActuelle <= col.GetComponentInParent<MonsterLifeManager>().vieMax * 10 / 100)
            {
                col.GetComponentInParent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(soMalediction.plumeDamage),soMalediction.staggerplume);
            }
            else
            {
                col.GetComponentInParent<MonsterLifeManager>().TakeDamage(1,0.1f);
            }
            
        }
    }
}
