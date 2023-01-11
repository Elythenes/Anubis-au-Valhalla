using System;
using System.Collections;
using DG.Tweening;
using GenPro;
using Pathfinding;
using UnityEngine;

using Random = UnityEngine.Random;

public class IA_Monstre1 : MonoBehaviour
{
    [Header("Vie et visuels")]
    public float vieMax;
    public float vieActuelle;
    public Animator anim;
    public GameObject emptyLayers;
    public MonsterLifeManager life;
    public bool isElite;

    [Header("Déplacements")]
    private Rigidbody2D rb;
    public GameObject player;
    public AIPath aipath;
    public AIDestinationSetter playerFollow;
    IAstarAI ai;
    public float speedX;
    public float speedY;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] audioClipArray;
    public bool DoOnce;

    [Header("Dash")] 
    public bool canDash;
    public bool isChasing;
    public bool isPreparing;
    public bool isDashing;
    public bool stopDash;
    public bool ShakeEnable;
    public bool hitboxActive; 
    
    public float CooldownDashTimer;
    public float CooldownDash;
    public float dashdurationTimer;
    public float dashDuration;
    public float LagDebutDashTimer;
    public float LagDebutDashMax;
    public float radiusDash;
    public float dashSpeed;
    public Vector2 targetPlayer;


    [Header("Attaque")] 
    public Transform pointAttaque;
    public LayerMask HitboxPlayer;
    public float rangeAttaque;
    public int puissanceAttaque;


    //Fonctions ******************************************************************************************************************************************************
    
    private void Awake()
    {
        puissanceAttaque = GetComponentInParent<MonsterLifeManager>().data.damage;
    }
    
    private void Start()
    {
        ai = GetComponent<IAstarAI>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        
        anim.SetBool("IsIdle", true);
        player = GameObject.FindGameObjectWithTag("Player");
        
        playerFollow.enabled = true;
        playerFollow.target = player.transform;
        
        vieActuelle = life.vieMax;
        CooldownDashTimer = CooldownDash;
        
        if (life.elite)
        {
            isElite = true;
        }
        if (life.overdose || SalleGenerator.Instance.currentRoom.overdose)
        {
            ai.maxSpeed *= 2;
            dashdurationTimer *= 0.75f;
            LagDebutDashTimer *= 0.3f;
            CooldownDash *= 0.2f;
        }
    }

    private void FixedUpdate()
    {
        if (isChasing && !isDashing)
        {
            rb.AddForce(new Vector2(aipath.targetDirection.x * speedX,aipath.targetDirection.y * speedY) * Time.deltaTime);
            anim.SetBool("IsRuning", true);
            anim.SetBool("StopDash", false);
        }
  
        if (isDashing && !isChasing)
        {
            dashdurationTimer += Time.deltaTime;
  
            if (dashdurationTimer <= dashDuration)
            {
                rb.AddForce(targetPlayer.normalized * (dashSpeed * Time.deltaTime),ForceMode2D.Impulse);
            }
            else
            {
                anim.SetBool("Dash",false);
                rb.velocity = Vector2.zero;
                dashdurationTimer = 0;
                isDashing = false;
                isChasing = true;
            }
        }
    }

    public void Update()
    {
        #region Movements

          if (isChasing && !isDashing)
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

          #endregion
      
       # region AttaquesTrigger

         if (!canDash)
         {
             CooldownDashTimer += Time.deltaTime;
         }

         if (isDashing)
         {
             hitboxActive = true;
         }
         else
         {
             hitboxActive = false;
         }
    

         if (CooldownDashTimer >= CooldownDash)
         {
             canDash = true;
             CooldownDashTimer = 0;
             ShakeEnable = true;
             DoOnce = true;
         }

         if (canDash)
         {
             if (Vector3.Distance(aipath.destination, transform.position) <= radiusDash)
             {
                 if (isChasing)
                 {
                     targetPlayer = aipath.destination - transform.position;
                     isChasing = false;
                 }

                 isPreparing = true;
             
                 anim.SetBool("StartDash", true);
                 anim.SetBool("IsIdle", false);
                 anim.SetBool("IsRuning", false);
             }

             if (isPreparing)
             {
                 if (DoOnce)
                 {
                     audioSource.pitch = 1;
                     audioSource.PlayOneShot(audioClipArray[1]);
                     DoOnce = false;
                 }
                 
                 LagDebutDashTimer += Time.deltaTime;
                 
                 
                 if (ShakeEnable)
                 {
                    rb.velocity = Vector2.zero;
                     life.transform.DOShakePosition(LagDebutDashMax, 0.4f);
                     ShakeEnable = false;
                 }
                 
                 if (LagDebutDashTimer >= LagDebutDashMax)
                 {
                     isPreparing = false;
                     isDashing = true; 
                     canDash = false;
                     LagDebutDashTimer = 0;
                     
                     anim.SetBool("StartDash",false);
                     anim.SetBool("Dash", true);
                 }
             }
         }
         #endregion

         if (life.gotHit)
         {
             audioSource.pitch = Random.Range(0.8f, 1.2f);
             audioSource.PlayOneShot(audioClipArray[2]);
         }
         
         if (hitboxActive&& !life.isMomified) // Active la hitbox et fait des dégâts
        {
            Collider2D[] toucheJoueur = Physics2D.OverlapCircleAll(pointAttaque.position, rangeAttaque, HitboxPlayer);

            foreach (Collider2D joueur in toucheJoueur)
            {
                joueur.GetComponent<DamageManager>().TakeDamage(puissanceAttaque, gameObject);
            }
        }
    }
}
