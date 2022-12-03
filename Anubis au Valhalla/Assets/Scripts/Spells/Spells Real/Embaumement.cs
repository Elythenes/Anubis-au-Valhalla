using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Embaumement : MonoBehaviour
{
    public PouvoirPlaieObject sOPlaie;
    private Rigidbody2D rb;

    private void Start()
    {
        Destroy(gameObject,sOPlaie.bulletDuration);
        rb = gameObject.GetComponent<Rigidbody2D>();
        transform.localScale = sOPlaie.bulletScale;
    }

    void Update()
    {
        rb.velocity = transform.right * sOPlaie.bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            MonsterLifeManager monstre = col.GetComponentInParent<MonsterLifeManager>();
            monstre.DamageText(sOPlaie.thrustDamage);
            monstre.TakeDamage(sOPlaie.thrustDamage, sOPlaie.staggerThrust);
            monstre.isMomified = true;
            monstre.MomifiedTime = sOPlaie.dureeMomification;
        }
    }
}
