using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pouvoir Ame" ,menuName = "Spell System/SpellObject/Pouvoir Ame")]
public class PouvoirAmeObject : SpellObject
{
    public bool canCast;
    public float duration;
    public GameObject hitboxAttaqueNormale;
    public GameObject hitboxThrust;
    public GameObject hitboxDash;
    public float stagger;

    [Header("ATTAQUE NORMALE")]
    public int attaqueNormaleDamage;
    public float attaqueNormaleDuration;

    [Header("ATTAQUE DASH")]
    public float staggerThrust;
    public float hitboxthrustDuration;
    public int thrustDamage;
    public Vector3 explosionScale;
    
    [Header("ATTAQUE Thrust")]
    public int zoneAmount;
    public int moucheAmount;
    public int hitboxDashDuration = 2;
    public int dashPuissanceAttaque = 5;
    public float espacementDoT = 2f;
    public Vector3 zoneScale;
   
    public void Awake()
    {
        type = SpellType.PouvoirAme;
    }
}
