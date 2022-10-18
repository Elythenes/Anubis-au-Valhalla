using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Throwing Spell" ,menuName = "Spell System/SpellObject/Throwing")]

public class SpellThrowingObject : SpellObject
{
    public int duration = 2;
    public float bulletSpeed;
    public int puissanceAttaque;
    [HideInInspector] public float cooldownTimer;
    public float cooldown;
    public bool canCast;
    
    public void Awake()
    {
        type = SpellType.Throwing;
    }
}
