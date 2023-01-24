using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FireCircleExplosion : MonoBehaviour
{
    public SpriteRenderer exploSR;
    public CircleCollider2D exploCollider;
    public GameObject vFXChild;
    
    void Start()
    {

        transform.DOScale(transform.localScale * 10, 0.5f); 
        vFXChild.transform.DOScale(vFXChild.transform.localScale * 10, 0.5f);
        Destroy(gameObject,0.5f);
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            col.GetComponentInParent<MonsterLifeManager>().TakeDamage(30 + (int)AnubisCurrentStats.instance.magicForce, 0.5f);
        }
    }
}
