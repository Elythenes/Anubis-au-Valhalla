using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spell System/SpellStaticAreaType")]

public class SpellStaticAreaType : ScriptableObject
{
    //public GameObject flameArea;
    public int duration = 2;
    public int puissanceAttaque = 5;
    public int nombreOfDot = 4;
    public float espacementDoT = 2f;
    [HideInInspector] public float cooldownTimer;
    public float cooldown = 1f;
    public bool canCast;
}
