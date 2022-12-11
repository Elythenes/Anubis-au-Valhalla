using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

public class IA_Shaman : MonoBehaviour
{
    [Header("Vie et visuels")]
    public GameObject emptyLayers;
    public bool isElite;
    public LayerMask layerPlayer;
    public EnemyType enemyType;
    public MonsterLifeManager life;

    [Header("Déplacements")] 
    private Rigidbody2D rb;
    private GameObject player;
    private Seeker seeker;
    private AIPath aipath;
    private Path path;
    private SpriteRenderer sr;
    private IAstarAI ai;
    private AIDestinationSetter playerFollow;
    public float radiusWondering;
    public bool isWondering;
    public bool isFleeing;
    public float distanceMaxPlayer;
    public float timeFleeing;
    public float timeFleeingTimer;
    public float forceRepulse;
    public Vector3 pointToGO;


    [Header("Attaque")] 
    public bool isAttacking;
    public float StartUpSummonTime;
    public float StartUpSummonTimeTimer;
    public float SummoningTime;
    public float SummoningTimeTimer;
    private bool hasShaked;
    public int corbeauSoulDroped;
    public GameObject corbeau;
    public GameObject particulesSummon;
    
    public enum EnemyType
    {
        Shaman = 0,
        Corbeau = 1,
        Loup = 2,
        Guerrier = 3,
        Valkyrie = 4
    }
    
    //Fonctions ******************************************************************************************************************************************************
    
    private void Awake()
    {
        //puissanceAttaque = GetComponentInParent<MonsterLifeManager>().data.damage;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        seeker = GetComponent<Seeker>();
        sr = GetComponent<SpriteRenderer>();
        ai = GetComponent<IAstarAI>();
        playerFollow = GetComponent<AIDestinationSetter>();
        if (life.elite)
        {
            isElite = true;
        }
        if (life.overdose || SalleGennerator.instance.currentRoom.overdose)
        {
            ai.maxSpeed *= 1.5f;
            forceRepulse *= 2f;
            StartUpSummonTime *= 0.2f;
            SummoningTime *= 0.5f;
        }
    }


    public void Update()
    {
        SortEnemies();
        if (!isAttacking&& !life.isMomified)
        {
            Flip();
        }
        switch (enemyType)
        {
            
            case EnemyType.Shaman:
                    bool oneUse = true;
                
                if (isWondering && !isFleeing&& !life.isMomified)
                {
                    Roam();
                }
                CompareOwnPosToPlayer();
        
                StartUpSummonTimeTimer += Time.deltaTime;
                if (StartUpSummonTimeTimer >= StartUpSummonTime&& !life.isMomified)
                {
                   
                    isAttacking = true;
                    isWondering = false;
                    SummoningTimeTimer += Time.deltaTime;
                    particulesSummon.SetActive(true);
                    

                    if (life.gotHit)
                    {
                        isAttacking = false;
                        isWondering = true;
                        SummoningTimeTimer = 0;
                        StartUpSummonTimeTimer = 0;
                        particulesSummon.SetActive(false);
                    }
                }

                if (SummoningTimeTimer >= SummoningTime && !life.isMomified)
                {
                    particulesSummon.SetActive(false);
                    Summon();
                }
                break;
            
            
        }
    }
        void DetectPlayerRelativePos()
    {
        
        if (Vector3.Distance(player.transform.position, transform.position) <= radiusWondering)
        {
            isFleeing = true;
            Vector2 angle = transform.position - player.transform.position;
            rb.AddForce(angle.normalized * forceRepulse);
        }
        else
        {
            isFleeing = false;
        }
    }
    void SortEnemies()
    {
        if (player.transform.position.y >
            emptyLayers.transform.position.y) // Faire en sorte que le perso passe derrière ou devant l'ennemi.
        {
            sr.sortingOrder = 2;
        }
        else
        {
            sr.sortingOrder = 1;
        }
    }
    void Flip()
    {
        if (transform.position.x <
            player.transform.position
                .x&& !life.isMomified) // Permet d'orienter le monstre vers la direction dans laquelle il se déplace
        {
            transform.localScale = new Vector3(-1, 2.2909f, 1);
        }
        else if (transform.position.x > player.transform.position.x)
        {
            transform.localScale = new Vector3(1, 2.2909f, 1);
        }
    }
    void Roam()
    {
        if (!ai.pathPending && ai.reachedEndOfPath || !ai.hasPath) 
        {
            playerFollow.enabled = false;
            PickRandomPoint();
            ai.destination = pointToGO;
            ai.SearchPath();
        }
    }
    #region ShamanIA

    void CompareOwnPosToPlayer()
    {
        if(Vector3.Distance(player.transform.position, transform.position) <= distanceMaxPlayer)
        {
            if (!isAttacking)
            {
                isFleeing = true;
                isWondering = false;
                
                if (isFleeing)
                {
                    timeFleeingTimer += Time.deltaTime;
                    
                    if (timeFleeingTimer <= timeFleeing)
                    {
                        DetectPlayerRelativePos();
                    }
                    else
                    {
                        timeFleeingTimer = 0;
                        isFleeing = false;
                        isWondering = true;
                    }
                }
            }
        }
        else
        {
            isFleeing = false;
            isWondering = true;
        }
    }

    void Summon()
    {
        var summon = Instantiate(corbeau, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
        SalleGennerator.instance.currentRoom.currentEnemies.Add(summon);
        summon.GetComponent<MonsterLifeManager>().soulValue = corbeauSoulDroped;
        corbeauSoulDroped -= 1;
        if (corbeauSoulDroped < 0)
        {
            corbeauSoulDroped = 0;
        }
        StartUpSummonTimeTimer = 0;
        SummoningTimeTimer = 0;
        isWondering = true;
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
            pointToGO =point; 
            return;
        }
    }

    #endregion
   
}
