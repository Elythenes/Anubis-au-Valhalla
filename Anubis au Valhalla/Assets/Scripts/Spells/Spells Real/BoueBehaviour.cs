using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoueBehaviour : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 5);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
       
        if (col.gameObject.tag == "Monstre")
        {
            Debug.Log("oui2");
            col.GetComponent<MonsterLifeManager>().isEnvased = true;
        }
    }
}
