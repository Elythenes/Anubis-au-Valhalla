using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnEntity Spell" ,menuName = "System/Spell System/SpellObject/SpawnEntity")]

public class SpellSpawnEntityObject : SpellObject
{
   public int duration = 2;
   public int puissanceAttaque = 5;
   public int nombreOfDot = 4;
   public float espacementDoT = 2f;
   [HideInInspector] public float cooldownTimer;
   public float timerStep2;
   public float cooldown = 1f;
   public float maxDistanceSpawn; 
   public bool canCast;
   public float stagger;
    
   public void Awake()
   {
      type = SpellType.SpawnEntity;
   }
}
