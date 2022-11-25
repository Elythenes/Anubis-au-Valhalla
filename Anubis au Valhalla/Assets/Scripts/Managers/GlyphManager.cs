using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections;
using UnityEngine;

public class GlyphManager : MonoBehaviour
{
    public static GlyphManager Instance; //singleton
    public List<int> indexActiveGlyphs = new List<int>();
    
    [Header("LAME")]
    public GlyphWrap[] arrayLame = new GlyphWrap[5];

    [Header("MANCHE")] 
    public GlyphWrap[] arrayManche = new GlyphWrap[5];
    
    [Header("POIGNEE")]
    public GlyphWrap[] arrayPoignee = new GlyphWrap[5];
    
    
    
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
        
    }

    void LameManager()              //sert à intégrer les effets des glyphes pour le Perso    //à généraliser après
    {
        for (int i = 0; i < arrayLame.Length; i++)
        {
            if (arrayLame[i].gState == GlyphWrap.State.Active)
            {
                switch (arrayLame[i].glyphObject.effectType)
                {
                    case GlyphObject.GlyphEffect.BasicStatUp:
                        UpdateBasicStatUp(arrayLame[i].glyphObject);
                        break;
                    
                    case GlyphObject.GlyphEffect.SituaionalStatUp:
                        UpdateSituationalStatUp(arrayLame[i].glyphObject);
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
        switch (gBasicStatUp.anubisStat)
        {
            case GlyphObject.AnubisStat.AnubisBaseDamage:   //augmente les débâts de toutes les attaques du combo et le Thrust
                for (int i = 0; i < 3; i++)
                {
                    AnubisCurrentStats.instance.comboDamage[i] += gBasicStatUp.bonusBasicStat;
                }
                AnubisCurrentStats.instance.thrustDamage = +gBasicStatUp.bonusBasicStat;
                break;
            
            case GlyphObject.AnubisStat.AllComboDamage:     //augmente les dégâts de toutes les attaques du combo
                for (int i = 0; i < 3; i++)
                {
                    AnubisCurrentStats.instance.comboDamage[i] += gBasicStatUp.bonusBasicStat; 
                }
                break;
            
            case GlyphObject.AnubisStat.Combo1Damage:       //augmente les dégâts de la 1ère attaque du combo
                AnubisCurrentStats.instance.comboDamage[0] += gBasicStatUp.bonusBasicStat;
                break;
            
            case GlyphObject.AnubisStat.Combo2Damage:       //augmente les dégâts de la 2ème attaque du combo
                AnubisCurrentStats.instance.comboDamage[1] += gBasicStatUp.bonusBasicStat;
                break;
            
            case GlyphObject.AnubisStat.Combo3Damage:       //augmente les dégâts de la 3ème attaque du combo
                AnubisCurrentStats.instance.comboDamage[2] += gBasicStatUp.bonusBasicStat;
                break;
            
            case GlyphObject.AnubisStat.ThrustDamage:       //augmente les dégâts du Thrust
                AnubisCurrentStats.instance.thrustDamage += gBasicStatUp.bonusBasicStat;
                break;
            
            case GlyphObject.AnubisStat.Range:              //augmente la portée / Range d'Anubis
                for (int i = 0; i < 3; i++)
                {
                    var vector2 = AnubisCurrentStats.instance.rangeAttaque[i];
                    vector2.x *= gBasicStatUp.bonusBasicStat;
                    vector2.y *= gBasicStatUp.bonusBasicStat;
                    AnubisCurrentStats.instance.rangeAttaque[i] = vector2;
                }
                break;
            
            case GlyphObject.AnubisStat.HealthPoint:       //augmente les PV max d'Anubis
                AnubisCurrentStats.instance.vieMax += gBasicStatUp.bonusBasicStat;
                break;
            
            case GlyphObject.AnubisStat.Armor:            //augmente la réduction de dégâts d'Anubis
                AnubisCurrentStats.instance.damageReduction += gBasicStatUp.bonusBasicStat;
                break;
            
            case GlyphObject.AnubisStat.Speed:              //augmente la vitesse de déplacement d'Anubis
                AnubisCurrentStats.instance.speedX *= gBasicStatUp.bonusBasicStat;
                AnubisCurrentStats.instance.speedY *= gBasicStatUp.bonusBasicStat;
                break;
            
            case GlyphObject.AnubisStat.DashCd:              //réduit la durée avant qu'Anubis ne puisse re-Dash
                AnubisCurrentStats.instance.dashCooldown -= gBasicStatUp.bonusBasicStat;
                break;
        }
    }


    void UpdateSituationalStatUp(GlyphObject yah)
    {
        switch (yah.index)
        {
            case 0:
                break ;
        }
    }
    
    
    
    //Fonctions des Glyphes ********************************************************************************************
    
    
    //hello 
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    //********************************   ouais je garde tous mes scripts obsolètes ici    *******************************
    
    /*void TestGlyphTest1()
    {
        if (glyphTest1.gState == GlyphWrap.State.Active)
        {
            Debug.Log(glyphTest1.glyphObject.gNom + "est actif");
        }
    }*/

    /*List<int> UpdateActiveGlyphList(GlyphWrap[] array) //regarde dans une partie tous les Glyphes Actifs et donne une liste contenant leurs index
    {
        List<int> indexActive = new();
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].gState == GlyphWrap.State.Active)
            {
                indexActive.Add(array[i].glyphObject.index);
            }
        }
        indexActiveGlyphs = indexActive;
        return indexActive;
    }*/
    
    /*GlyphObject.GlyphEffect GlyphTypeConvertor()
    {
        GlyphObject.GlyphEffect gType = new GlyphObject.GlyphEffect();
        return gType;
    }*/
}
