using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainScriptableObject : MonoBehaviour
{
    public static ContainScriptableObject instance;
    public SpellObject spellInside;
    public GameObject prefabInside;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
