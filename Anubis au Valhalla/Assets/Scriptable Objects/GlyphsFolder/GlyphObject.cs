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
    [TextArea(6,20)] public string description;
    //[TextArea(5, 10)] public string citation;
    //public GlyphElement element;
    [Range(1, 4)] public int tier = 1;
    //public GlyphEffect effectType;
    
    [BoxGroup("PRICE")] public float lowerPriceRange;
    [BoxGroup("PRICE")] public float upperPriceRange;
    [BoxGroup("GRAPH")] public Texture icone;
    [BoxGroup("GRAPH")] public Texture iconeElement; // (visible à côté / dans l'icone) pour indiquer l'élément dans le Shop
    [BoxGroup("GRAPH")] public Texture fondElement; // (visible dans l'Inventaire) pour savoir s'il est associé à un élément 

    [BoxGroup("SHOW VALUES")] public bool isBasicStatUp;
    [BoxGroup("SHOW VALUES")] public bool isSituationalStatUp;
    [BoxGroup("SHOW VALUES")] public bool isSpecialStatUp;
    [BoxGroup("SHOW VALUES")] public bool isAdditionalEffect;
    [BoxGroup("SHOW VALUES")] public bool isTriggerEffect;
    [BoxGroup("SHOW VALUES")] public bool isChargeBased;
    [BoxGroup("SHOW VALUES")] public bool isTimeBased;
    [BoxGroup("SHOW VALUES")] public bool isBoolEffect;
    [BoxGroup("SHOW VALUES")] public bool isOther;

    [ShowIf("isBasicStatUp")] [BoxGroup("BASIC STAT UP")] public AnubisStat anubisStat = AnubisStat.NoneAnubisStat;
    [ShowIf("isBasicStatUp")] [BoxGroup("BASIC STAT UP")] public float bonusBasicStat = 5;
    [ShowIf("isBasicStatUp")] [BoxGroup("BASIC STAT UP")] public AnubisStat otherStat = AnubisStat.NoneAnubisStat;
    [ShowIf("isBasicStatUp")] [BoxGroup("BASIC STAT UP")] public float otherBonusBasicStat = 0;
    
    [ShowIf("isSituationalStatUp")] [BoxGroup("SITUATIONAL STAT UP")] public AnubisStat situationalStat = AnubisStat.NoneAnubisStat;
    [ShowIf("isSituationalStatUp")] [BoxGroup("SITUATIONAL STAT UP")] public float bonusSituationalStat = 0;
    [ShowIf("isSituationalStatUp")] [BoxGroup("SITUATIONAL STAT UP")] public bool dependsOnSoulCount;

    [ShowIf("isSpecialStatUp")] [BoxGroup("SPECIAL STAT UP")] public SpecialEffect specialStat = SpecialEffect.NoneSpecialEffect;
    [ShowIf("isSpecialStatUp")] [BoxGroup("SPECIAL STAT UP")] public float specialStatValue;
    
    //[BoxGroup("ELEMENTAL")] public GlyphElement glyphElement;
    
    [ShowIf("isAdditionalEffect")] [BoxGroup("ADDITIONAL EFFECT")] public SpecialEffect specialAdditionalEffect = SpecialEffect.NoneSpecialEffect;
    [ShowIf("isAdditionalEffect")] [BoxGroup("ADDITIONAL EFFECT")] public float specialAdditionalValue;
    
    [ShowIf("isTriggerEffect")] [BoxGroup("TRIGGER EFFECT/general")] public TriggerMove triggerMove = TriggerMove.NoneTriggerMove;
    [ShowIf("isTriggerEffect")] [BoxGroup("TRIGGER EFFECT/general")] public string revokeTrigger;
    [ShowIf("isTriggerEffect")] [BoxGroup("TRIGGER EFFECT/basic stat")] public AnubisStat triggerStat = AnubisStat.NoneAnubisStat;
    [ShowIf("isTriggerEffect")] [BoxGroup("TRIGGER EFFECT/basic stat")] public float additionalDamage = 0;
    [ShowIf("isTriggerEffect")] [BoxGroup("TRIGGER EFFECT/special")] public SpecialEffect specialTriggerEffect = SpecialEffect.NoneSpecialEffect;
    [ShowIf("isTriggerEffect")] [BoxGroup("TRIGGER EFFECT/special")] public float specialTriggerValue;

    [ShowIf("isChargeBased")] [BoxGroup("CHARGE BASED")] public int chargeBase = 0;
    [ShowIf("isChargeBased")] [BoxGroup("CHARGE BASED")] public int chargeNumber = 10;

    [ShowIf("isTimeBased")] [BoxGroup("TIME BASED")] public TimeType timeType = TimeType.NoneTimeType;
    [ShowIf("isTimeBased")] [BoxGroup("TIME BASED")] public float periodicTimeValue;

    [ShowIf("isBoolEffect")] [BoxGroup("BOOL EFFECT")] public bool startsActive = true;
    [ShowIf("isBoolEffect")] [BoxGroup("BOOL EFFECT")] public string boolTrigger;
    [ShowIf("isBoolEffect")] [BoxGroup("BOOL EFFECT")] public string revokeBool;

    [ShowIf("isOther")] [BoxGroup("OTHER")] public string otherEffect;
    
    
    public enum GlyphPart
    {
        Lame,
        Manche,
        Poignee
    }

    public enum GlyphEffect
    {
        BasicStatUp,
        SituationalStatUp,
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
        MagicForce,
        InvincibilityFrames,
        NoneAnubisStat
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
        TakeDamage,
        Death,
        NoneTriggerMove
    }

    public enum SpecialEffect
    {
        AdditionalDamage,
        Regeneration, 
        Knockback,
        Shield,
        Stagger,
        SummonBeetle,
        SummonLocusts,
        SummonFlies,
        SoulBlessing,
        ChangeEnemyDrop,
        ReflectProjectiles,
        NoneSpecialEffect
    }

    public enum TimeType
    {
        Continuous,
        Periodic,
        NoneTimeType
    }
}
