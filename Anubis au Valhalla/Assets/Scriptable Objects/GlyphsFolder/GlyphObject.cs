using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Glyph" ,menuName = "System/Glyph System/GlyphObject")]
public class GlyphObject : ScriptableObject
{
    [Header("GENERAL")]
    public string nom;
    [TextArea(6,20)] public string description;
    
    public GlyphPart partie;
    public int index;
    
    [Header("SHOP")]
    [Range(1, 3)] public int rarity = 1;
    public bool unique;
    public float price;
    
    [Header("GRAPH")] 
    public Texture icone;

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

    public enum AnubisStat
    {
        AnubisBaseDamage,
        AllComboDamage,
        Combo1Damage,
        Combo2Damage,
        Combo3Damage,
        ThrustDamage,
        Range,
        HealthPoint,
        Armor,
        Speed,
        DashCd,
        MagicForce,
        CriticalChances,
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
