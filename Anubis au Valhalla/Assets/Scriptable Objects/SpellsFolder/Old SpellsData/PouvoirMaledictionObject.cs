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
    public int damageAttaqueNormale;
    public float dureeAttaqueNormale;
    public float staggerAttaqueNormale;
    public Vector3 ScaleYAttaqueNormale;

    [Header("ATTAQUE DASH")]
    public int dashDamage;
    public float staggerDash;
    public float durationRefiled;

    [Header("ATTAQUE Thrust")]
    public int plumeDamage;
    public float staggerplume;
    public float plumeHitboxDuration;
 
    public void Awake()
    {
        type = SpellType.PouvoirMalediction;
    }
}
