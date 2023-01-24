
using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class UiManager : MonoBehaviour
{
    public GameObject anubis;

    public static UiManager instance; //singleton
    public EventSystem eventSystem;
    public Button boutonSmash;
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
    [Foldout("POTION MENU")] public GameObject potionChoiceArrow;
    [Foldout("POTION MENU")] public float rotaZ;

    [Foldout("PAUSE MENU")] public GameObject menuPause;
    [Foldout("PAUSE MENU")] public KeyCode buttonPause;
    [Foldout("PAUSE MENU")] public bool isPause;

    [Foldout("INVENTORY")] public List<GameObject> listBoxInventaire;
    [Foldout("INVENTORY")] public List<GameObject> listBoxInventaireIcone;
    [Foldout("INVENTORY")] public GameObject boxGlyphTitre;
    [Foldout("INVENTORY")] public GameObject boxGlyphTexte;
    [Foldout("INVENTORY")] public GameObject boxGlyphImage;
    [Foldout("INVENTORY")] public Image boxGlyphCadre;
    [Foldout("INVENTORY")] public Color colorPretre;
    [Foldout("INVENTORY")] public Sprite cadrePretre;
    [Foldout("INVENTORY")] public Color colorPharaon;
    [Foldout("INVENTORY")] public Sprite cadrePharaon;
    [Foldout("INVENTORY")] public Color colorDivinite;
    [Foldout("INVENTORY")] public Sprite cadreDivinite;
    [Foldout("INVENTORY")] public Color colorMinor;
    [Foldout("INVENTORY")] public Sprite cadreMinor;
    [Foldout("INVENTORY")] public Color colorUnique;
    [Foldout("INVENTORY")] public Sprite cadreUnique;
    [Foldout("INVENTORY")] public Color colorRien;

    [Foldout("INVENTORY")] public GameObject boxPotionTitre;
    [Foldout("INVENTORY")] public GameObject boxPotionTexte;
    [Foldout("INVENTORY")] public GameObject boxPotionImage;
    [Foldout("INVENTORY")] public Sprite boxPotionVide;

    [Foldout("INVENTORY")] public GameObject globalBoxGlyph;
    [Foldout("INVENTORY")] public GameObject globalBoxPotion;
    [Foldout("INVENTORY")] public Image globalBoxPotionImage;
    [Foldout("INVENTORY")] public GameObject globalBoxPowers;
    [Foldout("INVENTORY")] public TextMeshProUGUI globalBoxTextRarity;
    
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clipDialoguesUp;



    //Fonctions systèmes ***********************************************************************************************

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        //spriteSpell1.GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
        //spriteSpell2.GetComponent<RawImage>().color = new Color(255, 255, 255, 0);

        
    }

    private void Start()
    {
        if (!PotionManager.Instance.isPotionSlotFill)
        {
            spritePotion.GetComponent<Image>().color = new Color(255, 255, 255, 0);
            boxPotionImage.GetComponent<Image>().color = new Color(255, 255, 255, 255);
            boxPotionImage.GetComponent<Image>().sprite = boxPotionVide;
        }
        globalBoxGlyph.SetActive(false);
        globalBoxPotion.SetActive(false);
        globalBoxPowers.SetActive(false);

        foreach (var boxCase in listBoxInventaireIcone)
        {
            boxCase.GetComponent<Image>().color = new Color(0,0,0,0);
        }
        
        
    }

    private void Update()
    {
        ControlPause();
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
        SelectSmashButton();
        DisablePageDroite(default);
        Pause();
        menuPause.SetActive((true));
        
        FillBoxInventoryForGlyphs();
        if (PotionManager.Instance.currentPotion is not null)
        {
            boxPotionImage.GetComponent<Image>().sprite = PotionManager.Instance.currentPotion.sprite;
        }
        isPause = true;
    }




    //Fonctions : Inventaire ************************************************************************************************************************************************************************************

    public void FillBoxInventoryForGlyphs() //met les icônes des glyphes de l'inventaire système dans le menu inventaire
    {
        for (int i = 0; i < GlyphInventory.Instance.glyphInventory.Count; i++)
        {
            //listBoxInventaire[i].GetComponent<RawImage>().texture = GlyphInventory.Instance.glyphInventory[i].icone;
            //boxGlyphIcones[i].GetComponent<RawImage>().texture = GlyphInventory.Instance.glyphInventory[i].icone;
            //Texture2D texture = GlyphInventory.Instance.glyphInventory[i].icone;
            listBoxInventaireIcone[i].GetComponent<Image>().sprite = GlyphInventory.Instance.glyphInventory[i].icone;
            listBoxInventaireIcone[i].GetComponent<Image>().color = new Color(255,255,255,255);
            listBoxInventaire[i].GetComponent<Button>().enabled = true;
            switch (GlyphInventory.Instance.glyphInventory[i].rare)
            {
                case Rarity.Prêtre:
                    if (GlyphInventory.Instance.glyphInventory[i].price == 0)
                    {
                        listBoxInventaire[i].GetComponent<Image>().sprite = cadreMinor;
                        listBoxInventaire[i].GetComponent<Image>().color = colorMinor;
                        Debug.Log("is minor");
                    }
                    else
                    {
                        listBoxInventaire[i].GetComponent<Image>().sprite = cadrePretre;
                        listBoxInventaire[i].GetComponent<Image>().color = colorPretre;
                        Debug.Log("is prêtre");
                    }
                    break;
                case Rarity.Pharaon:
                    listBoxInventaire[i].GetComponent<Image>().sprite = cadrePharaon;
                    listBoxInventaire[i].GetComponent<Image>().color = colorPharaon;
                    Debug.Log("is pharaon");
                    break;
                case Rarity.Divinité:
                    listBoxInventaire[i].GetComponent<Image>().sprite = cadreDivinite;
                    listBoxInventaire[i].GetComponent<Image>().color = colorDivinite;
                    Debug.Log("is divinité");
                    break;
                case Rarity.Unique:
                    listBoxInventaire[i].GetComponent<Image>().sprite = cadreUnique;
                    listBoxInventaire[i].GetComponent<Image>().color = colorUnique;
                    Debug.Log("is unique");
                    break;
                default:
                    Debug.Log("NON");
                    break;
            }
        }

        if (GlyphInventory.Instance.glyphInventory.Count < listBoxInventaire.Count)
        {
            int difference = listBoxInventaire.Count - GlyphInventory.Instance.glyphInventory.Count;
            for (int i = 0; i < difference; i++)
            {
                listBoxInventaire[i + GlyphInventory.Instance.glyphInventory.Count].GetComponent<Button>().enabled = false;
                listBoxInventaire[i + GlyphInventory.Instance.glyphInventory.Count].GetComponent<Image>().sprite = cadrePretre;
                listBoxInventaire[i + GlyphInventory.Instance.glyphInventory.Count].GetComponent<Image>().color = colorRien;
                //listBoxInventaireIcone[i + GlyphInventory.Instance.glyphInventory.Count].GetComponent<Image>().color = new Color(0,0,0,0);
            }
        }
        
    }

    public void FillDescriptionInventory(int boxPos) //change le titre et la description dans les box à droite du livre
    {
        Debug.Log(boxPos);
        boxGlyphTitre.GetComponent<TextMeshProUGUI>().text = GlyphInventory.Instance.glyphInventory[boxPos - 1].nom;
        boxGlyphTexte.GetComponent<TextMeshProUGUI>().text = GlyphInventory.Instance.glyphInventory[boxPos - 1].description;
        boxGlyphImage.GetComponent<Image>().sprite = GlyphInventory.Instance.glyphInventory[boxPos - 1].icone;
        switch (GlyphInventory.Instance.glyphInventory[boxPos - 1].rare)
        {
            case Rarity.Prêtre:
                if (GlyphInventory.Instance.glyphInventory[boxPos - 1].price == 0)
                {
                    globalBoxTextRarity.SetText("Mineure");
                    globalBoxTextRarity.color = colorMinor;
                    boxGlyphCadre.sprite = cadreMinor;
                    boxGlyphCadre.color = colorMinor;
                }
                else
                {
                    globalBoxTextRarity.SetText("Prêtre");
                    globalBoxTextRarity.color = colorPretre;
                    boxGlyphCadre.sprite = cadrePretre;
                    boxGlyphCadre.color = colorPretre;
                }
                break;
            case Rarity.Pharaon:
                globalBoxTextRarity.SetText("Pharaon");
                globalBoxTextRarity.color = colorPharaon;
                boxGlyphCadre.sprite = cadrePharaon;
                boxGlyphCadre.color = colorPharaon;
                break;
            case Rarity.Divinité:
                globalBoxTextRarity.SetText("Divinité");
                globalBoxTextRarity.color = colorDivinite;
                boxGlyphCadre.sprite = cadreDivinite;
                boxGlyphCadre.color = colorDivinite;
                break;
            case Rarity.Unique:
                globalBoxTextRarity.SetText("Unique");
                globalBoxTextRarity.color = colorUnique;
                boxGlyphCadre.sprite = cadreUnique;
                boxGlyphCadre.color = colorUnique;
                break;
            default:
                Debug.Log("NON2");
                break;
        }
    }

    public void FillDescriptionPowers()
    {
        
    }

    public void DisablePageDroite(string page)
    {
        Debug.Log("oui");
        switch (page)
        {
            case "glyph":
                globalBoxGlyph.SetActive(true);
                globalBoxPotion.SetActive(false);
                globalBoxPowers.SetActive(false);
                Debug.Log("box glyph");
                break;
            
            case "potion":
                globalBoxGlyph.SetActive(false);
                globalBoxPotion.SetActive(true);
                globalBoxPowers.SetActive(false);
                Debug.Log("box potion");
                break;
            
            case "powers":
                globalBoxGlyph.SetActive(false);
                globalBoxPotion.SetActive(false);
                globalBoxPowers.SetActive(true);
                Debug.Log("box powers");
                break;
            
            default:
                globalBoxGlyph.SetActive(false);
                globalBoxPotion.SetActive(false);
                globalBoxPowers.SetActive(false);
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
        spritePotion.GetComponent<Image>().sprite = PotionManager.Instance.currentPotion.sprite;
        spritePotion.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        menuCollectPotion.SetActive(false);
        
        boxPotionImage.GetComponent<Image>().sprite = PotionManager.Instance.currentPotion.sprite;
        boxPotionImage.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        
        TimeBack();
    }

    public void CollectPotionDebug()
    {
        PotionManager.Instance.isPotionSlotFill = true;
        spritePotion.GetComponent<Image>().sprite = PotionManager.Instance.currentPotion.sprite;
        spritePotion.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        
        boxPotionImage.GetComponent<Image>().sprite = PotionManager.Instance.currentPotion.sprite;
        boxPotionImage.GetComponent<Image>().color = new Color(255, 255, 255, 255);
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
            newPotionDescription.GetComponent<TextMeshProUGUI>().text = PotionRepository.Instance.potionInside.description;
            newPotionCitation.GetComponent<TextMeshProUGUI>().text = PotionRepository.Instance.potionInside.citation;
            newPotionSprite.GetComponent<Image>().sprite = PotionRepository.Instance.potionInside.sprite;
        }
        else
        {
            buttonPrendre.SetActive(false);
            buttonSwitch.SetActive(true);

            currentPotionName.GetComponent<TextMeshProUGUI>().text = PotionManager.Instance.currentPotion.nom;
            currentPotionName.SetActive(true);
            currentPotionDescription.GetComponent<TextMeshProUGUI>().text = PotionManager.Instance.currentPotion.description;
            currentPotionCitation.GetComponent<TextMeshProUGUI>().text = PotionManager.Instance.currentPotion.citation;
            currentPotionSprite.GetComponent<Image>().sprite = PotionManager.Instance.currentPotion.sprite;
            currentPotionSprite.SetActive(true);

            newPotionName.GetComponent<TextMeshProUGUI>().text = PotionRepository.Instance.potionInside.nom;
            newPotionDescription.GetComponent<TextMeshProUGUI>().text = PotionRepository.Instance.potionInside.description;
            newPotionCitation.GetComponent<TextMeshProUGUI>().text = PotionRepository.Instance.potionInside.citation;
            newPotionSprite.GetComponent<Image>().sprite = PotionRepository.Instance.potionInside.sprite;
            Debug.Log("potpot");
        }
        
    }
    
    public void FillDescriptionPotion() 
    {
        Debug.Log("ouiouiouoirf");
        if (PotionManager.Instance.currentPotion is not null)
        {
            boxPotionTitre.GetComponent<TextMeshProUGUI>().text = PotionManager.Instance.currentPotion.name;
            boxPotionTexte.GetComponent<TextMeshProUGUI>().text = PotionManager.Instance.currentPotion.description;
            
            globalBoxPotionImage.GetComponent<Image>().sprite = PotionManager.Instance.currentPotion.sprite;
            globalBoxPotionImage.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        }
        else
        {
            boxPotionTitre.GetComponent<TextMeshProUGUI>().text = "";
            boxPotionTexte.GetComponent<TextMeshProUGUI>().text = "Je n'ai pas de potion actuellement";
            boxPotionImage.GetComponent<Image>().sprite = boxPotionVide;
            globalBoxPotionImage.GetComponent<Image>().sprite = boxPotionVide;
        }
    }

    public void DebugOverButton()
    {
        Debug.Log("oui cocuouc");
    }

    public void InvertChoiceArrow(int cas)
    {
        switch (cas)
        {
            case 0:
                Debug.Log("yué soui là");
                potionChoiceArrow.GetComponent<RectTransform>().rotation = Quaternion.Euler(0,0,rotaZ);
                break;
            
            case 1:
                Debug.Log("paslà");
                potionChoiceArrow.GetComponent<RectTransform>().rotation = quaternion.EulerXYZ(0,0,0);
                break;
        }
    }
    
       
    //Fonctions : Son ***********************************************************************************************************************************************************************************
    
    public void PlayButtonSound()
    {
        audioSource.Play();
    }
    
    public void PlayDialogueUp()
    {
        audioSource.PlayOneShot(clipDialoguesUp,0.3f);
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

    public void SelectSmashButton()
    {
        StartCoroutine(SelectContinueButtonLater());
    }
    
    IEnumerator SelectContinueButtonLater()
    {
        yield return null;
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(boutonSmash.gameObject);
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