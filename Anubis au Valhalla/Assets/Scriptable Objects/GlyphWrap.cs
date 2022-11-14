using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

[Serializable]
public class GlyphWrap
{
    public enum State
    {
        Inactive,
        Active,
        OutLeveled, //pour les glyphes de niveau supérieur
        Overridden, //pour les éléments (vu qu'ils peuvent pas se stack)
    }
    
    public GlyphObject glyphObject;
    public State gState = State.Inactive;
}

