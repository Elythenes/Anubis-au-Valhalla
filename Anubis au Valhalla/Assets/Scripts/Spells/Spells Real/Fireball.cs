using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public PouvoirFeuObject soPouvoirFeu;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        Destroy(gameObject,soPouvoirFeu.bulletDuration);
        transform.localScale = soPouvoirFeu.bulletScale;
    }

    void Update()
    {
        rb.velocity = transform.right * soPouvoirFeu.bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            Debug.Log("touché");
            col.GetComponentInParent<MonsterLifeManager>().DamageText(soPouvoirFeu.thrustDamage);
            col.GetComponentInParent<MonsterLifeManager>().TakeDamage(soPouvoirFeu.thrustDamage, soPouvoirFeu.stagger);
        }
    }
}
