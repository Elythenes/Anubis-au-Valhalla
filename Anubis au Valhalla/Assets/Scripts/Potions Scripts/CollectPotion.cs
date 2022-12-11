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


    void Start()
    {
        isPotionCollectable = false;
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
