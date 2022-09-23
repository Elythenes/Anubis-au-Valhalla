using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public SpellThrowingType sOFireball;
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
            col.GetComponent<IA_Monstre1>().TakeDamage(sOFireball.puissanceAttaque);
            col.GetComponent<IA_Monstre1>().DamageText(sOFireball.puissanceAttaque);
        }
    }
}
