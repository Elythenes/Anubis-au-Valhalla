using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SarcophageBehaviour : MonoBehaviour
{
    public PouvoirPlaieObject soPlaie;
    private Vector2 _direction;


    void Start()
    {
        Destroy(gameObject,soPlaie.forceDuration);
        transform.localRotation = new Quaternion(0, 0, 0, 0);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Monstre"))
        {
             _direction = other.transform.position - transform.position;
             Vector2 driNormalized = _direction.normalized * soPlaie.forceAttraction;
            other.GetComponent<Rigidbody2D>().velocity = _direction;
            //other.GetComponent<Rigidbody2D>().AddForce((transform.position - other.transform.position) * soPlaie.forceAttraction,ForceMode2D.Impulse);
        }
    }
}
