using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxFallValkyrie : MonoBehaviour
{
    public IAManager ia;

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(0.006f, 0.006f, 0);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Vector2 pushForce = col.transform.position - transform.position;
        if (col.CompareTag("Player"))
        {
        Debug.Log(pushForce);
            DamageManager.instance.TakeDamage(ia.FallDamage);
            col.GetComponentInParent<Rigidbody2D>().AddForce(pushForce*ia.pushForce,ForceMode2D.Impulse);
        }
    }
}
