using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spell System/SpellObject")]

public class SpellObject : ScriptableObject
{
    [Header("Général")] 
    public GameObject player;
    public KeyCode spell1;
    public KeyCode spell2;
    public GameObject targetUser;
    public LayerMask layerMonstres;

    [Header("FireBall")]
    public GameObject fireBall;
    public float launchVelocity = 100f;
    public float rangeAttaqueFireBall;
    public int puissanceAttaqueFireBall;
    
}
