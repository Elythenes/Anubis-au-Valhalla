using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BoutonLame : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator anim;

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
}
