using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMenuShop : MonoBehaviour
{
    public Animator animChoix1;
    public Animator animChoix2;
    public Animator animChoix3;
    public CanvasGroup UIParent;
    public CanvasGroup soldOutText;
    public bool fade;
    public bool[] hasMovedOut;
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] audioClipArray;

    private void Start()
    {
        MoveOutAll();
        FadeOutOther(soldOutText);
    }

    public void Update()  
    {
        if (fade)// Gérer le fade in et out de l'entièreté du menu Shop
        {
            if (UIParent.alpha > 0)
            {
                UIParent.alpha -= Time.unscaledDeltaTime; 
            }
        }
        else if(!fade)
        {
            if (UIParent.alpha < 1)
            {
                UIParent.alpha += Time.unscaledDeltaTime;    
            }
        }
    }

    public void PlayBuyShop()
    {
        audioSource.PlayOneShot(audioClipArray[0]);
    }
    
    public void PlayButtonSound()
    {
        audioSource.PlayOneShot(audioClipArray[1]);
    }

    public void MoveOutAll()
    {
        MoveOutChoix1();
        MoveOutChoix2();
        MoveOutChoix3();
    }

    public void MoveInAll()
    {
        MoveInChoix1();
        MoveInChoix2();
        MoveInChoix3();
    }
    public void MoveInChoix1()
    {
        if (hasMovedOut[0]) return;
        animChoix1.SetBool("MoveOut",false);
        animChoix1.SetBool("MoveIn",true);
    }
    
    public void MoveInChoix2()
    {
        if (hasMovedOut[1]) return;
        animChoix2.SetBool("MoveOut",false);
        animChoix2.SetBool("MoveIn",true);
    }
    
    public void MoveInChoix3()
    {
        if (hasMovedOut[2]) return;
        animChoix3.SetBool("MoveOut",false);
        animChoix3.SetBool("MoveIn",true);
    }
    public void MoveOutChoix1()
    {
        animChoix1.SetBool("MoveOut",true);
        animChoix1.SetBool("MoveIn",false);
    }
    
    public void MoveOutChoix2()
    {
        animChoix2.SetBool("MoveOut",true);
        animChoix2.SetBool("MoveIn",false);
    }
    
    public void MoveOutChoix3()
    {
        animChoix3.SetBool("MoveOut",true);
        animChoix3.SetBool("MoveIn",false);
    }
    
    
    public void FadeOut()
    {
        fade = true;
        UIParent.interactable = false;
        UIParent.blocksRaycasts= false;

    }
    
    public void FadeIn()
    {
        fade = false;
        UIParent.interactable = true;
        UIParent.blocksRaycasts = true;
    }

    public void FadeOutOther(CanvasGroup otherObject)
    {
        otherObject.alpha = 0;
        otherObject.interactable = false;
        otherObject.blocksRaycasts = false;
    }

    public void FadeInOther(CanvasGroup otherObject)
    {
        otherObject.alpha = 1;
        otherObject.interactable = true;
        otherObject.blocksRaycasts = true;
    }
    
}