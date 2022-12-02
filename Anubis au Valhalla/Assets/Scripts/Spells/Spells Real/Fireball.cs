using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public PouvoirFeuObject soPouvoirFeu;
    private SpriteRenderer sr;
    private SpriteRenderer srExplo;
    private CircleCollider2D hitbox;
    private Rigidbody2D rb;
    public GameObject hitboxExplosion;
    public bool isExploding;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        hitbox = GetComponent<CircleCollider2D>();
        srExplo = hitboxExplosion.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        Destroy(gameObject,soPouvoirFeu.bulletDuration);
        transform.localScale = soPouvoirFeu.bulletScale;
    }

    void Update()
    {
        if (!isExploding)
        {
            rb.velocity = transform.right * soPouvoirFeu.bulletSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
     

        if (isExploding)
        {
            if (hitboxExplosion.transform.localScale.x < soPouvoirFeu.explosionScale && hitboxExplosion.transform.localScale.y < soPouvoirFeu.explosionScale)
            {
                hitboxExplosion.transform.localScale += new Vector3(0.05f, 0.05f, 0);
                Vector2 S = srExplo.sprite.bounds.size;
                hitbox.radius = srExplo.transform.localScale.y /2;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            sr.enabled = false;
            hitboxExplosion.SetActive(true);
            isExploding = true;
            col.GetComponentInParent<MonsterLifeManager>().DamageText(soPouvoirFeu.thrustDamage);
            col.GetComponentInParent<MonsterLifeManager>().TakeDamage(soPouvoirFeu.thrustDamage, soPouvoirFeu.stagger);
        }
    }
}
