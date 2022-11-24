using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PotionType
{
    InstantEffect,
    EffectOverTime
}
public abstract class PotionObject : ScriptableObject
{
    [Header("GENERAL")]
    public PotionType type;
    public string nom;
    public int potionIndex;
    [TextArea(10,20)] public string description;
    public int buffAmount;
    public int buffDuration;
    public float cooldownTimer;
    public float cooldown;
    public bool canCast;
    public int valeurRestored;
    
    [Header("GRAPH")]
    public Texture sprite;
}
