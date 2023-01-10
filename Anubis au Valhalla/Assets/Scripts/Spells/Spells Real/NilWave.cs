using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NilWave : MonoBehaviour
{
    public SpellSpawnEntityObject soNilWave;
    public float speed;
    public float forceKnockback;
    public GameObject boue;
    private float timerBoueTimer;
    public float timerBoue;
    
    
    public void Update()
    {
        transform.Translate(transform.right * speed,Space.World);
        timerBoueTimer += Time.deltaTime;

        if (timerBoueTimer >= timerBoue)
        {
            Instantiate(boue, transform.position, Quaternion.identity);
            timerBoueTimer = 0;
        }
      
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        
        Vector2 angle = col.transform.position - transform.position;  
        
        if (col.gameObject.tag == "Monstre")
        {
            Debug.Log("oui");
            col.GetComponent<Rigidbody2D>().AddForce(angle*forceKnockback);
        }
    }

}
