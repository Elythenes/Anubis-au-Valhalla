using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spell System/SpellThrowingType")]

public class SpellThrowingType : ScriptableObject
{
    public GameObject fireball;
    public int duration = 2;
    public float bulletSpeed;
    public int puissanceAttaque;
    [HideInInspector] public float cooldownTimer;
    public float cooldown;
    public bool canCast;
}
