using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GenPro;
using TMPro;
using UnityEngine;

public class PotDeVie : MonoBehaviour
{
    public TextMeshProUGUI lifeText;

    public TextMeshProUGUI costText;
    public int[] healValues;
    public int[] costValues;
    
    public GameObject CanvasInteraction;
    public TextMeshProUGUI TextInteraction;
    public Vector3 offset;
    public Shop shop;
    public float priceMultiplicator;
    public enum Type
    {
        First,
        Second,
        Third
    }

    public Type currentType;

    private bool canInteract;
    // Start is called before the first frame update
    void Start()
    {
        CanvasInteraction = GameObject.FindWithTag("CanvasInteraction");
        TextInteraction = GameObject.Find("TexteAction").GetComponent<TextMeshProUGUI>();
        for (int i = 0; i < costValues.Length; i++)
        {
            if (!SalleGenerator.Instance.zone2)
            {
                costValues[i] *= Mathf.RoundToInt(SalleGenerator.Instance.roomsDone * priceMultiplicator);
            }
            else
            {
                costValues[i] *= Mathf.RoundToInt((8 + SalleGenerator.Instance.roomsDone) * priceMultiplicator);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentType)
        {
            case Type.First:
                lifeText.text = healValues[0] + "%";
                costText.text = costValues[0] + "";
                if (Souls.instance.soulBank < costValues[0])
                {
                    canInteract = false;
                    costText.color = Color.red;
                    break;
                }
                costText.color = Color.white;
                break;
            case Type.Second:
                lifeText.text = healValues[1] + "%";
                costText.text = costValues[1] + "";
                if (Souls.instance.soulBank < costValues[1])
                {
                    canInteract = false;
                    costText.color = Color.red;
                    break;
                }
                costText.color = Color.white;
                break;
            case Type.Third:
                lifeText.text = healValues[2] + "%";
                costText.text = costValues[2] + "";
                if (Souls.instance.soulBank < costValues[2])
                {
                    canInteract = false;
                    costText.color = Color.red;
                    break;
                }
                costText.color = Color.white;
                break;
        }
        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.F)&& AnubisCurrentStats.instance.vieActuelle < AnubisCurrentStats.instance.vieMax )
            {
                switch (currentType)
                {
                    case Type.First:
                        DamageManager.instance.Heal(healValues[0]);
                        Souls.instance.soulBank -= costValues[0];
                        Souls.instance.UpdateSoulsCounter();
                        break;
                    case Type.Second:
                        DamageManager.instance.Heal(healValues[1]);
                        Souls.instance.soulBank -= costValues[1];
                        Souls.instance.UpdateSoulsCounter();
                        break;
                    case Type.Third:
                        DamageManager.instance.Heal(healValues[2]);
                        Souls.instance.soulBank -= costValues[2];
                        Souls.instance.UpdateSoulsCounter();
                        break;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            CanvasInteraction.GetComponent<Canvas>().enabled = true;
            CanvasInteraction.transform.position = transform.position + offset;
            CanvasInteraction.transform.localScale = new Vector3(0,0,CanvasInteraction.transform.localScale.z);
            CanvasInteraction.transform.DOScale(new Vector3(1, 1, CanvasInteraction.transform.localScale.z),0.25f);
            TextInteraction.SetText("Acheter");
            CanvasInteraction.SetActive(true);
            canInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasInteraction.GetComponent<Canvas>().enabled = false;
            canInteract = false;
        }
    }
}
