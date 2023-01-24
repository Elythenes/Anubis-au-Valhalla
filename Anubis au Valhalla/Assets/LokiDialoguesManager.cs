using System;
using System.Collections;

using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class LokiDialoguesManager : MonoBehaviour
{
    public bool canNextDialogue;
    public float isFade;
    public float index;
    public float textSpeed;
    public float upDownY;
    public float menuSpeed;
    public CanvasGroup bouttonAnswer;
    public Button bouttonAnswerBoutton;
    public bool fadeBoutton;
    public CanvasGroup bouttonAnswer2;
    public Button bouttonAnswerBoutton2;
    public bool fadeBoutton2;
    public TextMeshProUGUI LokiName;
    public GameObject FNext;
    public Image LokiImage;
    public bool colorChange;
    public float colorValue;
    public TextMeshProUGUI textDialogues;
    public CanvasGroup BlackScreen;
    public static LokiDialoguesManager instance;
    public CanvasGroup victoryScreen;
    public bool fadeVictoryScreen;
    public AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        BlackScreen = GameObject.Find("TransitionFonduDialogues").GetComponent<CanvasGroup>();
    }

    public void Update()
    {
        if (colorChange && colorValue <= 255)
        {
            colorValue += Time.deltaTime;
            Color imagecolor = new Color(colorValue, colorValue, colorValue);
            LokiImage.color = imagecolor;
        }
        else if ( colorValue >= 255)
        {
            colorChange = false;
        }
        
        if (fadeVictoryScreen && victoryScreen.alpha < 1)
        {
            victoryScreen.alpha += Time.deltaTime;
        }
        
        if (fadeBoutton && bouttonAnswer.alpha < 1)
        {
            bouttonAnswer.alpha += Time.deltaTime;
        }
        else if (!fadeBoutton && bouttonAnswer.alpha > 0)
        {
            bouttonAnswer.alpha -= Time.deltaTime;
        }
        
        if (fadeBoutton2 && bouttonAnswer2.alpha < 1)
        {
            bouttonAnswer2.alpha += Time.deltaTime;
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
            UiManager.instance.PlayButtonSound();
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
                StartCoroutine(Type("Anubis, te voilà enfin..."));
                break;
            case 1 :
               StopDialogue();
               StartCoroutine( Type("Tu en as mis du temps, comment oses-tu faire attendre l'être le plus puissant des 9 mondes ?"));
                break;
            case 2 :
               StopDialogue();
               StartCoroutine(  Type("Tu l'auras compris, je suis Loki, le plus grand des dieux nordiques. Que dis-je, le plus grand des dieux !"));
               LokiName.text = "Loki";
               colorChange = true;
                break;
            case 3 :
                StopDialogue();
                StartCoroutine(Type("Tes âmes m'ont été d'une grande aide, et je crains que leur pouvoir ne soit sur le point de se retourner contre toi."));
                break;
            case 4 :
                StopDialogue();
                StartCoroutine(Type("Je te présente Hildr, la légendaire Valkyrie. Elle est à mon service désormais. Et tu mourras de sa main."));
                break;
            case 5 :
                StopDialogue();
                StartCoroutine(Type("Mais dis-moi Anubis, je me demandais, qu'est ce que ça fait de tuer un dieu de la mort ? N'as-tu pas hâte de le découvrir ?"));
                canNextDialogue = false;
                bouttonAnswerBoutton.interactable = true;
                fadeBoutton = true;
                FNext.SetActive(false);
                break;
            case 6 :
                StopDialogue();
                StartCoroutine(Type("C'est impossible !? Même avec le pouvoir de tes âmes elle n'a pas pu t'arrêter ?"));
                canNextDialogue = true;
                bouttonAnswer.gameObject.SetActive(false);
                bouttonAnswerBoutton.interactable = false;
                FNext.SetActive(true);
                break;
            case 7 :
                StopDialogue();
                StartCoroutine(Type("Alors je comprend maintenant ce que signifie le destin. Le Ragnarök est donc inéluctable..."));
                bouttonAnswer2.blocksRaycasts = true;
                break;
            case 8 :
                StopDialogue();
                StartCoroutine(Type("Anubis, de tous les guerriers que j'ai pu rencontrer ici, tu es sûrement le plus puissant."));
                break;
            case 9 :
                StopDialogue();
                StartCoroutine(Type("Nous nous reverrons, Anubis, une fois que j'aurai fait face à mon destin."));
                fadeBoutton2 = true;
                bouttonAnswerBoutton2.interactable = true;
                bouttonAnswer2.interactable = true;
                bouttonAnswer2.blocksRaycasts = true;
                FNext.SetActive(false);
                break;
        }
    }

    public void StartCombat()
    {
        DialogueDOWN();
        CinematiqueBoss.instance.CotoutineStartCombat();
    }

    public void Victory()
    {
        VictoryScreen.instance.UpdateScore();
        victoryScreen.blocksRaycasts = true;
        DialogueDOWN();
        fadeVictoryScreen = true;
        audioSource.Play();
        victoryScreen.interactable = true;
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
