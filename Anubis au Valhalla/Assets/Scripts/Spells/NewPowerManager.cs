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

    public float cooldownPower1 = 8f;
    public float currentCooldownPower1;
    public float cooldownPower2 = 8f;
    public float currentCooldownPower2;

    public List<GameObject> spellPower1 = new List<GameObject>(1);
    public List<GameObject> comboPower1 = new List<GameObject>(1);
    public List<GameObject> thrustPower1 = new List<GameObject>(1);
    public List<GameObject> dashPower1 = new List<GameObject>(1);
    public List<GameObject> spellPower2 = new List<GameObject>(1);
    public List<GameObject> comboPower2 = new List<GameObject>(1);
    public List<GameObject> thrustPower2 = new List<GameObject>(1);
    public List<GameObject> dashPower2 = new List<GameObject>(1);

    
    [Header("DEBUG")] 
    public bool canUsePowers;
    public bool canUsePower1;
    public bool canUsePower2;
    
    public int startingLevelPower1 = 1;
    public int startingLevelPower2 = 1;

    public bool isPower1Active;
    public bool isPower2Active;
    
    public bool spellPower1Unlocked;
    public bool spellPower2Unlocked;
    
    
    
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
    
    
    
    void Start()
    {
        ChangeLevelPower(1, startingLevelPower1); 
        ChangeLevelPower(2, startingLevelPower2);
        canUsePowers = true;
        canUsePower1 = true;
        canUsePower2 = true;
    }
    
    
    void Update()
    {
        DoPower();
        
    }


    void DoPower()
    {
        if (canUsePowers && canUsePower1 && Input.GetKeyDown(keyPower1))
        {
            isPower1Active = true;
            if (isPower2Active)
            {
                isPower2Active = false;
                //faire entrer le power 2 en cooldown
            }
            if (spellPower1Unlocked)
            {
                //Fonction du spell qui se lance automatique à l'activation du pouvoir
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
            if (spellPower2Unlocked)
            {
                //Fonction du spell qui se lance automatique à l'activation du pouvoir
            }
            //Fonctions qui utilisent les pouvoirs
            //
            //
        }
        
    }
    
    
    void ChangeLevelPower(int powerIndex, int currentLevel) //fonction à appeler quand on level Up; ou pour tester
    {
        if (powerIndex == 1)
        {
            currentLevelPower1 = currentLevel;
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
    }
}
