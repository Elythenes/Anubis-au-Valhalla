using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BoutonLame : MonoBehaviour
{
    public Animator animLame;
   
   

    public void SelectLame()
    {
        animLame.SetBool("LameUp",true);
        animLame.SetBool("LameDown",false);
    }
    
    public void UnselectLame()
    {
        animLame.SetBool("LameDown",true);
        animLame.SetBool("LameUp",false);
    }
    
    
    
    
    
    
    
    
    
    
    
   /* public void OnPointerEnter(PointerEventData pointerEventData)
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
    }*/
    

    /*public void ChoseLame()
    {
        animMenuPartie.SetBool("ChoseLame",true);
        animMenuChoices.SetBool("SpawnChoices",true);
        animMenuChoices.SetBool("BackIdle",false);
    }
    

    public void BackIdle()
    {
        animMenuPartie.SetBool("BackIdle",true);
        animMenuPartie.SetBool("ChoseLame",false);
        animMenuPartie.SetBool("ChoseHampe",false);
        animMenuPartie.SetBool("ChosePomeau",false);
    }*/
}
