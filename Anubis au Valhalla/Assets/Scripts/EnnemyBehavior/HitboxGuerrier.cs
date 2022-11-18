using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxGuerrier : MonoBehaviour
{
    public IA_Guerrier ia;

    private void Start()
    {
        Destroy(gameObject,ia.dureeAttaque);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<DamageManager>().TakeDamage(ia.puissanceAttaque);
        }
    }
}
