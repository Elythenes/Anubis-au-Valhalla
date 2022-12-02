using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pouvoir Plaie" ,menuName = "Spell System/SpellObject/Pouvoir Eau")]
public class PouvoirEauObject : SpellObject
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
    public int nbOfDotRayon;
    public float espacementDoTRayon;
    public float staggerRayon;
    public int rayonDamage;
    public Vector3 rayonScale;
    
    
    [Header("ATTAQUE Thrust")]
    public int thrustDamage;
    public float bulletSpeed;
    public float bulletDuration;
    public int bulletHeal;
    public float staggerThrust;
    public Vector3 bulletScale;
    public void Awake()
    {
        type = SpellType.PouvoirEau;
    }
}
