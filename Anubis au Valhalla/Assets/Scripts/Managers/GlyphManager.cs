using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GlyphManager : MonoBehaviour
{
    public static GlyphManager Instance; //singleton

    public List<GlyphObject> listLame = new List<GlyphObject>(0);
    public List<GlyphObject> listManche = new List<GlyphObject>(0);
    public List<GlyphObject> listPoignee = new List<GlyphObject>(0);
    
    public bool showBools = false; //Liste de bool pour les fonctions
    
    [ShowIf("showBools")] [BoxGroup("Soul Power")] public int soulPowerForceValue;
    [ShowIf("showBools")] [BoxGroup("Soul Power")] public int soulPowerDefenseValue;

    [ShowIf("showBools")] [BoxGroup("Dodge")] public bool dashForce;
    [ShowIf("showBools")] [BoxGroup("Dodge")] public int dashForceValue;
    [ShowIf("showBools")] [BoxGroup("Dodge")] public int dodgeHealValue;



    //Fonctions Système ************************************************************************************************
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        DisableAllGlyphsBooleans();
    }

    void Start()
    {
        
    }

    void Update()
    {
        UpdateGlyph();
    }
    
    
    //Fonctions *********************************************************************************************************
    
    void DisableAllGlyphsBooleans()
    {
        soulPowerForceValue = 0;
        soulPowerDefenseValue = 0;
        dodgeHealValue = 0;
        dashForceValue = 0;

        dashForce = false;
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
    
    void SetOnBasicStatUp(GlyphObject hiero)
    {
        switch (hiero.anubisStat)
        {
            case GlyphObject.AnubisStat.AnubisBaseDamage:   //augmente les débâts de toutes les attaques du combo et le Thrust
                //AnubisCurrentStats.instance.baseDamageForSoul += Mathf.RoundToInt(hiero.bonusBasicStat);
                AnubisCurrentStats.instance.AddBonusDamage(1,hiero.bonusBasicStat);
                break;
            
            case GlyphObject.AnubisStat.AllComboDamage:     //augmente les dégâts de toutes les attaques du combo
                AnubisCurrentStats.instance.AddBonusDamage(2,hiero.bonusBasicStat);
                break;
            
            case GlyphObject.AnubisStat.Combo1Damage:       //augmente les dégâts de la 1ère attaque du combo
                AnubisCurrentStats.instance.AddBonusDamage(3,hiero.bonusBasicStat);
                break;
            
            case GlyphObject.AnubisStat.Combo2Damage:       //augmente les dégâts de la 2ème attaque du combo
                AnubisCurrentStats.instance.AddBonusDamage(4,hiero.bonusBasicStat);
                break;
            
            case GlyphObject.AnubisStat.Combo3Damage:       //augmente les dégâts de la 3ème attaque du combo
                AnubisCurrentStats.instance.AddBonusDamage(5,hiero.bonusBasicStat);
                break;
            
            case GlyphObject.AnubisStat.ThrustDamage:       //augmente les dégâts du Thrust
                AnubisCurrentStats.instance.AddBonusDamage(6,hiero.bonusBasicStat);
                break;
            
            case GlyphObject.AnubisStat.CriticalChances:   //augmente les chances de coup critique de toutes les attaques du combo et le Thrust
                AnubisCurrentStats.instance.AddBonusDamage(7,hiero.bonusBasicStat);
                break;
            
            case GlyphObject.AnubisStat.Range:              //augmente la portée / Range d'Anubis
                for (int i = 0; i < 3; i++)
                {
                    if (hiero.bonusBasicStat != 0)
                    {
                        var vector2 = AnubisCurrentStats.instance.rangeAttaque[i];
                        vector2.x *= hiero.bonusBasicStat;
                        vector2.y *= hiero.bonusBasicStat;
                        AnubisCurrentStats.instance.rangeAttaque[i] = vector2;
                    }
                    else
                    {
                        Debug.LogError("Bonus Basic Stat pas bonne, car = 0");
                    }
                }
                break;
            
            case GlyphObject.AnubisStat.HealthPoint:       //augmente les PV max d'Anubis
                AnubisCurrentStats.instance.vieMax += Mathf.RoundToInt(hiero.bonusBasicStat);
                break;
            
            case GlyphObject.AnubisStat.Armor:            //augmente la réduction de dégâts d'Anubis
                AnubisCurrentStats.instance.damageReduction += Mathf.RoundToInt(hiero.bonusBasicStat);
                break;
            
            case GlyphObject.AnubisStat.Speed:              //augmente la vitesse de déplacement d'Anubis
                if (hiero.bonusBasicStat != 0)
                {
                    AnubisCurrentStats.instance.speedX *= hiero.bonusBasicStat;
                    AnubisCurrentStats.instance.speedY *= hiero.bonusBasicStat;
                }
                else
                {
                    Debug.LogError("Bonus Basic Stat pas bonne, car = 0");
                }
                break;
            
            case GlyphObject.AnubisStat.DashCd:              //réduit la durée avant qu'Anubis ne puisse re-Dash
                AnubisCurrentStats.instance.dashCooldown -= hiero.bonusBasicStat;
                break;
        }
    }


    void SetOnSituationalStatUp(GlyphObject hiero)
    {
        switch (hiero.index)
        {
            case 135:
            case 136:
            case 137:
                soulPowerForceValue += Mathf.RoundToInt(hiero.bonusSituationalStat);
                break;

            case 221:
            case 222:
            case 223:
                soulPowerDefenseValue += Mathf.RoundToInt(hiero.bonusSituationalStat);
                break;
        }
    }

    void SetOnSpecialEffect(GlyphObject hiero)
    {
        switch (hiero.index)
        {
            case 0:
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
        Debug.Log("oui oui c'est ca");
        switch (hiero.index)
        {
            case 309:
            case 310:
            case 311:
                dashForce = true;
                dashForceValue += Mathf.RoundToInt(hiero.additionalDamage);
                    break;
            
            case 334:
            case 335:
            case 336:
                dodgeHealValue += Mathf.RoundToInt(hiero.specialTriggerValue);
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
        foreach (var hiero in listLame)
        {
            switch (hiero.index)
            {
                case 135: 
                case 136:
                case 137: 
                    SoulPowerForce();
                    break;
            }
        }

        foreach (var hiero in listManche)
        {
            switch (hiero.index)
            {
                case 221:
                case 222:
                case 223:
                    SoulPowerDefense();
                    break;
            }
        }

        foreach (var hiero in listPoignee)
        {
            switch (hiero.index)
            {
                case 309:
                case 310:
                case 311:
                    DashForce();
                    break;
                
                case 334:
                case 335:
                case 336:
                    HealDodge();
                    break;
            }
        }
    }


    

    //Fonctions des Glyphes ********************************************************************************************

    void HealDodge()
    {
        if (DamageManager.instance.isDodging)
        {
            DamageManager.instance.Heal(dodgeHealValue);
        }
    }
    
    void SoulPowerForce() //s'active plusieurs fois mais pas grave en soi, à régler ptet ?
    {
        AnubisCurrentStats.instance.soulBonusDamageForStat = Mathf.RoundToInt(Mathf.Log(Souls.instance.soulBank + 1) * soulPowerForceValue);
    }

    void SoulPowerDefense() //s'active plusieurs fois mais pas grave en soi, à régler ptet ?
    {
        AnubisCurrentStats.instance.soulBonusDamageReductionForStat = Mathf.RoundToInt(Mathf.Log(Souls.instance.soulBank + 1) * soulPowerDefenseValue);
        
    }

    void DashForce()
    {
        if (dashForce && CharacterController.instance.debutDash)
        {
            Debug.Log("ouiouoi");
            dashForce = false;
            StartCoroutine(DashForceCoroutine(2));
        }
    }

    private IEnumerator DashForceCoroutine(float duration)
    {
        AnubisCurrentStats.instance.totalBaseBonusDamage += dashForceValue;
        float compteur = 0f;
        while (compteur < duration)
        {
            compteur += 0.1f;
            Debug.Log(compteur);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        AnubisCurrentStats.instance.totalBaseBonusDamage -= dashForceValue;
        dashForce = true;
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
    
    
    /*void UpdateGlyph() //truc ancien (peut servir ptet)
    {
        for (int i = 0; i < indexActiveGlyphs.Count; i++)
        {
            switch (indexActiveGlyphs[i])
            {
                case 135: //soul Power Force 1
                    SoulPower();
                    Debug.Log("soul force 1");
                    //soulPowerForce1 = false; //on ne met pas le false car on calcule la fonction SoulPower() tout le temps
                    break;
                
                case 136: //soul Power Force 2
                    SoulPower();
                    //soulPowerForce1 = false; //on ne met pas le false car on calcule la fonction SoulPower() tout le temps
                    break;
                
                case 137: //soul Power Force 3
                    Debug.Log("soul force 3");
                    SoulPower();
                    //soulPowerForce1 = false; //on ne met pas le false car on calcule la fonction SoulPower() tout le temps
                    break;
            }
        }*/
}
