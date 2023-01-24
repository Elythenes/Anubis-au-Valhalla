
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MenuHighScore : MonoBehaviour
{
    public Transform container;
    public Transform template;
    public float espacementEntres;
    public List<HighScoreEntry> HighScoreEntrieList;
    public List<Transform> highscoreEntryTransformList;
    public static MenuHighScore instance;
    public GameObject noScoreText;
    public GameObject canvasParent;
    public bool isFade;
    public CanvasGroup canvasGroup;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        template.gameObject.SetActive(false);
        
        //PlayerPrefs.DeleteKey("highscoreTable");  // A n'activer que pour resset les scores
       
      // HighScoreEntrieList = new List<HighScoreEntry>(){new HighScoreEntry{score = 100}};
       
       HighScores highScores = new HighScores { HighScoreEntrieList = HighScoreEntrieList }; // Pour mettre des scores random (a initialiser dans une liste)
      string json = JsonUtility.ToJson(highScores);
      PlayerPrefs.SetString("highscoreTable",json);
      PlayerPrefs.Save();
      Debug.Log(PlayerPrefs.GetString("highscoreTable"));
      Debug.Log(json);
      
      DontDestroyOnLoad(canvasParent);
        
        
        
       string jsonString = PlayerPrefs.GetString("highscoreTable");
       HighScores highscores = JsonUtility.FromJson<HighScores>(jsonString);
     
         HighScoreEntrieList = highscores.HighScoreEntrieList;
     

    /*   if (highscores.HighScoreEntrieList is not null && highscores.HighScoreEntrieList.Count == 0)
       {
           noScoreText.SetActive(true);
       }
       else if (highscores.HighScoreEntrieList is not null && highscores.HighScoreEntrieList.Count > 0)
       {
           noScoreText.SetActive(false);
       }*/

     

       for (int i = 0; i < HighScoreEntrieList.Count; i++)
        {
            for (int j = i + 1; j < HighScoreEntrieList.Count; j++)
            {
                if (HighScoreEntrieList[j].score > HighScoreEntrieList[i].score)
                {
                    (HighScoreEntrieList[i], HighScoreEntrieList[j]) = (HighScoreEntrieList[j], HighScoreEntrieList[i]);
                }
            }
        }
        
        highscoreEntryTransformList = new List<Transform>();
        if (HighScoreEntrieList.Count >= 5)
        {
            for (int i = 0; i < 5; i++)
            {
                CreateScoreEntry(HighScoreEntrieList[i],container,highscoreEntryTransformList);
            }
        }
        else
        {
            for (int i = 0; i < HighScoreEntrieList.Count; i++)
            {
                CreateScoreEntry(HighScoreEntrieList[i],container,highscoreEntryTransformList);
            }
        }
     
    }

    public void Update()
    {
        if (isFade && canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime;
            canvasGroup.blocksRaycasts = true;
        }
        if (!isFade && canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void UpdateTable()
    {
     KillTable();
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        HighScores highscores = JsonUtility.FromJson<HighScores>(jsonString);
        if (highscores.HighScoreEntrieList is not null)
        {
            HighScoreEntrieList = highscores.HighScoreEntrieList;
        }

        if (highscores.HighScoreEntrieList is not null && highscores.HighScoreEntrieList.Count == 0)
        {
            noScoreText.SetActive(true);
        }
        else if (highscores.HighScoreEntrieList is not null && highscores.HighScoreEntrieList.Count > 0)
        {
            noScoreText.SetActive(false);
        }

     

        for (int i = 0; i < HighScoreEntrieList.Count; i++)
        {
            for (int j = i + 1; j < HighScoreEntrieList.Count; j++)
            {
                if (HighScoreEntrieList[j].score > HighScoreEntrieList[i].score)
                {
                    (HighScoreEntrieList[i], HighScoreEntrieList[j]) = (HighScoreEntrieList[j], HighScoreEntrieList[i]);
                }
            }
        }
        
        highscoreEntryTransformList = new List<Transform>();
        if (HighScoreEntrieList.Count >= 5)
        {
            for (int i = 0; i < 5; i++)
            {
                CreateScoreEntry(HighScoreEntrieList[i],container,highscoreEntryTransformList);
            }
        }
        else
        {
            for (int i = 0; i < HighScoreEntrieList.Count; i++)
            {
                CreateScoreEntry(HighScoreEntrieList[i],container,highscoreEntryTransformList);
            }
        }
    }

    public void SousMenuOff()
    {
        UiManager.instance.isSousMenu = false;
    }
    
    public void CreateScoreEntry(HighScoreEntry highScoreEntry, Transform container, List<Transform> transformList)
    {
        
            Transform templateObj = Instantiate(template, container);
            RectTransform entryRectTransform = templateObj.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -espacementEntres * transformList.Count);
            entryRectTransform.gameObject.SetActive(true);
            entryRectTransform.Find(("ScoreTexte")).GetComponent<TextMeshProUGUI>().text = highScoreEntry.score.ToString();
            int rank = transformList.Count + 1;
            entryRectTransform.Find("PositionTexte").GetComponent<TextMeshProUGUI>().text = rank.ToString();
           entryRectTransform.Find("Fond").gameObject.SetActive(rank % 2 == 1);

           if (rank == 1)
           {
               entryRectTransform.Find("PositionTexte").GetComponent<TextMeshProUGUI>().color =
                   new Color32(255, 189, 75,255);
           }
           if (rank == 2)
           {
               entryRectTransform.Find("PositionTexte").GetComponent<TextMeshProUGUI>().color =
                   new Color32(117, 126, 145,255);
           }
           if (rank == 3)
           {
               entryRectTransform.Find("PositionTexte").GetComponent<TextMeshProUGUI>().color =
                  new Color32(163,84,63,255);
           }
            transformList.Add(templateObj);
    }

    public void KillTable()
    {
        for (int i = 0; i < highscoreEntryTransformList.Count; i++)
        {
            Destroy(highscoreEntryTransformList[i].gameObject);
        }
    }
    public void QuitTable()
    {
        CharacterController.instance.allowMovements = true;
        isFade = false;
    }
    
    public virtual void AddHighScoreEntry(int score)
    {
        HighScoreEntry highscoreEntry = new HighScoreEntry {score = score};
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        HighScores highscores = JsonUtility.FromJson<HighScores>(jsonString);
        highscores.HighScoreEntrieList.Add(highscoreEntry);
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable",json);
        PlayerPrefs.Save();
        HighScoreEntrieList = highscores.HighScoreEntrieList;
        
    }
    private class HighScores
    {
        public List<HighScoreEntry> HighScoreEntrieList;
    }
    
    [System.Serializable]
    public class HighScoreEntry
    {
        public int score;
        //public int name;
    }
    
}
