using System;
using System.Collections;
using System.Collections.Generic;
using GenPro;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GlyphManager : MonoBehaviour
{
    public static GlyphManager Instance; //singleton

    public List<GlyphObject> listLame = new List<GlyphObject>(0);
    public List<GlyphObject> listManche = new List<GlyphObject>(0);
    public List<GlyphObject> listPoignee = new List<GlyphObject>(0);
    
    public bool showBools = false; //Liste de variables pour les fonctions
    
    [ShowIf("showBools")] [BoxGroup("Soul Power")] public int soulPowerForceValue;
    [ShowIf("showBools")] [BoxGroup("Soul Power")] public int soulPowerDefenseValue;

    [ShowIf("showBools")] [BoxGroup("Dodge")] public bool dashForce;
    [ShowIf("showBools")] [BoxGroup("Dodge")] public int dashForceValue;
    [ShowIf("showBools")] [BoxGroup("Dodge")] public int dodgeHealValue;
    [ShowIf("showBools")] [BoxGroup("Dodge")] public int dodgeForceValue;
    [ShowIf("showBools")] [BoxGroup("Dodge")] public int dodgeArmorValue;
    [ShowIf("showBools")] [BoxGroup("Dodge")] public int dodgeInflictValue;
    [ShowIf("showBools")] [BoxGroup("Dodge")] public float dodgeStaggerTime;

    [ShowIf("showBools")] [BoxGroup("Target all current Enemies")] public GameObject currentRoom;
    [ShowIf("showBools")] [BoxGroup("Target all current Enemies")] public bool stillEnemies;
    [ShowIf("showBools")] [BoxGroup("Target all current Enemies")] public int takeDamageInflictValue;
    [ShowIf("showBools")] [BoxGroup("Target all current Enemies")] public float takeDamageStaggerTime;

    [ShowIf("showBools")] [BoxGroup("Enter Room")] public bool enterNewRoom;
    [ShowIf("showBools")] [BoxGroup("Enter Room")] public int doRegenOnEnterValue;
    
    [ShowIf("showBools")] [BoxGroup("Other")] public float healBoost;
    

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
        DetectEnemies();
    }
    
    
    //Fonctions *********************************************************************************************************
    
    void DisableAllGlyphsBooleans()
    {
        soulPowerForceValue = 0;
        soulPowerDefenseValue = 0;
        
        dashForceValue = 0;
        dashForce = false;
        dodgeHealValue = 0;
        dodgeArmorValue = 0;
        dodgeInflictValue = 0;
        dodgeStaggerTime = 0;
        
        takeDamageInflictValue = 0;
        takeDamageStaggerTime = 0f;

        enterNewRoom = false;
        doRegenOnEnterValue = 0;

        healBoost = 0;
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
        switch (hiero.index)
        {
            case 212:
            case 213:
            case 214 :
                takeDamageInflictValue += Mathf.RoundToInt(hiero.specialTriggerValue);
                break;
            
            case 215:
                takeDamageStaggerTime = hiero.specialTriggerValue;
                break;

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
            
            case 337:
            case 338:
            case 339:
                dodgeForceValue += Mathf.RoundToInt(hiero.additionalDamage);
                break;
            
            case 340:
            case 341:
            case 342:
                dodgeArmorValue += Mathf.RoundToInt(hiero.additionalDamage);
                break;
            
            case 343:
            case 344:
            case 345:
                dodgeInflictValue += Mathf.RoundToInt(hiero.specialTriggerValue);
                break;

            case 346:
                dodgeStaggerTime = hiero.specialTriggerValue;
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
            case 251:
            case 252:
            case 253:
                doRegenOnEnterValue += Mathf.RoundToInt(hiero.boolValue);
                break;
        }
    }
    
    void SetOnOther(GlyphObject hiero)
    {
        switch (hiero.index)
        {
            case 295:
            case 296:
            case 297:
                healBoost *= hiero.otherBonusBasicStat;
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
                case 212:
                case 213:
                case 214:
                    if (DamageManager.instance.isHurt)
                    {
                        DoEffectToEnemies(takeDamageInflictValue, 0.2f);
                    }
                    break;
                
                case 215:
                    if (DamageManager.instance.isHurt)
                    {
                        DoEffectToEnemies(0, takeDamageStaggerTime);
                    }
                    break;
                
                case 221:
                case 222:
                case 223:
                    SoulPowerDefense();
                    break;
                
                case 251:
                case 252:
                case 253:
                    OnEnterRoom(0);
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
                
                case 337:
                case 338:
                case 339:
                    DodgeForce();
                    break;
                
                case 340:
                case 341:
                case 342:
                    DodgeArmor();
                    break;
                
                case 343:
                case 344:
                case 345:
                    DodgeDoEffect(dodgeInflictValue,0.2f);
                    break;
                
                case 346:
                    DodgeDoEffect(0,dodgeStaggerTime);
                    break;
            }
        }
    }


    
    
    //Fonctions système des Glyphes ********************************************************************************************
    
    void DetectEnemies()
    {
        //Debug.Log(currentRoom.GetComponent<SalleGenerator>().currentRoom.currentEnemies.Count);
        if (SalleGenerator.Instance.currentRoom.GetComponent<Salle>().currentEnemies.Count != 0)
        {
            stillEnemies = true;
        }
        else
        {
            stillEnemies = false;
        }
    }

    
    
    //Fonctions des Glyphes ********************************************************************************************

    void HealDodge() //344,345,346,
    {
        if (DamageManager.instance.isDodging)
        {
            DamageManager.instance.Heal(dodgeHealValue);
        }
    }
    
    void SoulPowerForce() //135,136,137,
    {
        AnubisCurrentStats.instance.soulBonusDamageForStat = Mathf.RoundToInt(Mathf.Log(Souls.instance.soulBank + 1) * soulPowerForceValue);
    }

    void SoulPowerDefense() //221,222,223,
    {
        AnubisCurrentStats.instance.soulBonusDamageReductionForStat = Mathf.RoundToInt(Mathf.Log(Souls.instance.soulBank + 1) * soulPowerDefenseValue);
        
    }
    
    void DashForce() //309,310,311,
    {
        if (dashForce && CharacterController.instance.debutDash)
        {
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
            yield return new WaitForSecondsRealtime(0.1f);
        }
        AnubisCurrentStats.instance.totalBaseBonusDamage -= dashForceValue;
        dashForce = true;
    }

    void DodgeForce() //337,338,339,
    {
        //Debug.Log("dodge force");
        if (DamageManager.instance.isDodging)
        {
            //Debug.Log("is dodging");
            StartCoroutine(DodgeForceCoroutine(2));
        }
    }
    private IEnumerator DodgeForceCoroutine(float duration)
    {
        AnubisCurrentStats.instance.totalBaseBonusDamage += dodgeForceValue;
        float compteur = 0f;
        while (compteur < duration)
        {
            compteur += 0.1f;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        AnubisCurrentStats.instance.totalBaseBonusDamage -= dodgeForceValue;
    }

    void DodgeArmor() //340,341,342
    {
        //Debug.Log("dodge armor");
        if (DamageManager.instance.isDodging)
        {
            //Debug.Log("is dodging");
            StartCoroutine(DodgeArmorCoroutine(2));
        }
    }
    private IEnumerator DodgeArmorCoroutine(float duration)
    {
        AnubisCurrentStats.instance.damageReduction += dodgeArmorValue;
        float compteur = 0f;
        while (compteur < duration)
        {
            compteur += 0.1f;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        AnubisCurrentStats.instance.damageReduction-= dodgeArmorValue;
    }
    
    void DoEffectToEnemies(int damage, float stagger) //212,213,214,215,
    {
        if (stillEnemies)
        {
            foreach (var enemy in currentRoom.GetComponent<SalleGenerator>().currentRoom.currentEnemies)
            {
                Debug.Log("tiens dans ta gueule");
                enemy.TakeDamage(damage,stagger);
            }
        }
    }

    void DodgeDoEffect(int damage, float stagger) //343,344,345,346
    {
        if (DamageManager.instance.isDodging)
        {
            DoEffectToEnemies(damage,stagger);
        }
    }

    void OnEnterRoom(int cas) //251,252,253,
    {
        switch (cas)
        {
            case 0: //Regen
                if (enterNewRoom)
                {
                    DamageManager.instance.Heal(doRegenOnEnterValue);
                }
                break;
        }
        enterNewRoom = false;
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
