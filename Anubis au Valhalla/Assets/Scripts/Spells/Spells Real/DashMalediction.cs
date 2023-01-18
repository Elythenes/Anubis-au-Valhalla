using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMalediction : MonoBehaviour
{
    public PouvoirMaledictionObject soPouvoirMalediction;

    private void Start()
    {
        Destroy(gameObject,CharacterController.instance.dashDuration - CharacterController.instance.timerDash);
    }

    private void Update()
    {
        Debug.Log( SpellManager.instance.spellPMData.secondesRestantes);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            col.GetComponentInParent<MonsterLifeManager>().TakeDamage(soPouvoirMalediction.dashDamage, soPouvoirMalediction.staggerDash);
            
            if (col.GetComponentInParent<MonsterLifeManager>().vieActuelle <= 0)
            {
                SpellManager.instance.spellPMData.secondesRestantes += soPouvoirMalediction.durationRefiled;
            }
        }
    }
}
