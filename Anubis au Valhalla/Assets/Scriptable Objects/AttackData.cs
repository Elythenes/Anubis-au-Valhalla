using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace Attacks
{
    [CreateAssetMenu(menuName = "Attack/AttackBaseStat", fileName = "Globals Stats")]
    public class AttackBaseStat : ScriptableObject
    {
        public List<AttackStat> listBaseStats;
    }
}