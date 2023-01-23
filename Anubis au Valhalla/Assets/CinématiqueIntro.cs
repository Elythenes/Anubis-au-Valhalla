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
    public float disolveValue;
    public Image backgroundRenderer;

    private void Start()
    {
        textTransform = GetComponent<RectTransform>();
        StartCoroutine(TimerCinematique());
    }

    void FixedUpdate()
    {
        disolveValue += 0.006f;
        backgroundRenderer.material.SetFloat("_Step", disolveValue);
        
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
        yield return new WaitForSeconds(1f);
        SoundManager.instance.ChangeToHub();
        SoundManager.instance.PlayHub();
        SceneManager.LoadScene("Hub");
    }
    
    IEnumerator TimerCinematique()
    {
        yield return new WaitForSeconds(cinematiqueDuration);
        SoundManager.instance.ChangeToHub();
        SoundManager.instance.PlayHub();
        SceneManager.LoadScene("Hub");
    }
    
    
}
