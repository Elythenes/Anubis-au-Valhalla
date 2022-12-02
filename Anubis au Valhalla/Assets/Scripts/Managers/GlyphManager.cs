using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NaughtyAttributes;
using Unity.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public class GlyphManager : MonoBehaviour
{
    public static GlyphManager Instance; //singleton
    [NaughtyAttributes.ReadOnly] public List<int> indexActiveGlyphs = new();

    [Header("LAME")]
    public GlyphWrap[] arrayLame = new GlyphWrap[60];

    [Header("MANCHE")] 
    public GlyphWrap[] arrayManche = new GlyphWrap[60];
    
    [Header("POIGNEE")]
    public GlyphWrap[] arrayPoignee = new GlyphWrap[60];
    
    
    
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

    void LameManager(GlyphObject hiero)              //sert à intégrer les effets des glyphes pour le Perso    //à généraliser après
    {
        for (int i = 0; i < arrayLame.Length; i++)
        {
            if (arrayLame[i].gState == GlyphWrap.State.Active)
            {
                if (arrayLame[i].glyphObject.isBasicStatUp)
                {
                    UpdateBasicStatUp(arrayLame[i].glyphObject);
                }
                if (arrayLame[i].glyphObject.isSituationalStatUp)
                {
                    UpdateSituationalStatUp(arrayLame[i].glyphObject);
                }
            }
        }
        
    }

    public void AddGlyphToManager(GlyphObject hiero)
    {
        if (hiero.isBasicStatUp)
        {
            UpdateBasicStatUp(hiero);
        }
        if (hiero.isSituationalStatUp)
        {
            UpdateSituationalStatUp(hiero);
        }
        if (hiero.isSpecialStatUp)
        {
            UpdateSpecialEffect(hiero);
        }
        if (hiero.isAdditionalEffect)
        {
            UpdateAdditionalEffect(hiero);
        }
        if (hiero.isTriggerEffect)
        {
            UpdateTriggerEffect(hiero);
        }
        if (hiero.isChargeBased)
        {
            UpdateChargeBasedEffect(hiero);
        }
        if (hiero.isTimeBased)
        {
            UpdateTimeBasedEffect(hiero);
        }
        if (hiero.isBoolEffect)
        {
            UpdateBoolEffect(hiero);
        }
        if (hiero.isOther)
        {
            UpdateOther(hiero);
        }
    }

    void UpdateBasicStatUp(GlyphObject gBasicStatUp)
    {
        switch (gBasicStatUp.anubisStat)
        {
            case GlyphObject.AnubisStat.AnubisBaseDamage:   //augmente les débâts de toutes les attaques du combo et le Thrust
                for (int i = 0; i < 3; i++)
                {
                    AnubisCurrentStats.instance.comboDamage[i] += Mathf.RoundToInt(gBasicStatUp.bonusBasicStat);
                }
                AnubisCurrentStats.instance.thrustDamage += Mathf.RoundToInt(gBasicStatUp.bonusBasicStat);
                break;
            
            case GlyphObject.AnubisStat.AllComboDamage:     //augmente les dégâts de toutes les attaques du combo
                for (int i = 0; i < 3; i++)
                {
                    AnubisCurrentStats.instance.comboDamage[i] += Mathf.RoundToInt(gBasicStatUp.bonusBasicStat); 
                }
                break;
            
            case GlyphObject.AnubisStat.Combo1Damage:       //augmente les dégâts de la 1ère attaque du combo
                AnubisCurrentStats.instance.comboDamage[0] += Mathf.RoundToInt(gBasicStatUp.bonusBasicStat);
                break;
            
            case GlyphObject.AnubisStat.Combo2Damage:       //augmente les dégâts de la 2ème attaque du combo
                AnubisCurrentStats.instance.comboDamage[1] += Mathf.RoundToInt(gBasicStatUp.bonusBasicStat);
                break;
            
            case GlyphObject.AnubisStat.Combo3Damage:       //augmente les dégâts de la 3ème attaque du combo
                AnubisCurrentStats.instance.comboDamage[2] += Mathf.RoundToInt(gBasicStatUp.bonusBasicStat);
                break;
            
            case GlyphObject.AnubisStat.ThrustDamage:       //augmente les dégâts du Thrust
                AnubisCurrentStats.instance.thrustDamage += Mathf.RoundToInt(gBasicStatUp.bonusBasicStat);
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
                AnubisCurrentStats.instance.vieMax += Mathf.RoundToInt(gBasicStatUp.bonusBasicStat);
                break;
            
            case GlyphObject.AnubisStat.Armor:            //augmente la réduction de dégâts d'Anubis
                AnubisCurrentStats.instance.damageReduction += Mathf.RoundToInt(gBasicStatUp.bonusBasicStat);
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


    void UpdateSituationalStatUp(GlyphObject hiero)
    {
        switch (hiero.index)
        {
            case 0:
                break ;
        }
    }

    void UpdateSpecialEffect(GlyphObject hiero)
    {
        switch (hiero.index)
        {
            case 0:
                break ;
        }
    }
    
    void UpdateAdditionalEffect(GlyphObject hiero)
    {
        switch (hiero.index)
        {
            case 0:
                break ;
        }
    }
    
    void UpdateTriggerEffect(GlyphObject hiero)
    {
        switch (hiero.index)
        {
            case 0:
                break ;
        }
    }
    
    void UpdateChargeBasedEffect(GlyphObject hiero)
    {
        switch (hiero.index)
        {
            case 0:
                break ;
        }
    }
    
    void UpdateTimeBasedEffect(GlyphObject hiero)
    {
        switch (hiero.index)
        {
            case 0:
                break ;
        }
    }
    
    void UpdateBoolEffect(GlyphObject hiero)
    {
        switch (hiero.index)
        {
            case 0:
                break ;
        }
    }
    
    void UpdateOther(GlyphObject hiero)
    {
        switch (hiero.index)
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
