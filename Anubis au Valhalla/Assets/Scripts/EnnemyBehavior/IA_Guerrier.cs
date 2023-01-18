using System;
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
    public bool isDead;

    [Header("Déplacements")] 
    public float speedX;
    public float speedY;
    public float radiusWondering;
    public Vector2 pointToWonder;
    public GameObject player;
    public Rigidbody2D rb;
    public Seeker seeker;
    public AIPath aipath;
    private Path path;
    private SpriteRenderer sr;
    IAstarAI ai;
    public AIDestinationSetter playerFollow;
   
   
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] audioClipArray;

    [Header("States")]
    public bool isAttacking;
    public bool isChasing;
    public bool chasingTrigger;
    public bool isWondering;

    [Header("Attaque")] 
    public bool doOnce;
    public GameObject swing;
    public Transform pointAttaque;
    public LayerMask HitboxPlayer;
    [NaughtyAttributes.ReadOnly] public int puissanceAttaque;
    public int damageElite;
    public float dureeAttaque;
    public float rangeAttaque;
    public float radiusAttack;
    public float StartUpAttackTime;
    public float StartUpAttackTimeTimer;
    public float WonderingTime;
    public float WonderingTimeTimer;
    private bool hasShaked;

    [Header("Stats Overdose")] 
    public float OvWonderingTime = 0.5f;
    public float OvSpeed = 2;
    public float OvStartUpAttackTime = 0.25f;

    
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
        
        if (life.eliteChallenge)
        {
            isElite = true;
        }
        if (isElite)
        {
            puissanceAttaque = damageElite;
        }
        
        if (life.overdose || SalleGenerator.Instance.currentRoom.overdose)
        {
            WonderingTime *= OvWonderingTime;
            ai.maxSpeed *= OvSpeed;
            StartUpAttackTime *= OvStartUpAttackTime;
        }
        
        if (life.elite)
        {
            isElite = true;
        }
    }

    private void FixedUpdate() // Les AddForce
    {
        if (isWondering && !life.isMomified)
        {
            rb.AddForce(new Vector2(aipath.targetDirection.x * speedX,aipath.targetDirection.y * speedY) * Time.deltaTime);
        }
        else
        {
            doOnce = false;
            ai.destination = player.transform.position;
        }
        
        
        if (isChasing && !life.isMomified && !isWondering)
        {
            doOnce = false;
            aipath.destination = player.transform.position;
            anim.SetBool("isIdle",false);
            anim.SetBool("isRuning",true);
            ai.destination = player.transform.position;
            rb.AddForce(new Vector2(aipath.targetDirection.x * speedX,aipath.targetDirection.y * speedY) * Time.deltaTime);
        }
    }

    public void Update()
    {
        if (isWondering && !life.isMomified)
        {
            anim.SetBool("isIdle",false);
            anim.SetBool("isRuning",true);
            WonderingTimeTimer += Time.deltaTime;
            if (doOnce && isWondering && !isChasing)
            {
                StartCoroutine(FindWonderPosition());
                doOnce = false;
            }
        }
        
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

        if (life.gotHit)  // Son
        {
            audioSource.Stop();
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(audioClipArray[2]);
            this.enabled = false;
            aipath.canMove = false;
            StartCoroutine(RestartScripts());
        }

        if (isDead) // Mort
        {
            aipath.destination = Vector2.zero;
            //rb.velocity = Vector2.zero;
            this.enabled = false;
        }

      
        if ((Vector3.Distance(aipath.destination, transform.position) <= radiusAttack) && !life.isMomified) // Quand le monstre arrive proche du joueur, il commence à attaquer
        {
                if (!isWondering && isChasing && !doOnce)
                {
                    isAttacking = true;
                    isChasing = false;
                }
        }


        if (life.vieActuelle <= 0)
        {
            anim.SetBool("isDead",true);
        }

        if (isAttacking && !life.isMomified)  // L'attaque
        {
            anim.SetBool("isRuning",false);
            anim.SetBool("isIdle",false);
            anim.SetBool("PrepareAttack",true);
            anim.SetBool("isAttacking",false);
            StartUpAttackTimeTimer += Time.deltaTime;
            hasShaked = false;
        }

        if (!hasShaked&& !life.isMomified)
        {
            transform.DOShakePosition(0.2f, 0.3f);
            hasShaked = true;
        }
        
        IEnumerator Attaque() 
        {
            yield return new WaitForSeconds(0.27f);
            GameObject swingOj = Instantiate(swing, pointAttaque.position, Quaternion.identity);
            swingOj.GetComponent<HitboxGuerrier>().ia = this;
            swingOj.transform.localScale = new Vector2(rangeAttaque,rangeAttaque);
            float angle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            swingOj.transform.localRotation = Quaternion.AngleAxis(angle,Vector3.forward);
        }

        if (StartUpAttackTimeTimer >= StartUpAttackTime&& !life.isMomified)
        {
            audioSource.pitch = 1;
            audioSource.PlayOneShot(audioClipArray[1]);
            anim.SetBool("isIdle",false);
            anim.SetBool("PrepareAttack",false);
            anim.SetBool("isAttacking",true);
            StartCoroutine(Attaque());

            isWondering = true;
            doOnce = true;
            isAttacking = false;
            StartUpAttackTimeTimer = 0;
        }

       
        
        if (WonderingTimeTimer >= WonderingTime&& !life.isMomified && !isChasing)
        {
            StopCoroutine(FindWonderPosition());
            doOnce = false;
            ai.destination = player.transform.position;
            isWondering = false;
            isAttacking = false;
            chasingTrigger = true;
            WonderingTimeTimer = 0;
            StartUpAttackTimeTimer = 0;
        }

        if (chasingTrigger && !life.isMomified)
        {
            StopCoroutine(FindWonderPosition());
            doOnce = false;
            chasingTrigger = false;
            isChasing = true;
        }
    }

    IEnumerator FindWonderPosition()
    {
        if (doOnce && isWondering)
        {
            yield return new WaitForSeconds(0.8f);
            aipath.destination = PickRandomPoint();
            doOnce = true;
        }
        else
        {
            StopCoroutine(FindWonderPosition());
            aipath.destination = player.transform.position;
        }
            
    }

    IEnumerator RestartScripts()
    {
        if (!isDead)
        {
            yield return new WaitForSeconds(0.3f);
            this.enabled = true;
            aipath.canMove = true;
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
