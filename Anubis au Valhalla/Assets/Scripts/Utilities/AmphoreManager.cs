using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AmphoreManager : MonoBehaviour
{
    public ParticleSystem partculesAmphores;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("AttaqueNormale"))
        {
            Souls.instance.CreateSouls(transform.position, Random.Range(0,4));
            Instantiate(partculesAmphores, transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            Souls.instance.CreateSouls(transform.position, Random.Range(0,4));
            Instantiate(partculesAmphores, new Vector3(transform.position.x,transform.position.y,10),Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

