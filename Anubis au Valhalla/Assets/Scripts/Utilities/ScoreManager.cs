using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int currentScore;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    public void AddScore(int scoreAdded)
    {
        currentScore += scoreAdded;
    }
}
