using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Glyph" ,menuName = "Glyph System/GlyphObject")]

public class GlyphObject : ScriptableObject
{
    [Header("GENERAL")]
    public string nom;
    public int glyphIndex;
    [TextArea(10,20)] public string description;

    [Header("GRAPH")]
    public Texture icon;
    
    
}
