using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextZone : MonoBehaviour
{
    public CanvasGroup canvasgroup;
    public float timeToFadeOut;

    private void Start()
    {
        StartCoroutine(WaitFadeOut());
    }

  

    IEnumerator WaitFadeOut()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(FadeOut());
    }
    IEnumerator WaitDestroy()
    {
        yield return new WaitForSeconds(timeToFadeOut);
        Destroy(gameObject);
    }

    IEnumerator FadeOut()
    {
        float timeElapsed = 0;
        while (timeElapsed < timeToFadeOut)
        {
            canvasgroup.alpha = Mathf.Lerp(1, 0, timeElapsed / timeToFadeOut);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        canvasgroup.alpha = 0;
        StartCoroutine(WaitDestroy());
    }
}
