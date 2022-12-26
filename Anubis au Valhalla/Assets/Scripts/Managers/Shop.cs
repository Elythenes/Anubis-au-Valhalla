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

    bool open = false;

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

    // Update is called once per frame
    void FixedUpdate()
    {
       /* if (open && cam.orthographicSize > 2)
        {
            OpenShop();
        }*/

        if (cost.Count > 0)
        {
            for (int i = 0; i < cost.Count; i++)
            {
                if (cost[i] > Souls.instance.soulBank)
                {
                    upsButton[i].interactable = false;
                }
            }
        }

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
        shopUI.interactable = true;
        open = true;
        CharacterController.instance.controls.Disable();
        CharacterController.instance.canDash = false;
        AttaquesNormales.instance.canAttack = false;
        shopUI.DOFade(1, 1);
        shopUIController.fade = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
    /*void OpenShop()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 1.9f, zoomSpeed);
    }

    void QuitShop()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, camBaseSize, zoomSpeed);
    }*/

    public void CloseShop()
    {
        open = false;
        CharacterController.instance.controls.Enable();
        CharacterController.instance.canDash = true;
        AttaquesNormales.instance.canAttack = true;
        shopUI.interactable = false;
        shopUI.DOFade(0, 1).OnComplete(() =>
        {
            //shopUI.gameObject.SetActive(false);
            weaponShop.transform.localScale = Vector3.one;
        });
        weaponShop.SetActive(false);

    }

    #endregion

    #region WeaponShop

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
                for (int i = 0; i < upsButton.Count; i++)
                {
                    var index = Random.Range(0, upgradesList.UpsLame.Count);
                    var lame = upgradesList.UpsLame[index].Lames[0];
                    Debug.Log(lame.price);
                    Debug.Log((int)lame.level);
                    var itemCost = Mathf.RoundToInt(100 * 
                                                    Mathf.Pow((((int)lame.level+1) * lame.tier), (1+(int)lame.level) / 14)); //Multiplicator, based on the upgrades tier and level
                    choice.Add(lame);
                    cost.Add(itemCost);
                    costText[i].text = "" + itemCost;
                    buttonText[i].text = "" + lame.nom;
                    description[i].text = "" + lame.description;
                }
                break;
            case 1:
                for (int i = 0; i < upsButton.Count; i++)
                {
                    var index = Random.Range(0, upgradesList.UpsHampe.Count);
                    var hampe = upgradesList.UpsHampe[index].Hampes[0];
                    var itemCost = Mathf.RoundToInt(100 * 
                                                    Mathf.Pow((((int)hampe.level+1) * hampe.tier), ((int)hampe.level+1)/ 14)); //Multiplicator, based on the upgrades tier and level
                    choice.Add(hampe);
                    cost.Add(itemCost);
                    costText[i].text = "" + itemCost;
                    buttonText[i].text = "" + hampe.nom;
                    description[i].text = "" + hampe.description;
                    
                }
                break;
            case 2:
                for (int i = 0; i < upsButton.Count; i++)
                {
                    var index = Random.Range(0, upgradesList.UpsManche.Count);
                    var manche = upgradesList.UpsManche[index].Manches[0];
                    var itemCost = Mathf.RoundToInt(100 * 
                                                    Mathf.Pow((((int)manche.level+1) * manche.tier), ((int)manche.level+1) / 14)); //Multiplicator, based on the upgrades tier and level
                    choice.Add(manche);
                    cost.Add(itemCost);
                    costText[i].text = "" + itemCost;
                    buttonText[i].text = "" + manche.nom;
                    description[i].text = "" + manche.description;
                }
                Debug.Log(cost.Count);
                break;
        }
    }

    public void OnClickUpgrade(int buttonType)
    {
        glyphUpdater.AddGlyph(choice[buttonType]);
        switch (buttonType)
        {
            case 0:
                upgradesList.UpsLame[buttonType].Lames.Remove(choice[buttonType]);
                break;
            case 1:
                upgradesList.UpsHampe[buttonType].Hampes.Remove(choice[buttonType]);
                break;
            case 2:
                upgradesList.UpsManche[buttonType].Manches.Remove(choice[buttonType]);
                break;
        }
        Souls.instance.soulBank -= cost[buttonType];
        Souls.instance.UpdateSoulsShop();
        choice.Clear();
        cost.Clear();
        Debug.Log(cost.Count);
    }

    #endregion
    
    public void GenerateConsumables()
    {
        for (int i = 0; i < consumablesButton.Count; i++)
        {
            if (i == 0)
            {
                consumablesTitle[i].text = "Soins";
                consumableDesc[i].text = "Vous soigne de " + Mathf.RoundToInt(DamageManager.instance.vieMax / 3.5f) + " PV";
                consumablesCost.Add(Mathf.RoundToInt(10 * Mathf.Pow(SalleGenerator.Instance.roomsDone, 0.8f)));
                costConsumables[i].text = consumablesCost[i].ToString();
                continue;
            }

            var potionToSell = PotionManager.Instance.potionsForShop[Random.Range(0, PotionManager.Instance.potionsForShop.Count)];
            consumablesTitle[i].text = potionToSell.nom;
            consumableDesc[i].text = potionToSell.description;
            consumablesCost.Add(potionToSell.prix);
            costConsumables[i].text = consumablesCost[i].ToString();
            consumableObject.Add(potionToSell);
            consumableSprites[i].texture = potionToSell.sprite;
        }
    }

    public void InstantHeal()
    {
        DamageManager.instance.vieActuelle += Mathf.RoundToInt(DamageManager.instance.vieMax / 3.5f);
        Souls.instance.soulBank -= consumablesCost[0];
        consumablesCost.Clear();
        Souls.instance.UpdateSoulsShop();
    }

    public void GetConsumable(int index)
    {
        PotionManager.Instance.currentPotion = consumableObject[index];
        UiManager.instance.spritePotion.GetComponent<RawImage>().texture = consumableObject[index].sprite;
        Souls.instance.soulBank -= consumablesCost[index];
        Souls.instance.UpdateSoulsShop();
        consumablesCost.Clear();
        consumableObject.Clear();
        consumableObject.Add(null);
        
    }
}


