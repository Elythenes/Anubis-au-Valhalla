using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spell System/SpellFollowingAreaType")]

public class SpellFollowingAreaType : ScriptableObject
{
    //public GameObject sandstorm;
    public int duration = 2;
    public int puissanceAttaque = 5;
    public int numberOfDot = 4;
    public float espacementDoT = 2;
    [HideInInspector] public float cooldownTimer;
    public float cooldown = 5;
    public bool canCast;
}
