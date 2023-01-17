using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;

public class TothBehiavour : MonoBehaviour
{
   public bool isTalkable;
   public bool firstTime;
   public float menuSpeed;
   public float textSpeed;
   public int isFade;
   private float upDownY;
   private bool menuState;
   public Vector3 offset;
   public GameObject CanvasInteraction;
   public TextMeshProUGUI TextInteraction;
   public GameObject dialogueMenu;
   public TextMeshProUGUI textDialogues;
   public CanvasGroup BlackScreen;
   public static TothBehiavour instance;
   public TextMeshProUGUI eyeCatcher;
   public GameObject tableauDesScores;

   public enum Situation {isHub,isTutoPouvoirs,isTutoCombat,isTutoLD,isTutoShop}
   public Situation activeSituation;
   public List<TextMeshProUGUI> listeTextBouton = new List<TextMeshProUGUI>();
   public enum OptionChoisie {A,B,C,D}
   public OptionChoisie boutonPressed;
   

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
      StartCoroutine(GetData());

      if (!firstTime)
      {
         Destroy(eyeCatcher);
      }
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
      
      if (Input.GetKeyDown(KeyCode.F))
      {
         if(isTalkable)
         {
            UiManager.instance.isSousMenu = true;
            isFade = 1;
            isTalkable = false;
            ChangeText();
            DialogueUP();
            CharacterController.instance.allowMovements = false;
         }

         if (firstTime)
         {
            Destroy(eyeCatcher);
            firstTime = false;
         }
      }


      if (Input.GetKeyDown(KeyCode.Escape) && menuState)
      {
         DialogueDOWN();
      }
   
   }

   private void OnTriggerEnter2D(Collider2D col)
   {
      if (col.CompareTag("Player"))
      {
         CanvasInteraction.GetComponent<Canvas>().enabled = true;
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
         CanvasInteraction.GetComponent<Canvas>().enabled = false;
         //CanvasInteraction.SetActive(false);
         isTalkable = false;
      }
   }
   
   
   // ******************************* Fonction Situations **************************************************************************************

   
   public void ChangeText()
   {
      switch (activeSituation)
      { 
         case Situation.isHub:
            StartCoroutine(Type("Bonjour Anubis, je peux te guider dans ta quête de vangeance."));
            listeTextBouton[0].text = ("Commencer le tutoriel");
            listeTextBouton[1].text = ("Consulter le Tableau des scores");
            listeTextBouton[2].text = ("Revoir l'introduction");
            listeTextBouton[3].text = ("Au revoir Thot");
            break;
         case Situation.isTutoCombat: 
            StartCoroutine(Type("Tu rencontreras de puissants ennemis dans le Valhalla. Tu devras les vaincre comme le dieu de la mort que tu es."));
            listeTextBouton[0].text = ("Comment combattre mes adversaires ?");
            listeTextBouton[1].text = ("Quelles sont les menaces ?");
            listeTextBouton[2].text = ("Les ennemis élites ?");
            listeTextBouton[3].text = ("C'est compris.");
            break;
         case Situation.isTutoPouvoirs: 
            StartCoroutine(Type("Regardons maintenant la magie que je t'ai enseigné, elle te sera utile pour vaincre tes adversaires."));
            listeTextBouton[0].text = ("Rappel moi en quoi consiste la magie.");
            listeTextBouton[1].text = ("Comment utiliser cette magie ?");
            listeTextBouton[2].text = ("Puis-je améliorer ma magie ?");
            listeTextBouton[3].text = ("Je tâcherais de m'en rappeler");
            break;
         case Situation.isTutoShop: 
            StartCoroutine(Type("Dans le Valhalla, tu trouveras des salles dans lesquelles mon influence est grande. Apportemoi les âmes que tu récolteras et je les changerais en de nouveaux pouvoirs."));
            listeTextBouton[0].text = ("Comment récolter des âmes ?");
            listeTextBouton[1].text = ("Quelles pouvoirs pourras tu m'offrir ?");
            listeTextBouton[2].text = ("Peut-tu soigner mes blessures ?");
            listeTextBouton[3].text = ("Merci pour ton assistance Thot.");
            break;
         case Situation.isTutoLD: 
            StartCoroutine(Type("Pour te repérer dans le royaume des morts, observe bien les différentes portes, elles t'indiqueront le chemin à suivre."));
            listeTextBouton[0].text = ("Les salles normales ?");
            listeTextBouton[1].text = ("Les salles challenges ?");
            listeTextBouton[2].text = ("Les salles Ethérées ?");
            listeTextBouton[3].text = ("C'est d'accord, merci pour tes conseils.");
            break;
      }
   }
   
   public void UpdateSituation()
   {
      switch (boutonPressed)
      {
         case OptionChoisie.A:
            if (activeSituation == Situation.isHub)
            {
               SceneManager.LoadScene("Tuto Combat");
            }
            if (activeSituation == Situation.isTutoCombat)
            {
               StopDialogue();
               StartCoroutine(Type("Souvient toi de ton entrainement au Sceptre. Tu peut frapper les ennemis avec ton arme en utilisant le Clique Gauche. Avec le clique droit, tu pourras les repousser avec. Reste en mouvement en appuyant sur Espace pour effectuer un dash."));
            }
            if (activeSituation == Situation.isTutoPouvoirs)
            {
               StopDialogue();
               StartCoroutine(Type("Pour te repérer dans le royaume des morts, observe bien les differentes portes, elles t'indiqueront le chemin à suivre."));
            }
            if (activeSituation == Situation.isTutoShop)
            {
               StopDialogue();
               StartCoroutine(Type("Pour te repérer dans le royaume des morts, observe bien les differentes portes, elles t'indiqueront le chemin à suivre."));
            }
            if (activeSituation == Situation.isTutoLD)
            {
               StopDialogue();
               StartCoroutine(Type("Les salles normales sont indiquées par les portes avec un glyphe en forme de Y. Elles contiennent des dangers et des récompenses standards."));
            }
            break;
         case OptionChoisie.B:
            if (activeSituation == Situation.isHub)
            {
               DialogueDOWN();
               tableauDesScores.SetActive(true);
               CharacterController.instance.allowMovements = false;
            }
            if (activeSituation == Situation.isTutoCombat)
            {
               StopDialogue();
               StartCoroutine(Type("Tu rencontreras les guerries morts-vivants, très résistants mais peu mobiles. Les loups eux sont très rapides, leur charge oblige à rester en mouvment. Les corbeaux eux sont plus faibles, mais gardent leur distances pour attaquer. "));
            }
            if (activeSituation == Situation.isTutoPouvoirs)
            {
               
            }
            if (activeSituation == Situation.isTutoShop)
            {
               
            }
            if (activeSituation == Situation.isTutoLD)
            {
               StopDialogue();
               StartCoroutine(Type("Les salles Challenge sont indiquées par les portes avec un glyphe en forme de H. Elles contiennent de grands dangers mais promettent des récompenses importantes à la clé. Soit sûr que tu es prêt avant d'entrer dans une de ces salles."));
            }
            break;
         case OptionChoisie.C:
            if (activeSituation == Situation.isHub)
            {
               SceneManager.LoadScene("Cinematique Intro");
            }
            if (activeSituation == Situation.isTutoCombat)
            {
               StopDialogue();
               StartCoroutine(Type("Les ennemis élites sont des ennemis rendus plus puissants par l'influence de Loki. Ils sont insensibles aux repoussements et infligent de plus gros dégâts."));
            }
            if (activeSituation == Situation.isTutoPouvoirs)
            {
               
            }
            if (activeSituation == Situation.isTutoShop)
            {
               
            }
            if (activeSituation == Situation.isTutoLD)
            {
               StopDialogue();
               StartCoroutine(Type("Tu pourras trouver les salles éthérées en passant par les portails violets. Dans ces salles, tu pourras communiquer avec moi. Ramène-moi les âmes que tu récupères pour que je les transforme en améliorations pour ton arme."));
            }
            break;
         case OptionChoisie.D:
            DialogueDOWN();
            break;
         
      }
   }


   public void ChoseA()
   {
      boutonPressed = OptionChoisie.A;
   }
   public void ChoseB()
   {
      boutonPressed = OptionChoisie.B;
   }
   public void ChoseC()
   {
      boutonPressed = OptionChoisie.C;
   }
   public void ChoseD()
   {
      boutonPressed = OptionChoisie.D;
   }

   // ******************************* Fonction Maîtres **************************************************************************************
   
   public void DialogueUP()
   {
      menuState = true;
      dialogueMenu.SetActive(true);
      dialogueMenu.transform.DOLocalMove(new Vector3(transform.position.x,upDownY,transform.position.z),menuSpeed);
   }
   
   public void DialogueDOWN()
   {
      menuState = false;
      UiManager.instance.isSousMenu = false;
      isTalkable = true;
      StopDialogue();
      isFade = 2;
      //StartCoroutine(Type("Bonne chance Anubis, je veillerais sur toi."));
      dialogueMenu.transform.DOLocalMove(new Vector3(transform.position.x,upDownY - 440,transform.position.z),menuSpeed);
      CharacterController.instance.allowMovements = true;
      dialogueMenu.SetActive(false);
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
      textDialogues.text = String.Empty.Substring(0);
      
   }


   IEnumerator GetData()
   {
      yield return new WaitForSeconds(0.1f);
      CanvasInteraction = GameObject.FindWithTag("CanvasInteraction");
      TextInteraction = GameObject.Find("TexteAction").GetComponent<TextMeshProUGUI>();
      dialogueMenu = GameObject.Find("MenuDialogues");
      textDialogues = GameObject.Find("TextDialogues").GetComponent<TextMeshProUGUI>();
      BlackScreen = GameObject.Find("TransitionFonduDialogues").GetComponent<CanvasGroup>();
   }
}
