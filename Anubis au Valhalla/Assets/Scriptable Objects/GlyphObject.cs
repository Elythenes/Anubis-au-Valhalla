using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Glyph" ,menuName = "Glyph System/GlyphObject")]
public class GlyphObject : ScriptableObject
{
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
        AfterSmthEffect,
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
    
    [Header("GENERAL")]
    public string gNom;
    public GlyphPart gPartie;
    public GlyphType gType;
    public int gIndex;
    [TextArea(10,20)] public string gDescription;
    //[TextArea(5, 10)] public string gCitation;
    public GlyphElement gElement;

    [Header("GRAPH")]
    public Texture gIcone;
    public Texture gIconeElement; // (visible à côté / dans l'icone) pour indiquer l'élément dans le Shop
    public Texture gFondElement; // (visible dans l'Inventaire) pour savoir s'il est associé à un élément 


}
