using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlyphManager : MonoBehaviour
{
    [Header("LAME")]
    public GlyphWrap glyphTest1;

    [Header("MANCHE")] 
    public GlyphWrap glyphTest2;
    
    [Header("HAMPE")]
    public GlyphWrap glyphTest3;

    
    
    //Fonctions Système ************************************************************************************************
    
    void Update()
    {
        GlyphTest1();
    }
    
    
    //Fonctions des Glyphes ********************************************************************************************

    void GlyphTest1()
    {
        if (glyphTest1.gState == GlyphWrap.State.Active)
        {
            Debug.Log(glyphTest1.glyphObject.gNom + "est actif");
        }
    }
}
