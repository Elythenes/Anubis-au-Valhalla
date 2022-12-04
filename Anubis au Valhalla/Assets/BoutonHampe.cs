using UnityEngine;
using UnityEngine.EventSystems;


public class BoutonHampe : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator anim;

    public void OnPointerEnter(PointerEventData pointerEventData)
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
}
