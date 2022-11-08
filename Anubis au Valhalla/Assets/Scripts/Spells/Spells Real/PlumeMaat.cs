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
            if (col.GetComponent<MonsterLifeManager>().vieActuelle <= col.GetComponent<MonsterLifeManager>().vieMax / 2)
            {
                col.GetComponent<MonsterLifeManager>().DamageText(soPlumeMaat.puissanceAttaque);
                col.GetComponent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(soPlumeMaat.puissanceAttaque),soPlumeMaat.stagger);
            }
            
        }
    }
}
