using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GenPro;
using NaughtyAttributes;
using Pathfinding;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
// ReSharper disable All

public class IA_Valkyrie : MonoBehaviour
{
    [Header("Général")] 
    public static IA_Valkyrie instance;
    public bool isElite;
    public GameObject emptyLayers;
    public MonsterLifeManager life;
    public SpriteRenderer[] spriteArray;
    public float moveSpeed;
    public float idleTimeMax;
    public float idleTimeMin;
    private float currentIdleTime;
    public bool isAttacking;


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

    [Space(20)]

    [BoxGroup("Attaque - Javelot")] public int puissanceAttaqueJavelot;
    [BoxGroup("Attaque - Javelot")] public float javelotSpeed;
    [BoxGroup("Attaque - Javelot")] public float StartUpJavelotTime;
    [BoxGroup("Attaque - Javelot")] public float IndicJavelotTime;
    [BoxGroup("Attaque - Javelot")] public List<int> javelotsToSpawn = new List<int>();
    [BoxGroup("Attaque - Javelot")] public List<float> javelotInterval = new List<float>();
    [BoxGroup("Attaque - Javelot")] public float StartUpJavelotTimeTimer;
    [Header("Refs et visus")]
    [BoxGroup("Attaque - Javelot")] public JavelotValkyrie projectilJavelot;
    [BoxGroup("Attaque - Javelot")] [HideInInspector] public Vector2 dir;
    [BoxGroup("Attaque - Javelot")] public Transform[] restingPosList;
    [Space(20)]
    
    [BoxGroup("Attaque - Jump")] [NaughtyAttributes.ReadOnly] public int FallDamage;
    [BoxGroup("Attaque - Jump")] public float fallDamageMultiplier = 1.5f;
    [BoxGroup("Attaque - Jump")] public float jumpSpeed;
    [BoxGroup("Attaque - Jump")] public float airTravelSpeed;
    [BoxGroup("Attaque - Jump")] public float pushForce;
    [BoxGroup("Attaque - Jump")] public bool hasShaked;
    [BoxGroup("Attaque - Jump")] public bool hasFallen;
    [BoxGroup("Attaque - Jump")] [Tooltip("Le temps que met l'attaque à se tick")] public float TriggerJumpTime;
    [BoxGroup("Attaque - Jump")] public float JumpTime;
    [BoxGroup("Attaque - Jump")] public float IndicationTime;
    [BoxGroup("Attaque - Jump")] public float FallTime;
    [Foldout("Timers - Jump")] public float TriggerJumpTimeTimer;     
    [Foldout("Timers - Jump")] public float JumpTimeTimer;            // Le temps que met la valkyrie à sauter et disparaitre
    [Foldout("Timers - Jump")] public float IndicationTimeTimer;           // Le temps que met la valkyrie entre l'indication de l'attaque (zone rouge) et la retombée
    [Foldout("Timers - Jump")] public float FallTimeTimer;           // Le temps que met la valkyrie entre la retombée et le retour à son etat normal.
    [Header("Refs et visus")]
    [BoxGroup("Attaque - Jump")] public GameObject indicationFall;
    [BoxGroup("Attaque - Jump")] public GameObject hitboxFall;
    [BoxGroup("Attaque - Jump")] private Vector2 fallPos;
    [BoxGroup("Attaque - Jump")] public GameObject SpriteObj;
    [BoxGroup("Attaque - Jump")] public GameObject ombreObj;
    [BoxGroup("Attaque - Jump")] public GameObject activeOmbre;
    [BoxGroup("Attaque - Jump")] public Vector3 stretchAmount;
    [BoxGroup("Attaque - Jump")] public List<Skyfall> skyfallIndic = new List<Skyfall>();
    private float distanceWithPlayer;
    private Vector3 savedScale;
    private Vector3 savedPos;

    [Space(20)] 
    [BoxGroup("Attaque - Dash")] public List<int> dashAmount = new List<int>();

    [BoxGroup("Attaque - Dash")] public bool isDashing;
    [BoxGroup("Attaque - Dash")] public bool charging;
    [BoxGroup("Attaque - Dash")] public float TriggerDashTimer;
    [BoxGroup("Attaque - Dash")] public float TriggerDashTime;
    [BoxGroup("Attaque - Dash")] public float windUpSpeed;
    [BoxGroup("Attaque - Dash")] public float windUpDistance;
    [BoxGroup("Attaque - Dash")] public float chargeSpeed;
    [BoxGroup("Attaque - Dash")] public float chargeDistance;
    [BoxGroup("Attaque - Dash")] public float TimeBetweenDashes;
    [BoxGroup("Attaque - Dash")] public float StunTime;
    [BoxGroup("Attaque - Dash")] public int executedDashes;
    [Header("Refs et visus")]
    [BoxGroup("Attaque - Dash")] public GameObject dashIndic;
    [BoxGroup("Attaque - Dash")] public GameObject IndicMask;
    

    //Fonctions ******************************************************************************************************************************************************
    
    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(instance);
        }
        instance = this;
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
            TriggerDashTimer += Time.deltaTime;
        }
        
        if (TriggerJumpTimeTimer >= TriggerJumpTime) // Attaque saut
        {
            TriggerSaut();
            JumpTimeTimer += Time.deltaTime;
        }
        if (JumpTimeTimer >= JumpTime) // Déplacement dans les airs
        {
            IndicationTimeTimer += Time.deltaTime;
            dir = new Vector2(CharacterController.instance.transform.position.x - transform.position.x,CharacterController.instance.transform.position.y - transform.position.y);
            dir.Normalize();
            rb.velocity = dir * airTravelSpeed;
            distanceWithPlayer = Vector2.Distance(CharacterController.instance.transform.position,transform.position);
            if (IndicationTimeTimer > IndicationTime || distanceWithPlayer <= 2)
            {
                if (!hasFallen)
                {
                    rb.velocity = Vector2.zero;
                    anim.enabled = false;
                    hasFallen = true;
                    TriggerJumpTimeTimer = 0;
                    JumpTimeTimer = 0;
                    var hitboxFall = Instantiate(indicationFall);
                    Destroy(hitboxFall, jumpSpeed);
                    StartCoroutine(LagFall());
                }
            }

        }

        if (TriggerDashTimer > TriggerDashTime && !isDashing && executedDashes < dashAmount[(int)currentState])
        {
            aipath.canMove = false;
            isDashing = true;
            Dash();
        }
        else if (isDashing && executedDashes >= dashAmount[(int)currentState])
        {
            aipath.canMove = true;
            isAttacking = false;
            isDashing = false;
            charging = false;
            TriggerDashTimer = 0;
            executedDashes = 0;
            
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
                projJavelot.javelotNumber = i;
                projJavelot.timeForIndic += 0.1f * i;
            }
            transform.DOShakePosition(1.1f,0.2f,50).OnComplete(() =>
            {
                savedPos = SpriteObj.transform.localPosition;
                savedScale = SpriteObj.transform.localScale;
                activeOmbre = Instantiate(ombreObj, transform);
                activeOmbre.transform.localScale = new Vector3(0.36f,0.36f,0.36f);
                ombreObj.SetActive(false);
                activeOmbre.transform.DOScale(Vector3.one * 0.2f, 0.5f);
                SpriteObj.transform.DOScale(stretchAmount, 0.1f).OnComplete((() => SpriteObj.SetActive(false)));
                anim.SetBool("isJumping",false);
                
            });
        }
    }   

    IEnumerator LagFall() // A la fin de l'attaque du saut
    {
        /*GameObject hitboxObj = Instantiate(hitboxFall, transform.position, Quaternion.identity);
        hitboxObj.GetComponent<HitBoxFallValkyrie>().ia = this;
        yield return new WaitForSeconds(1);
        Destroy(hitboxObj);
        ai.canMove = true;
        IndicationTimeTimer = 0;
        isAttacking = false;*/
        yield return new WaitForSeconds(1);
        activeOmbre.transform.DOScale(Vector3.one * 0.36f, 0.5f);
        SpriteObj.transform.DOScale(savedScale, 0.2f);
        yield return new WaitForSeconds(0.5f);
        SpriteObj.SetActive(true);
        Destroy(activeOmbre);
        var hitbox = Instantiate(hitboxFall, transform.position, Quaternion.identity);
        transform.DOShakePosition(FallTime, 0.2f, 50).OnComplete(() =>
        { 
            Destroy(hitbox);
            isAttacking = false;
            anim.enabled = true;
            hasFallen = false;
            TriggerJumpTimeTimer = 0;
            JumpTimeTimer = 0;
            IndicationTimeTimer = 0;
            ai.canMove = true;
            hasShaked = false;
        });

    }
    #endregion

    #region Dashs

    void Dash()
    {
        isAttacking = true;
        dir = new Vector2(CharacterController.instance.transform.position.x - transform.position.x,CharacterController.instance.transform.position.y - transform.position.y);



        dir.Normalize();
        Debug.Log(dir);
        var reversedir = -dir;
        Debug.Log(reversedir);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var Indication = Instantiate(dashIndic, transform.position, Quaternion.Euler(0, 0, angle));
        /*var currentMask = Instantiate(IndicMask, transform.position, quaternion.Euler(0, 0, angle));
        currentMask.transform.DOMove(dir * chargeDistance, windUpSpeed);*/
        transform.DOPunchPosition(reversedir* windUpDistance,windUpSpeed).OnComplete((() =>
        {
            charging = true;
            //Destroy(currentMask);
            Destroy(Indication);
            transform.DOMove(dir * chargeDistance, chargeSpeed).OnComplete((() =>
            {
                executedDashes++;
                if (executedDashes < dashAmount[(int)currentState])
                {
                    transform.DOMove(transform.position, TimeBetweenDashes).OnComplete((() =>
                    {
                        charging = false;
                        isDashing = false;
                    }));
                }
            }));
        }));

    }

    #endregion

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && charging)
        {
            DamageManager.instance.TakeDamage(FallDamage, gameObject);
        }
    }
}

