using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Throwing Spell" ,menuName = "Spell System/SpellObject/Throwing")]

public class SpellThrowingObject : SpellObject
{
    public int duration = 2;
    public float bulletSpeed;
    public int puissanceAttaque;
    public float cooldownTimer;
    public float cooldown;
    public bool canCast;
    public float DebuffTime;
    public float stagger;
    
    public void Awake()
    {
        type = SpellType.Throwing;
    }
}
