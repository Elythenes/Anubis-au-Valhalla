using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pouvoir Plaie" ,menuName = "Spell System/SpellObject/Pouvoir Plaie")]


public class PouvoirPlaieObject : SpellObject
{
    public bool canCast;
    public float duration;
    public GameObject hitboxAttaqueNormale;
    public GameObject hitboxThrust;
    public GameObject hitboxDash;

    [Header("ATTAQUE NORMALE")]
    public int damageAttaqueNormale;
    public float durationAttaqueNormale;
    public float durationStunAttaqueNormale;
    public float staggerAttaqueNormale;

    [Header("ATTAQUE DASH")] 
    public float dashHitboxDuration;
    public float dashSpawnRate;
    public int dashDamage;
    public float timeToStep2;
    public float staggerDash;
    public float wallDuration;
  
    [Header("ATTAQUE Thrust")]
    public int thrustDamage;
    public float bulletSpeed;
    public float bulletDuration;
    public float nuberOfBullets;
    public float staggerThrust;
    public float dureeMomification;
    public Vector3 bulletScale;
    public void Awake()
    {
        type = SpellType.PouvoirPlaie;
    }
}
