using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class NewPowerManager : MonoBehaviour
{
    public GameObject targetUser;
    public KeyCode keyPower1;
    public KeyCode keyPower2;
    public LayerMask layerMonstres;
    
    [Foldout("SYSTEM")] public int levelPower1;
    [Foldout("SYSTEM")] public int levelPower2;

    [Foldout("SYSTEM")] public GameObject comboPower1;
    [Foldout("SYSTEM")] public GameObject thrustPower1;
    [Foldout("SYSTEM")] public GameObject dashPower1;
    [Foldout("SYSTEM")] public GameObject comboPower2;
    [Foldout("SYSTEM")] public GameObject thrustPower2;
    [Foldout("SYSTEM")] public GameObject dashPower2;
    
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
        
    }

    
    void Update()
    {
        
    }
}
