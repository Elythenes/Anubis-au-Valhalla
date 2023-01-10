using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticArea Spell" ,menuName = "System/Spell System/SpellObject/StaticArea")]

public class SpellStaticAreaObject : SpellObject
{
    public int duration = 2;
    public int puissanceAttaque = 5;
    public int nombreOfDot = 4;
    public float espacementDoT = 2f;
    [HideInInspector] public float cooldownTimer;
    public float cooldown = 1f;
    public bool canCast;
    public float stagger;
    
    public void Awake()
    {
        type = SpellType.StaticArea;
    }
}
