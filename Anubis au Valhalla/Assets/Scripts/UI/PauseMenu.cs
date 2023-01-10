using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public MeshRenderer player;
    public bool isDisolve;
    public float disolveValue = 1;
    public float timeToLastStep;

    private void Awake()
    {
        disolveValue = 1;
        player = GameObject.Find("Personnage Spine").GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (isDisolve)
        {
            //StartCoroutine(AnimationDisolve());
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
        yield return new WaitForSecondsRealtime(timeToLastStep);
        SceneManager.LoadScene("Test");
    }
    
    IEnumerator BackHubTimer()
    {
        yield return new WaitForSecondsRealtime(timeToLastStep);
        SceneManager.LoadScene("Hub");
    }
}
