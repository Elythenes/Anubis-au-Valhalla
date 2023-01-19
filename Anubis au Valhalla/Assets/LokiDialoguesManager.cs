using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LokiDialoguesManager : MonoBehaviour
{
    public bool canNextDialogue;
    public float isFade;
    public float index;
    public float textSpeed;
    public float upDownY;
    public float menuSpeed;
    public CanvasGroup bouttonAnswer;
    public bool fadeBoutton;
    public TextMeshProUGUI LokiName;
    public GameObject FNext;
    public Image LokiImage;
    public bool colorChange;
    public float colorValue;
    public TextMeshProUGUI textDialogues;
    public CanvasGroup BlackScreen;
    public static LokiDialoguesManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Update()
    {
        if (colorChange && colorValue <= 255)
        {
            colorValue += Time.deltaTime;
            Color imagecolor = new Color(colorValue, colorValue, colorValue);
            LokiImage.color = imagecolor;
        }
        
        if (fadeBoutton && bouttonAnswer.alpha < 1)
        {
            bouttonAnswer.alpha += Time.deltaTime;
        }
        
        if (isFade == 1)
        {
            if (BlackScreen.alpha < 1)
            {
                BlackScreen.alpha += Time.deltaTime;
            }

            if (BlackScreen.alpha == 1)
            {
                isFade = 0;
            }
        }
        else if (isFade == 2)
        {
            if (BlackScreen.alpha > 0)
            {
                BlackScreen.alpha -= Time.deltaTime;
            }

            if (BlackScreen.alpha == 0)
            {
                isFade = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.F) && canNextDialogue)
        {
            index++;
            NextDialogue();
        }
    }

    public void NextDialogue()
    {
        switch (index)
        {
            case 0:
                StopDialogue();
                StartCoroutine(Type("Anubis, te voila enfin..."));
                break;
            case 1 :
               StopDialogue();
               StartCoroutine( Type("Tu en as mis du temps, comment oses-tu faire attendre l'être le plus puissant des 7 mondes ?"));
                break;
            case 2 :
               StopDialogue();
               StartCoroutine(  Type("Tu l'auras compris, je suis Loki, le plus grand des dieux nordiques. Que dis-je le plus grand des dieux !"));
               LokiName.text = "Loki";
               colorChange = true;
                break;
            case 3 :
                StopDialogue();
                StartCoroutine(Type("La puissance de tes âmes m'ont été d'une grande aide, et je crains que celle-ci ne soit sur le point de se retourner contre toi."));
                break;
            case 4 :
                StopDialogue();
                StartCoroutine(Type("Je te présente Freyja, la légendaire Valkyrie. Elle est à mon service désormais. Et tu moureras de sa main."));
                break;
            case 5 :
                StopDialogue();
                StartCoroutine(Type("Mais dit moi Anubis, je me demendais, qu'est ce que ça fait de tuer un dieu de la mort ? N'as-tu pas hâte de le découvrir ?"));
                canNextDialogue = false;
                fadeBoutton = true;
                FNext.SetActive(false);
                break;
        }
    }

    public void StartCombat()
    {
        DialogueDOWN();
        CinematiqueBoss.instance.CotoutineStartCombat();
    }


    IEnumerator Type(string textToType)
    {
        float t = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            t += Time.deltaTime * textSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textDialogues.text = textToType.Substring(0, charIndex);
            yield return null;
        }

        textDialogues.text = textToType;
    }
    
    public void DialogueUP()
    {
        isFade = 1;
        canNextDialogue = true;
        transform.DOLocalMove(new Vector3(0,0,transform.position.z),menuSpeed);
    }
    
    public void DialogueDOWN()
    {
        canNextDialogue = false;
        UiManager.instance.isSousMenu = false;
        isFade = 2;
        transform.DOLocalMove(new Vector3(0,-1000,transform.position.z),menuSpeed);
    }
    
    void StopDialogue()
    {
        StopAllCoroutines();
        textDialogues.text = String.Empty.Substring(0);
    }

}
