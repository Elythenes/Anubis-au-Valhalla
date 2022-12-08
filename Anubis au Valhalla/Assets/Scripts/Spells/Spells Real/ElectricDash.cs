using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricDash : MonoBehaviour
{
    public PouvoirFoudreObject soPouvoirFoudre;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Monstre"))
        {
            col.GetComponentInParent<MonsterLifeManager>().DamageText(soPouvoirFoudre.dashPuissanceAttaque);
            col.GetComponentInParent<MonsterLifeManager>().TakeDamage(soPouvoirFoudre.dashPuissanceAttaque, soPouvoirFoudre.staggerDash);
        }
    }
}
