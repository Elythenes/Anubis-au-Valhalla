using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Glyph" ,menuName = "Glyph System/GlyphObject")]
public class GlyphObject : ScriptableObject
{
    [Header("GENERAL")]
    public string nom;
    public GlyphPart partie;
    //public GlyphType type;
    public GlyphLevel level;
    public int index;
    [TextArea(10,20)] public string description;
    //[TextArea(5, 10)] public string citation;
    //public GlyphElement element;
    [Range(1, 4)] public int tier = 1;
    
    [Header("GRAPH")]
    public Texture icone;
    public Texture iconeElement; // (visible à côté / dans l'icone) pour indiquer l'élément dans le Shop
    public Texture fondElement; // (visible dans l'Inventaire) pour savoir s'il est associé à un élément 

    [Header("BASIC STAT UP")] 
    public bool isBasicStatUp = false;
    public AnubisStat anubisStat = AnubisStat.None;
    public float bonusBasicStat = 5f;
    
    [Header("SITUATIONAL STAT UP")]
    public bool isSituationStatUp = false;
    public int valeurPourLeHeader1;
    
    [Header("ELEMENTAL")] 
    public bool isElemental = false;
    public GlyphElement glyphElement;
    
    [Header("ADDITIONAL EFFECT")]
    public bool isAdditionalEffect = false;
    public int valeurPourLeHeader2;
    
    [Header("TRIGGER EFFECT")]
    public bool isTriggerEffect = false;
    public bool isTriggerActive = false;
    
    [Header("CHARGE BASED")] 
    public bool isCharge = false;
    public int chargeBase = 0;
    public int chargeNumber = 10;

    [Header("TIME BASED")] 
    public bool isTimeBased = false;
    public float cooldownBeforeEffect = 5f;
    
    [Header("BOOL EFFECT")]
    public bool isBoolEffect = false;
    public bool isEffectActive = true;
    
    
    public enum GlyphPart
    {
        Lame,
        Manche,
        Hampe
    }

    public enum GlyphType
    {
        BasicStatUp = 1,
        SituationalStatUp = 2,
        Elemental = 3,
        AdditionalEffect,
        TriggerEffect,
        Charge,
        TimeBased,
        BoolEffect,
        Others
    }

    public enum GlyphElement
    {
        None,
        Bandage,
        Water,
        Fire,
        SpiritualFire,
        Thunder,
        Curse,
        Sand,
        Bleeding,
        Wind
    }

    public enum GlyphLevel
    {
        Unique,
        MinimumLevel,
        MiddleLevel,
        MaximumLevel
    }
    
    public enum AnubisStat
    {
        None,
        AnubisBaseDamage,
        LameDamage,
        SwingDamage,
        ThrustDamage,
        SmashDamage,
        Range,
        AttackSpeed,
        Knockback,
        HealthPoint,
        Defense,
        Speed,
        DashRange,
        DashCd,
        MagicForce
    }
    
}
