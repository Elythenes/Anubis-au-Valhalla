using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NaughtyAttributes;
using Unity.Collections;
using UnityEngine;

public class GlyphManager : MonoBehaviour
{
    public static GlyphManager Instance; //singleton
    private GlyphObject.GlyphType _gType;
    private GlyphObject.GlyphPart _gPart;
    
    [Header("LAME")]
    public GlyphWrap[] arrayLame = new GlyphWrap[5];
    [NaughtyAttributes.ReadOnly] public int swingDamage = AnubisCurrentStats.instance.damage[0];
    [NaughtyAttributes.ReadOnly] public int thrustDamage = AnubisCurrentStats.instance.damage[1];
    [NaughtyAttributes.ReadOnly] public int smashDamage = AnubisCurrentStats.instance.damage[2];
    
    [Header("MANCHE")] 
    public GlyphWrap[] arrayManche = new GlyphWrap[5];
    
    [Header("HAMPE")]
    public GlyphWrap[] arrayHampe = new GlyphWrap[5];
    
    
    
    //Fonctions Système ************************************************************************************************
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {

    }

    void Update()
    {
        UpdateActiveGlyphList(arrayLame);
    }
    
    
    //Fonctions des Glyphes ********************************************************************************************
    
    /*void TestGlyphTest1()
    {
        if (glyphTest1.gState == GlyphWrap.State.Active)
        {
            Debug.Log(glyphTest1.glyphObject.gNom + "est actif");
        }
    }*/

    List<int> UpdateActiveGlyphList(GlyphWrap[] array) //regarde dans une partie tous les Glyphes Actifs et donne une liste contenant leurs index
    {
        List<int> indexActive = new();
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].gState == GlyphWrap.State.Active)
            {
                indexActive.Add(array[i].glyphObject.index);
            }
        }
        return indexActive;
    }
    
    GlyphObject.GlyphType GlyphTypeConvertor()
    {
        GlyphObject.GlyphType gType = new GlyphObject.GlyphType();
        
        return gType;
    }

    void LameManager()
    {
        for (int i = 0; i < arrayLame.Length; i++)
        {
            if (arrayLame[i].gState == GlyphWrap.State.Active)
            {
                switch (_gType)
                {
                    case GlyphObject.GlyphType.BasicStatUp:
                        UpdateBasicStatUp(arrayLame[i].glyphObject);
                        break;
                }
            }
        }

        /*regarde si c'est Active
         * en fonction du type de Glyph (case ?)
         * fait une fonction type (pour les BSU, ajouter à la bonne stat la valeur du SO)
         * BSU : case pour la catégorie de la stat focus
         * ajouter pour cette case le stat bonus
        */
    }

    void UpdateBasicStatUp(GlyphObject gBasicStatUp)
    {

    }
}
