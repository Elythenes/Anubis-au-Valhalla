using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FollowingArea Spell" ,menuName = "System/Spell System/SpellObject/DefenceShield")]


public class SpellDefenceObject : SpellObject
{
    public int reducteurDamage;
    public float secondesTotales;
    [HideInInspector] public float cooldownTimer;
    public float cooldown = 0;
    public bool canCast;
    
    public void Awake()
    {
        type = SpellType.DefenceShield;
    }
}
