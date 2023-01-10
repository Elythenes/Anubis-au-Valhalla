using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public NewPowerManager manager;
    private float bulletSpeed;
    private SpriteRenderer sr;
    private SpriteRenderer srExplo;
    private CircleCollider2D hitbox;
    private Rigidbody2D rb;
    public GameObject hitboxExplosion;
    public bool isExploding;

    private void Start()
    {
        manager = GameObject.Find("NewPowerManager").GetComponent<NewPowerManager>();
        sr = GetComponent<SpriteRenderer>();
        hitbox = GetComponent<CircleCollider2D>();
        srExplo = hitboxExplosion.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        bulletSpeed = manager.p1ThrustBallVelocities[manager.currentLevelPower1];
        Destroy(gameObject,3f);
    }
    

    void Update()
    {
        if (!isExploding)
        {
            rb.velocity = transform.right * bulletSpeed;
        }
        else 
        {
            rb.velocity = Vector2.zero;
        }
     

        if (isExploding)
        {
            
                if (!manager.p1ThrustExplosionSize)
                {
                    if (hitboxExplosion.transform.localScale.x < manager.p1ThrustSize1)
                    {
                        hitboxExplosion.transform.localScale += new Vector3(0.05f, 0.05f, 0);
                        Vector2 S = srExplo.sprite.bounds.size;
                        hitbox.radius = srExplo.transform.localScale.y / 2;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    if (hitboxExplosion.transform.localScale.x < manager.p1ThrustSize2)
                    {
                        hitboxExplosion.transform.localScale += new Vector3(0.05f, 0.05f, 0);
                        Vector2 S = srExplo.sprite.bounds.size;
                        hitbox.radius = srExplo.transform.localScale.y / 2;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
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
            if (!manager.p1ThrustBallExecute)
            {
                col.GetComponentInParent<MonsterLifeManager>().TakeDamage(manager.p1ThrustBallDamages[manager.currentLevelPower1], 0.5f);
            }
            else
            {
                if (col.GetComponentInParent<MonsterLifeManager>().vieActuelle <= col.GetComponentInParent<MonsterLifeManager>().vieMax * 20 / 100)
                {
                    col.GetComponentInParent<MonsterLifeManager>().TakeDamage(manager.p1ThrustBallDamages[manager.currentLevelPower1]*100, 0.5f);
                }
                else
                {
                    col.GetComponentInParent<MonsterLifeManager>().TakeDamage(manager.p1ThrustBallDamages[manager.currentLevelPower1], 0.5f);
                }
            }
           
        }
    }
}
