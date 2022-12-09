using System;
using System.Collections;
using DG.Tweening;
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



    public int soulValue = 4;

    /*private void Awake()
    {
        puissanceAttaque = GetComponentInParent<EnemyData>().damage;
    }*/
    
    private void Start()
    {
        ai = GetComponent<IAstarAI>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        
        anim.SetBool("isIdle", true);
        player = GameObject.FindGameObjectWithTag("Player");
        
        playerFollow.enabled = true;
        playerFollow.target = player.transform;
        
        vieActuelle = vieMax;
        CooldownDashTimer = CooldownDash;
        
        if (life.elite)
        {
            isElite = true;
        }
        if (life.overdose)
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
                rb.velocity = Vector2.zero;
                isDashing = false;
                isChasing = true;
                dashdurationTimer = 0;
                
                anim.SetBool("StopDash", true);
                anim.SetBool("Dash",false);
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
                 LagDebutDashTimer += Time.deltaTime;
                 
                 
                 if (ShakeEnable)
                 {
                     transform.DOShakePosition(LagDebutDashMax, 0.4f);
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
       
       
        /*if (aipath.reachedDestination&& !life.isMomified) // Quand le monstre arrive proche du joueur, il commence le dash
        {
            if (isDashing == false && canDash)
            {
                CooldownDashTimer = 0;
                targetPerso  = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
                aipath.canMove = false;
                isDashing = true;
            }
        }

        if (isDashing && !life.isMomified) // Faire dasher le monstre
        {
            anim.SetBool("StartDash", true);
            anim.SetBool("IsIdle", false);
            anim.SetBool("IsRuning", false);
            LagDebutDash += Time.deltaTime;

            if (ShakeEnable)
            {
                transform.DOShakePosition(0.5f, 0.4f);
                ShakeEnable = false;
            }

            if (LagDebutDash >= LagDebutDashMax&& !life.isMomified)
            {
                anim.SetBool("StartDash",false);
                anim.SetBool("Dash", true);
                stopDash = true;
                hitboxActive = true;
                timerDash += Time.deltaTime;

                if (timerDash > dashDuration&& !life.isMomified)
                {
                    anim.SetBool("StopDash", true);
                    anim.SetBool("Dash",false);
                    hitboxActive = false;
                    ShakeEnable = true;
                    rb.velocity = (Vector2.zero);
                    isDashing = false;
                    canDash = false;
                }
            }
        }
        
        if (isDashing == false&& !life.isMomified) // Reset le dash quand il est terminé
        {
            aipath.canMove = false;
            CooldownDashTimer += Time.deltaTime;
            anim.SetBool("Dash", false);
            anim.SetBool("StopDash", true);
            //gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        

        if (CooldownDashTimer >= CooldownDash&& !life.isMomified)// Cooldown de l'attaque
        {
            anim.SetBool("IsRuning", true);
            anim.SetBool("StopDash", false);
            isWondering = false;
            aipath.canMove = true;
            canDash = true;
            LagDebutDash = 0;
            timerDash = 0;
            isDashing = false;
        }

        if (stopDash&& !life.isMomified)
        {
            StartCoroutine(DashImpulse());
        }

      
        IEnumerator DashImpulse()
        {
            rb.AddForce(targetPerso.normalized*dashSpeed,ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.001f);
            stopDash = false;
        }*/

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
