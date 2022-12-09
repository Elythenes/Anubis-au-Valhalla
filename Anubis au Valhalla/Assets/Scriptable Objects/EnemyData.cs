using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Room Content/EnemyData")]
public class EnemyData : ScriptableObject
{
    public GameObject prefab;
    
    [Header("PRINCIPAL")]
    public bool isElite;
    public int cost;
    public int damage;
    public int maxHealth;

    [Header("ANNEXE")] 
    public int soulScore;
    public int score;

    [Foldout("TEST")] public int armorPercentage;
    [Foldout("TEST")] [Range(1, 4)] public int level = 1;
    [Foldout("TEST")] public float[] levelMultiplier = new float[3]{1.2f,1.2f,1.2f};
}
