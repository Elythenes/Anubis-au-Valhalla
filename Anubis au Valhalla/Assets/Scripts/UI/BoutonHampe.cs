using UnityEngine;
using UnityEngine.EventSystems;


public class BoutonHampe : MonoBehaviour
{
    public Animator animHampe;
   
    public void SelectHampe()
    {
        animHampe.SetBool("HampeUp",true);
        animHampe.SetBool("HampeDown",false);
    }
    
    public void UnselectHampe()
    {
        animHampe.SetBool("HampeDown",true);
        animHampe.SetBool("HampeUp",false);
    }
    
    
    
    
    
    
    
    
    
   /* public void OnPointerEnter(PointerEventData pointerEventData)
    {
        anim.SetBool("HampeUp",true);
        anim.SetBool("HampeDown",false);
        anim.SetBool("IdleHampe",false);
    }
    
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        anim.SetBool("HampeDown",true);
        anim.SetBool("HampeUp",false);
        anim.SetBool("IdleHampe",true);
    }
    
    public void ChoseHampe()
    {
        animMenuPartie.SetBool("ChoseHampe",true);
        animMenuChoices.SetBool("SpawnChoices",true);
        animMenuChoices.SetBool("BackIdle",false);
    }*/
}
