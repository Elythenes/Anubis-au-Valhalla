using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlumeMaat : MonoBehaviour
{
    
    public PouvoirMaledictionObject soMalediction;

    private void Start()
    {
        Destroy(gameObject,soMalediction.dashHitboxDuration);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            if (col.GetComponentInParent<MonsterLifeManager>().vieActuelle <= col.GetComponentInParent<MonsterLifeManager>().vieMax * 25 / 100)
            {
                col.GetComponentInParent<MonsterLifeManager>().DamageText(soMalediction.dashDamage);
                col.GetComponentInParent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(soMalediction.dashDamage),soMalediction.staggerDash);
            }
            else
            {
                col.GetComponentInParent<MonsterLifeManager>().DamageText(1);
                col.GetComponentInParent<MonsterLifeManager>().TakeDamage(1,0.1f);
            }
            
        }
    }
}
