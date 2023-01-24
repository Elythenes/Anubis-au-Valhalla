using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public MeshRenderer player;
    public bool isDisolve;
    public float disolveValue = 1;
    public float timeToLastStep;
    public float timeToLastStepDisolve;

    private void Awake()
    {
        disolveValue = 1;
        player = GameObject.Find("Personnage Spine").GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (isDisolve)
        {
            StartCoroutine(AnimationDisolve());
            disolveValue -= 0.003f;
            player.material.SetFloat("_Step", disolveValue);
        }
    }
    
    public void RestartRun()
    {
        isDisolve = true;
        StartCoroutine(Restart());
    }

    public void BackHub()
    {
        isDisolve = true;
        StartCoroutine(BackHubTimer());
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    IEnumerator AnimationDisolve()
    {
        Time.timeScale = 1;
        float timeElapsed = 0;
        while (timeElapsed < timeToLastStep)
        {
            disolveValue = Mathf.Lerp(1, 0, timeElapsed / timeToLastStep);
            timeElapsed += 0.5f * Time.deltaTime;
            yield return null;
        }
    }
    
    IEnumerator Restart()
    {
        ScoreManager.instance.currentScore = 0;
        yield return new WaitForSecondsRealtime(timeToLastStep);
        SoundManager.instance.ChangeToZone1();
        SoundManager.instance.PlayZone1();
        SceneManager.LoadScene("Test");
    }
    
    IEnumerator BackHubTimer()
    {
        yield return new WaitForSecondsRealtime(timeToLastStep);
        //ScoreManager.instance.AddHighScoreEntry(ScoreManager.instance.currentScore);
        SoundManager.instance.ChangeToHub();
        SoundManager.instance.PlayHub();
        ScoreManager.instance.currentScore = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene("Hub");
    }
    
  
}
