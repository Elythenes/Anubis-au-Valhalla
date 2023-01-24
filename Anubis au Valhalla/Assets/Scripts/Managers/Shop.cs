using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GenPro;
using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Shop : MonoBehaviour
{

    public bool open = false;

    public CanvasGroup shopUI;
    public UIMenuShop shopUIController;

    public GameObject weaponShop;

    private GlyphInventory glyphUpdater;
    public UpragesList upgradesList;
    public List<Button> upsButton;
    public List<TextMeshProUGUI> buttonText;
    public List<TextMeshProUGUI> description;
    public List<TextMeshProUGUI> costText;
    public List<int> cost = new List<int>();
    public List<GameObject> iconeGlyphes;
    public List<GameObject> cadreGlyphes;
    public List<Sprite> cadreRarity;
    public TextMeshProUGUI popupShop;

    [Foldout("Consumables")] public List<Button> consumablesButton;
    [Foldout("Consumables")] public List<TextMeshProUGUI> consumablesTitle;
    [Foldout("Consumables")] public List<TextMeshProUGUI> consumableDesc;
    [Foldout("Consumables")] public List<TextMeshProUGUI> costConsumables;
    [Foldout("Consumables")] public List<RawImage> consumableSprites;
    private List<int> consumablesCost = new List<int>();
    public List<PotionObject> consumableObject = new List<PotionObject>();

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] audioClipArray;
    
    public List<GlyphObject> choice;

    public bool lockInteract;
    public bool canInteract;
    public Vector3 offset;
    public GameObject CanvasInteraction;
    public TextMeshProUGUI TextInteraction;
    public int generatedPharaon;
    public int generatedDivine;

    private bool hasgenerated = false;
    public bool[] boughtUpgrades;
    private int currentType;
    public List<int> indexes;
    private int soldOut = 0;
    
    public List<TextMeshProUGUI> textsRarityChoix;


    public enum UpsTypes
    {
        Lame,
        Hampe,
        Manche
    }

    
    #region classes
    // Start is called before the first frame update
    [Serializable]
    public class UpragesList
    {
        public List<LameUps> UpsLame;
        public List<HampeUps> UpsHampe;
        public List<MancheUps> UpsManche;
    }
    [Serializable]
    public class LameUps
    {
        public List<GlyphObject> Lames;
    }
    [Serializable]
    public class HampeUps
    {
        public List<GlyphObject> Hampes;
    }
    [Serializable]
    public class MancheUps
    {
        public List<GlyphObject> Manches;
    }   
    #endregion
    
    void Awake()
    {
        glyphUpdater = GameObject.Find("GlyphManager").GetComponent<GlyphInventory>();
        Souls.instance.UpdateSoulsShop();
        consumableObject.Add(null);
        CanvasInteraction = GameObject.FindWithTag("CanvasInteraction");
        TextInteraction = GameObject.Find("TexteAction").GetComponent<TextMeshProUGUI>();
    }


    private void Update()
    {
        if (open)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                CloseShop();
                shopUIController.FadeOut();
                UiManager.instance.isSousMenu = false;
            }
        }
        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                audioSource.pitch = 1;
                audioSource.PlayOneShot(audioClipArray[0],0.8f);
                CanvasInteraction.GetComponent<Canvas>().enabled = false;
                lockInteract = true;
                GetShop();
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !lockInteract)
        {
            CanvasInteraction.GetComponent<Canvas>().enabled = true;
            CanvasInteraction.transform.position = transform.position + offset;
            CanvasInteraction.transform.localScale = new Vector3(0,0,CanvasInteraction.transform.localScale.z);
            CanvasInteraction.transform.DOScale(new Vector3(1, 1, CanvasInteraction.transform.localScale.z),0.25f);
            TextInteraction.SetText("Acheter");
            CanvasInteraction.SetActive(true);
            canInteract = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasInteraction.GetComponent<Canvas>().enabled = false;
            canInteract = false;
        }
    }

    #region GeneralShopFunctions

    
    
    
    void GetShop()
    {
        shopUI.GetComponent<CanvasGroup>().blocksRaycasts = true;
        Time.timeScale = 0;
        UiManager.instance.EnterSousMenu();
        shopUI.interactable = true;
        open = true;
        CharacterController.instance.controls.Disable();
        CharacterController.instance.canDash = false;
        AttaquesNormales.instance.canAttack = false;
        CharacterController.instance.movement = Vector2.zero;
        CharacterController.instance.rb.velocity = Vector2.zero;
        shopUI.DOFade(1, 1);
        shopUI.blocksRaycasts = true;
        shopUIController.fade = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        GetAvailableUpgrades();
        Souls.instance.UpdateSoulsShop();
    }

    public void CloseShop()
    {
        shopUI.GetComponent<CanvasGroup>().blocksRaycasts = false;
        UiManager.instance.ExitSousMenu();
        audioSource.pitch = 0.5f;
        audioSource.PlayOneShot(audioClipArray[1],0.8f);
        Time.timeScale = 1;
        open = false;
        CharacterController.instance.controls.Enable();
        CharacterController.instance.canDash = true;
        AttaquesNormales.instance.canAttack = true;
        shopUI.interactable = false;
        SalleGenerator.Instance.UnlockDoors();
        shopUI.DOFade(0, 1).OnComplete(() =>
        {
            //shopUI.gameObject.SetActive(false);
            weaponShop.transform.localScale = Vector3.one;
        });
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        lockInteract = false;
    }

    #endregion

    #region WeaponShop

    public void GetAvailableUpgrades()
    {
        if (hasgenerated)
        {
            switch (currentType)
            {
                case 0:
                    for (int i = 0; i < upsButton.Count; i++)
                    {
                        upsButton[i].interactable = cost[currentType + i] <= Souls.instance.soulBank;
                    }
                    break;
                case 3:
                    for (int i = 0; i < upsButton.Count; i++)
                    {
                        upsButton[i].interactable = cost[currentType + i] <= Souls.instance.soulBank;
                    }
                    break;
                case 6:
                    for (int i = 0; i < upsButton.Count; i++)
                    {
                        upsButton[i].interactable = cost[currentType + i] <= Souls.instance.soulBank;
                    }
                    break;
            }
            return;
        }

        hasgenerated = true;
        for (int i = 0; i <= 8; i++)
        {
            if (i <= 2)
            {
                var index = Random.Range(0, upgradesList.UpsLame.Count);
                var lame = upgradesList.UpsLame[index].Lames[UpgradeRarity()];
                var itemCost = Mathf.RoundToInt(lame.price);
                choice.Add(lame);
                cost.Add(itemCost);
                indexes.Add(index);
            }
            else if (i <= 5)
            {
                var index = Random.Range(0, upgradesList.UpsHampe.Count);
                var hampe = upgradesList.UpsHampe[index].Hampes[UpgradeRarity()];
                var itemCost = Mathf.RoundToInt(hampe.price);
                choice.Add(hampe);
                cost.Add(itemCost);
                indexes.Add(index);
            }
            else
            {
                var index = Random.Range(0, upgradesList.UpsManche.Count);
                var manche = upgradesList.UpsManche[index].Manches[UpgradeRarity()];
                var itemCost = Mathf.RoundToInt(manche.price);
                choice.Add(manche);
                cost.Add(itemCost);
                indexes.Add(index);
            }

        }
    }
    public void ChooseType(string chosenType)
    {
        switch (chosenType)
        {
            case "Lame":
                ChooseUps(0);
                break;
            case "Hampe":
                ChooseUps(1);
                break;
            case "Manche":
                ChooseUps(2);
                break;
        }
    }

    public void ChooseUps(int type)
    {
        switch (type)
        {
            case 0:
                currentType = 0;
                for (int i = 0; i < upsButton.Count; i++)
                {
                    if (boughtUpgrades[currentType + i])
                    {
                        costText[i].text = "Vendu";
                        buttonText[i].text = "" + choice[currentType + i].nom;
                        description[i].text = "" + choice[currentType + i].description;
                        upsButton[i].interactable = false;
                        continue;
                    }
                    upsButton[i].interactable = true;
                    costText[i].text = "" + cost[currentType + i];
                    buttonText[i].text = "" + choice[currentType + i].nom;
                    description[i].text = "" + choice[currentType + i].description;
                    SetRarity(choice[currentType + i], textsRarityChoix[i], cadreGlyphes[i]);
                    iconeGlyphes[i].GetComponent<Image>().sprite = choice[currentType + i].icone;
                    if (cost[currentType + i] > Souls.instance.soulBank)
                    {
                        upsButton[i].interactable = false;
                    }
                }
                break;
            case 1:
                currentType = 3;
                for (int i = 0; i < upsButton.Count; i++)
                {
                    if (boughtUpgrades[currentType + i])
                    {
                        costText[i].text = "Vendu";
                        buttonText[i].text = "" + choice[currentType + i].nom;
                        description[i].text = "" + choice[currentType + i].description;
                        upsButton[i].interactable = false;
                        continue;
                    }
                    upsButton[i].interactable = true;
                    costText[i].text = "" + cost[currentType + i];
                    buttonText[i].text = "" + choice[currentType + i].nom;
                    description[i].text = "" + choice[currentType + i].description;
                    SetRarity(choice[currentType + i], textsRarityChoix[i], cadreGlyphes[i]);
                    iconeGlyphes[i].GetComponent<Image>().sprite = choice[currentType + i].icone;
                    if (cost[currentType + i] > Souls.instance.soulBank)
                    {
                        upsButton[i].interactable = false;
                    }
                }
                break;
            case 2:
                currentType = 6;
                for (int i = 0; i < upsButton.Count; i++)
                {
                    if (boughtUpgrades[currentType + i])
                    {
                        costText[i].text = "Vendu";
                        buttonText[i].text = "" + choice[currentType + i].nom;
                        description[i].text = "" + choice[currentType + i].description;
                        upsButton[i].interactable = false;
                        continue;
                    }
                    upsButton[i].interactable = true;
                    costText[i].text = "" + cost[currentType + i];
                    buttonText[i].text = "" + choice[currentType + i].nom;
                    description[i].text = "" + choice[currentType + i].description;
                    SetRarity(choice[currentType + i], textsRarityChoix[i], cadreGlyphes[i]);
                    iconeGlyphes[i].GetComponent<Image>().sprite = choice[currentType + i].icone;
                    if (cost[currentType + i] > Souls.instance.soulBank)
                    {
                        upsButton[i].interactable = false;
                    }
                }
                break;
        }
    }

    void SetRarity(GlyphObject hiero, TextMeshProUGUI textBox, GameObject bouton)
    {
        switch (hiero.rare)
        {
            case Rarity.Prêtre:
                textBox.SetText("Prêtre");
                textBox.color = UiManager.instance.colorPretre;
                bouton.GetComponent<Image>().sprite = cadreRarity[0];
                bouton.GetComponent<Image>().color = UiManager.instance.colorPretre;
                break;
            case Rarity.Pharaon:
                textBox.SetText("Pharaon");
                textBox.color = UiManager.instance.colorPharaon;
                bouton.GetComponent<Image>().sprite = cadreRarity[1];
                bouton.GetComponent<Image>().color = UiManager.instance.colorPharaon;
                break;
            case Rarity.Divinité:
                textBox.SetText("Divinité");
                textBox.color = UiManager.instance.colorDivinite;
                bouton.GetComponent<Image>().sprite = cadreRarity[2];
                bouton.GetComponent<Image>().color = UiManager.instance.colorDivinite;
                break;
            default:
                
                break;
        }
    }
    

    public void OnClickUpgrade(int buttonType)
    {
        glyphUpdater.AddNewGlyph(choice[buttonType + currentType]);


        costText[buttonType].text = "Vendu";
        boughtUpgrades[currentType + buttonType] = true;
        upsButton[buttonType].interactable = false;
        shopUIController.hasMovedOut[buttonType] = true;
        Souls.instance.soulBank -= cost[buttonType+ + currentType];
        Souls.instance.UpdateSoulsShop();
        switch (currentType)
        {
            case 0:
                for (int i = 0; i < upsButton.Count; i++)
                {
                    if (cost[currentType + i] > Souls.instance.soulBank)
                    {
                        upsButton[i].interactable = false;
                    }
                }
                break;
            case 3:
                for (int i = 0; i < upsButton.Count; i++)
                {
                    if (cost[currentType + i] > Souls.instance.soulBank)
                    {
                        upsButton[i].interactable = false;
                    }
                }
                break;
            case 6:
                for (int i = 0; i < upsButton.Count; i++)
                {
                    if (cost[currentType + i] > Souls.instance.soulBank)
                    {
                        upsButton[i].interactable = false;
                    }
                }
                break;
        }
        soldOut = 0;
        foreach (var bought in boughtUpgrades)
        {
            if (bought)
            {
                soldOut++;
            }

            if (soldOut == 3)
            {
                shopUIController.FadeInOther(shopUIController.soldOutText);
            }
        }
    }

    #endregion

    public void InstantHeal(float percentHeal, int price)
    {
        AnubisCurrentStats.instance.vieActuelle += Mathf.RoundToInt(AnubisCurrentStats.instance.vieMax * (percentHeal / 100));
        if (AnubisCurrentStats.instance.vieActuelle > AnubisCurrentStats.instance.vieMax)
            AnubisCurrentStats.instance.vieActuelle = AnubisCurrentStats.instance.vieMax;
        Souls.instance.soulBank -= price;
        Souls.instance.UpdateSoulsCounter();
    }

    private int UpgradeRarity()
    {
        var sg = SalleGenerator.Instance;
        var randomValue = Random.Range(0, 101);
        if (randomValue <= sg.chancePharaon[sg.spawnedShops])
        {
            if (randomValue <= sg.chanceDivinité[sg.spawnedShops] && generatedDivine < sg.nbMaxDivinite[sg.spawnedShops])
            {
                generatedDivine++;
                return 2;
            }
            if (generatedPharaon < sg.nbMaxPharaon[sg.spawnedShops])
            {
                generatedPharaon++;
                return 1;
            }

            return 0;
        }

        return 0;
    }
}


