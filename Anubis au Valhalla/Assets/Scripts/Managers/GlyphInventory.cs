using System;
using System.Collections;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class GlyphInventory : MonoBehaviour
{
    public List<GlyphObject> glyphInventory;

    [Expandable] public GlyphObject gOTest;

    private int gLevelForLame = 100;
    private int gLevelForManche = 200;
    private int gLevelForPoignee = 300;
    

    void Start()
    {
        AddGlyph(gOTest);
    }

    public void AddGlyph(GlyphObject gO) 
    {
        GlyphWrap wrap = new GlyphWrap();                                               //définition d'une variable pour la fonction qui va contenir le glyphe et sa State
        wrap.glyphObject = gO;                                                          //attache du GlypheObject en soi
        wrap.gState = GlyphWrap.State.Active;                                           //attache de son state 
        Debug.Log("glyph set Active");

        if (gO.partie == GlyphObject.GlyphPart.Lame)
        {
            GlyphManager.Instance.arrayLame[gO.index-gLevelForLame] = wrap;            //assignation à la liste du GlyphManager de la Lame en enlevant la valeur de l'index en trop (ici 100)
            Debug.Log("Glyph added in LameManager, nom : " + wrap.glyphObject.nom);
            VerifyIfOutleveled(gO,GlyphManager.Instance.arrayLame, gLevelForLame);
        }
        else if (gO.partie == GlyphObject.GlyphPart.Manche)
        {
            GlyphManager.Instance.arrayLame[gO.index-gLevelForManche] = wrap;          //assignation à la liste du GlyphManager de la Lame en enlevant la valeur de l'index en trop (ici 200)
            Debug.Log("Glyph added in MancheManager, nom : " + wrap.glyphObject.nom);
            VerifyIfOutleveled(gO,GlyphManager.Instance.arrayManche, gLevelForManche);
        }
        else if (gO.partie == GlyphObject.GlyphPart.Poignee)
        {
            GlyphManager.Instance.arrayLame[gO.index-gLevelForPoignee] = wrap;           //assignation à la liste du GlyphManager de la Lame en enlevant la valeur de l'index en trop (ici 300)
            Debug.Log("Glyph added in PoigneeManager, nom : " + wrap.glyphObject.nom);
            VerifyIfOutleveled(gO,GlyphManager.Instance.arrayPoignee, gLevelForPoignee);
        }
        else
        {
            Debug.Log("erreur dans l'ajout du glyphe");
            return;
        }
        glyphInventory.Add(gO);                                             //ajout dans l'inventaire qui pourra être consulté
        GlyphManager.Instance.AddGlyphToManager(gO);                        //ajout dans le Manager pour que les stats soient update
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
