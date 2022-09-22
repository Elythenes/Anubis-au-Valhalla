using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float fireSpeed;
    private Rigidbody2D rb;
    public int puissanceAttaqueFireball;

    private void Start()
    {
        fireSpeed = SkillManager.instance.bulletSpeed;
        puissanceAttaqueFireball = SkillManager.instance.puissanceAttaqueFireBall;
        fireSpeed = SkillManager.instance.bulletSpeed;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = transform.right * fireSpeed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            Debug.Log("touché");
            col.GetComponent<IA_Monstre1>().TakeDamage(puissanceAttaqueFireball);
        }
    }
}
