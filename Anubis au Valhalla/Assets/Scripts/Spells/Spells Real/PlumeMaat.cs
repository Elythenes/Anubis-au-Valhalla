using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlumeMaat : MonoBehaviour
{
    
    public SpellStaticAreaObject soPlumeMaat;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            if (col.GetComponentInParent<MonsterLifeManager>().vieActuelle <= col.GetComponent<MonsterLifeManager>().vieMax * 25 / 100)
            {
                col.GetComponentInParent<MonsterLifeManager>().DamageText(soPlumeMaat.puissanceAttaque);
                col.GetComponentInParent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(soPlumeMaat.puissanceAttaque),soPlumeMaat.stagger);
            }
            
        }
    }
}
