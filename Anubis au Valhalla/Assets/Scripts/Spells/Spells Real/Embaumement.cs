using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Embaumement : MonoBehaviour
{
    public SpellThrowingObject sOEmbaumement;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = transform.right * sOEmbaumement.bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            MonsterLifeManager monstre = col.GetComponentInParent<MonsterLifeManager>();
            monstre.DamageText(sOEmbaumement.puissanceAttaque);
            monstre.TakeDamage(sOEmbaumement.puissanceAttaque, sOEmbaumement.stagger);
            monstre.isMomified = true;
            monstre.MomifiedTime = sOEmbaumement.DebuffTime;
        }
    }
}
