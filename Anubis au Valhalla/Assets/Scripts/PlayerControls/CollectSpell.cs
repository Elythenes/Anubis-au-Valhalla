using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollectSpell : MonoBehaviour
{
    public KeyCode interaction;

    public bool isSpellCollectable = false;

    public GameObject spellCollectManager;
    public GameObject collectableSpell;
    public SpellObject scriptContainSo;
    public GameObject prefabContain;
    
    public SpellManager spellManager;
    private KeyCode castSpell1;
    private KeyCode castSpell2;
    //pas oublier de faire la référence avec le skillManager pour pas lancer de spell quand on en ramasse

    void Start()
    {
        isSpellCollectable = false;
        Debug.Log("isSpellCollectable commence à false");
        castSpell1 = spellManager.spell1;
        castSpell2 = spellManager.spell2;
    }

    void Update()
    {
        CollectingSpell();
    }

    private void OnTriggerEnter2D(Collider2D other) //détecte si un spell est sur le joueur
    {
        if (other.gameObject.CompareTag("CollectableSpell"))
        {
            isSpellCollectable = true;
            collectableSpell = other.gameObject;
            scriptContainSo = other.gameObject.GetComponent<ContainScriptableObject>().spellInside;
            prefabContain = other.gameObject.GetComponent<ContainScriptableObject>().prefabInside;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("CollectableSpell"))
        {
            isSpellCollectable = false;
            collectableSpell = null;
            scriptContainSo = null;
        }
    }

    void CollectingSpell() //fonction pour ramasser un spell
    {
        if (isSpellCollectable == true && Input.GetKey(interaction))
        {
            spellManager.canCastSpells = false;
            if (Input.GetKeyDown(castSpell1))
            {
                Debug.Log("interaction sur spell");
                isSpellCollectable = false; // ligne à retirer à la fin car détruit l'objet en soi (une fois tout bien fait)
                //spellManager.containerSlot1.Add(scriptContainSo);
                //Debug.Log("ajout dans le slot 1");
                spellManager.containerA = scriptContainSo;
                spellManager.prefabA = prefabContain;
                Debug.Log("ajout dans le slot A");
                spellManager.isSpell1fill = true;
            }

            if (Input.GetKeyDown(castSpell2))
            {
                Debug.Log("interaction sur spell");
                isSpellCollectable = false; // ligne à retirer à la fin car détruit l'objet en soi (une fois tout bien fait)
                //spellManager.containerSlot2.Add(scriptContainSo);
                //Debug.Log("ajout dans le slot 2");
                spellManager.containerB = scriptContainSo;
                spellManager.prefabB = prefabContain;
                Debug.Log("ajout dans le slot B");
                spellManager.isSpell2fill = true;
            }
        }
        else
        {
            //skillManager.canCastSpells = true;
        }
    }
}
