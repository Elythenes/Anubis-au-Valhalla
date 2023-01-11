
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuOptions : MonoBehaviour
{
  private List<int> widths = new List<int>() { 568, 960, 1280, 1920};
  private List<int> heights = new List<int>() {320,540,800,1080};

  public AudioMixer audioMixer;
  public Slider sliderMaster;
  public Slider sliderMusic;
  public Slider sliderSFX;
  public TextMeshProUGUI textMaster, textMusic, textSFX;


  private void Update()
  {
      if (Input.GetKeyDown(KeyCode.Escape))
      {
        gameObject.SetActive(false);
        UiManager.instance.ActivatePause();
        UiManager.instance.isSousMenu = false;
        UiManager.instance.PlayButtonSound();
      }
  }

  public void SetScreenSize(int index)
  {
    bool fullscreen = Screen.fullScreen;
    int width = widths[index];
    int height = heights[index];
    Screen.SetResolution(width,height,fullscreen);
  }

  public void SetMasterVolume()
  {
    audioMixer.SetFloat("MasterVolume", sliderMaster.value);
    textMaster.text = Mathf.RoundToInt(sliderMaster.value + 80).ToString();
  }
  
  public void SetMusicVolume()
  {
    audioMixer.SetFloat("MusicVolume", sliderMusic.value);
    textMusic.text = Mathf.RoundToInt(sliderMusic.value + 80).ToString();
  }
  
  public void SetSFXVolume()
  {
    audioMixer.SetFloat("SFXVolume", sliderSFX.value);
    textSFX.text = Mathf.RoundToInt(sliderSFX.value + 80).ToString();
  }
  public void SetFullScreen(bool fullscreen)
  {
    Screen.fullScreen = fullscreen;
  }
}
