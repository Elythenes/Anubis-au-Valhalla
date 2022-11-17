using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SarcophageBehaviour : MonoBehaviour
{
    public SpellSpawnEntityObject soSarcophage;
    private CircleCollider2D hitbox;
    public float timeToCloseTimer;
    public float forceRapprochement;
    
    void Start()
    {
        hitbox = GetComponent<CircleCollider2D>();
        transform.localRotation = new Quaternion(0, 0, 0, 0);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Monstre"))
        {
            if (timeToCloseTimer >= soSarcophage.timerStep2)
            {
                // phase de refermement
                hitbox.radius = 0.5f;
                other.GetComponent<MonsterLifeManager>().TakeDamage(soSarcophage.puissanceAttaque,soSarcophage.stagger);
                other.GetComponent<MonsterLifeManager>().DamageText(soSarcophage.puissanceAttaque);
                Destroy(gameObject);
            }
            else if (timeToCloseTimer < soSarcophage.timerStep2)
            {
                // phase de raprochement
                other.GetComponent<Rigidbody2D>().AddForce((transform.position - other.transform.position) * forceRapprochement);
            }
        }
    }

    void Update()
    {
        timeToCloseTimer += Time.deltaTime;
    }
}
