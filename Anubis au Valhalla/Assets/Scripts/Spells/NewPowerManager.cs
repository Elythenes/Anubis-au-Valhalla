using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class NewPowerManager : MonoBehaviour
{
    public static NewPowerManager Instance;
    public GameObject targetUser;
    public KeyCode keyPower1;
    public KeyCode keyPower2;
    public LayerMask layerMonstres;
    
    private CharacterController cc;
    private AttaquesNormales an;
    
    [Header("SYSTEM")]
    [Range(1,10)] public int currentLevelPower1;
    [Range(1,10)] public int currentLevelPower2;

    public List<GameObject> powersCollected = new();

    public List<float> p1ComboConeDamages = new(10);
    public List<float> p1ComboConeReaches = new(10);
    public List<float> p1ThrustBallDamages = new(10);
    public List<float> p1ThrustBallVelocities = new(10);
    public List<float> p1DashContactDamages = new(10);
    public List<float> p1DashContactSlowForces = new(10);

    public List<float> p2ComboWaveDamages = new(10);
    public List<float> p2ComboWaveRadiuses = new(10);
    public List<float> p2ThrustBandageDamages = new(10);
    public List<float> p2ThrustBandageSizes = new(10);
    public List<float> p2DashTrailDamagesPerTick = new(10);
    public List<float> p2DashTrailDurations = new(10);

    [Header("UTILISATION")]
    public float durationPower1 = 8f;
    public float cooldownPower1 = 12f;
    public float currentCooldownPower1;
    
    public float durationPower2 = 8f;
    public float cooldownPower2;
    public float currentCooldownPower2;

    
    [Header("DEBUG / Test")] 
    public int startingLevelPower1 = 1;
    public int startingLevelPower2 = 1;

    public bool testCustomLevel;
    public List<GameObject> testPowersCollected = new();
    public KeyCode testCustomKey = KeyCode.K;

    [Header("DEBUG / Var")] 
    public bool canUsePowers;
    public bool canUsePower1;
    public bool canUsePower2;

    public bool isPower1Active;
    public bool isPower2Active;

    [Foldout("p1ComboCone")] public float p1ComboConeDamage;
    [Foldout("p1ComboCone")] public float p1ComboConeReach;
    [Foldout("p1ComboCone")] public bool p1ComboConeStagger;
    [Foldout("p1ComboCone")] public bool p1ComboConeHalfSphere;
    [Foldout("p1ComboCone")] public bool p1ComboConeCenterCone;
    
    [Foldout("p1TrustBall")] public float p1ThrustBallDamage;
    [Foldout("p1TrustBall")] public float p1ThrustBallVelocity;
    [Foldout("p1TrustBall")] public bool p1ThrustBallExplosionSize;
    [Foldout("p1TrustBall")] public bool p1ThrustBallTriple;
    [Foldout("p1TrustBall")] public bool p1ThrustBallExecute;
    
    [Foldout("p1DashContact")] public float p1DashContactDamage;
    [Foldout("p1DashContact")] public float p1DashContactSlowDuration;
    [Foldout("p1DashContact")] public bool p1DashContactSlowForce;
    [Foldout("p1DashContact")] public bool p1DashContactStagger;
    [Foldout("p1DashContact")] public bool p1DashContactPowerExtend;
    
    [Foldout("p2ComboWave")] public float p2ComboWaveDamage;
    [Foldout("p2ComboWave")] public float p2ComboWaveRadius;
    [Foldout("p2ComboWave")] public bool p2ComboWaveDot;
    [Foldout("p2ComboWave")] public float p2ComboWaveDotDamage;
    [Foldout("p2ComboWave")] public bool p2ComboWaveDeathExplosion;
    [Foldout("p2ComboWave")] public bool p2ComboWaveDouble;
    
    [Foldout("p2ThrustBandage")] public float p2ThrustBandageDamage;
    [Foldout("p2ThrustBandage")] public float p2ThrustBandageSize;
    [Foldout("p2ThrustBandage")] public int p2ThrustBandageMaxHit = 1;
    [Foldout("p2ThrustBandage")] public bool p2ThrustBandageHoming;
    
    [Foldout("p2DashTrail")] public float p2DashTrailDamagePerTick;
    [Foldout("p2DashTrail")] public float p2DashTrailDuration;
    [Foldout("p2DashTrail")] public bool p2DashTrailSize;
    [Foldout("p2DashTrail")] public bool p2DashTrailDotStacking;
    [Foldout("p2DashTrail")] public float p2DashTrailDotDamageMultiplier;
    [Foldout("p2DashTrail")] public bool p2DashTrailInfection;
    
    
    [Header("TEXTS")] 
    [Foldout("NEW POWER 1 TEXT")] public string nomPower1;
    [Foldout("NEW POWER 1 TEXT")] [TextArea(10,20)] public string descriptionGlobalPower1;
    [Foldout("NEW POWER 1 TEXT")] [TextArea(3, 10)] public string citationPower1;
    [Foldout("NEW POWER 1 TEXT")] public string nomComboPower1;
    [Foldout("NEW POWER 1 TEXT")] [TextArea(6,20)] public string descriptionComboPower1;
    [Foldout("NEW POWER 1 TEXT")] public string nomThrustPower1;
    [Foldout("NEW POWER 1 TEXT")] [TextArea(6,20)] public string descriptionThrustPower1;
    [Foldout("NEW POWER 1 TEXT")] public string nomDashPower1;
    [Foldout("NEW POWER 1 TEXT")] [TextArea(6,20)] public string descriptionDashPower1;
    
    [Foldout("NEW POWER 2 TEXT")] public string nomPower2;
    [Foldout("NEW POWER 2 TEXT")] [TextArea(10,20)] public string descriptionGlobalPower2;
    [Foldout("NEW POWER 2 TEXT")] [TextArea(3, 10)] public string citationPower2;
    [Foldout("NEW POWER 2 TEXT")] public string nomComboPower2;
    [Foldout("NEW POWER 2 TEXT")] [TextArea(6,20)] public string descriptionComboPower2;
    [Foldout("NEW POWER 2 TEXT")] public string nomThrustPower2;
    [Foldout("NEW POWER 2 TEXT")] [TextArea(6,20)] public string descriptionThrustPower2;
    [Foldout("NEW POWER 2 TEXT")] public string nomDashPower2;
    [Foldout("NEW POWER 2 TEXT")] [TextArea(6,20)] public string descriptionDashPower2;
    
    
    //Fonctions système ************************************************************************************************

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        canUsePowers = true;
        canUsePower1 = true;
        canUsePower2 = true;

        currentLevelPower1 = startingLevelPower1;
        currentLevelPower2 = startingLevelPower2;
    }
    
    
    void Update()
    {
        if (testCustomLevel && Input.GetKeyDown(testCustomKey))
        {
            foreach (var gb in testPowersCollected)
            {
                Instantiate(gb);
            }
        }
        
        UsePower();
    }


    //Fonctions Powers *************************************************************************************************

    public void PowerLevelUp(GameObject powerRepo)
    {
        switch(powerRepo.GetComponent<NewPowerRepository>().newPowerType)
        {
            case NewPowerType.None:
                Debug.Log("pas de level up si PowerType = None");
                break;
            
            case NewPowerType.Power1:
                currentLevelPower1++;
                break;
            
            case NewPowerType.Power2:
                currentLevelPower2++;
                break;
        }
        PowerLevelUnlock();
        
    }

    void PowerLevelUnlock()
    {
        switch (currentLevelPower1)
        {
            case 3:
                p1ComboConeStagger = true;
                p1ThrustBallExplosionSize = true;
                p1DashContactSlowForce = true;
                break;
            
            case 5:
                p1ComboConeHalfSphere = true;
                p1ThrustBallTriple = true;
                p1DashContactStagger = true;
                break;
            
            case 8:
                p1ComboConeCenterCone = true;
                p1ThrustBallExecute = true;
                p1DashContactPowerExtend = true;
                break;
        }
        switch (currentLevelPower2)
        {
            case 3:
                p2ComboWaveDot = true;
                p2ThrustBandageMaxHit = 2;
                p2DashTrailSize = true;
                break;
            
            case 5:
                p2ComboWaveDeathExplosion = true;
                p2ThrustBandageMaxHit = 100;
                p2DashTrailDotStacking = true;
                break;
            
            case 8:
                p2ComboWaveDouble = true;
                p2ThrustBandageHoming = true;
                p2DashTrailInfection = true;
                break;
        }
    }
    
    
    
    
    void UsePower()
    {
        if (Input.GetKeyDown(keyPower1) && canUsePower1)
        {
            isPower1Active = true;
            Debug.Log("P1 Actif");
        }
        while (isPower1Active)
        {
            CooldownManager(1);
            /*if (an.attaque3) //si attaque smash
            {
                //fonction Cone
            }
            if (an.thrust) //si thrust
            {
                //fonction Ball
            }
            if (cc.isDashing) //si dash
            {
                //fonction DashContact
            }*/
        }
        while (isPower2Active)
        {
            CooldownManager(1);
            /*if (an.attaque3) //si attaque smash
            {
                //fonction Cone
            }
            if (an.thrust) //si thrust
            {
                //fonction Ball
            }
            if (cc.isDashing) //si dash
            {
                //fonction DashContact
            }*/
        }
    }
    
    

    void CooldownManager(int power)
    {
        switch (power)
        {
            case 1:
                if (durationPower1 > 0) //tant qu'on est sous power 1
                {
                    //manque la fonction de la duration
                    Debug.Log("duration est = " + durationPower1);
                }
                else //si power entièrement épuisé
                {
                    canUsePower1 = false;
                    isPower1Active = false;
                    StartCoroutine(CoroutinePower()); //lance la récupération du power
                }
                break;
            
            case 2:
                break;
        }
    }

    private IEnumerator CoroutinePower()
    {
        while (currentCooldownPower1 < cooldownPower1)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            currentCooldownPower1 += 0.1f;
            Debug.Log("currentCooldown Power 1 = " + currentCooldownPower1);
        }
        canUsePower1 = true;
    }

    private IEnumerator CoroutineDuration() //faire cette foutue fonction qui enlève de la duration
    {
        float compteur = 0f;
        while (compteur < durationPower1)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            compteur -= 0.1f;
            Debug.Log("currentCooldown Power 1 = " + currentCooldownPower1);
        }
        
    }
    
    
    //old Fonctions ****************************************************************************************************
    
    /*void ChangeLevelPower(int powerIndex, int currentLevel) //fonction à appeler quand on level Up; ou pour tester
    {
        if (powerIndex == 1)
        {
            currentLevelPower1 = currentLevel;
            switch (currentLevel)
            {
                case 1:
                    //effet du niveau
                    Debug.Log("niveau 1");
                    break;

                case <= 2:
                    //bonus du niveau
                    Debug.Log("niveau 2");
                    break;

                case <= 3:
                    //bonus du niveau
                    Debug.Log("niveau 3");
                    break;
                
                case <= 4:
                    //bonus du niveau
                    break;
                
                case <= 5:
                    //bonus du niveau
                    break;
                
                case <= 6: 
                    //bonus du niveau
                    break;
                
                case <= 7:
                    //bonus du niveau
                    break;
                
                case <= 8:
                    //bonus du niveau
                    break;
                
                case <= 9:
                    //bonus du niveau
                    break;
                
                case <= 10:
                    //bonus du niveau
                    break;
            }

        }
        else if (powerIndex == 2)
        {
            currentLevelPower2 = currentLevel;
            switch (currentLevel)
            {
                case <= 1:
                    //effet du niveau
                    break;

                case <= 2:
                    //bonus du niveau
                    break;

                case <= 3:
                    //bonus du niveau
                    break;
                
                case <= 4:
                    //bonus du niveau
                    break;
                
                case <= 5:
                    //bonus du niveau
                    break;
                
                case <= 6: 
                    //bonus du niveau
                    break;
                
                case <= 7:
                    //bonus du niveau
                    break;
                
                case <= 8:
                    //bonus du niveau
                    break;
                
                case <= 9:
                    //bonus du niveau
                    break;
                
                case <= 10:
                    //bonus du niveau
                    break;
            }
        }
        else
        {
            Debug.Log("Il n'y a que 2 pouvoirs, arrête d'essayer.");
        }
    }*/
}
