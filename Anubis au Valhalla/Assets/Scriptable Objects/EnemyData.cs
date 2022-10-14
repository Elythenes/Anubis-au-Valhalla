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
    
    [Header("Déplacements - Général")] 
    public bool isFleeing;
    public bool isWondering;
    public bool isAttacking;
    public bool hasShaked;
    public Vector3 pointToGOFleeing;
    
    [Header("Shaman - Déplacements")]
    public float radiusWonderingShaman;
    public float forceRepulseShaman;
    public float distanceMaxPlayerShaman;
    public float timeFleeingShaman;
    public float timeFleeingShamanTimer;
   
    [Header("Shaman - Summon")] 
    public float StartUpSummonTime;
    public float StartUpSummonTimeTimer;
    public float SummoningTime;
    public float SummoningTimeTimer;
    public GameObject corbeau;

    [Header("Valkyrie - Javalot")]
    public int puissanceAttaqueJavelot;
    public float javelotSpeed;
    public float StartUpJavelotTime;
    public float StartUpJavelotTimeTimer;
    public GameObject projectilJavelot;

    [Header("Valkyrie - Jump")]
    public GameObject indicationFall;
    public GameObject hitboxFall;
    private Vector2 fallPos;
    public int FallDamage;
    public float pushForce;
    public bool hasFallen;
    public float TriggerJumpTime;
    public float TriggerJumpTimeTimer;     // Le temps que met l'attaque à se tick
    public float JumpTime;
    public float JumpTimeTimer;            // Le temps que met la valkyrie à sauter et disparaitre
    public float IndicationTime;
    public float IndicationTimeTimer;           // Le temps que met la valkyrie entre l'indication de l'attaque (zone rouge) et la retombée
    public float FallTime;
    public float FallTimeTimer;           // Le temps que met la valkyrie entre la retombée et le retour à son etat normal.
}
