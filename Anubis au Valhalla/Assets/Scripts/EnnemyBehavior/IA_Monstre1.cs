using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class IA_Monstre1 : MonoBehaviour
{
    [Header("Vie et visuels")]
    public float vieMax;
    public float vieActuelle;
    public Animator animator;
    public GameObject emptyLayers;

    [Header("Déplacements")]
    public GameObject player;
    public Seeker seeker;
    public AIPath aipath;
    private Path path;

    [Header("Dash")] 
    public bool canDash;
    public bool isDashing;
    public float timerDash;
    public float dashDuration;
    public float LagDebutDash;
    public float LagDebutDashMax;
    private float CooldownDashTimer;
    public float CooldownDash;
    private Rigidbody2D rb;
    public float dashSpeed;
    private Vector2 targetPerso;
    public bool stopDash;

    [Header("Attaque")] 
    public Transform pointAttaque;
    public LayerMask HitboxPlayer;
    public float rangeAttaque;
    public int puissanceAttaque;
    
    [Header("Pop up Dégâts")] 
    public Transform damageTextPrefab;


    public int soulValue = 4;

    private void Start()
    {
        vieActuelle = vieMax;
        seeker = GetComponent<Seeker>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    

    public void Update()
    {
        if (player.transform.position.y > emptyLayers.transform.position.y) // Faire en sorte que le perso passe derrière ou devant l'ennemi.
        {
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }

        if (aipath.desiredVelocity.x >= 0.01f) // Permet d'orienter le monstre vers la direction dans laquelle il se déplace
        {
            transform.localScale = new Vector3(-1, 2.2909f, 1);
        }
        else if (aipath.desiredVelocity.x <= 0.01f)
        {
            transform.localScale = new Vector3(1, 2.2909f, 1);
        }

        if (aipath.reachedDestination) // Quand le monstre arrive proche du joueur, il commence le dash
        {
            if (isDashing == false && canDash)
            { 
                CooldownDashTimer = 0;
                targetPerso  = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
                Debug.Log("commence");
                aipath.canMove = false;
                isDashing = true;
            }
        }

        if (isDashing == false) // Reset le dash quand il terminé
        {
            CooldownDashTimer += Time.deltaTime;
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }

        if (isDashing) // Faire dasher le monstre
        {
            LagDebutDash += Time.deltaTime;
            
            if (LagDebutDash >= LagDebutDashMax)
            {
                stopDash = true;
                timerDash += Time.deltaTime;

                if (timerDash > dashDuration)
                {
                    rb.velocity = (Vector2.zero);
                    aipath.canMove = true;
                    isDashing = false;
                    canDash = false;
                }
            }
        }
        
        if (CooldownDashTimer >= CooldownDash) // Cooldown de l'attaque
        {
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
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(AnimationDamaged());
        vieActuelle -= damage;

        if (vieActuelle <= 0)
        {
            Die();
        }
    }

    public void DamageText(int damageAmount)
    {
        Transform damagePopUpTransform = Instantiate(damageTextPrefab, new Vector3(transform.position.x,transform.position.y,-5), Quaternion.identity);
        DamagePopUp.instance.Setup(damageAmount);
        //damagePopUpTransform.GetComponent<TextMeshPro>().SetText(damageAmount.ToString());
        Debug.Log(damageAmount);
    }

    IEnumerator AnimationDamaged()
    {
        animator.SetBool("IsTouched", true);
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("IsTouched", false); 
    }

    void Die()
    {
        Souls.instance.CreateSouls(gameObject.transform.position, soulValue);
        Destroy(gameObject);
    }
}
