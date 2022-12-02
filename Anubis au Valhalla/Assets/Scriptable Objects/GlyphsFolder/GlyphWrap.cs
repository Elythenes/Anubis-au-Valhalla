using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

[Serializable]
public class GlyphWrap
{
    public enum State
    {
        Inactive,
        Active,
        Outleveled //pour les glyphes de niveau supérieur
    }
    
    [Expandable] public GlyphObject glyphObject;
    public State hieroState = State.Inactive;
}

