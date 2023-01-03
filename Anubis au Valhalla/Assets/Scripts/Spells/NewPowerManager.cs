using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class NewPowerManager : MonoBehaviour
{
    public GameObject targetUser;
    public KeyCode keyPower1;
    public KeyCode keyPower2;
    public LayerMask layerMonstres;
    
    
    [Header("SYSTEM")]
    [Range(1,10)] public int currentLevelPower1;
    [Range(1,10)] public int currentLevelPower2;

    public List<NewPowerObject> powersCollected = new();

    public List<float> p1ComboConeDamages = new(10);
    public List<float> p1ComboConeReaches = new(10);
    

    [Header("UTILISATION")]
    public float cooldownPower1 = 8f;
    public float currentCooldownPower1;
    public float cooldownPower2 = 8f;
    public float currentCooldownPower2;

    
    [Header("DEBUG / Test")] 
    public int startingLevelPower1 = 1;
    public int startingLevelPower2 = 1;
    
    public bool testCustomLevel;
    
    
    [Header("DEBUG / Var")] 
    public bool canUsePowers;
    public bool canUsePower1;
    public bool canUsePower2;

    public bool isPower1Active;
    public bool isPower2Active;

    public float p1ComboConeDamage;
    public float p1ComboConeReach;
    public float p1ThrustBallDamage;
    public float p1ThrustBallVelocity;
    public float p1DashContactDamage;
    public float p1DashContactSlowForce;

    public float p2ComboWaveDamage;
    public float p2ComboWaveRadius;
    public float p2ThrustBandageDamage;
    public float p2ThrustBandageSize;
    public float p2DashTrailDamagePerTick;
    public float p2DashTrailDuration;
    
    
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
    
    void Start()
    {
        canUsePowers = true;
        canUsePower1 = true;
        canUsePower2 = true;

        if (testCustomLevel)
        {
            PowerLevelUp(1, startingLevelPower1);
            PowerLevelUp(2, startingLevelPower2);
        }
        else
        {
            PowerLevelUp(1, 1);
            PowerLevelUp(2, 1);
        }
    }
    
    
    void Update()
    {
        UsePower();
        
    }


    //Fonctions Powers *************************************************************************************************

    void PowerLevelUp(int power, int level)
    {
        switch (level)
        {
            case 1:
                switch (power)
                {
                    case 1:
                        p1ComboConeDamage = p1ComboConeDamages[0];
                        p1ComboConeReach = p1ComboConeReaches[0];
                        //mettre à false toutes les verrous des ajouts d'effet
                        break;
                    
                    case 2:
                        
                        break;
                }
                break;

            case 2:
                switch (power)
                {
                    case 1:
                        
                        break;
                    
                    case 2:
                        
                        break;
                }
                break;

            case 3:
                switch (power)
                {
                    case 1:
                        
                        break;
                    
                    case 2:
                        
                        break;
                }
                break;
                
            case 4:
                switch (power)
                {
                    case 1:
                        
                        break;
                    
                    case 2:
                        
                        break;
                }
                break;
                
            case 5:
                switch (power)
                {
                    case 1:
                        
                        break;
                    
                    case 2:
                        
                        break;
                }
                break;
                
            case 6: 
                switch (power)
                {
                    case 1:
                        
                        break;
                    
                    case 2:
                        
                        break;
                }
                break;
                
            case 7:
                switch (power)
                {
                    case 1:
                        
                        break;
                    
                    case 2:
                        
                        break;
                }
                break;
                
            case 8:
                switch (power)
                {
                    case 1:
                        
                        break;
                    
                    case 2:
                        
                        break;
                }
                break;
                
            case 9:
                switch (power)
                {
                    case 1:
                        
                        break;
                    
                    case 2:
                        
                        break;
                }
                break;
                
            case 10:
                switch (power)
                {
                    case 1:
                        
                        break;
                    
                    case 2:
                        
                        break;
                }
                break;
        }
    }
    
    void UsePower()
    {
        if (canUsePowers && canUsePower1 && Input.GetKeyDown(keyPower1))
        {
            isPower1Active = true;
            if (isPower2Active)
            {
                isPower2Active = false;
                //faire entrer le power 2 en cooldown
            }
            //Fonctions qui utilisent les pouvoirs
            //
            //
        }
        
        if (canUsePowers && canUsePower2 && Input.GetKeyDown(keyPower2))
        {
            isPower2Active = true;
            if (isPower1Active)
            {
                isPower1Active = false;
                //faire entrer le power 1 en cooldown
            }
            //Fonctions qui utilisent les pouvoirs
            //
            //
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
