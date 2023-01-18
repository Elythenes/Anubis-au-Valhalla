using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BarqueHub : MonoBehaviour
{
    public bool canInteract;
    public Vector3 offset;
    public GameObject CanvasInteraction;
    public TextMeshProUGUI TextInteraction;

    private void Start()
    {
        CanvasInteraction = GameObject.FindWithTag("CanvasInteraction");
        TextInteraction = GameObject.Find("TexteAction").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                TakeBarque();
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
            TextInteraction.SetText("Départ");
            CanvasInteraction.SetActive(true);
            canInteract = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            CanvasInteraction.GetComponent<Canvas>().enabled = false;
            canInteract = false;
        }
    }

    void TakeBarque()
    {
        SoundManager.instance.ChangeToZone1();
        SoundManager.instance.PlayZone1();
        SceneManager.LoadScene("Test");
    }
}
