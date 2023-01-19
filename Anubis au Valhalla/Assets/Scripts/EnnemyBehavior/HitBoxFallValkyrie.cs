using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HitBoxFallValkyrie : MonoBehaviour
{
    public IA_Valkyrie ia;
    public float expandSpeed;
    public float speedUpRate;

    private void Awake()
    {
        ia = IA_Valkyrie.instance;
    }

    private void Start()
    {
        
        transform.DOScale(Vector3.one * 3, ia.FallTime/2).OnComplete(() => Destroy(gameObject));
    }

    // Update is called once per frame


    private void OnTriggerEnter2D(Collider2D col)
    {
        Vector2 pushForce = col.transform.position - transform.position;
        if (col.CompareTag("Player"))
        {
            DamageManager.instance.TakeDamage(ia.FallDamage, gameObject);
            col.GetComponentInParent<Rigidbody2D>().AddForce(pushForce*ia.pushForce,ForceMode2D.Impulse);
        }
    }
}
