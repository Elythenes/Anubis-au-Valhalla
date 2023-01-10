using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoueBehaviour : MonoBehaviour
{
    public PouvoirPlaieObject soPlaie;
    private void Start()
    {
        Destroy(gameObject, soPlaie.dashHitboxDuration);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
       
        if (col.gameObject.tag == "Monstre")
        {
            col.GetComponentInParent<MonsterLifeManager>().isEnvased = true;
        }
    }
}
