using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptions : MonoBehaviour
{
  private List<int> widths = new List<int>() { 568, 960, 1280, 1920};
  private List<int> heights = new List<int>() {320,540,800,1080};



  public void SetScreenSize(int index)
  {
    bool fullscreen = Screen.fullScreen;
    int width = widths[index];
    int height = heights[index];
    Screen.SetResolution(width,height,fullscreen);
  }

  public void SetFullScreen(bool fullscreen)
  {
    Screen.fullScreen = fullscreen;
  }
}
