using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UINewGlyphManager : MonoBehaviour
{
    public CanvasGroup myCanvas;
    public float alphaGained;
    public float textDuration;
    public TextMeshProUGUI mytext;
    public bool fadeIn;
    public static UINewGlyphManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (fadeIn)
        {
            if (myCanvas.alpha < 1)
            {
                myCanvas.alpha += alphaGained;
            }
        }
        else
        {
            if (myCanvas.alpha > 0)
            {
                myCanvas.alpha -= alphaGained;
            }
        }
    }

    public void NewGlyph()
    {
        StartCoroutine(ActivateDesactivate());
    }


    IEnumerator ActivateDesactivate()
    {
        myCanvas.alpha = 0;
        fadeIn = true;
        yield return new WaitForSeconds(textDuration);
        fadeIn = false;
    }
}
