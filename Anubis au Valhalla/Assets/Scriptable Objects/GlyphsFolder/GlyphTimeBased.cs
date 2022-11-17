using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Time Based" ,menuName = "Glyph System/GlyphObject/TimeBased")]
public class GlyphTimeBased : GlyphObject
{
    [Header("TIME BASED")]
    public float cooldownBeforeEffect = 5f;
}
