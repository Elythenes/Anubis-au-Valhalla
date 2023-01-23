using System.Collections;
using GenPro;
using Pathfinding;
using Pathfinding.Util;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class IA_Corbeau : MonoBehaviour
{
    [Header("Vie et visuels")] 
    public Animator anim;
    public GameObject emptyLayers;
    public bool isElite;
    private Rigidbody2D rb;
    public LayerMask layerPlayer;
    public MonsterLifeManager life;
    public bool isDead;

    [Header("Déplacements")] 
    public GameObject player;
    public Seeker seeker;
    public AIPath aipath;
    private Path path;
    private SpriteRenderer sr;
    IAstarAI ai;
    public AIDestinationSetter playerFollow;
    public bool canMove = true;
    public bool isFleeing;
    public bool isRotating;
    public bool isChasing;
    public float forceRepulse;
    public float radiusFleeing;
    public float speedTowardPlayer;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] audioClipArray;

    [Header("Attaque")] public float speedX;
    public float speedY;
    public float disolveValue;
    public bool isAttacking;
    public float rotationSpeed;
    public float rotationSpeedSlown;
    [NaughtyAttributes.ReadOnly] public int puissanceAttaque;
    public float StartUpAttackTime;
    public float StartUpAttackTimeTimer;
    public float AttackTime;
    public float AttackTimeTimer;
    public GameObject projectilPlume;
    public GameObject indicationAttaque;
    private GameObject holder;
    private bool indic = true;
    public float plumeSpeed;
    public Vector2 directionProj;
    public Gradient gradientIndic;
    
    [Header("Stats Overdose")] 
    public float OvAttackTime = 0.3f;
    public float OvSpeed = 2;
    public float OvStartUpAttackTime = 0.25f;
    public float OvSpeedTowardPlayer = 100f;


    //Fonctions ******************************************************************************************************************************************************
    
    private void Awake()
    {
        puissanceAttaque = GetComponentInParent<MonsterLifeManager>().data.damage;
    }

    private void Start()
    {
        anim.SetBool("isAttacking",true);
        StartUpAttackTime = Random.Range(0.8f, 1.6f);
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        seeker = GetComponent<Seeker>();
        sr = GetComponent<SpriteRenderer>();
        ai = GetComponent<IAstarAI>();
        playerFollow.enabled = true;
        playerFollow.target = player.transform;
        if (life.eliteChallenge)
        {
            isElite = true;
        }
        
        if (life.elite)
        {
            isElite = true;
        }

        if (life.overdose || SalleGenerator.Instance.currentRoom.overdose)
        {
            speedTowardPlayer *= OvSpeedTowardPlayer;
            rotationSpeed *= OvSpeed;
            rotationSpeedSlown *= OvSpeed;
            StartUpAttackTime *= OvStartUpAttackTime;
            AttackTime *= OvAttackTime;
        }
        
    }

    private void FixedUpdate()
    {
        if (isChasing && !life.isMomified && canMove)
        {
            aipath.destination = player.transform.position;
            anim.SetBool("isIdle",false);
            anim.SetBool("isRuning",true);
            ai.destination = player.transform.position;
            rb.AddForce(new Vector2(aipath.targetDirection.x * speedX,aipath.targetDirection.y * speedY) * Time.deltaTime);
        }
        
       /* if (isFleeing && !life.isMomified && canMove)
        {
            Vector2 fuite = player.transform.position - transform.position;
            anim.SetBool("isIdle",false);
            anim.SetBool("isRuning",true);
            rb.AddForce(new Vector2(fuite.x * speedX,fuite.y * speedY) * Time.deltaTime);
        }*/
    }

    
    public void Update()
    {
        StartUpAttackTimeTimer += Time.deltaTime;

        if (life.gotHit)
        {
            audioSource.Stop();
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(audioClipArray[2]);
            this.enabled = false;
            aipath.canMove = false;
            StartCoroutine(RestartScripts());
        }

        if (isDead)
        {
            ai.destination = Vector2.zero;
            canMove = false;
            this.enabled = false;
        }

        if (life.vieActuelle <= 0)
        {
            anim.SetBool("isDead",true);
            if (holder.gameObject is not null)
            {
                Destroy(holder.gameObject);
            }
        }
        
        if (player.transform.position.y >
            emptyLayers.transform.position.y) // Faire en sorte que le perso passe derrière ou devant l'ennemi.
        {
            sr.sortingOrder = 2;
        }
        else
        {
            sr.sortingOrder = 1;
        }

        if (!isAttacking)
        {
            if (transform.position.x < player.transform.position.x) // Permet d'orienter le monstre vers la direction dans laquelle il se déplace
            {
                var localRotation = transform.localRotation;
                localRotation = Quaternion.Euler(localRotation.x, -180, localRotation.z);
                transform.localRotation = localRotation;
            }
            else if (transform.position.x > player.transform.position.x)
            {
                var localRotation = transform.localRotation;
                localRotation = Quaternion.Euler(localRotation.x, 0, localRotation.z);
                transform.localRotation = localRotation;
            }  

        }

        if (StartUpAttackTimeTimer >= StartUpAttackTime && !life.isMomified)
        {
            AttackTimeTimer += Time.deltaTime;
            
            if (indic)
            {
                anim.SetBool("isAttacking",false);
                anim.SetBool("StartUpAttaque",true);
                anim.SetBool("isWalking",false);
                isFleeing = false;
                isChasing = false;
                isRotating = false;
                aipath.canMove = false;
                canMove = false;
                rb.velocity = Vector2.zero;
                directionProj = new Vector2(CharacterController.instance.transform.position.x - transform.position.x,
                    CharacterController.instance.transform.position.y - transform.position.y);
                float angle = Mathf.Atan2(directionProj.y, directionProj.x) * Mathf.Rad2Deg;
               GameObject indicOBJ = Instantiate(indicationAttaque,transform.position,  Quaternion.Euler(0,0,angle));
               //indicOBJ.transform.parent = emptyLayers.transform;
               holder = indicOBJ;
               Destroy(indicOBJ,AttackTime+0.1f);
               indic = false;
            }
            else if(life.gotHit)
            {
                anim.SetBool("isAttacking",false);
                anim.SetBool("StartUpAttaque",false);
                anim.SetBool("isWalking",true);
                
                if (holder is not null)
                {
                    Destroy(holder.gameObject);
                }
                StartUpAttackTimeTimer = 0;
                AttackTimeTimer = 0;
                canMove = true;
                indic = true;
            }
            
           
            if (holder is not null)
            {
                if (disolveValue < 30)
                {
                    disolveValue += 0.05f;
                    holder.GetComponent<SpriteRenderer>().material.SetFloat("_Force_rayon",disolveValue);
                }
            }
            
            
            if (AttackTimeTimer >= AttackTime && !life.isMomified)
            {
                disolveValue = 9;
                audioSource.pitch = 1;
                audioSource.PlayOneShot(audioClipArray[1]);
                anim.SetBool("isAttacking",true);
                anim.SetBool("StartUpAttaque",false);
                aipath.canMove = true;
                GameObject projPlume = Instantiate(projectilPlume, transform.position, Quaternion.identity);
                projPlume.GetComponent<ProjectileCorbeau>().ia = this;
                StartUpAttackTimeTimer = 0;
                AttackTimeTimer = 0;
                indic = true;
                canMove = true;
                anim.SetBool("isWalking",true);
            }
        }
        if (Vector3.Distance(player.transform.position, transform.position) <= radiusFleeing * 3 && !life.isMomified && !isFleeing && canMove) // Quand le monstre arrive proche du joueur, il commence à attaquer
        {
            isRotating = true;
            isChasing = false;
            
                        
           
            
            if (life.isEnvased && canMove && !life.gotHit)
            {
                transform.RotateAround(player.transform.position, Vector3.forward, rotationSpeedSlown * Time.deltaTime);
                //rb.AddForce(Vector2.Perpendicular(transform.position - player.transform.position * rotationSpeedSlown ),ForceMode2D.Force);
            }
            else if(canMove && !life.gotHit)
            {
                //rb.AddForce(Vector2.Perpendicular(transform.position - player.transform.position* rotationSpeed),ForceMode2D.Force);
                transform.RotateAround(player.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
            }

        }
        else
        {
            isRotating = false;
        }
        
            
       
        if(Vector3.Distance(player.transform.position, transform.position) >= radiusFleeing*3 && !isFleeing && !isRotating && canMove)
        {
            isChasing = true;
            isFleeing = false;
        }
        else
        {
            isChasing = false;
        }
    }

    IEnumerator RestartScripts()
    {
        yield return new WaitForSeconds(0.3f);
        this.enabled = true;
        aipath.canMove = true;
    }
    
}