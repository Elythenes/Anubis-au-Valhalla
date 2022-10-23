using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Room Content/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int cost;
    public GameObject prefab;
    public bool isElite;
}
