using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Glyph" ,menuName = "Glyph System/GlyphObject")]
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
    

    [Header("GRAPH")]
    public Texture icone;
    public Texture iconeElement; // (visible à côté / dans l'icone) pour indiquer l'élément dans le Shop
    public Texture fondElement; // (visible dans l'Inventaire) pour savoir s'il est associé à un élément 

    public enum GlyphPart
    {
        Lame,
        Manche,
        Hampe
    }

    public enum GlyphType
    {
        BasicStatUp,
        SituationalStatUp,
        Elemental,
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
