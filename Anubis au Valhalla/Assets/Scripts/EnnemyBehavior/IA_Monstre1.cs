using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class IA_Monstre1 : MonoBehaviour
{
    [Header("Vie et visuels")]
    public float vieMax;
    public float vieActuelle;
    public Animator animator;
    public GameObject emptyLayers;

    [Header("Déplacements")]
    public GameObject player;
    public AIPath aipath;
    private Path path;
    private SpriteRenderer sr;
    IAstarAI ai;
    public AIDestinationSetter playerFollow;
    public float radiusWondering;
    public bool isWondering;

    [Header("Dash")] 
    public bool canDash;
    public bool isDashing;
    public float timerDash;
    public float dashDuration;
    public float LagDebutDash;
    public float LagDebutDashMax;
    public float CooldownDashTimer;
    public float CooldownDash;
    private Rigidbody2D rb;
    public float dashSpeed;
    private Vector2 targetPerso;
    public bool stopDash;
    public bool ShakeEnable;

    [Header("Attaque")] 
    public Transform pointAttaque;
    public LayerMask HitboxPlayer;
    public float rangeAttaque;
    public int puissanceAttaque;



    public int soulValue = 4;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerFollow.enabled = true;
        ai = GetComponent<IAstarAI>();
        vieActuelle = vieMax;
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        playerFollow.target = player.transform;
        CooldownDashTimer = CooldownDash;
    }
    

    public void Update()
    {
        if (player.transform.position.y > emptyLayers.transform.position.y) // Faire en sorte que le perso passe derrière ou devant l'ennemi.
        {
           sr.sortingOrder = 2;
        }
        else
        {
          sr.sortingOrder = 1;
        }
        
        if (!isDashing)
        {
            if (transform.position.x < player.transform.position.x) // Permet d'orienter le monstre vers la direction dans laquelle il se déplace
            {
                transform.localScale = new Vector3(-1, 1.316351f, 1);
            }
            else if (transform.position.x > player.transform.position.x)
            {
                transform.localScale = new Vector3(1, 1.316351f, 1);
            }
        }

        if (aipath.reachedDestination) // Quand le monstre arrive proche du joueur, il commence le dash
        {
            if (isDashing == false && canDash)
            { 
                CooldownDashTimer = 0;
                targetPerso  = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
                aipath.canMove = false;
                isDashing = true;
            }
        }

        if (isDashing) // Faire dasher le monstre
        {
            LagDebutDash += Time.deltaTime;

            if (ShakeEnable)
            {
                transform.DOShakePosition(0.5f, 0.4f);
                ShakeEnable = false;
            }

            if (LagDebutDash >= LagDebutDashMax)
            {
                
                stopDash = true;
                timerDash += Time.deltaTime;

                if (timerDash > dashDuration)
                {
                    ShakeEnable = true;
                    rb.velocity = (Vector2.zero);
                    isDashing = false;
                    canDash = false;
                }
            }
        }
        
        if (isDashing == false) // Reset le dash quand il est terminé
        {
            aipath.canMove = false;
            CooldownDashTimer += Time.deltaTime;
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        

        if (CooldownDashTimer >= CooldownDash) // Cooldown de l'attaque
        {
            isWondering = false;
            aipath.canMove = true;
            canDash = true;
            LagDebutDash = 0;
            timerDash = 0;
            isDashing = false;
        }

        if (stopDash)
        {
            StartCoroutine(DashImpulse());
        }

      
        IEnumerator DashImpulse()
        {
            rb.AddForce(targetPerso*dashSpeed,ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.001f);
            stopDash = false;
        }

        if (isDashing) // Active la hitbox et fait des dégâts
        {
            Collider2D[] toucheJoueur = Physics2D.OverlapCircleAll(pointAttaque.position, rangeAttaque, HitboxPlayer);

            foreach (Collider2D joueur in toucheJoueur)
            {
                Debug.Log("touché");
                joueur.GetComponent<DamageManager>().TakeDamage(puissanceAttaque);
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
}