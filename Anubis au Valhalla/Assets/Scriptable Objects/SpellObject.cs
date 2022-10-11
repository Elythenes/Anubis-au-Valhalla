using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SpellType
{
    Throwing,
    FollowingArea,
    StaticArea
}

public abstract class SpellObject : ScriptableObject
{
    [Header("GENERAL")]
    public GameObject prefab;
    public SpellType type;
    public string nom;
    [TextArea(15,20)]
    public string description;
    
    [Header("GRAPH")]
    public Image sprite;
    
}
