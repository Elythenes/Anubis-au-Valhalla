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
    public Vector2 titleStartPos;
    public Vector2 titleEndPos;
    public Vector2 descStartPos;
    public Vector2 descEndPos;

    public CanvasGroup titleAlpha;
    public CanvasGroup descAlpha;

    public float textDuration;

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

    }

    public void Appear(CanvasGroup alpha, Vector2 start, TextMeshProUGUI text)
    {
        alpha.alpha = 1;
        text.transform.localPosition = start;
        text.transform.localScale = Vector3.one;
        text.enabled = true;
    }
    public void FadeOut(CanvasGroup alpha, Vector2 end, TextMeshProUGUI text)
    {
        alpha.LeanAlpha(0, 1).setEaseInCubic();
        text.transform.LeanMoveLocal(end,1).setEaseInSine();
        text.transform.LeanScale(new Vector3(0.9f, 0.9f, 0.9f), 0.5f);
        text.enabled = false;
    }
}
