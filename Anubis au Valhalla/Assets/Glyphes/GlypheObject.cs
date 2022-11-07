using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Glyphe" ,menuName = "Glyphe System/GlypheObject")]

public class GlypheObject : ScriptableObject
{
    [Header("GENERAL")]
    public string nom;
    public int glypheIndex;
    [TextArea(10,20)] public string description;

    [Header("GRAPH")]
    public Texture icone;
    
    
}
