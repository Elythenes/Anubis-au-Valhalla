using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FollowingArea Spell" ,menuName = "System/Spell System/SpellObject/FollowingArea")]

public class SpellFollowingAreaObject : SpellObject
{
    public int duration = 2;
    public int puissanceAttaque = 5;
    public int numberOfDot = 4;
    public float espacementDoT = 2;
    [HideInInspector] public float cooldownTimer;
    public float cooldown = 5;
    public bool canCast;
    public float stagger;
    
    public void Awake()
    {
        type = SpellType.FollowingArea;
    }
}
