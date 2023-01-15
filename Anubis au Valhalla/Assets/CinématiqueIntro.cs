using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CinématiqueIntro : MonoBehaviour
{
    private RectTransform textTransform;
    public CanvasGroup canvas;
    public CanvasGroup canvas2;
    public Slider sliderPasser;
    public float alphaGained;
    public float passerGained;
    public float vitesseText;
    public float cinematiqueDuration;
    public bool EndCinematique;

    private void Start()
    {
        textTransform = GetComponent<RectTransform>();
        StartCoroutine(TimerCinematique());
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            sliderPasser.value += passerGained;
        }
        else if(!EndCinematique)
        {
            sliderPasser.value -= passerGained*2;
        }
        
        textTransform.anchoredPosition += new Vector2(0, vitesseText);

        if (!EndCinematique)
        {
            if (canvas2.alpha < 1)
            {
                canvas2.alpha += alphaGained;
            }
            if (canvas.alpha < 1)
            {
                canvas.alpha += alphaGained;
            }
        }
        else
        {
            canvas.alpha -= alphaGained;
            StartCoroutine(Scene());
        }

        if ((int)sliderPasser.value == (int)sliderPasser.maxValue)
        {
            EndCinematique = true;
            sliderPasser.value = sliderPasser.maxValue;
        }
    }

    IEnumerator Scene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Hub");
    }
    
    IEnumerator TimerCinematique()
    {
        yield return new WaitForSeconds(cinematiqueDuration);
        SceneManager.LoadScene("Hub");
    }
    
    
}
