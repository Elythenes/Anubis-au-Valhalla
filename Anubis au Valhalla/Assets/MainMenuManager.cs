using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

   [Header("Elements du menu")] 
   public GameObject PressButtonText;
   public GameObject GroupButtons;
   public CanvasGroup GroupButtonsCanvas;


   public AudioSource AudioSource;
   
   public bool isStarted;


   public void Update()
   {
      if (!isStarted)
      {
         if (Input.anyKeyDown)
         {
            AudioSource.Play();
            isStarted = true;
            PressButtonText.SetActive(false);
            GroupButtons.SetActive(true);
         }
      }

      if (isStarted)
      {
         if (GroupButtonsCanvas.alpha < 1)
         {
            GroupButtonsCanvas.alpha += Time.deltaTime * 0.7f;
         }
      }
   }



   public void Quit()
   {
      Application.Quit();
   }


   public void StartGame()
   {
      SceneManager.LoadScene("Cinematique Intro");
   }


   public void Options()
   {
      
   }
}
