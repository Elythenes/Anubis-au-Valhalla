using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxFallValkyrie : MonoBehaviour
{
    public IA_Valkyrie ia;
    public float expandSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(expandSpeed, expandSpeed, 0);

        if (ia == null)
        {
            Destroy(gameObject);
        }
    }

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
