using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Souls : MonoBehaviour
{

    public GameObject soul;

    public static Souls instance;

    public int soulBank = 0;
    public TextMeshProUGUI soulText;
    public TextMeshProUGUI soulTextShop;
    private RectTransform baseTextTransform;
    public float shakeIntensity;
    public List<GameObject> soulsInScene;
    public AudioSource audiosource;
    public AudioClip getSoul;
    public bool isSoundOn;
    public float soundTime;
    public float soundTimeTimer;

    void Awake()
    {
        //baseTextTransform = soulText.rectTransform;
        if (instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            instance = this;
        }
        
        UpdateSoulsCounter();
    }

    private void Update()
    {
        if (isSoundOn)
        {
            soundTimeTimer += Time.timeScale;
            if (soundTimeTimer >= soundTime)
            {
                isSoundOn = false;
                soundTimeTimer = 0;
            }
        }
    }

    public void CreateSouls(Vector2 ennemyPos, int soulAmount)
    {
        for (int i = 0; i <= soulAmount; i++)
        {
            soulsInScene.Add(Instantiate(soul, ennemyPos, Quaternion.Euler(0,0,0)));
        }
    }

    public void CollectSouls(GameObject collectedSoul, int value)
    {
        if (!isSoundOn)
        {
            audiosource.pitch = Random.Range(0.8f, 1.2f);
            audiosource.PlayOneShot(getSoul,1.5f);
            isSoundOn = true;
        }
        soulsInScene.Remove(collectedSoul);
        Destroy(collectedSoul);
        soulBank += value;
        UpdateSoulsCounter();
    }

    public void ClearOfSouls()
    {
        List<GameObject> souls = soulsInScene;
        for (int i = 0; i < souls.Count; i++)
        {
            CollectSouls(soulsInScene[0],1);
        }
    }

    public void UpdateSoulsCounter()
    {
        soulText.text = soulBank.ToString();
    }

    public void UpdateSoulsShop()
    {
        soulTextShop = GameObject.Find("Ames Shop").GetComponent<TextMeshProUGUI>();
        soulTextShop.text = soulBank.ToString();
        UpdateSoulsCounter();
    }
}
