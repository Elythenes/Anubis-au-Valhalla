using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class UiManager : MonoBehaviour
{
    public GameObject anubis;
    public static UiManager instance; //singleton
    public CollectSpell cS;
    public CollectPotion cP;
    
    [Header("GENERAL")]
    public GameObject spriteSpell1;
    public GameObject spriteSpell2;
    public GameObject currentSpell1;
    public GameObject currentSpell2;
    public GameObject currentSpell1Holder;
    public GameObject currentSpell2Holder;
    
    [Header("COLLECTED SPELL MENU")]
    public GameObject menuCollectSpell;
    public GameObject spriteCs;
    public GameObject textCsName;
    public GameObject textCsDescription;
    public GameObject textCsCitation;
    
    [Header("OWNED SPELL MENU 1")]
    public GameObject spriteOs1;
    public GameObject textOs1Name;
    public GameObject textOs1Description;
    
    [Header("OWNED SPELL MENU 2")]
    public GameObject spriteOs2;
    public GameObject textOs2Name;
    public GameObject textOs2Description;

    [Header("COLLECTED POTION MENU")] 
    public GameObject menuCollectPotion;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CollectSpell(int spellSlot)
    {
        switch (spellSlot)
        {

            case 1:
                if (currentSpell1Holder is not null)
                {
                    Destroy(currentSpell1Holder);
                }
                Debug.Log("ajout dans le slot A");
                SpellManager.instance.containerA = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside;
                SpellManager.instance.prefabA = cS.collectableSpell.GetComponent<ContainScriptableObject>().prefabInside;
                currentSpell1 = cS.collectableSpell.GetComponent<ContainScriptableObject>().prefabInside;
                SpellManager.instance.isSpell1Fill = true;
                Debug.Log("sprite 1 changé");
                spriteSpell1.GetComponent<RawImage>().texture = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside.sprite;
                FillLittleMenu(cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside, 1);
                currentSpell1Holder = Instantiate(currentSpell1, anubis.transform.position,Quaternion.identity);
                currentSpell1Holder.transform.parent = anubis.transform;
                break;
            
            case 2:
                if (currentSpell2Holder is not null)
                {
                    Destroy(currentSpell2Holder);
                }
                Debug.Log("ajout dans le slot B");
                SpellManager.instance.containerB = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside;
                SpellManager.instance.prefabB = cS.collectableSpell.GetComponent<ContainScriptableObject>().prefabInside;
                currentSpell2 = cS.collectableSpell.GetComponent<ContainScriptableObject>().prefabInside;
                currentSpell2Holder = Instantiate(currentSpell2, anubis.transform.position,Quaternion.identity);
                currentSpell2Holder.transform.parent = anubis.transform;
                SpellManager.instance.isSpell2Fill = true;
                Debug.Log("sprite 2 changé");
                spriteSpell2.GetComponent<RawImage>().texture = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside.sprite;
                FillLittleMenu(cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside, 2);
                break;
        }
        menuCollectSpell.SetActive(false);
        TimeBack();
        Destroy(cS.collectableSpell);
    }

   /* public void CollectPotion()
    {
        PotionManager.instance.containerA = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside; 
        PotionManager.instance.isPotionFill = true;
    }*/

    public void DebugButton()
    {
        Debug.Log("boop");
    }

    public void ActivateMenu()
    {
        menuCollectSpell.SetActive(true);
        Time.timeScale = 0;
        FillMenu();
    }
    
    public void ActivateMenuPotion()
    {
        menuCollectPotion.SetActive(true);
        Time.timeScale = 0;
        FillMenuPotion();
    }
    
    public void TimeBack()
    {
        Time.timeScale = 1;
    }

    void FillMenu()
    {
        Debug.Log("fonction fillMenu utilisée");
        textCsName.GetComponent<TextMeshProUGUI>().text = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside.nom; 
        textCsDescription.GetComponent<TextMeshProUGUI>().text = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside.description;
        textCsCitation.GetComponent<TextMeshProUGUI>().text = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside.citation;
        spriteCs.GetComponent<RawImage>().texture = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside.sprite; 
    }
    
    void FillMenuPotion()
    {
        Debug.Log("fonction fillMenu utilisée");
        textCsName.GetComponent<TextMeshProUGUI>().text = cP.collectablePotion.GetComponent<ContainScriptableObject>().spellInside.nom; 
        textCsDescription.GetComponent<TextMeshProUGUI>().text = cP.collectablePotion.GetComponent<ContainScriptableObject>().spellInside.description;
        textCsCitation.GetComponent<TextMeshProUGUI>().text = cP.collectablePotion.GetComponent<ContainScriptableObject>().spellInside.citation;
        spriteCs.GetComponent<RawImage>().texture = cP.collectablePotion.GetComponent<ContainScriptableObject>().spellInside.sprite; 
    }

    void FillLittleMenu(SpellObject sO, int slot)
    {
        switch (slot)
        {
            case 1:
                Debug.Log("fonction fill 1 utilisée");
                textOs1Name.GetComponent<TextMeshProUGUI>().text = sO.nom;
                textOs1Description.GetComponent<TextMeshProUGUI>().text = sO.description;
                spriteOs1.GetComponent<RawImage>().texture = sO.sprite;
                break;
            case 2:
                Debug.Log("fonction fill 2 utilisée");
                textOs2Name.GetComponent<TextMeshProUGUI>().text = sO.nom;
                textOs2Description.GetComponent<TextMeshProUGUI>().text = sO.description;
                spriteOs2.GetComponent<RawImage>().texture = sO.sprite;
                break;
        }
    }
    
}
