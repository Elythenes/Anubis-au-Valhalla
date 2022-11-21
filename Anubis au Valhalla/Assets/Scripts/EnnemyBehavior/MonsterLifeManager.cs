using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class MonsterLifeManager : MonoBehaviour
{
    public GameObject textDamage;
    public Animator animator;
    public Rigidbody2D rb;
    public HealthBarMonstre healthBar;
    public AIPath ai;
    public int vieMax;
    public int vieActuelle;
    public int soulValue = 4;
    public float delay;
    private float forceKnockBack = 10;
    public UnityEvent OnBegin, OnDone;
    public float criticalPick;
    
    public GameObject root;

    [Header("Alterations d'état")] 
    public float InvincibleTime;
    public float InvincibleTimeTimer;
    public bool isInvincible;
    public float MomifiedTime = 3;
    public float MomifiedTimeTimer;
    public bool isMomified;
    public GameObject bandelettesMomie;
    public GameObject bandelettesHolder;
    private bool activeBandelettes;
    public bool isEnvased;
    public float EnvasedTime = 5;
    public float EnvasedTimeTimer;
    private float demiSpeed;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vieActuelle = vieMax;
        demiSpeed = ai.speed / 2;
    }

    private void Update()
    {
        transform.localRotation = Quaternion.identity;
        if (isInvincible)
        {
            InvincibleTimeTimer += Time.deltaTime;

            if (InvincibleTimeTimer >= InvincibleTime)
            {
                isInvincible = false;
                InvincibleTimeTimer = 0;
            }
        }

        if (isEnvased)
        {
            EnvasedTimeTimer += Time.deltaTime;
            ai.speed = demiSpeed;
            
            if (EnvasedTimeTimer >= EnvasedTime)
            {
                ai.speed *= 2;
                EnvasedTimeTimer = 0;
                isEnvased = false;
                
            }
        }
        
        if (isMomified)
        {
            MomifiedTimeTimer += Time.deltaTime;
            ai.canMove = false;
            GameObject bandelettesHolder = Instantiate(bandelettesMomie, transform);
            
            if (MomifiedTimeTimer >= MomifiedTime)
            {
                activeBandelettes = true;
                MomifiedTimeTimer = 0;
                isMomified = false;
                Destroy(bandelettesHolder);
                ai.canMove = true;
            }
        }
    }

    public void TakeDamage(int damage, float staggerDuration)
    {
        if (!isInvincible)
        {
            criticalPick = Random.Range(0,100);
            StartCoroutine(AnimationDamaged(staggerDuration));
            transform.DOShakePosition(staggerDuration, 0.5f, 50).OnComplete(() =>
            {
                ai.canMove = true;
            });
            
            if (criticalPick <= AttaquesNormales.instance.criticalRate[AttaquesNormales.instance.comboActuel])
            {
                vieActuelle -= damage * 2; 
                healthBar.SetHealth(vieActuelle);
                isInvincible = true;
                criticalPick = 0;
            }
            else
            {
                vieActuelle -= damage; 
                healthBar.SetHealth(vieActuelle);
                isInvincible = true;
            }
        
        }
        
        if (vieActuelle <= 0)
        {
            Die();
        }
    }
    
    IEnumerator AnimationDamaged(float duration)
    {
        animator.SetBool("IsTouched", true);
        yield return new WaitForSeconds(duration);
        ai.canMove = true;
        animator.SetBool("IsTouched", false);
    }
    
    public void DamageText(int damageAmount)
    {
        if (!isInvincible)
        {
            if (criticalPick <= AttaquesNormales.instance.criticalRate[AttaquesNormales.instance.comboActuel])
            {
                textDamage.GetComponentInChildren<TextMeshPro>().SetText((damageAmount * 2).ToString());
                GameObject textOBJ = Instantiate(textDamage, new Vector3(transform.position.x,transform.position.y + 1,-5), Quaternion.identity);
                textOBJ.transform.localScale *= 2;

            }
            else
            {
                textDamage.GetComponentInChildren<TextMeshPro>().SetText(damageAmount.ToString());
                Instantiate(textDamage, new Vector3(transform.position.x,transform.position.y + 1,-5), Quaternion.identity);
            }
          
        }
    }
    
    public void OnTriggerEnter2D(Collider2D col)
    {
        Vector2 direction = (transform.position - col.transform.position);
        direction.Normalize();
        if (col.CompareTag("AttaqueNormale"))
        {
            //StopAllCoroutines();
            OnBegin?.Invoke();
            //rb.velocity = Vector2.zero;
            //rb.AddForce(direction * forceKnockBack,ForceMode2D.Impulse);
            StartCoroutine(Reset());
        }
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector3.zero;
        OnDone?.Invoke();
    }

    void Die()
    {
        Souls.instance.CreateSouls(gameObject.transform.position, soulValue);
        SalleGennerator.instance.currentRoom.currentEnemies.Remove(root);
        SalleGennerator.instance.currentRoom.CheckForEnemies();
        Destroy(root);
    }
}
