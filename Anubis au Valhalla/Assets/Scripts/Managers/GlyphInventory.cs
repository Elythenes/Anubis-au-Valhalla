using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlyphInventory : MonoBehaviour
{
    public List<GlyphObject> glyphInventory;

    void Update()
    {
        
    }

    void AddInInventory(GlyphObject glyphObject)
    {
        glyphInventory.Add(glyphObject);
        Debug.Log("New glyph added in Inventory. Name is " + glyphObject.gNom);
    }
    
}
