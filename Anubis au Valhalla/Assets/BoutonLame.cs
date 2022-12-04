using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BoutonLame : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator anim;
    public Animator animMenuPartie;
    public Animator animMenuChoices;
    public CanvasGroup choseMenu;
    public bool fade;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        anim.SetBool("LameUp",true);
        anim.SetBool("LameDown",false);
        anim.SetBool("Idle",false);
    }
    
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        anim.SetBool("LameDown",true);
        anim.SetBool("LameUp",false);
        anim.SetBool("Idle",true);
    }
    
    public void Update()
    {
        if (fade) // Gérer le fade in et out du menu de choix des parties
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

    public void ChoseLame()
    {
        animMenuPartie.SetBool("ChoseLame",true);
        animMenuChoices.SetBool("SpawnChoices",true);
        animMenuChoices.SetBool("BackIdle",false);
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

    public void BackIdle()
    {
        animMenuPartie.SetBool("BackIdle",true);
        animMenuPartie.SetBool("ChoseLame",false);
        animMenuPartie.SetBool("ChoseHampe",false);
        animMenuPartie.SetBool("ChosePomeau",false);
    }
}
