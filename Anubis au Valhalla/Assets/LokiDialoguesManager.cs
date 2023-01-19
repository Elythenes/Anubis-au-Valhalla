using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class LokiDialoguesManager : MonoBehaviour
{
    public bool canNextDialogue;
    public float index;
    public float textSpeed;
    public float upDownY;
    public float menuSpeed;
    public TextMeshProUGUI textDialogues;
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
        if (Input.GetKeyDown(KeyCode.F) && canNextDialogue)
        {
            index++;
        }
        
        switch (index)
        {
            case 0:
                //StopDialogue();
                Type("voici le premier dialogue");
                break;
            case 1 :
               // StopDialogue();
                Type("oui oui vous avec raison");
                break;
            case 2 :
               // StopDialogue();
                Type("je suis beau");
                break;
            case 3 :
                //StopDialogue();
                Type("c'est vrai il est beau");
                break;
            case 4 :
                //StopDialogue();
                Type("Pouahaha, Guerrier");
                break;
            case 5 :
                //StopDialogue();
                Type("hmmm ce que vous dites est vrai");
                break;
        }
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
        canNextDialogue = true;
        transform.DOLocalMove(new Vector3(0,0,transform.position.z),menuSpeed);
    }
    
    void StopDialogue()
    {
        StopAllCoroutines();
        textDialogues.text = String.Empty.Substring(0);
      
    }

}
