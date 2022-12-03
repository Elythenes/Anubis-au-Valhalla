using UnityEngine;
using UnityEngine.EventSystems;

public class UIMenuShop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator anim;
    
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
       anim.SetBool("SelectUpgrade",true);
       anim.SetBool("Idle",false);
    }
    
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        anim.SetBool("SelectUpgrade",false);
        anim.SetBool("Idle",true);
    }
}