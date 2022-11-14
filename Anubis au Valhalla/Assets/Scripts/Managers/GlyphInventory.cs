using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GlyphInventory : MonoBehaviour
{
    public List<GlyphObject> glyphInventory;

    void Update()
    {
        
    }

    GlyphWrap AddGlyph(GlyphObject glyphObject, GlyphWrap.State state)
    {
        GlyphWrap wrap = new GlyphWrap();

        if (glyphObject.gType == GlyphObject.GlyphType.BasicStatUp)
        {
            state = GlyphWrap.State.Active;
        }
        
        return wrap;
    }
    
    void AddInInventory(GlyphObject glyphObject)
    {
        glyphInventory.Add(glyphObject);
        Debug.Log("New glyph added in Inventory. Name is " + glyphObject.gNom);
    }
    
}
