
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTuto : MonoBehaviour
{
    public int currentRoomIndex;
    public bool isNext;
    public bool isBack;
    

    public void NextRoom()
    {
        if (isNext)
        {
            currentRoomIndex++;
        }
        else
        {
            currentRoomIndex--;
        }
        
        
        if (currentRoomIndex == 0)
        {
            SceneManager.LoadScene("Tuto Combat");
        }
        if (currentRoomIndex == 1)
        {
            SceneManager.LoadScene("Tuto Pouvoir");
        }
        if (currentRoomIndex == 2)
        {
            SceneManager.LoadScene("Tuto Shop");
        }
        if (currentRoomIndex == 3)
        {
            SceneManager.LoadScene("Tuto LD");
        }
        if (currentRoomIndex == 4)
        {
            SceneManager.LoadScene("Hub");
        }
    }

  
}
