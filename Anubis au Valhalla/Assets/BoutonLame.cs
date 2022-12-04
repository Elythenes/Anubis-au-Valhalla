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

    public void ChoseLame()
    {
        animMenuPartie.SetBool("ChoseLame",true);
        animMenuChoices.SetBool("SpawnChoices",true);
    }

    public void FadeOut()
    {
        fade = true;
    }
    
    public void FadeIn()
    {
        fade = false;
    }
}
