using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TothBehiavour : MonoBehaviour
{
   public bool isTalkable;
   public float menuSpeed;
   public float textSpeed;
   public int isFade;
   private float upDownY;
   public Vector3 offset;
   public GameObject CanvasInteraction;
   public TextMeshProUGUI TextInteraction;
   public GameObject dialogueMenu;
   public TextMeshProUGUI textDialogues;
   public CanvasGroup BlackScreen;
   public static TothBehiavour instance;

   private void Awake()
   {
      if (instance != null)
      {
         DestroyImmediate(gameObject);
         return;
      }
      instance = this;
   }

   private void Start()
   {
      CanvasInteraction = GameObject.FindWithTag("CanvasInteraction");
      TextInteraction = GameObject.Find("TexteAction").GetComponent<TextMeshProUGUI>();
      dialogueMenu = GameObject.Find("MenuTuto");
      textDialogues = GameObject.Find("TextDialogues").GetComponent<TextMeshProUGUI>();
      BlackScreen = GameObject.Find("TransitionFonduDialogues").GetComponent<CanvasGroup>();
   }

   private void Update()
   {
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
      
      if (Input.GetKeyDown(KeyCode.A))
      {
         if(isTalkable)
         {
            isFade = 1;
            isTalkable = false;
            DialogueUP();
            CharacterController.instance.allowMovements = false;
         }
      }
   }

   private void OnTriggerEnter2D(Collider2D col)
   {
      if (col.CompareTag("Player"))
      {
         CanvasInteraction.transform.position = transform.position + offset;
         CanvasInteraction.transform.localScale = new Vector3(0,0,CanvasInteraction.transform.localScale.z);
         CanvasInteraction.transform.DOScale(new Vector3(1, 1, CanvasInteraction.transform.localScale.z),0.25f);
         TextInteraction.SetText("Parler");
         CanvasInteraction.SetActive(true);
         isTalkable = true;
      }
   }
   
   private void OnTriggerExit2D(Collider2D col)
   {
      if (col.CompareTag("Player"))
      {
         CanvasInteraction.SetActive(false);
         isTalkable = false;
      }
   }

   public void DialogueUP()
   {
      dialogueMenu.transform.DOLocalMove(new Vector3(transform.position.x,upDownY,transform.position.z),menuSpeed);
      StartCoroutine(Type("Bonjour Anubis, si tu as la moindre question, je suis pret à y répondre."));
   }
   
   public void DialogueDOWN()
   {
      isTalkable = true;
      StopDialogue();
      isFade = 2;
      StartCoroutine(Type("Bonne chance Anubis, je veillerais sur toi."));
      dialogueMenu.transform.DOLocalMove(new Vector3(transform.position.x,upDownY - 440,transform.position.z),menuSpeed);
      CharacterController.instance.allowMovements = true;
   }

   public void DialogueSorts()
   {
      StopDialogue();
      StartCoroutine(Type("Les sorts sont des abilités puissantes que tu trouvera dans des coffres en terminant des salles. Tu peux en porter 2 à la fois et les utiliser aves les touches E et R. Mais attention, leurs utilisations sont limités donc utilise les avec parcimonie."));
   }
   
   public void DialogueShop()
   {
      StopDialogue();
      StartCoroutine(Type("Tu trouveras dans le Valhalla, des endrois propice à la médiatation. Dans ceux-ci, connecte toi avec les esprits pour utiliser les âmes aquises au combat pour améliorer ton arme. Tu pouras choisir d'améliorer la lame, la hamp ou la poignée qui proposent respectivement des améliorations offensives, défensives ou de mobilité."));
   }
   
   public void DialogueLD()
   {
      StopDialogue();
      StartCoroutine(Type("Dans le Valhalla, tu trouveras differents types de salles. Les salles normales qui renferment des ennemis et des récompences basiques, les salles Challenge, qui contiennent de grands dangers mais aussi de plus grandes récompences et enfin les salles de méditation qui te permettrons d'améliorer ton arme."));
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

   void StopDialogue()
   {
      StopAllCoroutines();
      textDialogues.text = String.Empty;
   }
}
