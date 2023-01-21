
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public static VictoryScreen instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void UpdateScore()
    {
        scoreText.text = ScoreManager.instance.currentScore.ToString();
    }
    
    public void RestartRun()
    {
        ScoreManager.instance.currentScore = 0;
        SoundManager.instance.audioSource.Stop();
        SoundManager.instance.ChangeToZone1();
        SoundManager.instance.PlayZone1();
        SceneManager.LoadScene("Test");
    }

    public void BackHub()
    {
        MenuHighScore.instance.AddHighScoreEntry(ScoreManager.instance.currentScore);
        SoundManager.instance.ChangeToHub();
        SoundManager.instance.PlayHub();
        SoundManager.instance.audioSource.Stop();
        ScoreManager.instance.currentScore = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene("Hub");
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
