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
    public ScriptableObject scriptContainSo;
    
    public SkillManager skillManager;
    public KeyCode castSpell1;
    public KeyCode castSpell2;
    //pas oublier de faire la référence avec le skillManager pour pas lancer de spell quand on en ramasse

    public SpellCollectManager spellCm;

    void Start()
    {
        isSpellCollectable = false;
        Debug.Log("isSpellCollectable commence à false");
        castSpell1 = skillManager.spell1;
        castSpell2 = skillManager.spell2;
    }

    void Update()
    {
        CollectingSpell();
    }

    private void OnTriggerStay2D(Collider2D other) //détecte si un spell est sur le joueur
    {
        if (other.gameObject.CompareTag("CollectableSpell"))
        {
            isSpellCollectable = true;
            collectableSpell = other.gameObject;
            scriptContainSo = other.gameObject.GetComponent<ContainScriptableObject>().spellInside;
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
        if (isSpellCollectable == true && Input.GetKey(interaction) && (Input.GetKey(castSpell1) || Input.GetKey(castSpell2) ))
        {
            Debug.Log("interaction sur spell");
            isSpellCollectable = false; // ligne à retirer à la fin car détruit l'objet en soi (une fois tout bien fait)
            spellCm.spellList.Add(scriptContainSo);
            Debug.Log("ajout du spell");
        }
    }
}
