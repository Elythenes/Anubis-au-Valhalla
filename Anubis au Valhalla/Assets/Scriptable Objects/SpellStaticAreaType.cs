using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Spell System/SpellStaticAreaType")]

public class SpellStaticAreaType : ScriptableObject
{
    public string nom;
    public Image sprite;
    public GameObject flameArea;
    public int duration = 2;
    public int puissanceAttaque = 5;
    public int nombreOfDot = 4;
    public float espacementDoT = 2f;
    [HideInInspector] public float cooldownTimer;
    public float cooldown = 1f;
    public bool canCast;
}
