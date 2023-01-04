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
    public void OnEnable()
    {
        StartCoroutine(FadeOut());
        Debug.Log("oui");
        alpha.alpha = 1;
        transform.localPosition = startPos;
        transform.localScale = Vector3.one;
    }

    IEnumerator FadeOut()
    {

        yield return new WaitForSeconds(textDuration);
        Debug.Log("pourquoi");
        alpha.LeanAlpha(0, 1).setEaseInCubic();
        transform.LeanMoveLocal(endPos,1).setEaseInSine();
        transform.LeanScale(new Vector3(0.9f, 0.9f, 0.9f), 0.5f);
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        //title.gameObject.SetActive(false);
    }
}
