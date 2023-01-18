
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

  public TMP_Dropdown resolutionDropdown;
  private Resolution[] resolutions;
  
  public AudioMixer audioMixer;
  public Slider sliderMaster;
  public Slider sliderMusic;
  public Slider sliderSFX;
  public TextMeshProUGUI textMaster, textMusic, textSFX;

  private void Start()
  {
    resolutions = Screen.resolutions;

    resolutionDropdown.ClearOptions();

    List<string> options = new List<string>();

    int currentResolutionIndex = 0;

    for (int i = 0; i < resolutions.Length; i++)
    {
      string option = resolutions[i].width + "x" + resolutions[i].height;
      options.Add(option);

      if (resolutions[i].width == Screen.currentResolution.width
          && resolutions[i].height == Screen.currentResolution.height)
      {
        currentResolutionIndex = i;
      }
    }
    
    resolutionDropdown.AddOptions(options);
    resolutionDropdown.value = currentResolutionIndex;
    resolutionDropdown.RefreshShownValue();
  }

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

  public void SetScreenSize(int resolutionIndex)
  {
    Resolution resolution = resolutions[resolutionIndex];
    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
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
