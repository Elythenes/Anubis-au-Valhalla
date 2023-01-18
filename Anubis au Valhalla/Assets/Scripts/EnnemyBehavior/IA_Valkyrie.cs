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
    public float moveSpeed;
    public float idleTimeMax;
    public float idleTimeMin;
    private float currentIdleTime;


        [Header("Déplacements")]
    public GameObject player;
    public Seeker seeker;
    public AIPath aipath;
    private Path path;
    public GameObject canvasLifeBar;
    private Rigidbody2D rb;
    private Collider2D collider;
    public Animator anim;
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
    [BoxGroup("Attaque - Javelot")] public float IndicJavelotTime;
    [BoxGroup("Attaque - Javelot")] public List<int> javelotsToSpawn = new List<int>();
    [BoxGroup("Attaque - Javelot")] public List<float> javelotInterval = new List<float>();
    [Header("Refs et visus")]
    [BoxGroup("Attaque - Javelot")] public JavelotValkyrie projectilJavelot;
    [BoxGroup("Attaque - Javelot")] [HideInInspector] public Vector2 dir;
    [BoxGroup("Attaque - Javelot")] public Transform[] restingPosList;
    
    [BoxGroup("Attaque - Jump")] [NaughtyAttributes.ReadOnly] public int FallDamage;
    [BoxGroup("Attaque - Jump")] public float fallDamageMultiplier = 1.5f;
    [BoxGroup("Attaque - Jump")] public float jumpSpeed;
    [BoxGroup("Attaque - Jump")] public float airTravelSpeed;
    [BoxGroup("Attaque - Jump")] public float pushForce;
    [BoxGroup("Attaque - Jump")] public bool hasShaked;
    [BoxGroup("Attaque - Jump")] public bool isSeeking;
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
    [BoxGroup("Attaque - Jump")] public GameObject SpriteObj;
    [BoxGroup("Attaque - Jump")] public GameObject ombreObj;
    [BoxGroup("Attaque - Jump")] public GameObject activeOmbre;
    [BoxGroup("Attaque - Jump")] public Vector3 stretchAmount;
    private float distanceWithPlayer;
    
    
    //Fonctions ******************************************************************************************************************************************************
    
    private void Awake()
    {
        DOTween.SetTweensCapacity(500,50);
        puissanceAttaqueJavelot = GetComponentInParent<MonsterLifeManager>().data.damage;
        FallDamage = Mathf.RoundToInt(puissanceAttaqueJavelot * fallDamageMultiplier);

    }
    
    private void Start()
    {
        spriteArray = GetComponentsInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        seeker = GetComponent<Seeker>();
        collider = GetComponent<BoxCollider2D>();
        ai = GetComponent<IAstarAI>();
        playerFollow.enabled = true;
        playerFollow.target = player.transform;
        Debug.Log(ai.maxSpeed);
        ai.maxSpeed = moveSpeed;
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
        
        if(!isAttacking&& !life.isMomified) // Cooldown des attaques;
        {
            StartUpJavelotTimeTimer += Time.deltaTime;
            TriggerJumpTimeTimer += Time.deltaTime;
        }
        
        if (TriggerJumpTimeTimer >= TriggerJumpTime) // Attaque saut
        {
            TriggerSaut();
            JumpTimeTimer += Time.deltaTime;
        }
        if (JumpTimeTimer >= JumpTime) // Déplacement dans les airs
        {
            IndicationTimeTimer += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position,
                CharacterController.instance.transform.position, airTravelSpeed);
            distanceWithPlayer = Vector2.Distance(CharacterController.instance.transform.position,transform.position);

        }
        if (!hasFallen && (IndicationTimeTimer > IndicationTime || distanceWithPlayer <= 2))
        {
            Debug.Log("hein?");
            hasFallen = true;
            var hitboxFall = Instantiate(indicationFall);
            Destroy(hitboxFall, jumpSpeed);
            transform.DOShakePosition(jumpSpeed).OnComplete(() => 
            {
                SpriteObj.transform.DOMove(SpriteObj.transform.position + Vector3.down * 100, jumpSpeed);
            });
        }




        if (StartUpJavelotTimeTimer >= StartUpJavelotTime) // Attaque javelot
        {
            attaqueJavelot();
        }
        if (!isFleeing&& !life.isMomified) // Déplacements
        {
            deplacement();
        }
        
    }

    #region Mouvement
    void PickRandomPoint() 
    {
        var point = Random.insideUnitCircle * radiusWondering;
        point.x += ai.position.x;
        point.y += ai.position.y;
        
        if (Vector3.Distance(player.transform.position, point) !<= radiusWondering)
        {
            PickRandomPoint();
        }
        else
        {
            pointToGo = point;
        }
    }
    void deplacement()
    {
        if (!ai.pathPending && ai.reachedEndOfPath || !ai.hasPath)
        {
            currentIdleTime -= Time.deltaTime;
            if (currentIdleTime < 0)
            {
                currentIdleTime = idleTimeMin;
                playerFollow.enabled = false;
                PickRandomPoint();
                ai.destination = pointToGo;
                ai.SearchPath();
            }
        }
    }
    #endregion


    #region Javelot

    void attaqueJavelot()
    {
        isAttacking = true;
        anim.SetBool("isAttacking",true);
        StartUpJavelotTimeTimer = 0;
        transform.DOShakePosition(IndicJavelotTime,1).OnComplete(() =>
        {
            for (int i = 0; i < javelotsToSpawn[(int)currentState]; i++)
            {
                var projJavelot = Instantiate(projectilJavelot, transform.position, Quaternion.identity);
                projJavelot.ia = this;
                projJavelot.restingPos = restingPosList[i];
                projJavelot.timeForAim += javelotInterval[(int)currentState] * i;

            }
            isAttacking = false;
            anim.SetBool("isAttacking",false);
        });
    }

    #endregion
    
    #region Charge au sol
    void TriggerSaut()
    {
        FallTimeTimer = 0;
        hasFallen = false;
        isAttacking = true;
        ai.canMove = false;
            
        if (!hasShaked)
        {
            hasShaked = true;
            anim.SetBool("isJumping",true);
            for (int i = 0; i < javelotsToSpawn[(int)currentState] - 2; i++)
            {
                var projJavelot = Instantiate(projectilJavelot, transform.position, Quaternion.identity);
                projJavelot.ia = this;
                projJavelot.timeForAim += javelotInterval[(int)currentState] * i;
                projJavelot.restingPos = restingPosList[i];
                projJavelot.skyFall = true;
            }
            transform.DOShakePosition(1.1f,0.2f,50).OnComplete(() =>
            {
                activeOmbre = Instantiate(ombreObj, transform);
                activeOmbre.transform.localScale = new Vector3(0.36f,0.36f,0.36f);
                ombreObj.SetActive(false);
                activeOmbre.transform.DOScale(Vector3.one * 0.2f, 0.5f);
                SpriteObj.transform.DOScale(stretchAmount, 0.2f);
                SpriteObj.transform.DOMove(SpriteObj.transform.position + Vector3.up * 100, jumpSpeed);
                anim.SetBool("isJumping",false);
            });
        }
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
    
    #endregion



}

