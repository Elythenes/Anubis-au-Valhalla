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
            StartCoroutine(Type("Bonjour Anubis, je serais à tes côtés pour te guider dans ta quête de vengeance."));
            listeTextBouton[0].text = ("Commencer le tutoriel");
            listeTextBouton[1].text = ("Consulter le Tableau des Scores");
            listeTextBouton[2].text = ("Revoir l'introduction");
            listeTextBouton[3].text = ("Au revoir Thot");
            break;
         case Situation.isTutoCombat: 
            StartCoroutine(Type("Loki a assemblé une puissante armée de morts avec les âmes qu'il t'as volées. Tu devras te préparer à les affronter pour obtenir ta vengeance."));
            listeTextBouton[0].text = ("Rappel moi mon entraînement au sceptre.");
            listeTextBouton[1].text = ("Des conseils pour mieux combattre ?");
            listeTextBouton[2].text = ("Les ennemis élites ?");
            listeTextBouton[3].text = ("Très bien, je serais prêt.");
            break;
         case Situation.isTutoPouvoirs: 
            StartCoroutine(Type("Voyons maintenant la magie que je t'ai enseignée, elle te sera utile pour vaincre tes adversaires."));
            listeTextBouton[0].text = ("Rappel moi en quoi consiste la magie.");
            listeTextBouton[1].text = ("Comment puis-je l'utiliser ?");
            listeTextBouton[2].text = ("Puis-je améliorer ma magie ?");
            listeTextBouton[3].text = ("Je tâcherais de m'en rappeler");
            break;
         case Situation.isTutoShop: 
            StartCoroutine(Type("Dans les salles éthérées comme celle-ci, tu pourras me contacter pour échanger des âmes et améliorer ton arme. Tu peux les consulter tes améliorations avec la touche Echap. Voici quelques âmes, essaye de les dépenser à l'autel."));
            listeTextBouton[0].text = ("Comment récolter des âmes ?");
            listeTextBouton[1].text = ("Quelles améliorations peut-tu m'offrir ?");
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
               StartCoroutine(Type("Souvient toi de mes leçons. Tu peux frapper les ennemis avec ton arme en utilisant le clic gauche. Avec le clic droit, tu peux les repousser d'un coup d'estoc. Reste en mouvement en appuyant sur Espace pour effectuer un dash."));
            }
            if (activeSituation == Situation.isTutoPouvoirs)
            {
               StopDialogue();
               StartCoroutine(Type("Tes deux pouvoirs d'âme et de sable sont des sorts permettant d'infliger des dégâts ou des effets aux ennemis. Active les avec les touches E et R. Tu seras alors sous l'effet d'un pouvoir pendant 5 secondes."));
            }
            if (activeSituation == Situation.isTutoShop)
            {
               StopDialogue();
               StartCoroutine(Type("En tuant des ennemis, tu récupérera les âmes que Loki leur a insufflées. Les coffres que tu trouveras dans les salles renferment également des âmes perdues."));
            }
            if (activeSituation == Situation.isTutoLD)
            {
               StopDialogue();
               StartCoroutine(Type("Les salles normales sont indiquées par les portes portant une rune Y (comme la porte de gauche). Elles contiennent des dangers et des récompenses standard."));
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
               StartCoroutine(Type("Pendant un dash, tu n'es pas sensible aux dégâts. Tu peux même entrer en contact avec une attaque ennemie pour effectuer une esquive. Certaines améliorations pourront donner des propriétés à ton esquive."));
            }
            if (activeSituation == Situation.isTutoPouvoirs)
            {
               StopDialogue();
               StartCoroutine(Type("Une fois sous l'effet d'un pouvoir, des effets supplémentaires sont ajoutés à ton estoc, ton dash et à la troisième attaque de ton combo. Tu peux donc associer mouvement, attaque et magie pour prendre l'avantage au combat."));
            }
            if (activeSituation == Situation.isTutoShop)
            {
               StopDialogue();
               StartCoroutine(Type("Dans les autels, tu auras le choix d'améliorer la lame, la hampe ou la poignée de ton arme. Tu auras le choix entre 3 améliorations par autel. Celles-ci sont divisée en 3 raretés : Prêtre, Pharaon et Divinité, qui correspondent à 3 niveaux de puissance."));
            }
            if (activeSituation == Situation.isTutoLD)
            {
               StopDialogue();
               StartCoroutine(Type("Les salles Challenges sont indiquées par les portes marquées d'une rune H (comme la porte du centre). Elles contiennent de grands dangers mais promettent des récompenses importantes à la clé. Soit sûr d'être prêt avant d'entrer d'y entrer."));
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
               StartCoroutine(Type("Les ennemis élites sont des ennemis rendus plus puissants par l'influence de Loki. Ils sont plus puissants et résistants et sont insensibles aux repoussements."));
            }
            if (activeSituation == Situation.isTutoPouvoirs)
            {
               StopDialogue();
               StartCoroutine(Type("Dans les coffres du Valhalla, tu trouveras des orbes de pouvoir qui amélioreront les effets de tes magies. Les orbes violets amélioreront ton pouvoir d'âme et les jaunes, ton pouvoir de sable. Tu peux consulter leurs améliorations actives en appuyant sur Echap."));

            }
            if (activeSituation == Situation.isTutoShop)
            {
               StopDialogue();
               StartCoroutine(Type("Tu peux utiliser l'énergie des âmes pour regagner de la santé. Utilise l'un des trois vases Canopes sur la droite. Ils te rendront respectivement 10, 25 ou 50% de ta santé si tu y dépose des âmes."));

            }
            if (activeSituation == Situation.isTutoLD)
            {
               StopDialogue();
               StartCoroutine(Type("Tu pourras trouver les salles éthérées en passant par les portails violets (comme la porte de droite). Dans ces salles, tu pourras communiquer avec moi. Ramène-moi les âmes que tu récupères pour que je les transforme en améliorations pour ton arme."));
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
