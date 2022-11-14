using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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
    public float forceKnockBack;
    public UnityEvent OnBegin, OnDone;
    
    public GameObject root;

    [Header("Alterations d'état")] 
    public float InvincibleTime;
    public float InvincibleTimeTimer;
    public bool isInvincible;
    public float MomifiedTime;
    public float MomifiedTimeTimer;
    public bool isMomified;
    public GameObject bandelettesMomie;
    public GameObject bandelettesHolder;
    private bool activeBandelettes;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vieActuelle = vieMax;
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
            StartCoroutine(AnimationDamaged());
            transform.DOShakePosition(staggerDuration, 0.5f, 50).OnComplete(() =>
            {
                ai.canMove = true;
            });
            vieActuelle -= damage; 
            healthBar.SetHealth(vieActuelle);
            isInvincible = true;
        }
        
        if (vieActuelle <= 0)
        {
            Die();
        }
    }
    
    IEnumerator AnimationDamaged()
    {
        animator.SetBool("IsTouched", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("IsTouched", false);
    }
    
    public void DamageText(int damageAmount)
    {
        if (!isInvincible)
        {
            textDamage.GetComponentInChildren<TextMeshPro>().SetText(damageAmount.ToString());
            Instantiate(textDamage, new Vector3(transform.position.x,transform.position.y + 1,-5), Quaternion.identity);
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
            rb.AddForce(direction * forceKnockBack,ForceMode2D.Impulse);
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
