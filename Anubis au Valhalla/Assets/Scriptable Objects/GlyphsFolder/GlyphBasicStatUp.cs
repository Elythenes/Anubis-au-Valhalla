using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Basic Stat Up" ,menuName = "Glyph System/GlyphObject/BasicStatUp")]
public class GlyphBasicStatUp : GlyphObject
{
    [Header("BASIC STAT UP")]
    public AnubisStat anubisStat = AnubisStat.None;
    public float bonusBasicStat = 5f;

    //public bool isElemental = false; //si nos Glyphes qui ajoutent un élément sont trop faibles sans stats up
}