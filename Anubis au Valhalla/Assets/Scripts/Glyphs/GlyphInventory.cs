using System;
using System.Collections;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class GlyphInventory : MonoBehaviour
{
    public static GlyphInventory Instance;
    
    [Expandable] public List<GlyphObject> glyphInventory;

    public bool doStartHieroTest;
    [Expandable] public List<GlyphObject> hieroTest;

    
    //Fonction : Systèmes *******************************************************************************************************************
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    void Start()
    {
        if (doStartHieroTest)
        {
            foreach (var hiero in hieroTest)
            {
                AddNewGlyph(hiero);
            }
        }
    }
    
    
    public void AddNewGlyph(GlyphObject hiero)
    {
        Debug.Log("call AddNewGlyph");

        switch (hiero.partie)
        {
            case GlyphObject.GlyphPart.Lame:
                GlyphManager.Instance.listLame.Add(hiero);              //ajoute dans la liste en fonction de la partie de l'arme affecté par la glyphe
                GlyphManager.Instance.ActiveGlyphInManager(hiero);      //update les variables qui sont liée au glyphe en fonction de son type d'effet
                glyphInventory.Add(hiero);                              //ajoute dans l'inventaire pour l'UI
                break;
            
            case GlyphObject.GlyphPart.Manche:
                GlyphManager.Instance.listManche.Add(hiero);
                GlyphManager.Instance.ActiveGlyphInManager(hiero);
                glyphInventory.Add(hiero);
                break;
            
            case GlyphObject.GlyphPart.Poignee:
                GlyphManager.Instance.listPoignee.Add(hiero);
                GlyphManager.Instance.ActiveGlyphInManager(hiero);
                glyphInventory.Add(hiero);
                break;
            
            default:
                Debug.LogError("Frérot, t'as pas mis de partie associée à la glyphe : " + hiero.name + ".");
                break;
        }
    }








    
    
    
    
    
    
    
    
    
    /*
     [NaughtyAttributes.ReadOnly] public int indexConvertorForLame = 100;
    [NaughtyAttributes.ReadOnly] public int indexConvertorForManche = 200;
    [NaughtyAttributes.ReadOnly] public int indexConvertorPoignee = 300;
     
     
     public void AddGlyph(GlyphObject hiero) 
    {
        GlyphWrap wrap = new GlyphWrap();                                               //définition d'une variable pour la fonction qui va contenir le glyphe et sa State
        wrap.glyphObject = hiero;                                                       //attache du GlypheObject en soi
        wrap.hieroState = GlyphWrap.State.Active;                                       //attache de son state 
        Debug.Log("glyph set Active");

        if (hiero.partie == GlyphObject.GlyphPart.Lame)
        {
            GlyphManager.Instance.arrayLame[hiero.index-indexConvertorForLame] = wrap;            //assignation à la liste du GlyphManager de la Lame en enlevant la valeur de l'index en trop (ici 100)
            GlyphManager.Instance.ActiveGlyphInManager(hiero);                                    //ajout dans le Manager pour que les stats soient update
            glyphInventory.Add(hiero);                                                            //ajout dans l'inventaire qui pourra être consulté
            Debug.Log("Glyph added in LameManager, nom : " + wrap.glyphObject.nom);
            GlyphManager.Instance.indexActiveGlyphs.Add((hiero.index));
        }
        else if (hiero.partie == GlyphObject.GlyphPart.Manche)
        {
            GlyphManager.Instance.arrayManche[hiero.index-indexConvertorForManche] = wrap;
            GlyphManager.Instance.ActiveGlyphInManager(hiero); 
            glyphInventory.Add(hiero); 
            Debug.Log("Glyph added in MancheManager, nom : " + wrap.glyphObject.nom);
        }
        else if (hiero.partie == GlyphObject.GlyphPart.Poignee)
        {
            GlyphManager.Instance.arrayPoignee[hiero.index-indexConvertorPoignee] = wrap;
            GlyphManager.Instance.ActiveGlyphInManager(hiero);
            glyphInventory.Add(hiero);
            Debug.Log("Glyph added in PoigneeManager, nom : " + wrap.glyphObject.nom);
        }
        else
        {
            Debug.Log("erreur dans l'ajout du glyphe");
            return;
        }
        
    }*/
    
    
    
    /*
    void AddInInventory(GlyphObject glyphObject)
    {
        glyphInventory.Add(glyphObject);
    }
    

    void VerifyIfOutleveled(GlyphObject hiero, GlyphWrap[] array, int indexConvertor)
    {
        if (hiero.level == GlyphObject.GlyphLevel.MiddleLevel || hiero.level == GlyphObject.GlyphLevel.MaximumLevel) //si le Glyphe obtenu est d'un niveau supérieur (>1) dans sa catégorie 
        {
            int compteur = 1;
            while(array[hiero.index-indexConvertor-compteur].glyphObject.level == GlyphObject.GlyphLevel.MiddleLevel 
                  || array[hiero.index-indexConvertor-compteur].glyphObject.level == GlyphObject.GlyphLevel.MinimumLevel) //regarde si y'a des Glyphes avec un tag Minimum ou MiddleLevel
            {
                //Debug.Log(compteur + "est le compteur");
                array[hiero.index-indexConvertor-compteur].hieroState = GlyphWrap.State.Outleveled; //si oui, remplace en Outleveled tous les glyphes de la catégorie qui sont possiblement actif
                compteur += 1;
            }
        }
    }

    void VerifyIfOverriden()
    {
        
    }
    
    */
    
}