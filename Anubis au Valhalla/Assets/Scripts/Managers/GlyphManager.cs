using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GlyphManager : MonoBehaviour
{
    public static GlyphManager instance; //singleton
    
    [Header("LAME")]
    public GlyphWrap[] arrayLame = new GlyphWrap[5];

    [Header("MANCHE")] 
    public GlyphWrap[] arrayManche = new GlyphWrap[5];
    
    [Header("HAMPE")]
    public GlyphWrap[] arrayHampe = new GlyphWrap[5];

    
    
    //Fonctions Système ************************************************************************************************
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {

    }

    void Update()
    {
        //TestGlyphTest1();
    }
    
    
    //Fonctions des Glyphes ********************************************************************************************

    
    
    /*void TestGlyphTest1()
    {
        if (glyphTest1.gState == GlyphWrap.State.Active)
        {
            Debug.Log(glyphTest1.glyphObject.gNom + "est actif");
        }
    }*/
}
