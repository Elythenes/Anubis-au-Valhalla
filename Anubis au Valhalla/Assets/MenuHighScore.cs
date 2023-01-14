using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuHighScore : MonoBehaviour
{
    public Transform container;
    public Transform template;
    public float espacementEntres;
    private List<HighScoreEntry> HighScoreEntrieList;
    public List<Transform> highscoreEntryTransformList;


    void Awake()
    {
        template.gameObject.SetActive(false);

        HighScoreEntrieList = new List<HighScoreEntry>()
        {
            new HighScoreEntry{score = 999},
            new HighScoreEntry{score = 214},
            new HighScoreEntry{score = 453},
            new HighScoreEntry{score = 156},
            new HighScoreEntry{score = 156},
        };

       string jsonString = PlayerPrefs.GetString("highscoreTable");
       HighScores highscores = JsonUtility.FromJson<HighScores>(jsonString);
       Debug.Log(PlayerPrefs.GetString("highscoreTable"));
       
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
        foreach (HighScoreEntry highscoreEntry in HighScoreEntrieList)
        {
            CreateScoreEntry(highscoreEntry,container,highscoreEntryTransformList);
        }

     /*   HighScores highScores = new HighScores { HighScoreEntriesList = HighScoreEntrieList };
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString("highscoreTable",json);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString("highscoreTable"));*/
    }


    void CreateScoreEntry(HighScoreEntry highScoreEntry, Transform container, List<Transform> transformList)
    {
        
            Transform templateObj = Instantiate(template, container);
            RectTransform entryRectTransform = templateObj.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -espacementEntres * transformList.Count);
            entryRectTransform.gameObject.SetActive(true);
            entryRectTransform.Find(("ScoreTexte")).GetComponent<TextMeshProUGUI>().text = highScoreEntry.score.ToString();
            int rank = transformList.Count + 1;
            entryRectTransform.Find("PositionTexte").GetComponent<TextMeshProUGUI>().text = rank.ToString();
            transformList.Add(entryRectTransform);
    }
    
    
    private class HighScores
    {
        public List<HighScoreEntry> HighScoreEntriesList;
    }
    
    [System.Serializable]
    public class HighScoreEntry
    {
        public int score;
        //public int name;
    }
    
}
