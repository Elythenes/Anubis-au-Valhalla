using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public bool isMainMenu;
    public bool isIntro;
    public bool isHub;
    public bool isZone1;
    public bool isShop;
    public bool isZone2;
    public bool isBoss;
        
        
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] audioClipArray;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        if (isMainMenu)
        {
            audioSource.clip = audioClipArray[0];
            audioSource.Stop();
            audioSource.Play();
        }
        if (isIntro)
        {
            audioSource.clip = audioClipArray[1];
            audioSource.Stop();
            audioSource.Play();
        }
        if (isHub)
        {
            audioSource.clip = audioClipArray[2];
            audioSource.Stop();
            audioSource.Play();
        }
        if (isZone1)
        {
            audioSource.clip = audioClipArray[3];
            audioSource.Stop();
            audioSource.Play();
        }
        if (isShop)
        {
            audioSource.clip = audioClipArray[4];
            audioSource.Stop();
            audioSource.Play();
        }
        if (isZone2)
        {
            audioSource.clip = audioClipArray[5];
            audioSource.Stop();
            audioSource.Play();
        }
        if (isBoss)
        {
            audioSource.clip = audioClipArray[6];
            audioSource.Stop();
            audioSource.Play();
        }
        
    }

    public void PlayMainMenu()
    {
        audioSource.clip = audioClipArray[0];
        audioSource.Stop();
        audioSource.Play();
    }
    
    public void PlayIntro()
    {
        audioSource.clip = audioClipArray[1];
        audioSource.Stop();
        audioSource.Play();
    }
    
    public void PlayHub()
    {
        audioSource.clip = audioClipArray[2];
        audioSource.Stop();
        audioSource.Play();
    }
    
    public void PlayZone1()
    {
        audioSource.clip = audioClipArray[3];
        audioSource.Stop();
        audioSource.Play();
    }
    
    public void PlayShop()
    {
        audioSource.clip = audioClipArray[4];
        audioSource.Stop();
        audioSource.Play();
    }
    
    public void PlayZone2()
    {
        audioSource.clip = audioClipArray[5];
        audioSource.Stop();
        audioSource.Play();
    }
    
    public void PlayBoss()
    {
        audioSource.clip = audioClipArray[6];
        audioSource.Stop();
        audioSource.Play();
    }
    
   //-----------------------------------------------------------------------------
 
    public void ChangeToIntro()
    {
        isMainMenu = false;
        isIntro = true;
    }
    
    public void ChangeToHub()
    {
        isIntro = false;
        isHub = true;
    }
    
    public void ChangeToZone1()
    {
        isHub = false;
        isZone1 = true;
        PlayZone1();
    }
    
    public void ChangeToShop()
    {
        isZone1 = false;
        isZone2 = false;
        isShop = true;
        PlayShop();
    }
    
    public void ChangeToZone2()
    {
        isShop = false;
        isZone1 = false;
        isZone2 = true;
        PlayZone2();
    }
    
    public void ChangeToBoss()
    {
        isZone2 = false;
        isBoss = true;
    }
}
