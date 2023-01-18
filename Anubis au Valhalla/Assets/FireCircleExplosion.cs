using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCircleExplosion : MonoBehaviour
{
    public SpriteRenderer exploSR;
    public CircleCollider2D exploCollider;
    
    void Update()
    {
        if (transform.localScale.x < 10)
        {
           transform.localScale += new Vector3(0.05f, 0.05f, 0);
            Vector2 S = exploSR.sprite.bounds.size;
            exploCollider.radius = (exploSR.sprite.bounds.size.x / 2);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            col.GetComponentInParent<MonsterLifeManager>().TakeDamage(30 + (int)AnubisCurrentStats.instance.magicForce, 0.5f);
        }
    }
}
