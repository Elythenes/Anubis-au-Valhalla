
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using Pathfinding;
using UnityEngine;

using Random = UnityEngine.Random;
// ReSharper disable All

public class IA_Valkyrie : MonoBehaviour
{
    [Header("Général")] 
    [Tooltip("vitesse de la Valkyrie quand elle n'attaque pas")]public float moveSpeed;
    [Tooltip("Temps entre les déplacements")]public float idleTime;
    [Header("Main débug")]
    public bool isAttacking;
    public bool isFleeing;
    public float radiusWondering;
    public Vector2 pointToGo;
    public State currentState;
    public bool midlifeKnockBack;
    


    public enum State
    {
        FullHp = 0,
        TroisQuarts = 1,
        Moitié = 2,
        UnQuart = 3
    }

    [Space(20)]

    [BoxGroup("Attaque - Javelot")] [Tooltip("Dégâts de l'attaque")]public int puissanceAttaqueJavelot;
    [BoxGroup("Attaque - Javelot")] [Tooltip("Vitesse du javelot")]public float javelotSpeed;
    [BoxGroup("Attaque - Javelot")] [Tooltip("Fréquence de l'attaque")]public float StartUpJavelotTime;
    [BoxGroup("Attaque - Javelot")] [Tooltip("Temps de l'indicateur du javelot")]public float IndicJavelotTime;
    [BoxGroup("Attaque - Javelot")] [Tooltip("Javelots à spawn selon la vie de la Valkyrie, utilisé aussi pour le saut")]public List<int> javelotsToSpawn = new List<int>();
    [BoxGroup("Attaque - Javelot")] [Tooltip("l'intervale de lancer entre chaque javelot selon la vie de la valkyrie")]public List<float> javelotInterval = new List<float>();
    [Header("Refs et visus")]
    [BoxGroup("Attaque - Javelot")] public JavelotValkyrie projectilJavelot;
    [BoxGroup("Attaque - Javelot")] [HideInInspector] public Vector2 dir;
    [BoxGroup("Attaque - Javelot")] public Transform[] restingPosList;
    [Header("Débug")]
    [BoxGroup("Attaque - Javelot")] public float StartUpJavelotTimeTimer;
    [Space(20)]
    
    [BoxGroup("Attaque - Jump")] [Tooltip("Dégâts de l'attaque")]public int FallDamage;
    [BoxGroup("Attaque - Jump")] [Tooltip("Vitesse de retombée de la Valkyrie et des javelots")]public float jumpSpeed;
    [BoxGroup("Attaque - Jump")] [Tooltip("vitesse de la valkyrie dans les airs selon sa vie")]public float[] airTravelSpeed;
    [BoxGroup("Attaque - Jump")] [Tooltip("knockback de l'attaque")]public float pushForce;
    [BoxGroup("Attaque - Jump")] [Tooltip("Fréquence d'attaque")] public float TriggerJumpTime;
    [BoxGroup("Attaque - Jump")] [Tooltip("Latence avant le déplacement dans les airs")]public float JumpTime;
    [BoxGroup("Attaque - Jump")] [Tooltip("Temps de déplacement avant la retombée")]public float IndicationTime;
    [BoxGroup("Attaque - Jump")] [Tooltip("Durée des hitboxs")]public float FallTime;
    [Header("Refs et visus")]
    [BoxGroup("Attaque - Jump")] public GameObject indicationFall;
    [BoxGroup("Attaque - Jump")] public GameObject hitboxFall;
    [BoxGroup("Attaque - Jump")] public GameObject SpriteObj;
    [BoxGroup("Attaque - Jump")] public GameObject ombreObj;
    [BoxGroup("Attaque - Jump")] public GameObject activeOmbre;
    [BoxGroup("Attaque - Jump")] public Vector3 stretchAmount;
    [BoxGroup("Attaque - Jump")] public List<Skyfall> skyfallIndic = new List<Skyfall>();
    [Header("Dégug")]
    [BoxGroup("Attaque - Jump")] public float TriggerJumpTimeTimer;
    [BoxGroup("Attaque - Jump")] public float JumpTimeTimer;
    [BoxGroup("Attaque - Jump")] public float IndicationTimeTimer;
    [BoxGroup("Attaque - Jump")] public float FallTimeTimer;
    [BoxGroup("Attaque - Jump")] public bool hasShaked;
    [BoxGroup("Attaque - Jump")] public bool hasFallen;
    [BoxGroup("Attaque - Jump")] private Vector2 fallPos;
    private float distanceWithPlayer;
    private Vector3 savedScale;
    private Vector3 savedPos;

    [Space(30)] 
    [BoxGroup("Attaque - Dash")] [Tooltip("Dégâts de l'attaque")]public int dashDamage;
    [BoxGroup("Attaque - Dash")] [Tooltip("Nombre de dashs par attaque selon la vie de la Valkyrie")]public List<int> dashAmount = new List<int>();
    [BoxGroup("Attaque - Dash")] [Tooltip("Fréquence de l'attaque")]public float TriggerDashTime;
    [BoxGroup("Attaque - Dash")] [Tooltip("Temps de chargement d'un dash selon la vie de la Valkyrie")]public float[] windUpSpeed;
    [BoxGroup("Attaque - Dash")] [Tooltip("Intensité du shake pendant le chargement")]public float windUpDistance;
    [BoxGroup("Attaque - Dash")] [Tooltip("Vitesse du dash (en secondes)")]public float chargeSpeed;
    [BoxGroup("Attaque - Dash")] [Tooltip("Distance du dash")]public float chargeDistance;
    [BoxGroup("Attaque - Dash")] [Tooltip("Latence entre deux dashs selon la la vie de la Valkyrie")]public float[] TimeBetweenDashes;
    [BoxGroup("Attaque - Dash")] [Tooltip("Stun à la fin de l'attaque selon la vie de la Valkyrie")]public float[] StunTime;
    [Header("Refs et visus")]
    [BoxGroup("Attaque - Dash")] public GameObject dashIndic;
    [BoxGroup("Attaque - Dash")] public GameObject IndicMask;
    [Header("Débug")]
    [BoxGroup("Attaque - Dash")] public bool isDashing;
    [BoxGroup("Attaque - Dash")] public bool charging;
    [BoxGroup("Attaque - Dash")] public int executedDashes;
    [BoxGroup("Attaque - Dash")] public float TriggerDashTimer;
    [Space(30)] 
    [BoxGroup("Attaque - Anneaux")] public int[] ringAmount;
    [BoxGroup("Attaque - Anneaux")] public float[] ringFrequency;
    [BoxGroup("Attaque - Anneaux")] public float triggerRingAttackTime;
    [BoxGroup("Attaque - Anneaux")] [Tooltip("Dégâts de l'attaque")]public int ringDmg;
    [BoxGroup("Attaque - Anneaux")] [Tooltip("valeur ajoutée au scale")]public float expansionAmount;
    [BoxGroup("Attaque - Anneaux")] [Tooltip("Vitesse à laquelle est appliquée 'Expansion Amount'(en secondes)")]public float ExpansionRate;
    [BoxGroup("Attaque - Anneaux")] [Tooltip("valeur soustraite au 'Expansion Rate'")]public float ExpansionSpeedUp;
    [BoxGroup("Attaque - Anneaux")] [Tooltip("Vitesse à laquelle est appliquée 'Expansion Speed Up'(en secondes)")]public float ExpansionSpeedUpRate;

    [Header("Refs et visus")] 
    [BoxGroup("Attaque - Anneaux")] public AnneauxDeFeu anneauxFeu;
    
    [Header("Débug")]
    [BoxGroup("Attaque - Anneaux")] public float triggerRingAttackTimer;





    [Foldout("Refs")]public static IA_Valkyrie instance;
    [Foldout("Refs")]public GameObject emptyLayers;
    [Foldout("Refs")]public MonsterLifeManager life;
    [Foldout("Refs")]public SpriteRenderer[] spriteArray;
    [Foldout("Refs")]private float currentIdleTime;
    [Foldout("Refs")]public GameObject player;
    [Foldout("Refs")]public Seeker seeker;
    [Foldout("Refs")]public AIPath aipath;
    [Foldout("Refs")]private Path path;
    [Foldout("Refs")]public GameObject canvasLifeBar;
    [Foldout("Refs")]private Rigidbody2D rb;
    [Foldout("Refs")]private Collider2D collider;
    [Foldout("Refs")]public Animator anim;
    [Foldout("Refs")]IAstarAI ai;
    [Foldout("Refs")]public AIDestinationSetter playerFollow;
    

    //Fonctions ******************************************************************************************************************************************************
    
    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(instance);
        }
        instance = this;
        DOTween.SetTweensCapacity(3125,50);
        puissanceAttaqueJavelot = GetComponentInParent<MonsterLifeManager>().data.damage;

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
        else if (life.vieActuelle > life.vieMax * 0.25f)
        {
            currentState = State.Moitié;
            if (!midlifeKnockBack)
            {
                StartCoroutine(AttaqueAnneaux());
                midlifeKnockBack = true;
            }
        }
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
            if ((int)currentState >= 2)
            {
                triggerRingAttackTimer += Time.deltaTime;
                //CharacterController.instance.rb.AddForce(pushForce*ia.pushForce,ForceMode2D.Impulse);
            }
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
            rb.velocity = dir * airTravelSpeed[(int)currentState];
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
            transform.DOMove(transform.position, StunTime[(int)currentState]).OnComplete((() =>
            {
                aipath.canMove = true;
                isAttacking = false;
                isDashing = false;
                charging = false;
                TriggerDashTimer = 0;
                executedDashes = 0;
            }));
        }

        if (triggerRingAttackTimer > triggerRingAttackTime && !isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttaqueAnneaux());
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
                currentIdleTime = idleTime;
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
                collider.enabled = false;
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
        yield return new WaitForSeconds(jumpSpeed * 0.7f);
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
            collider.enabled = true;
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
        transform.DOPunchPosition(reversedir* windUpDistance,windUpSpeed[(int)currentState]).OnComplete((() =>
        {
            charging = true;
            //Destroy(currentMask);
            Destroy(Indication);
            transform.DOMove(dir * chargeDistance, chargeSpeed).OnComplete((() =>
            {
                executedDashes++;
                if (executedDashes < dashAmount[(int)currentState])
                {
                    transform.DOMove(transform.position, TimeBetweenDashes[(int)currentState]).OnComplete((() =>
                    {
                        charging = false;
                        isDashing = false;
                    }));
                }
            }));
        }));

    }

    #endregion

    IEnumerator AttaqueAnneaux()
    {
        anim.SetBool("isAttacking",true);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("isAttacking",false);
        foreach (var sr in spriteArray)
        {
            sr.color = Color.yellow;
            sr.DOColor(Color.white, 0.7f);
        }
        yield return new WaitForSeconds(0.6f);
        isAttacking = true;
        ai.canMove = false;
        for (int i = 0; i < ringAmount[(int)currentState]; i++)
        {
            var current = Instantiate(anneauxFeu,transform.position,Quaternion.identity);
            current.sr.sortingOrder = i;
            current.mask.frontSortingOrder = i;
            yield return new WaitForSeconds(ringFrequency[(int)currentState]);
        }

        triggerRingAttackTimer = 0;
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && charging)
        {
            DamageManager.instance.TakeDamage(dashDamage, gameObject);
        }
    }
}

