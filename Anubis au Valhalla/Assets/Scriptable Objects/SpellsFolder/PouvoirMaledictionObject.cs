using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pouvoir Malédiction" ,menuName = "Spell System/SpellObject/Pouvoir Malédiction")]
public class PouvoirMaledictionObject : SpellObject
{
   
    public bool canCast;
    public float duration;
    public GameObject hitboxAttaqueNormale;
    public GameObject hitboxThrust;
    public GameObject hitboxDash;

    [Header("ATTAQUE NORMALE")]
    public float forceAttraction;
    public float forceDuration;

    [Header("ATTAQUE DASH")] 
    public float dashSpawnRate;
    public int dashDamage;
    public float timeToStep2;
    public float staggerDash;
    public float dashHitboxDuration;
  
    [Header("ATTAQUE Thrust")]
    public int thrustDamage;
    public float bulletSpeed;
    public float bulletDuration;
    public float nuberOfBullets;
    public float staggerThrust;
    public Vector3 bulletScale;
    public void Awake()
    {
        type = SpellType.PouvoirMalediction;
    }
}
