using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    [System.Serializable]
    public class AttackStat
    {
        [Header("Stats")]
        public GameObject hitboxObj;
        public int damage;
        public float dureeHitbox;
        public float stunAfterAttack;
        public float movementAttack;
        public float cooldownAbandonCombo;
        public Vector2 hitboxSize;
    }
}