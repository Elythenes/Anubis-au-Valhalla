using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMenuShop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator anim;
    public Animator animChosePartMenu;
    public Animator animChoseUpgradeMenu;
    public CanvasGroup choseMenuMenu;
    public CanvasGroup choseUpgradeMenu;
    public bool fade;
    public bool fade2;
    
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
       anim.SetBool("SelectUpgrade",true);
       anim.SetBool("Idle",false);
    }
    
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        anim.SetBool("SelectUpgrade",false);
        anim.SetBool("Idle",true);
    }

    public void Update()  
    {
        if (fade)// Gérer le fade in et out du menu de de choix (upgrade - consombales)
        {
            if (choseMenuMenu.alpha > 0)
            {
                choseMenuMenu.alpha -= Time.deltaTime; 
            }
        }
        else if(!fade)
        {
            if (choseMenuMenu.alpha < 1)
            {
                choseMenuMenu.alpha += Time.deltaTime;    
            }
        }
        
        if (fade2) // Gérer le fade in et out du menu de de choix des upgrades
        {
            if (choseUpgradeMenu.alpha > 0)
            {
                choseUpgradeMenu.alpha -= Time.deltaTime; 
            }
        }
        else if(!fade2)
        {
            if (choseUpgradeMenu.alpha < 1)
            {
                choseUpgradeMenu.alpha += Time.deltaTime;    
            }
        }
    }

    public void ResetAllAnimatorMenu()
    {
        fade2 = false;
        animChoseUpgradeMenu.SetBool("BackIdle",true);
        animChoseUpgradeMenu.SetBool("SpawnChoices",false);
        animChosePartMenu.SetBool("ChoseLame",false);
        animChosePartMenu.SetBool("ChoseHampe",false);
        animChosePartMenu.SetBool("ChosePomeau",false);
    }
    public void FadeOut()
    {
        fade = true;
        choseMenuMenu.interactable = false;
        choseMenuMenu.blocksRaycasts= false;

    }
    
    public void FadeIn()
    {
        fade = false;
        choseMenuMenu.interactable = true;
        choseMenuMenu.blocksRaycasts = true;
    }
    
    public void FadeOutChoseUpgrade()
    {
        fade2 = true;
        choseUpgradeMenu.interactable = false;
        choseUpgradeMenu.blocksRaycasts= false;
    }
    
    public void FadeInChoseUpgrade()
    {
        fade2 = false;
        choseUpgradeMenu.interactable = true;
        choseUpgradeMenu.blocksRaycasts= true;
    }
}