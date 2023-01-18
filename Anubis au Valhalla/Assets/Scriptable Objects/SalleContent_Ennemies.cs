using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Room Content/Pattern Enemies")]
public class SalleContent_Ennemies : ScriptableObject
{
    [Expandable] public EnemyData[] enemiesToSpawn;
}
