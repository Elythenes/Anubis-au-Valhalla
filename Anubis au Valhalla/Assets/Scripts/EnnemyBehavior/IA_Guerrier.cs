using System.Collections;
using DG.Tweening;
using GenPro;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class IA_Guerrier : MonoBehaviour
{
    [Header("Vie et visuels")] 
    public Animator anim;
    public GameObject emptyLayers;
    public bool isElite;
    public MonsterLifeManager life;

    [Header("Déplacements")]
    public GameObject player;
    public Seeker seeker;
    public AIPath aipath;
    private Path path;
    private SpriteRenderer sr;
    IAstarAI ai;
    public AIDestinationSetter playerFollow;
    public float radiusWondering;
    public bool isWondering;
   

    [Header("Attaque")] public GameObject swing;
    public bool isAttacking;
    public Transform pointAttaque;
    public LayerMask HitboxPlayer;
    public float dureeAttaque;
    public float rangeAttaque;
    [NaughtyAttributes.ReadOnly] public int puissanceAttaque;
    public float StartUpAttackTime;
    public float StartUpAttackTimeTimer;
    public float WonderingTime;
    public float WonderingTimeTimer;
    public int damageElite;
    private bool hasShaked;

    
    //Fonctions ******************************************************************************************************************************************************
    
    private void Awake()
    {
        puissanceAttaque = GetComponentInParent<MonsterLifeManager>().data.damage;
    }

    private void Start()
    {
        anim.SetBool("isRuning",true);
        player = GameObject.FindGameObjectWithTag("Player");
        seeker = GetComponent<Seeker>();
        sr = GetComponent<SpriteRenderer>();
        ai = GetComponent<IAstarAI>();
        playerFollow.enabled = true;
        playerFollow.target = player.transform;
        if (life.elite)
        {
            isElite = true;
        }
        if (isElite)
        {
            puissanceAttaque = damageElite;
        }
        
        if (life.overdose || SalleGenerator.Instance.currentRoom.overdose)
        {
            WonderingTime *= 0.5f;
            ai.maxSpeed *= 2;
            StartUpAttackTime *= 0.25f;
        }
    }


    public void Update()
    {
        if (!isAttacking && !life.isMomified)
        {
            if (transform.position.x < player.transform.position.x) // Permet d'orienter le monstre vers la direction dans laquelle il se déplace
            {
                var localRotation = transform.localRotation;
                localRotation = Quaternion.Euler(localRotation.x, 0, localRotation.z);
                transform.localRotation = localRotation;
            }
            else if (transform.position.x > player.transform.position.x)
            {
                var localRotation = transform.localRotation;
                localRotation = Quaternion.Euler(localRotation.x, -180, localRotation.z);
                transform.localRotation = localRotation;
            }  
        }

        if (life.gotHit)
        {
            ai.canMove = true;
        }
        
        if (aipath.reachedDestination && !life.isMomified) // Quand le monstre arrive proche du joueur, il commence à attaquer
        {
            if (isWondering)
            {
                //StartCoroutine(WaitMove());
            }
            else
            {
                isAttacking = true;
            }

        }

        if (life.vieActuelle <= 0)
        {
            anim.SetBool("isDead",true);
        }

        if (isAttacking&& !life.isMomified)
        {
            anim.SetBool("isRuning",false);
            anim.SetBool("isIdle",false);
            anim.SetBool("PrepareAttack",true);
            anim.SetBool("isAttacking",false);
            aipath.canMove = false;
            StartUpAttackTimeTimer += Time.deltaTime;
            hasShaked = false;
        }

        if (!hasShaked&& !life.isMomified)
        {
            transform.DOShakePosition(0.2f, 0.3f);
            hasShaked = true;
        }
        
        IEnumerator WaitMove()
        {
            yield return new WaitForSeconds(0.3f);
            GameObject swingOj = Instantiate(swing, pointAttaque.position, Quaternion.identity);
            swingOj.GetComponent<HitboxGuerrier>().ia = this;
            swingOj.transform.localScale = new Vector2(rangeAttaque,rangeAttaque);
            float angle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            swingOj.transform.localRotation = Quaternion.AngleAxis(angle,Vector3.forward);
        }

        if (StartUpAttackTimeTimer >= StartUpAttackTime&& !life.isMomified)
        {
            Debug.Log("attaque");
            anim.SetBool("isIdle",false);
            anim.SetBool("PrepareAttack",false);
            anim.SetBool("isAttacking",true);
            StartCoroutine(WaitMove());
           // Collider2D[] toucheJoueur = Physics2D.OverlapCircleAll(pointAttaque.position, rangeAttaque, HitboxPlayer);
           
            //foreach (Collider2D joueur in toucheJoueur)
           // {
                //Debug.Log("touché");
               // joueur.GetComponent<DamageManager>().TakeDamage(puissanceAttaque);
           // }

            aipath.canMove = true;
            isWondering = true;
            isAttacking = false;
            StartUpAttackTimeTimer = 0;
        }

        if (isWondering&& !life.isMomified)
        {
            anim.SetBool("isIdle",false);
             anim.SetBool("isRunning",true);
            WonderingTimeTimer += Time.deltaTime;
            if (!ai.pathPending && ai.reachedEndOfPath || !ai.hasPath) 
            {
                playerFollow.enabled = false;
                ai.destination = PickRandomPoint();
                ai.SearchPath();
            }
        }
        
        if (WonderingTimeTimer >= WonderingTime&& !life.isMomified)
        {
            isAttacking = false;
            isWondering = false;
            playerFollow.enabled = true;
            ai.SearchPath();
            WonderingTimeTimer = 0;
            StartUpAttackTimeTimer = 0;
        }
    }
    
    Vector2 PickRandomPoint() 
    {
        var point = Random.insideUnitCircle * radiusWondering;
        point.x += ai.position.x;
        point.y += ai.position.y;
        return point;
    }
}
