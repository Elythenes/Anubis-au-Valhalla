using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

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
    public bool isDashing;
    public float timerDash;
    public float dashDuration;
    public float LagDebutDash;
    public float LagDebutDashMax;
    public float CooldownDash;
    public float CooldownDashMax;
    public Rigidbody2D rb;
    public float dashSpeed;
    public Vector2 targetPerso;




    private void Start()
    {
        vieActuelle = vieMax;
        seeker = GetComponent<Seeker>();
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

        if (aipath.desiredVelocity.x >= 0.01f) // Permet d'orienter le personnage vers la direction dans laquelle il se déplace
        {
            transform.localScale = new Vector3(-1, 2.2909f, 1);
        }
        else if (aipath.desiredVelocity.x <= 0.01f)
        {
            transform.localScale = new Vector3(1, 2.2909f, 1);
        }

        if (aipath.reachedDestination) // Quand le monstre arrive proche du joueur, il commence le dash
        {
            if (isDashing == false)
            { 
                targetPerso  = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
                Debug.Log("commence");
                aipath.canMove = false;
                isDashing = true;
            }
        }

        if (isDashing == false)
        {
            LagDebutDash = 0;
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            timerDash = 0;
            CooldownDash = 0;
        }

        if (isDashing)
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            LagDebutDash += Time.deltaTime;
            
            if (LagDebutDash >= LagDebutDashMax)
            {
                timerDash += Time.deltaTime;
                rb.velocity = targetPerso*dashSpeed;
              
                if (timerDash > dashDuration)
                {
                    rb.velocity = (Vector2.zero);
                    //timerDash = 0;
                    //CooldownDash = 0;
                    //LagDebutDash = 0;
                    aipath.canMove = true;
                    CooldownDash += Time.deltaTime;
                    isDashing = false;
                    if (CooldownDash >= CooldownDashMax) // Cooldown de l'attaque
                    {
                        LagDebutDash = 0;
                        timerDash = 0;
                        isDashing = false;
                        CooldownDash = 0;
                    }
                }
            }
        }
    }

   /* IEnumerator Dash()
    {
        Vector2 targetPerso  = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        yield return new WaitForSeconds(LagDebutDash);
        timerDash += Time.deltaTime;
        rb.velocity = (targetPerso * dashSpeed);
              
        if (timerDash > dashDuration)
        { 
           
            rb.velocity = (Vector2.zero);
            timerDash = 0;
            CooldownDash = 0;
            yield return new WaitForSeconds(LagFinDash);
            aipath.canMove = true;
            CooldownDash += Time.deltaTime;
            isDashing = false;
            if (CooldownDash >= CooldownDashMax) // Cooldown de l'attaque
            {
                timerDash = 0;
                isDashing = false;
                CooldownDash = 0;
            }
        }
    }*/
    
    public void TakeDamage(int damage)
    {
        StartCoroutine(AnimationDamaged());
        vieActuelle -= damage;

        if (vieActuelle <= 0)
        {
            Die();
        }
    }

    IEnumerator AnimationDamaged()
    {
        animator.SetBool("IsTouched", true);
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("IsTouched", false); 
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
