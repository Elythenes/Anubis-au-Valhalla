using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NaughtyAttributes;
using Unity.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using Debug = UnityEngine.Debug;

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

    //Liste de bool pour les fonctions
    public bool showBools = false;

    [ShowIf("showBools")] [BoxGroup("SOUL POWER")] public bool soulPowerForce1;
    [ShowIf("showBools")] [BoxGroup("SOUL POWER")] public bool soulPowerForce2;
    [ShowIf("showBools")] [BoxGroup("SOUL POWER")] public bool soulPowerForce3;
    [ShowIf("showBools")] [BoxGroup("SOUL POWER")] public bool soulPowerDefense1;
    [ShowIf("showBools")] [BoxGroup("SOUL POWER")] public bool soulPowerDefense2;
    [ShowIf("showBools")] [BoxGroup("SOUL POWER")] public bool soulPowerDefense3;
    
    
    
    

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
    
    public void ActiveGlyphInManager(GlyphObject hiero)
    {
        if (hiero.isBasicStatUp)
        {
            SetOnBasicStatUp(hiero);
        }
        if (hiero.isSituationalStatUp)
        {
            SetOnSituationalStatUp(hiero);
        }
        if (hiero.isSpecialStatUp)
        {
            SetOnSpecialEffect(hiero);
        }
        if (hiero.isAdditionalEffect)
        {
            SetOnAdditionalEffect(hiero);
        }
        if (hiero.isTriggerEffect)
        {
            SetOnTriggerEffect(hiero);
        }
        if (hiero.isChargeBased)
        {
            SetOnChargeBasedEffect(hiero);
        }
        if (hiero.isTimeBased)
        {
            SetOnTimeBasedEffect(hiero);
        }
        if (hiero.isBoolEffect)
        {
            SetOnBoolEffect(hiero);
        }
        if (hiero.isOther)
        {
            SetOnOther(hiero);
        }
    }
    
    void SetOnBasicStatUp(GlyphObject gBasicStatUp)
    {
        switch (gBasicStatUp.anubisStat)
        {
            case GlyphObject.AnubisStat.AnubisBaseDamage:   //augmente les débâts de toutes les attaques du combo et le Thrust
                AnubisCurrentStats.instance.baseDamage += Mathf.RoundToInt(gBasicStatUp.bonusBasicStat);
                AnubisCurrentStats.instance.baseDamageforSoul += Mathf.RoundToInt(gBasicStatUp.bonusBasicStat);
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
                    if (gBasicStatUp.bonusBasicStat != 0)
                    {
                        var vector2 = AnubisCurrentStats.instance.rangeAttaque[i];
                        vector2.x *= gBasicStatUp.bonusBasicStat;
                        vector2.y *= gBasicStatUp.bonusBasicStat;
                        AnubisCurrentStats.instance.rangeAttaque[i] = vector2;
                    }
                    else
                    {
                        Debug.LogError("Bonus Basic Stat pas bonne, car = 0");
                    }
                }
                break;
            
            case GlyphObject.AnubisStat.HealthPoint:       //augmente les PV max d'Anubis
                AnubisCurrentStats.instance.vieMax += Mathf.RoundToInt(gBasicStatUp.bonusBasicStat);
                break;
            
            case GlyphObject.AnubisStat.Armor:            //augmente la réduction de dégâts d'Anubis
                AnubisCurrentStats.instance.damageReduction += Mathf.RoundToInt(gBasicStatUp.bonusBasicStat);
                break;
            
            case GlyphObject.AnubisStat.Speed:              //augmente la vitesse de déplacement d'Anubis
                if (gBasicStatUp.bonusBasicStat != 0)
                {
                    AnubisCurrentStats.instance.speedX *= gBasicStatUp.bonusBasicStat;
                    AnubisCurrentStats.instance.speedY *= gBasicStatUp.bonusBasicStat;
                }
                else
                {
                    Debug.LogError("Bonus Basic Stat pas bonne, car = 0");
                }
                break;
            
            case GlyphObject.AnubisStat.DashCd:              //réduit la durée avant qu'Anubis ne puisse re-Dash
                AnubisCurrentStats.instance.dashCooldown -= gBasicStatUp.bonusBasicStat;
                break;
        }
    }


    void SetOnSituationalStatUp(GlyphObject hiero)
    {
        switch (hiero.index)
        {
            case 0:
                break;
        }
    }

    void SetOnSpecialEffect(GlyphObject hiero)
    {
        switch (hiero.index)
        {
            case 135:
                soulPowerForce1 = true;
                break;
        }
    }
    
    void SetOnAdditionalEffect(GlyphObject hiero)
    {
        switch (hiero.index)
        {
            case 0:
                break;
        }
    }
    
    void SetOnTriggerEffect(GlyphObject hiero)
    {
        switch (hiero.index)
        {
            case 0:
                break;
        }
    }
    
    void SetOnChargeBasedEffect(GlyphObject hiero)
    {
        switch (hiero.index)
        {
            case 0:
                break;
        }
    }
    
    void SetOnTimeBasedEffect(GlyphObject hiero)
    {
        switch (hiero.index)
        {
            case 0:
                break;
        }
    }
    
    void SetOnBoolEffect(GlyphObject hiero)
    {
        switch (hiero.index)
        {
            case 0:
                break;
        }
    }
    
    void SetOnOther(GlyphObject hiero)
    {
        switch (hiero.index)
        {
            case 0:
                break;
        }
    }

    void UpdateGlyph()
    {
        for (int i = 0; i < arrayLame.Length; i++)
        {
            switch (arrayLame[i].glyphObject.index)
            {
                case 135: //soul Power Force 1
                    SoulPower();
                    //soulPowerForce1 = false; //on ne met pas le false car on calcule la fonction SoulPower() tout le temps
                    break;
            }
        }
    }
    

    //Fonctions des Glyphes ********************************************************************************************

    void SoulPower()
    {
        while (soulPowerForce1)
        {
            AnubisCurrentStats.instance.baseDamage = AnubisCurrentStats.instance.baseDamageforSoul + Mathf.RoundToInt(Mathf.Log(Souls.instance.soulBank + 1) *5);
        }
        
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
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
