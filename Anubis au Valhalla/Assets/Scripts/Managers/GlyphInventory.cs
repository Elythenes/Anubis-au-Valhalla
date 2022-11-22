using System;
using System.Collections;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class GlyphInventory : MonoBehaviour
{
    public List<GlyphObject> glyphInventory;

    public GlyphObject gOTest;

    private int gLevelForLame = 100;
    private int gLevelForManche = 200;
    private int gLevelForHampe = 300;
    

    void Start()
    {
        AddGlyph(gOTest);
    }

    void AddGlyph(GlyphObject gO) //faire une fonction pour chaque type de glyphe avec un case plus tôt ?? (Félix : flemme)
    {
        GlyphWrap wrap = new GlyphWrap();                                               //définition d'une variable pour la fonction 
        
        wrap.glyphObject = gO;                                                          //attache du GlypheObject en soi
        wrap.gState = GlyphWrap.State.Active;                                           //attache de son state 
        Debug.Log("glyph set Active");

        if (gO.partie == GlyphObject.GlyphPart.Lame)
        {
            GlyphManager.Instance.arrayLame[gO.index-gLevelForLame] = wrap;            //assignation à la liste du GlyphManager de la Lame en enlevant la valeur de l'index en trop (ici 100)
            Debug.Log("Glyph added in Manager, array Lame, nom : " + wrap.glyphObject.nom);
            VerifyIfOutleveled(gO,GlyphManager.Instance.arrayLame, gLevelForLame);
        }
        else if (gO.partie == GlyphObject.GlyphPart.Manche)
        {
            GlyphManager.Instance.arrayLame[gO.index-gLevelForManche] = wrap;          //assignation à la liste du GlyphManager de la Lame en enlevant la valeur de l'index en trop (ici 200)
            Debug.Log("Glyph added in Manager, array Manche, nom : " + wrap.glyphObject.nom);
            VerifyIfOutleveled(gO,GlyphManager.Instance.arrayLame, gLevelForManche);
        }
        else if (gO.partie == GlyphObject.GlyphPart.Hampe)
        {
            GlyphManager.Instance.arrayLame[gO.index-gLevelForHampe] = wrap;           //assignation à la liste du GlyphManager de la Lame en enlevant la valeur de l'index en trop (ici 300)
            Debug.Log("Glyph added in Manager, array Hampe, nom : " + wrap.glyphObject.nom);
            VerifyIfOutleveled(gO,GlyphManager.Instance.arrayLame, gLevelForHampe);
        }
        else
        {
            Debug.Log("erreur dans l'ajout du glyphe");
            return;
        }
        AddInInventory(wrap.glyphObject);
        
    }
    
    void AddInInventory(GlyphObject glyphObject)
    {
        glyphInventory.Add(glyphObject);
    }

    void VerifyIfOutleveled(GlyphObject gO, GlyphWrap[] array, int indexConvertor)
    {
        if (gO.level == GlyphObject.GlyphLevel.MiddleLevel || gO.level == GlyphObject.GlyphLevel.MaximumLevel) //si le Glyphe obtenu est d'un niveau supérieur (>1) dans sa catégorie 
        {
            int compteur = 1;
            while(array[gO.index-indexConvertor-compteur].glyphObject.level == GlyphObject.GlyphLevel.MiddleLevel 
                  || array[gO.index-indexConvertor-compteur].glyphObject.level == GlyphObject.GlyphLevel.MinimumLevel) //regarde si y'a des Glyphes avec un tag Minimum ou MiddleLevel
            {
                //Debug.Log(compteur + "est le compteur");
                array[gO.index-indexConvertor-compteur].gState = GlyphWrap.State.Outleveled; //si oui, remplace en Outleveled tous les glyphes de la catégorie qui sont possiblement actif
                compteur += 1;
            }
        }
    }

    void VerifyIfOverriden()
    {
        
    }
}
