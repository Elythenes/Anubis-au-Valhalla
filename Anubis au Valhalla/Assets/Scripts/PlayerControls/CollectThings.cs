using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CollectThings : MonoBehaviour
{
    public KeyCode interaction;

    [Header("DEBUG")]
    public bool isPowerCollectable = false;
    public GameObject collectablePower;
    
    public bool isPotionCollectable = false;
    public GameObject collectablePotion;

    public bool isGlyphCollectable = false;
    public GameObject collectableGlyph;

    
    
    //Fonction : Systèmes *******************************************************************************************************************
    void Start()
    {
        isPowerCollectable = false;
        isPotionCollectable = false;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(interaction))
        {
            if (isPowerCollectable)
            {
                UiThotManager.Instance.MoveIn = true;
                UiManager.instance.CollectPower(collectablePower);
                collectablePower.SetActive(false);
            }

            if (isPotionCollectable)
            {
                UiManager.instance.ActivateMenuPotion();
            }
            
            if (isGlyphCollectable)
            {
                //ramasser la glyph
            }
        }

        
    }
    
    
    //Fonction *******************************************************************************************************************
    private void OnTriggerStay2D(Collider2D other) //détecte si un spell est sur le joueur
    {
        if (other.gameObject.CompareTag("CollectableSpell"))
        {
            isPowerCollectable = true;
            collectablePower = other.gameObject;
        }
        
        if (other.gameObject.CompareTag("CollectablePotion"))
        {
            isPotionCollectable = true;
            collectablePotion = other.gameObject;
        }
        
        if (other.gameObject.CompareTag("CollectableGlyph"))
        {
            isGlyphCollectable = true;
            collectableGlyph = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) //c'est du Debug, ne sert pas vraiment
    { 
        if (other.gameObject.CompareTag("CollectableSpell"))
        {
            isPowerCollectable = false;
            collectablePower = null;
        }
        
        if (other.gameObject.CompareTag("CollectablePotion"))
        {
            isPotionCollectable = false;
            collectablePotion = null;
        }
        
        if (other.gameObject.CompareTag("CollectableGlyph"))
        {
            isGlyphCollectable = false;
            collectableGlyph = null;
        }
    }


    public void KillPotion()
    {
        Destroy(collectablePotion);
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    /* ancien code de ses morts (je le garde, on sait jamais :D)
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
                spellManager.isSpell1Fill = true;
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
                spellManager.isSpell2Fill = true;
            }
        }
        else
        {
            //skillManager.canCastSpells = true;
        }
    }*/
}
