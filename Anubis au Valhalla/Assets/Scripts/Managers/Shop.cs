using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GenPro;
using NaughtyAttributes;
using TMPro;
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
    public TextMeshProUGUI popupShop;

    [Foldout("Consumables")] public List<Button> consumablesButton;
    [Foldout("Consumables")] public List<TextMeshProUGUI> consumablesTitle;
    [Foldout("Consumables")] public List<TextMeshProUGUI> consumableDesc;
    [Foldout("Consumables")] public List<TextMeshProUGUI> costConsumables;
    [Foldout("Consumables")] public List<RawImage> consumableSprites;
    private List<int> consumablesCost = new List<int>();
    public List<PotionObject> consumableObject = new List<PotionObject>();

    public List<GlyphObject> choice;
    public bool choice1Avalable;
    public bool choice2Avalable;
    public bool choice3Avalable;
    public Animator animChoice1;
    public Animator animChoice2;
    public Animator animChoice3;

    public bool lockInteract;
    public bool canInteract;
    public Vector3 offset;
    public GameObject CanvasInteraction;
    public TextMeshProUGUI TextInteraction;

    private bool hasgenerated = false;
    public bool[] boughtUpgrades;
    private int currentType;
    public List<int> indexes;

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

    }

    void FixedUpdate()
    {
       
        if (consumablesCost.Count > 0)
        {
            for (int i = 0; i < consumablesCost.Count; i++)
            {
                if (consumablesCost[i] > Souls.instance.soulBank)
                {
                    consumablesButton[i].interactable = false;
                }
            }
        }
        
        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
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
        Time.timeScale = 0;
        UiManager.instance.EnterSousMenu();
        shopUI.interactable = true;
        open = true;
        CharacterController.instance.controls.Disable();
        CharacterController.instance.canDash = false;
        AttaquesNormales.instance.canAttack = false;
        shopUI.DOFade(1, 1);
        shopUI.blocksRaycasts = true;
        shopUIController.fade = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        GetAvailableUpgrades();
    }

    public void CloseShop()
    {
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
            return;
        }

        hasgenerated = true;
        for (int i = 0; i <= 8; i++)
        {
            Debug.Log("Wassup baby");
            if (i <= 2)
            {
                var index = Random.Range(0, upgradesList.UpsLame.Count);
                var lame = upgradesList.UpsLame[index].Lames[0];
                var itemCost = Mathf.RoundToInt(100 * 
                                                Mathf.Pow((((int)lame.level+1) * lame.tier), (1+(int)lame.level) / 14)); //Multiplicator, based on the upgrades tier and level
                choice.Add(lame);
                cost.Add(itemCost);
                indexes.Add(index);
            }
            else if (i <= 5)
            {
                var index = Random.Range(0, upgradesList.UpsHampe.Count);
                var hampe = upgradesList.UpsHampe[index].Hampes[0];
                var itemCost = Mathf.RoundToInt(100 * 
                                                Mathf.Pow((((int)hampe.level+1) * hampe.tier), ((int)hampe.level+1)/ 14)); //Multiplicator, based on the upgrades tier and level
                choice.Add(hampe);
                cost.Add(itemCost);
                indexes.Add(index);
            }
            else
            {
                var index = Random.Range(0, upgradesList.UpsManche.Count);
                var manche = upgradesList.UpsManche[index].Manches[0];
                var itemCost = Mathf.RoundToInt(100 * 
                                                Mathf.Pow((((int)manche.level+1) * manche.tier), ((int)manche.level+1) / 14)); //Multiplicator, based on the upgrades tier and level
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
                    if (cost[currentType + i] > Souls.instance.soulBank)
                    {
                        upsButton[i].interactable = false;
                    }
                }
                break;
        }
    }

    public void OnClickUpgrade(int buttonType)
    {
        glyphUpdater.AddGlyph(choice[buttonType + currentType]);
        switch (currentType)
        {
            case 0:
                upgradesList.UpsLame[indexes[buttonType + currentType]].Lames.Remove(choice[buttonType + currentType]);
                break;
            case 3:
                upgradesList.UpsHampe[indexes[buttonType + currentType]].Hampes.Remove(choice[buttonType + currentType]);
                break;
            case 6:
                upgradesList.UpsManche[indexes[buttonType + currentType]].Manches.Remove(choice[buttonType + currentType]);
                break;
        }

        costText[buttonType].text = "Vendu";
        boughtUpgrades[currentType + buttonType] = true;
        upsButton[buttonType].interactable = false;
        Souls.instance.soulBank -= cost[buttonType];
        Souls.instance.UpdateSoulsShop();
        Debug.Log(cost.Count);
    }

    #endregion

    public void InstantHeal(float percentHeal, int price)
    {
        AnubisCurrentStats.instance.vieActuelle += Mathf.RoundToInt(AnubisCurrentStats.instance.vieMax * (percentHeal / 100));
        Debug.Log("ca devrai heal tant de pv:" + AnubisCurrentStats.instance.vieMax * (percentHeal / 100));
        if (AnubisCurrentStats.instance.vieActuelle > AnubisCurrentStats.instance.vieMax)
            AnubisCurrentStats.instance.vieActuelle = AnubisCurrentStats.instance.vieMax;
        Souls.instance.soulBank -= price;
        Souls.instance.UpdateSoulsCounter();
    }
    
}


