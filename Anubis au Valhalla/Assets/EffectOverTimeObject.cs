using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Instant Potion" ,menuName = "Potion System/PotionObject/EffectOverTime")]
public class EffectOverTimeObject : PotionObject
{
    public int buffAmount;
    public int buffDuration;
    public float cooldownTimer;
    public float cooldown;
    public bool canCast;

    public void Awake()
    {
        type = PotionType.EffectOverTime;
    }
}
