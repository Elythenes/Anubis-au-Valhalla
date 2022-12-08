using DG.Tweening;
using TMPro;
using UnityEngine;

public class CollectSpell : MonoBehaviour
{
    public KeyCode interaction;
    //pas oublier de faire la référence avec le skillManager pour pas lancer de spell quand on en ramasse
    
    [Header("DEBUG")]
    public bool isSpellCollectable = false;
    public GameObject collectableSpell;
    public GameObject CanvasInteraction;
    public Vector3 offset;
    public TextMeshProUGUI TextInteraction;
    
    void Start()
    {
        isSpellCollectable = false;  CanvasInteraction = GameObject.FindWithTag("CanvasInteraction");
        CanvasInteraction = GameObject.FindWithTag("CanvasInteraction");
        TextInteraction = GameObject.Find("TexteAction").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("CollectableSpell"))
        {
            if (isSpellCollectable)
            {
                CanvasInteraction.transform.position = collectableSpell.transform.position + offset;
            }
            CanvasInteraction.transform.localScale = new Vector3(0,0,CanvasInteraction.transform.localScale.z);
            CanvasInteraction.transform.DOScale(new Vector3(1, 1, CanvasInteraction.transform.localScale.z),0.25f);
            TextInteraction.SetText("Prendre");
            CanvasInteraction.SetActive(true); 
        }
    }

    private void OnTriggerStay2D(Collider2D other) //détecte si un spell est sur le joueur
    {
        if (other.gameObject.CompareTag("CollectableSpell"))
        {
            isSpellCollectable = true;
            collectableSpell = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) //c'est du Debug, ne sert pas vraiment
    {
        if (other.gameObject.CompareTag("CollectableSpell"))
        {
            CanvasInteraction.SetActive(false);
            isSpellCollectable = false;
            collectableSpell = null;
        }
    }

    private void Update()
    {
        if (isSpellCollectable)
        {
            if (Input.GetKeyDown(interaction))
            {
                UiManager.instance.ActivateMenu();
            }
        }
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
