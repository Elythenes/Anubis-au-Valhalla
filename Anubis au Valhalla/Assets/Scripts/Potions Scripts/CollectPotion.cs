using DG.Tweening;
using TMPro;
using UnityEngine;

public class CollectPotion : MonoBehaviour
{
  
    public KeyCode interaction;
    //pas oublier de faire la référence avec le skillManager pour pas lancer de spell quand on en ramasse
    
    [Header("DEBUG")]
    public bool isPotionCollectable = false;
    public GameObject collectablePotion;
    public GameObject CanvasInteraction;
    public Vector3 offset;
    public TextMeshProUGUI TextInteraction;

    
    void Start()
    {
        isPotionCollectable = false;
        CanvasInteraction = GameObject.FindWithTag("CanvasInteraction");
        TextInteraction = GameObject.Find("TexteAction").GetComponent<TextMeshProUGUI>();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("CollectablePotion"))
        {
            if (isPotionCollectable)
            {
                CanvasInteraction.transform.position = collectablePotion.transform.position + offset;
            }
            CanvasInteraction.transform.localScale = new Vector3(0,0,CanvasInteraction.transform.localScale.z);
            CanvasInteraction.transform.DOScale(new Vector3(1, 1, CanvasInteraction.transform.localScale.z),0.25f);
            TextInteraction.SetText("Prendre");
            CanvasInteraction.SetActive(true); 
        }
    }

    private void OnTriggerStay2D(Collider2D other) //détecte si un spell est sur le joueur
    {
        if (other.gameObject.CompareTag("CollectablePotion"))
        {
            isPotionCollectable = true;
            collectablePotion = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) //c'est du Debug, ne sert pas vraiment
    {
        if (other.gameObject.CompareTag("CollectablePotion"))
        {
            CanvasInteraction.SetActive(false);
            isPotionCollectable = false;
            collectablePotion = null;
        }
    }

    private void Update()
    {
        if (isPotionCollectable)
        {
            if (Input.GetKeyDown(interaction))
            {
                UiManager.instance.ActivateMenuPotion();
                Destroy(collectablePotion);
            }
        }
    }
}
