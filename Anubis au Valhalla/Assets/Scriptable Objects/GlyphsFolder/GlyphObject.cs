using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Glyph" ,menuName = "System/Glyph System/GlyphObject")]
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
    
    /*[BoxGroup("Glyph Type")] public bool isBasicStatUp = false;
    [BoxGroup("Glyph Type")] public bool isSituationStatUp = false;
    //[BoxGroup("Glyph Type")] public bool isElemental = false;
    [BoxGroup("Glyph Type")] public bool isAdditionalEffect = false;
    [BoxGroup("Glyph Type")] public bool isTriggerEffect = false;
    [BoxGroup("Glyph Type")] public bool isCharge = false;
    [BoxGroup("Glyph Type")] public bool isTimeBased = false;
    [BoxGroup("Glyph Type")] public bool isBoolEffect = false;
    [BoxGroup("Glyph Type")] public bool isOther = false;*/

    public GlyphEffect effectType;
    [Foldout("PRICE")] public float lowerPriceRange;
    [Foldout("PRICE")] public float upperPriceRange;
    [Foldout("GRAPH")] public Texture icone;
    [Foldout("GRAPH")] public Texture iconeElement; // (visible à côté / dans l'icone) pour indiquer l'élément dans le Shop
    [Foldout("GRAPH")] public Texture fondElement; // (visible dans l'Inventaire) pour savoir s'il est associé à un élément 
    
    [Foldout("BASIC STAT UP")] public AnubisStat anubisStat = AnubisStat.None;
    [Foldout("BASIC STAT UP")] public int bonusBasicStat = 5;
    [Foldout("BASIC STAT UP")] public AnubisStat otherStat = AnubisStat.None;
    [Foldout("BASIC STAT UP")] public int otherBonusBasicStat = 0;
    
    [Foldout("SITUATIONAL STAT UP")] public AnubisStat situationalStat = AnubisStat.None;
    [Foldout("SITUATIONAL STAT UP")] public int bonusSituationalStat = 0;
    
    //[Foldout("ELEMENTAL")] public GlyphElement glyphElement;
    
    [Foldout("ADDITIONAL EFFECT")] public int valeurPourLeHeader2;
    
    [Foldout("TRIGGER EFFECT")] public bool isTriggerActive = false;

    [Foldout("CHARGE BASED")] public int chargeBase = 0;
    [Foldout("CHARGE BASED")] public int chargeNumber = 10;

    [Foldout("TIME BASED")] public float cooldownBeforeEffect = 5f;

    [Foldout("BOOL EFFECT")] public bool isEffectActive = true;

    [Foldout("BOOL EFFECT")] public float pourLeOther;
    
    
    public enum GlyphPart
    {
        Lame,
        Manche,
        Poignee
    }

    public enum GlyphEffect
    {
        BasicStatUp,
        SituaionalStatUp,
        SpecialStat,
        //Elemental,
        AdditionalDamage,
        TriggerEffect,
        Charge,
        TimeBased,
        BoolEffect,
        Others
    }

    /*public enum GlyphElement
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
    }*/

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
        AllComboDamage,
        Combo1Damage,
        Combo2Damage,
        Combo3Damage,
        ThrustDamage,
        Range,
        //AttackSpeed,
        //Knockback,
        HealthPoint,
        Armor,
        Speed,
        DashCd,
        //MagicForce
    }
    
}
