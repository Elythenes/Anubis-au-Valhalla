using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UiPowerManager : MonoBehaviour
{
    [Header("SYSTEME")]
    public GameObject boxPowerImage;
    public TextMeshProUGUI boxPowerTitre;
    public TextMeshProUGUI boxPowerDescription;
    public TextMeshProUGUI boxPowerAmelioration;

    public GameObject buttonFlecheHaut;
    public GameObject buttonFlecheBas;
    
    public List<TextMeshProUGUI> boxPowerTexts = new(3);
    public List<TextMeshProUGUI> boxPowerLevels = new(3);

    
    [Header("TEXTES POWER 1")]
    public List<string> p1TextsSmash = new(10);
    public List<string> p1TextsThrust = new(10);
    public List<string> p1TextsDash = new(10);
    
    
    [Header("TEXTES POWER 2")]
    public List<string> p2TextsSmash = new(10);
    public List<string> p2TextsThrust = new(10);
    public List<string> p2TextsDash = new(10);


    [Header("DEBUG")]
    public bool isPower1;
    public bool isPower2;
    
    public int powerLevelIndexForUi; //de 1 à 8, car définit l'index de la 1ère box, les deux autres étant lui +1 et +2

    public bool isSmashTexts;
    public bool isThrustTexts;
    public bool isDashTexts;
    
    
    
    
    void Start()
    {
        isPower1 = false;
        isPower2 = false;
        powerLevelIndexForUi = 1;
    }

    
    void Update()
    {
        ChangeTextBoxDependsOnLevel();
        DisableFleche();
    }




    void DisableFleche()
    {
        if (powerLevelIndexForUi == 8)
        {
            buttonFlecheBas.GetComponent<Button>().enabled = false;
        }
        else if (powerLevelIndexForUi == 1)
        {
            buttonFlecheHaut.GetComponent<Button>().enabled = false;
        }
        else
        {
            buttonFlecheBas.GetComponent<Button>().enabled = true;
            buttonFlecheHaut.GetComponent<Button>().enabled = true;
        }
    }

    public void ChangePowerLevelIndex(int chiffre) //fonction appelée par les boutons flèches haut et bas, pour changer les 3 box de textes affichés
    {
        if (powerLevelIndexForUi == 8 && chiffre > 0)
        {
            powerLevelIndexForUi = 8;
        }
        else if (powerLevelIndexForUi == 1 && chiffre < 0)
        {
            powerLevelIndexForUi = 1;
        }
        else
        {
            powerLevelIndexForUi += chiffre;
        }
    }
    

    
    void ChangeTextBoxDependsOnLevel() //change la série de 3 text box en fonction de l'indicateur powerLevelForUi
    {
        
        for (int i = 0; i < 3; i++)
        {
            if (powerLevelIndexForUi <= 8)
            {
                boxPowerLevels[i].SetText(powerLevelIndexForUi+i+"");
            }
            else
            {
                boxPowerLevels[i].SetText(powerLevelIndexForUi-1+i+"");
            }
        }

        if (isPower1)
        {
            if (isSmashTexts)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (powerLevelIndexForUi <= 8)
                    {
                        boxPowerTexts[i].SetText(p1TextsSmash[powerLevelIndexForUi - 1 + i] + "");
                    }
                    else
                    {
                        boxPowerTexts[i].SetText(p1TextsSmash[powerLevelIndexForUi - 1 + i] + "");
                    }
                }
            }

            if (isThrustTexts)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (powerLevelIndexForUi <= 8)
                    {
                        boxPowerTexts[i].SetText(p1TextsThrust[powerLevelIndexForUi - 1 + i] + "");
                    }
                    else
                    {
                        boxPowerTexts[i].SetText(p1TextsThrust[powerLevelIndexForUi - 1 + i] + "");
                    }
                }
            }

            if (isDashTexts)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (powerLevelIndexForUi <= 8)
                    {
                        boxPowerTexts[i].SetText(p1TextsDash[powerLevelIndexForUi - 1 + i] + "");
                    }
                    else
                    {
                        boxPowerTexts[i].SetText(p1TextsDash[powerLevelIndexForUi - 1 + i] + "");
                    }
                }
            }
        }

        if (isPower2)
        {
            if (isSmashTexts)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (powerLevelIndexForUi <= 8)
                    {
                        boxPowerTexts[i].SetText(p2TextsSmash[powerLevelIndexForUi - 1 + i] + "");
                    }
                    else
                    {
                        boxPowerTexts[i].SetText(p2TextsSmash[powerLevelIndexForUi - 1 + i] + "");
                    }
                }
            }

            if (isThrustTexts)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (powerLevelIndexForUi <= 8)
                    {
                        boxPowerTexts[i].SetText(p2TextsThrust[powerLevelIndexForUi - 1 + i] + "");
                    }
                    else
                    {
                        boxPowerTexts[i].SetText(p2TextsThrust[powerLevelIndexForUi - 1 + i] + "");
                    }
                }
            }

            if (isDashTexts)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (powerLevelIndexForUi <= 8)
                    {
                        boxPowerTexts[i].SetText(p2TextsDash[powerLevelIndexForUi - 1 + i] + "");
                    }
                    else
                    {
                        boxPowerTexts[i].SetText(p2TextsDash[powerLevelIndexForUi - 1 + i] + "");
                    }
                }
            }
        }
    }

    
    public void ChangeTextBoxDependsOnMove(int nb) //change la catégorie des 3 text box en fonction du boutou appuyé "Smash, Thrust ou Dash"
    {
        switch (nb)
        {
            case 0:
                isSmashTexts = true;
                isThrustTexts = false;
                isDashTexts = false;
                Debug.Log("samsh");
                break;
            
            case 1:
                isSmashTexts = false;
                isThrustTexts = true;
                isDashTexts = false;
                Debug.Log("thrust");
                break;
            
            case 2:
                isSmashTexts = false;
                isThrustTexts = false;
                isDashTexts = true;
                Debug.Log("dash");
                break;
        }
    }

    public void ChangePower(int i)
    {
        switch (i)
        {
            case 1:
                isPower1 = true;
                isPower2 = false;
                boxPowerTitre.SetText("Pouvoir Âmalediction");
                break;
            
            case 2:
                isPower1 = false;
                isPower2 = true;
                boxPowerTitre.SetText("Pouvoir Sablandage");
                break;
        }
    }

    public void DebugButton()
    {
        Debug.Log("ploc");
    }
    
    
}
