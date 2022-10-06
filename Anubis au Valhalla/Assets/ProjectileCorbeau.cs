using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ProjectileCorbeau : MonoBehaviour
{
    public IA_Corbeau ia;
    public Vector2 dir;
    public float angle;
    

    private void Start()
    {
        dir = new Vector2(CharacterController.instance.transform.position.x - transform.position.x,
            CharacterController.instance.transform.position.y - transform.position.y);
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        dir.Normalize();
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x + dir.x * ia.plumeSpeed, transform.position.y + dir.y * ia.plumeSpeed, 0);
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            DamageManager.instance.TakeDamage(ia.puissanceAttaque);
            Destroy(gameObject);
        }
    }

  /*  private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }*/
}