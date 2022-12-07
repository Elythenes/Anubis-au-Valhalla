using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PuppetHealth : MonsterLifeManager
{
    public bool canAttack = true;
    public SpriteRenderer sprite;
    public Color deactivatedColor = new Color(0.3f, 0.3f, 0.3f);

    public override void Start()
    {
        ResetHealth();
        sprite = GetComponent<SpriteRenderer>();
    }

    public override void Update()
    {
        if (isInvincible)
        {
            InvincibleTimeTimer += Time.deltaTime;

            if (InvincibleTimeTimer >= InvincibleTime)
            {
                isInvincible = false;
                InvincibleTimeTimer = 0;
            }
        }

        if (!canAttack && sprite.color != deactivatedColor)
        {
            Deactivate();
        }
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
        
        }
        
        if (vieActuelle <= 0)
        {
            canAttack = false;
        }
    }
    public override void DamageText(int damageAmount)
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
        if (transform.localScale.x >= 0.4f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 0.4f, FenrirBoss.instance.reduceScaleTimer);
            sprite.color = Color.Lerp(sprite.color, deactivatedColor, FenrirBoss.instance.reduceScaleTimer);
            return;
        }
        transform.localScale = Vector3.one * 0.4f;
        sprite.color = deactivatedColor;
    }

    public void ResetHealth()
    {
        vieActuelle = vieMax;
    }
}
