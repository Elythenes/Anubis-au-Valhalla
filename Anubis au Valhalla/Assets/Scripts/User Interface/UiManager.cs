using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance; //singleton
    public CollectSpell cS;
    
    [Header("GENERAL")]
    public GameObject spriteSpell1;
    public GameObject spriteSpell2;
    
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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void CollectSpell(int spellSlot)
    {
        switch (spellSlot)
        {
            case 1:
                Debug.Log("ajout dans le slot A");
                SpellManager.instance.containerA = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside;
                SpellManager.instance.prefabA = cS.collectableSpell.GetComponent<ContainScriptableObject>().prefabInside;
                SpellManager.instance.isSpell1Fill = true;
                Debug.Log("sprite 1 changé");
                spriteSpell1.GetComponent<RawImage>().texture = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside.sprite;
                FillLittleMenu(cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside, 1);
                break;
            
            case 2:
                Debug.Log("ajout dans le slot B");
                SpellManager.instance.containerB = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside;
                SpellManager.instance.prefabB = cS.collectableSpell.GetComponent<ContainScriptableObject>().prefabInside;
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
    
    public void TimeBack()
    {
        Time.timeScale = 1;
    }

    void FillMenu()
    {
        Debug.Log("fonction fillMenu utilisée");
        textCsName.GetComponent<TextMeshProUGUI>().text = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside.nom; //putain d'UGUI de MERDE
        textCsDescription.GetComponent<TextMeshProUGUI>().text = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside.description;
        textCsCitation.GetComponent<TextMeshProUGUI>().text = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside.citation;
        spriteCs.GetComponent<RawImage>().texture = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside.sprite; 
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