using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TextMove2 : MonoBehaviour
{
    public Vector2 startPos;
    public Vector2 endPos;

    public CanvasGroup alpha;
    // Start is called before the first frame update
    void OnEnable()
    {
        alpha.alpha = 1;
        transform.localPosition = startPos;
        transform.localScale = Vector3.one;
    }

    // Update is called once per frame
    public void FadeOut()
    {
        alpha.LeanAlpha(0, 1).setEaseInCubic();
        transform.LeanMoveLocal(endPos,1).setEaseInOutBack();
        transform.LeanScale(new Vector3(0.9f, 0.9f, 0.9f), 0.5f);
        transform.DOShakePosition(1.5f, 0).OnComplete((() =>
        {
            gameObject.GetComponent<TextMove2>().enabled = false;
        }));
    }
}
