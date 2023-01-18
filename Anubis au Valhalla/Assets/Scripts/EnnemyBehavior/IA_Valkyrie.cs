using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GenPro;
using NaughtyAttributes;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class IA_Valkyrie : MonoBehaviour
{
   [Header("Général")]
    public bool isElite;
    public GameObject emptyLayers;
    public MonsterLifeManager life;
    public SpriteRenderer[] spriteArray;


    [Header("Déplacements")]
    public GameObject player;
    public Seeker seeker;
    public AIPath aipath;
    private Path path;
    public GameObject canvasLifeBar;
    private Rigidbody2D rb;
    private Collider2D collider;
    IAstarAI ai;
    public AIDestinationSetter playerFollow;
    public bool isFleeing;
    public float radiusWondering;
    public Vector2 pointToGo;
    public State currentState;
    public enum State
    {
        FullHp = 0,
        TroisQuarts = 1,
        Moitié = 2,
        UnQuart = 3
    }



    [BoxGroup("Attaque - Javelot")] public bool isAttacking;
    [BoxGroup("Attaque - Javelot")] public int puissanceAttaqueJavelot;
    [BoxGroup("Attaque - Javelot")] public float javelotSpeed;
    [BoxGroup("Attaque - Javelot")] public float StartUpJavelotTime;
    [BoxGroup("Attaque - Javelot")] public float StartUpJavelotTimeTimer;
    [BoxGroup("Attaque - Javelot")] public bool isJavelotIndic;
    [BoxGroup("Attaque - Javelot")] public float IndicJavelotTime;
    [BoxGroup("Attaque - Javelot")] public float IndicJavelotTimeTimer;
    [BoxGroup("Attaque - Javelot")] public List<int> javelotsToSpawn = new List<int>();
    [BoxGroup("Attaque - Javelot")] public List<float> javelotInterval = new List<float>();
    [Header("Refs et visus")]
    [BoxGroup("Attaque - Javelot")] public JavelotValkyrie projectilJavelot;
    [BoxGroup("Attaque - Javelot")] [HideInInspector] public Vector2 dir;
    [BoxGroup("Attaque - Javelot")] public Transform[] restingPosList;
    
    [BoxGroup("Attaque - Jump")] [NaughtyAttributes.ReadOnly] public int FallDamage;
    [BoxGroup("Attaque - Jump")] public float fallDamageMultiplier = 1.5f;
    [BoxGroup("Attaque - Jump")] public float pushForce;
    [BoxGroup("Attaque - Jump")] public bool hasShaked;
    [BoxGroup("Attaque - Jump")] public bool hasFallen;
    [BoxGroup("Attaque - Jump")] public float TriggerJumpTime;
    [BoxGroup("Attaque - Jump")] public float TriggerJumpTimeTimer;     // Le temps que met l'attaque à se tick
    [BoxGroup("Attaque - Jump")] public float JumpTime;
    [BoxGroup("Attaque - Jump")] public float JumpTimeTimer;            // Le temps que met la valkyrie à sauter et disparaitre
    [BoxGroup("Attaque - Jump")] public float IndicationTime;
    [BoxGroup("Attaque - Jump")] public float IndicationTimeTimer;           // Le temps que met la valkyrie entre l'indication de l'attaque (zone rouge) et la retombée
    [BoxGroup("Attaque - Jump")] public float FallTime;
    [BoxGroup("Attaque - Jump")] public float FallTimeTimer;           // Le temps que met la valkyrie entre la retombée et le retour à son etat normal.
    [Header("Refs et visus")]
    [BoxGroup("Attaque - Jump")] public GameObject indicationFall;
    [BoxGroup("Attaque - Jump")] public GameObject hitboxFall;
    [BoxGroup("Attaque - Jump")] private Vector2 fallPos;
    
    
    //Fonctions ******************************************************************************************************************************************************
    
    private void Awake()
    {
        puissanceAttaqueJavelot = GetComponentInParent<MonsterLifeManager>().data.damage;
        FallDamage = Mathf.RoundToInt(puissanceAttaqueJavelot * fallDamageMultiplier);
    }
    
    private void Start()
    {
        StartUpJavelotTime = Random.Range(5, 9);
        TriggerJumpTime = Random.Range(8, 13);
        spriteArray = GetComponentsInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        seeker = GetComponent<Seeker>();
        collider = GetComponent<BoxCollider2D>();
        ai = GetComponent<IAstarAI>();
        playerFollow.enabled = true;
        playerFollow.target = player.transform;
    }

    void UpdateCurrentState()
    {
        if (life.vieActuelle > life.vieMax * 0.75f) currentState = State.FullHp;
        else if (life.vieActuelle > life.vieMax * 0.5f) currentState = State.TroisQuarts;
        else if (life.vieActuelle > life.vieMax * 0.25f) currentState = State.Moitié;
        else currentState = State.UnQuart;
    }
    public void Update()
    {
        if (life.gotHit)
        {
            aipath.canMove = true;
            UpdateCurrentState();
        }
        
        if(!isAttacking&& !life.isMomified) // Cooldwn des attaques;
        {
            StartUpJavelotTimeTimer += Time.deltaTime;
            TriggerJumpTimeTimer += Time.deltaTime;
        }
        
        if (TriggerJumpTimeTimer >= TriggerJumpTime) // Attaque saut
        {
            JumpTimeTimer += Time.deltaTime;
            TriggerSaut();
        }
        
        if (JumpTimeTimer >= JumpTime)
        {
                TriggerJumpTimeTimer = 0;
                hasShaked = false;
                canvasLifeBar.SetActive(false);
                foreach (SpriteRenderer sprite in spriteArray)
                {
                    sprite.enabled = false;
                }
                collider.enabled = false;
                IndicationTimeTimer += Time.deltaTime;
            
        }
        
        if (IndicationTimeTimer >= IndicationTime)
        {
            indicatorAndFall();
            FallTimeTimer += Time.deltaTime;
        }
        
        if (StartUpJavelotTimeTimer >= StartUpJavelotTime) // Attaque javelot
        {
            attaqueJavelot();
        }

        /*if (isJavelotIndic)
        {
            IndicJavelotTimeTimer += Time.deltaTime;
            if (indicHolder is not null)
            {
                Debug.Log("ouiqdqdd");
                indicHolder.GetComponent<SpriteRenderer>().color = gradientIndic.Evaluate(IndicJavelotTimeTimer);
            }
            
            if (IndicJavelotTimeTimer >= IndicJavelotTime)
            {
                StartUpJavelot();
                IndicJavelotTimeTimer = 0;
            }
        }*/
        
        if (!isFleeing&& !life.isMomified) // Déplacements
        {
            deplacement();
        }
        
    }

    void TriggerSaut()
    {
        FallTimeTimer = 0;
        hasFallen = false;
        isAttacking = true;
        ai.canMove = false;
            
        if (!hasShaked)
        {
            transform.DOShakePosition(1f, 1);
            hasShaked = true;
        }
    }
    void indicatorAndFall()
    {
        JumpTimeTimer = 0;
        if (!hasFallen)
        {
            fallPos = player.transform.position;
            hasFallen = true;
            GameObject indicationObj = Instantiate(indicationFall, player.transform.position, Quaternion.identity);
            Destroy(indicationObj,FallTime);
        }
        
        if (FallTimeTimer >= FallTime)
        {
            IndicationTimeTimer = 0;
            FallTimeTimer = 0;
            transform.position = fallPos;
            canvasLifeBar.SetActive(true);
            foreach (SpriteRenderer sprite in spriteArray)
            {
                sprite.enabled = true;
            }
            collider.enabled = true;
            StartCoroutine(LagFall());
        }             
    }
    
    void deplacement()
    {
        if (!ai.pathPending && ai.reachedEndOfPath || !ai.hasPath) 
        {
            playerFollow.enabled = false;
            PickRandomPoint();
            ai.destination = pointToGo;
            ai.SearchPath();
        }
    }

    void attaqueJavelot()
    {
        isAttacking = true;
        StartUpJavelotTimeTimer = 0;
        for (int i = 0; i < javelotsToSpawn[(int)currentState]; i++)
        {
            var projJavelot = Instantiate(projectilJavelot, transform.position, Quaternion.identity);
            projJavelot.ia = this;
            projJavelot.restingPos = restingPosList[i];
            projJavelot.timeForAim += javelotInterval[(int)currentState] * i;

        }
        transform.DOShakePosition(IndicJavelotTime,1).OnComplete(() =>
        {
            StartUpJavelot();
        });
    }
    
  
    void StartUpJavelot() // Au début de l'attaque du javelot
    {

        isAttacking = false;
        isJavelotIndic = false;
    }

    IEnumerator LagFall() // A la fin de l'attaque du saut
    {
        Debug.Log("oui");
        GameObject hitboxObj = Instantiate(hitboxFall, transform.position, Quaternion.identity);
        hitboxObj.GetComponent<HitBoxFallValkyrie>().ia = this;
        yield return new WaitForSeconds(1);
        Destroy(hitboxObj);
        ai.canMove = true;
        IndicationTimeTimer = 0;
        isAttacking = false;
    }    
    void PickRandomPoint() 
    {
        var point = Random.insideUnitCircle * radiusWondering;
        point.x += ai.position.x;
        point.y += ai.position.y;
        
        if (Vector3.Distance((Vector3)player.transform.position, point) !<= radiusWondering)
        {
            PickRandomPoint();
        }
        else
        {
            pointToGo = point; 
            return;
        }
    }
}

