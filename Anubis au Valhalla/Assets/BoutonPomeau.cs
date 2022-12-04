using UnityEngine;
using UnityEngine.EventSystems;

public class BoutonPomeau : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator anim;
    public Animator animMenuPartie;
    
    public void OnPointerEnter(PointerEventData pointerEventData)
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
    }
}
