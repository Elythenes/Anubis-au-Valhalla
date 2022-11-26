using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pouvoir Feu" ,menuName = "Spell System/SpellObject/Pouvoir Feu")]

public class PouvoirFeuObject : SpellObject
{
  public bool canCast;
  public float duration;
  public GameObject hitboxAttaqueNormale;
  public GameObject hitboxThrust;
  public GameObject hitboxDash;

  [Header("ATTAQUE NORMALE")]
  public int attaqueNormaleDamage;
  
  [Header("ATTAQUE DASH")]
  public float dashSpawnRate;
  public int hitboxDashDuration = 2;
  public int dashPuissanceAttaque = 5;
  public int nombreOfDot = 4;
  public float espacementDoT = 2f;
  public float stagger;
  public Vector3 zoneScale;
  
  [Header("ATTAQUE Thrust")]
  public int thrustDamage;
  public float bulletSpeed;
  public float bulletDuration;
  public Vector3 bulletScale;
  public void Awake()
  {
    type = SpellType.PouvoirFeu;
  }
}
