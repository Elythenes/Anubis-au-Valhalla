using UnityEngine;
using UnityEngine.EventSystems;

public class BoutonConsomables : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator anim;
    
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
}
