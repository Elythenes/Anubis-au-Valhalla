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
    public float price;
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
    [BoxGroup("GRAPH")] public Texture icone;
    [BoxGroup("GRAPH")] public Texture iconeElement; // (visible à côté / dans l'icone) pour indiquer l'élément dans le Shop
    [BoxGroup("GRAPH")] public Texture fondElement; // (visible dans l'Inventaire) pour savoir s'il est associé à un élément 
    
    [BoxGroup("BASIC STAT UP")] public AnubisStat anubisStat = AnubisStat.None;
    [BoxGroup("BASIC STAT UP")] public float bonusBasicStat = 5;
    [BoxGroup("BASIC STAT UP")] public AnubisStat otherStat = AnubisStat.None;
    [BoxGroup("BASIC STAT UP")] public float otherBonusBasicStat = 0;
    
    [BoxGroup("SITUATIONAL STAT UP")] public AnubisStat situationalStat = AnubisStat.None;
    [BoxGroup("SITUATIONAL STAT UP")] public float bonusSituationalStat = 0;
    
    //[BoxGroup("ELEMENTAL")] public GlyphElement glyphElement;
    
    [BoxGroup("ADDITIONAL EFFECT")] public float valeurPourLeHeader2;
    
    [BoxGroup("TRIGGER EFFECT")] public bool isTriggerActive = false;
    [BoxGroup("TRIGGER EFFECT")] public AnubisStat triggerStat = AnubisStat.None;
    [BoxGroup("TRIGGER EFFECT")] public float additionalDamage = 0;
    [BoxGroup("TRIGGER EFFECT")] public TriggerMove triggerMove = TriggerMove.None;
    [BoxGroup("TRIGGER EFFECT")] public string revokeTrigger;
        
    [BoxGroup("CHARGE BASED")] public int chargeBase = 0;
    [BoxGroup("CHARGE BASED")] public int chargeNumber = 10;

    [BoxGroup("TIME BASED")] public float cooldownBeforeEffect = 5f;

    [BoxGroup("BOOL EFFECT")] public bool isEffectActive = true;
    [BoxGroup("BOOL EFFECT")] public string boolTrigger;

    [BoxGroup("OTHER")] public float pourLeOther;
    
    
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
        //MagicForce,
        None
    }

    public enum TriggerMove
    {
        Attack,
        AttackSomething,
        UseDash,
        PerformDodge,
        DrinkPotion,
        UsePower,
        LaunchSpell,
        None
        
    }
    
}
