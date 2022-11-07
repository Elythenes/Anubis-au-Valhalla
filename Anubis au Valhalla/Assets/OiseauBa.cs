using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OiseauBa : MonoBehaviour
{
    public SpellThrowingObject soOiseauBa;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = transform.right * soOiseauBa.bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            Debug.Log("touché");
            col.GetComponent<MonsterLifeManager>().DamageText(soOiseauBa.puissanceAttaque);
            col.GetComponent<MonsterLifeManager>().TakeDamage(soOiseauBa.puissanceAttaque, soOiseauBa.stagger);
            
            if (col.GetComponent<MonsterLifeManager>().vieActuelle <= 0)
            {
                DamageManager.instance.vieActuelle += 5;
                LifeBarManager.instance.SetHealth(DamageManager.instance.vieActuelle);
                if (DamageManager.instance.vieMax < DamageManager.instance.vieActuelle)
                {
                    DamageManager.instance.vieActuelle = DamageManager.instance.vieMax;
                }
            }
        }
    }
}
