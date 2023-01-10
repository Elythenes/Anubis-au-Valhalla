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
        effetSlash.startRotation3D = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z * Mathf.Deg2Rad);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<DamageManager>().TakeDamage(ia.puissanceAttaque,ia.gameObject);
        }
    }
}
