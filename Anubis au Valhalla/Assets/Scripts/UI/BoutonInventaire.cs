using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoutonInventaire : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  public void OnPointerEnter(PointerEventData pointerEventData)
  {
      transform.DOScale(1.5f, 0.2f);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
      transform.DOScale(1, 0.2f);
  }
}
