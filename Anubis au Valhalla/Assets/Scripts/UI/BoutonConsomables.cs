using UnityEngine;
using UnityEngine.EventSystems;

public class BoutonConsomables : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator anim;
    public Animator animConsomables;
    public CanvasGroup choseMenu;
    public bool fade;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        anim.SetBool("SelectConsomable",true);
        anim.SetBool("Idle",false);
    }
    
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        anim.SetBool("SelectConsomable",false);
        anim.SetBool("Idle",true);
    }

    public void SpawnConsomables()
    {
        animConsomables.SetBool("SpawnConsomables",true);
    }
    
    public void Update()  // Gérer le fade in et out du menu de consomables
    {
        if (fade)
        {
            if (choseMenu.alpha > 0)
            {
                choseMenu.alpha -= Time.deltaTime; 
            }
        }
        else if(!fade)
        {
            if (choseMenu.alpha < 1)
            {
                choseMenu.alpha += Time.deltaTime;    
            }
        }
    }

    public void FadeOut()
    {
        fade = true;
        choseMenu.interactable = false;
        choseMenu.blocksRaycasts = false;
    }
    
    public void FadeIn()
    {
        fade = false;
        choseMenu.interactable = true;
        choseMenu.blocksRaycasts = true;
    }
}
