using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public SpellThrowingObject sOFireball;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = transform.right * sOFireball.bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            Debug.Log("touché");
            col.GetComponentInParent<MonsterLifeManager>().DamageText(sOFireball.puissanceAttaque);
            col.GetComponentInParent<MonsterLifeManager>().TakeDamage(sOFireball.puissanceAttaque, sOFireball.stagger);
        }
    }
}
