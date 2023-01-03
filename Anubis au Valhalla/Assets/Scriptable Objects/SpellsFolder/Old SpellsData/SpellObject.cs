using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SpellType
{
    Throwing,
    FollowingArea,
    StaticArea,
    DefenceShield,
    SpawnEntity,
    PouvoirFeu,
    PouvoirPlaie,
    PouvoirEau,
    PouvoirFoudre,
    PouvoirAme,
    PouvoirMalediction
}

public abstract class SpellObject : ScriptableObject
{
    [Header("GENERAL")]
    //public GameObject prefab;
    public SpellType type;
    public string nom;
    public int spellIndex;
    [TextArea(10,20)] public string description;
    [TextArea(5, 10)] public string citation;
    
    [Header("GRAPH")]
    public Texture sprite;
}