using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PuppetHealth : MonsterLifeManager
{
    public bool canAttack = true;
    public SpriteRenderer sprite;
    public Vector3 targetRecoil;
    public bool isHead;
    private Vector3 basePos;
    private Vector3 baseScale;
    private BoxCollider2D dmgTrigger;
    private FenrirBoss fb;

    public void Awake()
    {

    }

    public override void Start()
    {
        fb = FenrirBoss.instance;
        targetRecoil = new Vector3(transform.position.x,
            transform.position.y + fb.deactivationRecoil, 
            transform.position.z);
        basePos = transform.position;
        baseScale = transform.localScale;
        if (!isHead)
        {
            dmgTrigger = GetComponent<BoxCollider2D>();
        }
        ResetHealth();
        sprite = GetComponent<SpriteRenderer>();
        if (isHead)
        {
            isInvincible = true;
        }
    }

    public override void Update()
    {
        
    }

    public override void TakeDamage(int damage, float staggerDuration)
    {
        if (!isInvincible)
        {
            gotHit = true;
            criticalPick = Random.Range(0,100);
            if (criticalPick <= AttaquesNormales.instance.criticalRate)
            {
                vieActuelle -= damage * 2;
            }
            else
            {
                vieActuelle -= damage;
            }

            transform.DOComplete();
            transform.DOShakePosition(0.1f,0.2f,30);
        }
        
        if (vieActuelle <= 0)
        {
            Deactivate();
        }
    }
    public void DamageText(int damageAmount)
    {
        if (!isInvincible)
        {
            if (criticalPick <= AttaquesNormales.instance.criticalRate)
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

    public void Deactivate()
    {
        Debug.Log("oui");
        dmgTrigger.enabled = false;
        canAttack = false;
        transform.DOMoveY(fb.deactivationRecoil, 1f);
        transform.DOScale(Vector3.one * 0.4f, 1f);
        sprite.DOColor(fb.deactivatedColor, 1f);
        fb.CheckPaws();
    }

    public void Reactivate()
    {
        dmgTrigger.enabled = true;
        canAttack = true;
        transform.DOMove(basePos, 1f);
        transform.DOScale(baseScale, 1f);
        sprite.DOColor(Color.white, 1f);
        ResetHealth();
    }

    public void StunHead()
    {
        sprite.color = fb.vulnerableColor;
        sprite.DOColor(Color.white, 0.5f);
        transform.DOShakePosition(0.75f, 0.2f,30).OnComplete(() =>
        {
            transform.DOMove(transform.position + Vector3.down * 2, 0.5f);
            StartCoroutine(fb.StunReset());
        });
    }

    public void HeadReset()
    {
        isInvincible = true;
        transform.DOShakePosition(0.25f, 0.2f, 30).OnComplete(() =>
        {
            transform.DOMove(transform.position + Vector3.up * 2, 0.5f);
        });
    }

    public void ResetHealth()
    {
        vieActuelle = vieMax;
    }
}
