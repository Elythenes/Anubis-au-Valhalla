using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class OiseauBa : MonoBehaviour
{
    public PouvoirEauObject soEau;
    private Rigidbody2D rb;

    private void Start()
    {
        transform.localScale = soEau.bulletScale;
        Destroy(gameObject,soEau.bulletDuration);
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = transform.right * soEau.bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            col.GetComponentInParent<MonsterLifeManager>().DamageText(soEau.thrustDamage);
            col.GetComponentInParent<MonsterLifeManager>().TakeDamage(soEau.thrustDamage, soEau.staggerThrust);
            
            if (col.GetComponentInParent<MonsterLifeManager>().vieActuelle <= 0)
            {
                DamageManager.instance.Heal(soEau.bulletHeal);
            }
        }
    }
}
