using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
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
        Destroy(collectedSoul);
        soulBank += value;
        soulsInScene.Remove(collectedSoul);
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
