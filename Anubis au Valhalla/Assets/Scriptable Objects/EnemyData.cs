using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Room Content/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float speed;
    public float maxHP;
    public float damage;
    public int cost;
    public GameObject prefab;
}
