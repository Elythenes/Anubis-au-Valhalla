using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Shop : MonoBehaviour
{
    public Camera cam;
    public float camBaseSize = 7f;

    public float zoomSpeed;

    private bool open = false;

    public CanvasGroup shopUI;

    public GameObject weaponShop;

    private GlyphInventory glyphUpdater;
    public UpragesList upgradesList;
    public List<Button> upsButton;
    public List<TextMeshProUGUI> buttonText;
    public List<TextMeshProUGUI> description;
    public List<TextMeshProUGUI> costText;
    private List<int> cost = new List<int>();

    [Foldout("Consumables")] public List<Button> consumablesButton;
    [Foldout("Consumables")] public List<TextMeshProUGUI> consumablesTitle;
    [Foldout("Consumables")] public List<TextMeshProUGUI> consumableDesc;
    [Foldout("Consumables")] public List<TextMeshProUGUI> costConsumables;
    private List<int> consumablesCost = new List<int>();

    public List<GlyphObject> choice;

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
        cam = Camera.main;
        glyphUpdater = GameObject.Find("GlyphManager").GetComponent<GlyphInventory>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (open && cam.orthographicSize > 2)
        {
            OpenShop();
        }

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

        if (!open)
        {
            QuitShop();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            shopUI.gameObject.SetActive(true);
            open = true;
            CharacterController.instance.controls.Disable();
            CharacterController.instance.canDash = false;
            AttaquesNormales.instance.canAttack = false;
            shopUI.DOFade(1, 1);
            weaponShop.SetActive(true);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    #region GeneralShopFunctions

    void OpenShop()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 1.9f, zoomSpeed);
    }

    void QuitShop()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, camBaseSize, zoomSpeed);
    }

    public void CloseShop()
    {
        open = false;
        CharacterController.instance.controls.Enable();
        CharacterController.instance.canDash = true;
        AttaquesNormales.instance.canAttack = true;
        shopUI.DOFade(0, 1).OnComplete(() =>
        {
            //shopUI.gameObject.SetActive(false);
            weaponShop.transform.localScale = Vector3.one;
        });
        weaponShop.SetActive(false);

    }

    #endregion

    #region WeaponShop

        public void MoveCam(int buttonNumber)
    {
        cam.orthographicSize = 1;
        weaponShop.transform.localScale = Vector3.one * 3;
        if (buttonNumber == 0)
        {
            weaponShop.transform.Translate(230,20,0);
        }
        if (buttonNumber == 1)
        {
            weaponShop.transform.Translate(0,20,0);
        }
        if (buttonNumber == 2)
        {
            weaponShop.transform.Translate(-210,20,0);
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
                for (int i = 0; i < upsButton.Count; i++)
                {
                    var index = Random.Range(0, upgradesList.UpsLame.Count);
                    var lame = upgradesList.UpsLame[index].Lames[0];
                    var itemCost = Mathf.RoundToInt(lame.price * 
                                                    Mathf.Pow(((int)lame.level * lame.tier), (int)lame.level / 14)); //Multiplicator, based on the upgrades tier and level
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
                    var itemCost = Mathf.RoundToInt(hampe.price * 
                                                    Mathf.Pow(((int)hampe.level * hampe.tier), (int)hampe.level / 14)); //Multiplicator, based on the upgrades tier and level
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
                    var itemCost = Mathf.RoundToInt(manche.price * 
                                                    Mathf.Pow(((int)manche.level * manche.tier), (int)manche.level / 14)); //Multiplicator, based on the upgrades tier and level
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
        Souls.instance.UpdateSoulsCounter();
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
                consumablesCost.Add(Mathf.RoundToInt(10 * Mathf.Pow(SalleGennerator.instance.roomsDone, 0.8f)));
                costConsumables[i].text = consumablesCost[i].ToString();
            }
        }
    }

    public void InstantHeal()
    {
        DamageManager.instance.vieActuelle += Mathf.RoundToInt(DamageManager.instance.vieMax / 3.5f);
        Souls.instance.soulBank -= consumablesCost[0];
        consumablesCost.Clear();
    }
}


