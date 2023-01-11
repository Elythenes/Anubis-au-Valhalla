using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Image = UnityEngine.UIElements.Image;

public class UiManager : MonoBehaviour
{
    public GameObject anubis;

    public static UiManager instance; //singleton

    //public CollectSpell cS;
    public CollectPower cP;
    public bool isSousMenu;

    [Foldout("GENERAL")] public GameObject spriteSpell1;
    [Foldout("GENERAL")] public GameObject spriteSpell2;
    [Foldout("GENERAL")] public GameObject currentSpell1Holder;
    [Foldout("GENERAL")] public GameObject currentSpell2Holder;

    [Foldout("POTION MENU")] public GameObject menuCollectPotion;
    [Foldout("POTION MENU")] public GameObject spritePotion;
    [Foldout("POTION MENU")] public GameObject panelPotion;
    [Foldout("POTION MENU")] public GameObject currentPotionSprite;
    [Foldout("POTION MENU")] public GameObject currentPotionName;
    [Foldout("POTION MENU")] public GameObject currentPotionDescription;
    [Foldout("POTION MENU")] public GameObject currentPotionCitation;
    [Foldout("POTION MENU")] public GameObject newPotionSprite;
    [Foldout("POTION MENU")] public GameObject newPotionName;
    [Foldout("POTION MENU")] public GameObject newPotionDescription;
    [Foldout("POTION MENU")] public GameObject newPotionCitation;
    [Foldout("POTION MENU")] public GameObject buttonSwitch;
    [Foldout("POTION MENU")] public GameObject buttonPrendre;

    [Foldout("PAUSE MENU")] public GameObject menuPause;
    [Foldout("PAUSE MENU")] public KeyCode buttonPause;
    [Foldout("PAUSE MENU")] public bool isPause;
    [Foldout("PAUSE MENU")] public GameObject buttonResume;
    [Foldout("PAUSE MENU")] public GameObject buttonRerun;
    [Foldout("PAUSE MENU")] public GameObject buttonReturnToHub;
    [Foldout("PAUSE MENU")] public GameObject buttonCheatMenu;
    [Foldout("PAUSE MENU")] public GameObject buttonOptions;
    [Foldout("PAUSE MENU")] public GameObject buttonQuit;

    [Foldout("INVENTORY")] public List<GameObject> listBoxInventaire;
    [Foldout("INVENTORY")] public GameObject boxGlyphTitre;
    [Foldout("INVENTORY")] public GameObject boxGlyphTexte;
    [Foldout("INVENTORY")] public GameObject boxGlyphImage;
    [Foldout("INVENTORY")] public GameObject boxPowerTitre;
    [Foldout("INVENTORY")] public GameObject boxPowerGif;
    [Foldout("INVENTORY")] public List<GameObject> boxPowerTextesNiveaux;
    [Foldout("INVENTORY")] public List<GameObject> listBoxPowerType = new(3);
    
    [Header("Audio")]
    public AudioSource audioSource;



    //Fonctions systèmes ***********************************************************************************************

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        spriteSpell1.GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
        spriteSpell2.GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
        spritePotion.GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
    }

    private void Start()
    {

    }

    private void Update()
    {
        ControlPause();
    }


    public void DebugButton()
    {
        Debug.Log("boop");
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void TimeBack()
    {
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }



    //Fonctions : Pause ************************************************************************************************************************************************************************************

    public void ControlPause()
    {
        if (Input.GetKeyDown(buttonPause))
        {
            if (isPause && !isSousMenu) //quand on "Echap" depuis le menu Pause
            {
                DeActivatePause();
            }
            else if(!isSousMenu)//quand on pause avec "Echap"
            {
                ActivatePause();
            }
        }
    }

    public void DeActivatePause()
    {
        TimeBack();
        menuPause.SetActive(false);
        isPause = false;
    }

    public void ActivatePause()
    {
        Pause();
        menuPause.SetActive((true));
        FillBoxInventoryForGlyphs();
        isPause = true;
    }




    //Fonctions : Inventaire ************************************************************************************************************************************************************************************

    public void FillBoxInventoryForGlyphs() //met les icônes des glyphes de l'inventaire système dans le menu inventaire
    {
        for (int i = 0; i < GlyphInventory.Instance.glyphInventory.Count; i++)
        {
            listBoxInventaire[i].GetComponent<RawImage>().texture = GlyphInventory.Instance.glyphInventory[i].icone;
            listBoxInventaire[i + GlyphInventory.Instance.glyphInventory.Count].GetComponent<Button>().enabled = true;
        }

        if (GlyphInventory.Instance.glyphInventory.Count < listBoxInventaire.Count)
        {
            int difference = listBoxInventaire.Count - GlyphInventory.Instance.glyphInventory.Count;
            for (int i = 0; i < difference; i++)
            {
                listBoxInventaire[i + GlyphInventory.Instance.glyphInventory.Count].GetComponent<Button>().enabled = false;
            }
        }
    }

    public void FillDescriptionInventory(int boxPos) //change le titre et la description dans les box à droite du livre
    {
        Debug.Log(boxPos);
        boxGlyphTitre.GetComponent<TextMeshProUGUI>().text = GlyphInventory.Instance.glyphInventory[boxPos - 1].nom;
        boxGlyphTexte.GetComponent<TextMeshProUGUI>().text = GlyphInventory.Instance.glyphInventory[boxPos - 1].description;
        boxGlyphImage.GetComponent<RawImage>().texture = GlyphInventory.Instance.glyphInventory[boxPos - 1].icone;
    }

    public void FillDescriptionPowers()
    {
        
    }

    void DisablePageDroite(string page)
    {
        switch (page)
        {
            case "glyph":
                
                break;
            
            case "power":
                break;
            
            case "potion":
                break;
            
            default:
                Debug.Log("NON LA PAGE DROITE MARCHE PAS");
                break;
        }
    }

    /*public void SetBoxInventoryPositions()
    {
        for (int i = 0; i < GlyphInventory.Instance.glyphInventory.Count; i++)
        {
            listBoxInventaire[i].GetComponent<BoxInventory>().inventoryPosition = i;
        }
    }*/

    //Fonctions : Pouvoirs ***********************************************************************************************************************************************************************************

    public void CollectPower(GameObject gb)
    {
        NewPowerManager.Instance.powersCollected.Add(gb);
        NewPowerManager.Instance.PowerLevelUp(gb);
    }




    //Fonctions : Potions ***********************************************************************************************************************************************************************************

    public void CollectPotion()
    {
        //Debug.Log("entrée dans la fonction CollectPotion");
        PotionManager.Instance.currentPotion = PotionRepository.Instance.potionInside;
        PotionManager.Instance.isPotionSlotFill = true;
        spritePotion.GetComponent<RawImage>().texture = PotionManager.Instance.currentPotion.sprite;
        spritePotion.GetComponent<RawImage>().color = new Color(255, 255, 255, 1);
        menuCollectPotion.SetActive(false);
        TimeBack();
    }

    public void CollectPotionDebug()
    {
        PotionManager.Instance.isPotionSlotFill = true;
        spritePotion.GetComponent<RawImage>().texture = PotionManager.Instance.currentPotion.sprite;
        spritePotion.GetComponent<RawImage>().color = new Color(255, 255, 255, 1);
    }


    public void ActivateMenuPotion()
    {
        Pause();
        menuCollectPotion.SetActive(true);
        FillMenuPotion();
    }

    void FillMenuPotion()
    {
        if (PotionManager.Instance.currentPotion == null)
        {
            buttonPrendre.SetActive(true);
            buttonSwitch.SetActive(false);

            currentPotionName.SetActive(false);
            currentPotionDescription.GetComponent<TextMeshProUGUI>().text = "Je ne possède pas de potion actuellement.";
            currentPotionCitation.SetActive(false);
            currentPotionSprite.SetActive(false);

            newPotionName.GetComponent<TextMeshProUGUI>().text = PotionRepository.Instance.potionInside.nom;
            newPotionDescription.GetComponent<TextMeshProUGUI>().text =
                PotionRepository.Instance.potionInside.description;
            newPotionCitation.GetComponent<TextMeshProUGUI>().text = PotionRepository.Instance.potionInside.citation;
            newPotionSprite.GetComponent<RawImage>().texture = PotionRepository.Instance.potionInside.sprite;
        }
        else
        {
            buttonPrendre.SetActive(false);
            buttonSwitch.SetActive(true);

            currentPotionName.GetComponent<TextMeshProUGUI>().text = PotionManager.Instance.currentPotion.nom;
            currentPotionDescription.GetComponent<TextMeshProUGUI>().text =
                PotionManager.Instance.currentPotion.description;
            currentPotionCitation.GetComponent<TextMeshProUGUI>().text = PotionManager.Instance.currentPotion.citation;
            currentPotionSprite.GetComponent<RawImage>().texture = PotionManager.Instance.currentPotion.sprite;

            newPotionName.GetComponent<TextMeshProUGUI>().text = PotionRepository.Instance.potionInside.nom;
            newPotionDescription.GetComponent<TextMeshProUGUI>().text =
                PotionRepository.Instance.potionInside.description;
            newPotionCitation.GetComponent<TextMeshProUGUI>().text = PotionRepository.Instance.potionInside.citation;
            newPotionSprite.GetComponent<RawImage>().texture = PotionRepository.Instance.potionInside.sprite;
        }
        
    }
       
    //Fonctions : Son ***********************************************************************************************************************************************************************************
    
    public void PlayButtonSound()
    {
        audioSource.Play();
    }

    //Fonctions : MenuPause et Option ***********************************************************************************************************************************************************************************

    public void ExitSousMenu()
    {
        isSousMenu = false;
    }
    
    public void EnterSousMenu()
    {
        isSousMenu = true;
    }


    //Fonctions autres ***********************************************************************************************************************************************************************************


    /*void FillLittleMenu(SpellObject sO, int slot)
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
    }*/

    




    //Fonctions : Spells ************************************************************************************************************************************************************************************

    /*public void CollectSpell(int spellSlot)
    {
        switch (spellSlot)
        {

            case 1:
                if (currentSpell1Holder is not null)
                {
                    Destroy(currentSpell1Holder.gameObject);
                }
                Debug.Log("ajout dans le slot A");
                SpellManager.instance.containerA = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside;
                SpellManager.instance.prefabA = cS.collectableSpell.GetComponent<ContainScriptableObject>().prefabInside;
                currentSpell1 = cS.collectableSpell.GetComponent<ContainScriptableObject>().prefabInside;
                SpellManager.instance.isSpell1Fill = true;
                Debug.Log("sprite 1 changé");
                spriteSpell1.GetComponent<RawImage>().texture = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside.sprite;
                spriteSpell1.GetComponent<RawImage>().color = new Color(255,255,255,1);
                FillLittleMenu(cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside, 1);
                currentSpell1Holder = Instantiate(currentSpell1, anubis.transform.position,Quaternion.identity);
                currentSpell1Holder.transform.parent = anubis.transform;
                break;
            
            case 2:
                if (currentSpell2Holder is not null)
                {
                    Destroy(currentSpell2Holder.gameObject);
                }
                Debug.Log("ajout dans le slot B");
                SpellManager.instance.containerB = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside;
                SpellManager.instance.prefabB = cS.collectableSpell.GetComponent<ContainScriptableObject>().prefabInside;
                currentSpell2 = cS.collectableSpell.GetComponent<ContainScriptableObject>().prefabInside;
                SpellManager.instance.isSpell2Fill = true;
                Debug.Log("sprite 2 changé");
                spriteSpell2.GetComponent<RawImage>().texture = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside.sprite;
                spriteSpell2.GetComponent<RawImage>().color = new Color(255,255,255,1);
                FillLittleMenu(cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside, 2);
                currentSpell2Holder = Instantiate(currentSpell2, anubis.transform.position,Quaternion.identity);
                currentSpell2Holder.transform.parent = anubis.transform;
                break;
        }
        menuCollectSpell.SetActive(false);
        TimeBack();
        Destroy(cS.collectableSpell);
    }
    
    public void ActivateMenu()
    {
        menuCollectSpell.SetActive(true);
        Pause();
        FillMenu();
    }
    
    void FillMenu()
    {
        Debug.Log("fonction fillMenu utilisée");
        textCsName.GetComponent<TextMeshProUGUI>().text = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside.nom; 
        textCsDescription.GetComponent<TextMeshProUGUI>().text = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside.description;
        textCsCitation.GetComponent<TextMeshProUGUI>().text = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside.citation;
        spriteCs.GetComponent<RawImage>().texture = cS.collectableSpell.GetComponent<ContainScriptableObject>().spellInside.sprite; 
    }*/
    
    
    
    
}