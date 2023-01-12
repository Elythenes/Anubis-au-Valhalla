using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public void RestartRun()
    {
        SceneManager.LoadScene("Test");
    }

    public void BackHub()
    {
        SceneManager.LoadScene("Hub");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
