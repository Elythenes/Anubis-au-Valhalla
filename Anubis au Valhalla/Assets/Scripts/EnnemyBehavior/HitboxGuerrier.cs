using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxGuerrier : MonoBehaviour
{
    public IA_Guerrier ia;
    public ParticleSystem effetSlash;

    private void Start()
    {
        Destroy(gameObject,ia.dureeAttaque);
        effetSlash.transform.rotation = transform.rotation;
        if (effetSlash.transform.position.x - ia.transform.position.x > 0)
        {
            var transform1 = effetSlash.transform;
            var localScale = transform1.localScale;
            localScale = new Vector3(localScale.x, -0.5f, localScale.z);
            transform1.localScale = localScale;
        }
        else
        {
            var transform1 = effetSlash.transform;
            var localScale = transform1.localScale;
            localScale = new Vector3(localScale.x, 0.5f, localScale.z);
            transform1.localScale = localScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<DamageManager>().TakeDamage(ia.puissanceAttaque,ia.gameObject);
        }
    }
}
