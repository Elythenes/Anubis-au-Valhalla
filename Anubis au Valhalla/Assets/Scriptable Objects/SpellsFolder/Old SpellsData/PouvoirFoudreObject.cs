using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pouvoir Foudre" ,menuName = "Spell System/SpellObject/Pouvoir Foudre")]
public class PouvoirFoudreObject : SpellObject
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
    public Vector3 attaqueNormaleScale;
    public int nombreOfDot = 4;
    public float espacementDoT = 2f;

    [Header("ATTAQUE DASH")] 
    public float staggerDash;
    public float dashSpawnRate;
    public int dashPuissanceAttaque = 5;

    [Header("ATTAQUE Thrust")]
    public int thrustDamage;
    public float bulletSpeed;
    public float bulletDuration;
    public Vector3 bulletScale;
    public int maxBounce;
    public void Awake()
    {
        type = SpellType.PouvoirFoudre;
    }
}
