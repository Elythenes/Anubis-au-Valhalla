using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class SarcophageBehaviour : MonoBehaviour
{
    public PouvoirPlaieObject soPlaie;
    private Vector2 _direction;


    void Start()
    {
        Destroy(gameObject,soPlaie.durationAttaqueNormale);
        transform.localRotation = new Quaternion(0, 0, 0, 0);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Monstre"))
        {
            MonsterLifeManager monstre = other.GetComponentInParent<MonsterLifeManager>();
            monstre.DamageText(soPlaie.damageAttaqueNormale);
            monstre.TakeDamage(soPlaie.thrustDamage, soPlaie.staggerAttaqueNormale);
            monstre.isMomified = true;
            monstre.MomifiedTime = soPlaie.durationStunAttaqueNormale;
        }
    }
}
