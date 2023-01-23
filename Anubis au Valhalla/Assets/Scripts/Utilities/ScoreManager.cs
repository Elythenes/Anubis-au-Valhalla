using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MenuHighScore
{
    public static ScoreManager instance;
    public int currentScore;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void AddScore(int scoreAdded)
    {
        currentScore += scoreAdded;
    }

    public override void AddHighScoreEntry(int score)
    {
        base.AddHighScoreEntry(score);
    }
}
