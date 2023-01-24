using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Credit : MonoBehaviour
{
    public GameObject uiCredit;
    
    [Header("Interaction")]
    public bool canInteract;
    public GameObject CanvasInteraction;
    public Vector3 offset;
    public TextMeshProUGUI TextInteraction;


    private void Awake()
    {
        CanvasInteraction = GameObject.FindWithTag("CanvasInteraction");
        TextInteraction = GameObject.Find("TexteAction").GetComponent<TextMeshProUGUI>();
        
        uiCredit.SetActive(false);
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canInteract)
        {
            ShowCredits();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetOffCredit();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            CanvasInteraction.GetComponent<Canvas>().enabled = true;
            canInteract = true;
            CanvasInteraction.SetActive(true);
            CanvasInteraction.transform.position = transform.transform.position + offset;
            CanvasInteraction.transform.localScale = new Vector3(0, 0, CanvasInteraction.transform.localScale.z);
            CanvasInteraction.transform.DOScale(new Vector3(1, 1, CanvasInteraction.transform.localScale.z), 0.25f);
            TextInteraction.SetText("Regarder");
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) //c'est du Debug, ne sert pas vraiment
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanvasInteraction.GetComponent<Canvas>().enabled = false;
            //CanvasInteraction.SetActive(false);
            canInteract = false;
        }
    }

    public void ShowCredits()
    {
        uiCredit.SetActive(true);
        UiManager.instance.Pause();
    }

    public void SetOffCredit()
    {
        uiCredit.SetActive(false);
        UiManager.instance.TimeBack();
    }
}
