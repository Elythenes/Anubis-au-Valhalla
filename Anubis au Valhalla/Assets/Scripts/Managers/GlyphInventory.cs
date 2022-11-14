using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlyphInventory : MonoBehaviour
{
    public List<GlyphObject> glyphInventory;

    public GlyphObject gOTest;

    void Start()
    {
        AddGlyph(gOTest);
    }

    void AddGlyph(GlyphObject gO) //faire une fonction pour chaque type de glyphe avec un case plus tôt 
    {
        GlyphWrap wrap = new GlyphWrap();
        
        wrap.glyphObject = gO;
        wrap.gState = GlyphWrap.State.Active;
        Debug.Log("glyph set Active");

        if (gO.gPartie == GlyphObject.GlyphPart.Lame)
        {
            GlyphManager.instance.arrayLame[gO.gIndex-100] = wrap;
            Debug.Log("glyph in Manager, array Lame, nom : " + wrap.glyphObject.gNom);
        }
        else if (gO.gPartie == GlyphObject.GlyphPart.Manche)
        {
            GlyphManager.instance.arrayLame[gO.gIndex-200] = wrap;
            Debug.Log("glyph in Manager, array Manche, nom : " + wrap.glyphObject.gNom);
        }
        else if (gO.gPartie == GlyphObject.GlyphPart.Hampe)
        {
            GlyphManager.instance.arrayLame[gO.gIndex-300] = wrap;
            Debug.Log("glyph in Manager, array Hampe, nom : " + wrap.glyphObject.gNom);
        }
        else
        {
            Debug.Log("erreur dans l'ajout du glyphe");
            return;
        }
        AddInInventory(wrap.glyphObject);
        VerifyIfOutleveled(gO,GlyphManager.instance.arrayLame);
    }
    
    void AddInInventory(GlyphObject glyphObject)
    {
        glyphInventory.Add(glyphObject);
        Debug.Log("New glyph added in Inventory. Name is " + glyphObject.gNom);
    }

    void VerifyIfOutleveled(GlyphObject gO, GlyphWrap[] array)
    {
        if (gO.gLevel == GlyphObject.GlyphLevel.MiddleLevel)
        { 
            array[gO.gIndex-100-1].gState = GlyphWrap.State.Outleveled;
        }
        else if (gO.gLevel == GlyphObject.GlyphLevel.MaximumLevel)
        {
            
        }
    }

    void VerifyIfOverriden()
    {
        
    }
}
