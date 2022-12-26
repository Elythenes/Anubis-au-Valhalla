using UnityEngine;
using UnityEngine.EventSystems;

public class BoutonPomeau : MonoBehaviour
{
    public Animator animPomeau;
   
    
    public void SelectPomeau()
    {
        animPomeau.SetBool("PomeauUp",true);
        animPomeau.SetBool("PomeauDown",false);
    }
    
    public void UnselectPomeau()
    {
        animPomeau.SetBool("PomeauDown",true);
        animPomeau.SetBool("PomeauUp",false);
    }
    
    
    
    
    
    
    /*public void OnPointerEnter(PointerEventData pointerEventData)
    {
        anim.SetBool("PomeauUp",true);
        anim.SetBool("PomeauDown",false);
        anim.SetBool("IdlePomeau",false);
    }
    
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        anim.SetBool("PomeauDown",true);
        anim.SetBool("PomeauUp",false);
        anim.SetBool("IdlePomeau",true);
    }
    
    public void ChosePomeau()
    {
        animMenuPartie.SetBool("ChosePomeau",true);
        animMenuChoices.SetBool("SpawnChoices",true);
        animMenuChoices.SetBool("BackIdle",false);
    }*/
}
