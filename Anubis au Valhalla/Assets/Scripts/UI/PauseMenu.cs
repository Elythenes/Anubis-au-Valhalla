using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public void RestartRun()
    {
        SceneManager.LoadScene("Test");
        SoundManager.instance.audioSource.Stop();
        SoundManager.instance.ChangeToZone1();
        SoundManager.instance.PlayZone1();
    }

    public void BackHub()
    {
        SceneManager.LoadScene("Hub");
        SoundManager.instance.audioSource.Stop();
        SoundManager.instance.ChangeToHub();
        SoundManager.instance.PlayHub();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
