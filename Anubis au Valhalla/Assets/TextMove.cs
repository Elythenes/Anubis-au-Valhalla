using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class TextMove : MonoBehaviour
{
    public static TextMove instance; 
    public Vector2 startPos;
    public Vector2 endPos;

    public CanvasGroup alpha;

    public float textDuration;
    public TextMove script;

    public TextMeshProUGUI description;

    public TextMeshProUGUI title;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        script.enabled = false;
    }
    public void OnEnable()
    {
        title.GetComponent<TextMove2>().enabled = true;
        description.enabled = true;
        title.enabled = true;
        alpha.alpha = 1;
        transform.localPosition = startPos;
        transform.localScale = Vector3.one;
        transform.DOScale(Vector3.one, textDuration).OnComplete(() =>
        {
            Debug.Log("nan mais allo quoi");
            alpha.LeanAlpha(0, 1).setEaseInCubic();
            transform.LeanMoveLocal(endPos,1).setEaseInSine();
            transform.LeanScale(new Vector3(0.9f, 0.9f, 0.9f), 0.5f);
            
        });
    }



    

    private void OnDisable()
    {
        Debug.Log("AHHHHHHHHHH");
        title.GetComponent<TextMove2>().FadeOut();
    }
}
