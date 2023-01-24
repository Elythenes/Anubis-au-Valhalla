using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public NewPowerManager manager;
    private float bulletSpeed;
    public GameObject vfxOwn;
    private CircleCollider2D hitbox;
    private Rigidbody2D rb;
    public GameObject hitboxExplosion;
    public List<GameObject> explodeVfx = new List<GameObject>();
    public bool isExploding;
    public bool tweening;

    private void Start()
    {
        manager = GameObject.Find("NewPowerManager").GetComponent<NewPowerManager>();
        hitbox = GetComponent<CircleCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        bulletSpeed = manager.p1ThrustBallVelocities[manager.currentLevelPower1 -1];
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
     
        
        if (isExploding && !tweening)
        {
            tweening = true;
                if (!manager.p1ThrustExplosionSize)
                {
                    hitboxExplosion.transform.localScale = manager.p1ThrustSize1 * Vector3.one;
                    hitbox.radius = hitboxExplosion.transform.localScale.y / 2;
                    Destroy(gameObject,0.7f);
                    /*if (hitboxExplosion.transform.localScale.x < manager.p1ThrustSize1)
                    {
                        hitboxExplosion.transform.localScale += new Vector3(0.05f, 0.05f, 0);
                        hitbox.radius = hitboxExplosion.transform.localScale.y / 2;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }*/
                }
                else
                {
                    hitboxExplosion.transform.localScale = manager.p1ThrustSize2 * Vector3.one;
                    hitbox.radius = hitboxExplosion.transform.localScale.y / 2;
                    foreach (var oui in explodeVfx)
                    {
                        oui.transform.localScale *= 2;
                    }
                    Destroy(gameObject,0.7f);
                    /*if (hitboxExplosion.transform.localScale.x < manager.p1ThrustSize2)
                    {
                        hitboxExplosion.transform.localScale += new Vector3(0.05f, 0.05f, 0);
                        hitbox.radius = hitboxExplosion.transform.localScale.y / 2;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }*/
                }
                    
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            vfxOwn.SetActive(false);
            hitboxExplosion.SetActive(true);
            isExploding = true;
            if (!manager.p1ThrustBallExecute)
            {
                col.GetComponentInParent<MonsterLifeManager>().TakeDamage(manager.p1ThrustBallDamages[manager.currentLevelPower1 -1] + (int)AnubisCurrentStats.instance.magicForce, 0.5f);
            }
            else
            {
                if (col.GetComponentInParent<MonsterLifeManager>().vieActuelle <= col.GetComponentInParent<MonsterLifeManager>().vieMax * 20 / 100)
                {
                    col.GetComponentInParent<MonsterLifeManager>().TakeDamage(manager.p1ThrustBallDamages[manager.currentLevelPower1 -1]*100, 0.5f);
                }
                else
                {
                    col.GetComponentInParent<MonsterLifeManager>().TakeDamage(manager.p1ThrustBallDamages[manager.currentLevelPower1 -1] + (int)AnubisCurrentStats.instance.magicForce, 0.5f);
                }
            }
           
        }
    }
}
