using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMenuShop : MonoBehaviour
{
    public Animator animChosePartMenu;
    public Animator animChoseUpgradeMenu;
    public Animator animChoix1;
    public Animator animChoix2;
    public Animator animChoix3;
    public CanvasGroup UIParent;
    public bool fade;
   
    
    public void Update()  
    {
        if (fade)// Gérer le fade in et out de l'entièreté du menu Shop
        {
            if (UIParent.alpha > 0)
            {
                UIParent.alpha -= Time.deltaTime; 
            }
        }
        else if(!fade)
        {
            if (UIParent.alpha < 1)
            {
                UIParent.alpha += Time.deltaTime;    
            }
        }
    }

    public void ResetAllAnimatorMenu()
    {
        animChoseUpgradeMenu.SetBool("BackIdle",true);
        animChoseUpgradeMenu.SetBool("SpawnChoices",false);
        animChosePartMenu.SetBool("ChoseLame",false);
        animChosePartMenu.SetBool("ChoseHampe",false);
        animChosePartMenu.SetBool("ChosePomeau",false);
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
    
}