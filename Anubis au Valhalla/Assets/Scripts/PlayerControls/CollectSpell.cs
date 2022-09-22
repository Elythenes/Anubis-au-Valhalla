using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectSpell : MonoBehaviour
{
    public KeyCode interaction;

    public bool isSpellCollectable = false;

    public GameObject collectSpellManager;

    private GameObject variableX;

    void Start()
    {
        isSpellCollectable = false;
        Debug.Log("isSpellCollectable commence à false");
    }

    void Update()
    {
        //CollectingSpell();
    }

    private void OnTriggerStay2D(Collider2D other) //détecte si un spell est sur le joueur
    {
        if (other.gameObject.CompareTag("CollectableSpell"))
        {
            isSpellCollectable = true;
            variableX = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("CollectableSpell"))
        {
            isSpellCollectable = false;
        }
    }

    /*void CollectingSpell(GameObject other) //fonction pour ramasser un spell
    {
        if (isSpellCollectable == true && Input.GetKeyDown(interaction))
        {
            Debug.Log("interaction sur spell");
            isSpellCollectable = false; // ligne à retirer à la fin car détruit l'objet en soi
            other.GetComponent();
        }
    }*/
}
