using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BarqueHub : MonoBehaviour
{
    public bool canInteract;
    public Vector3 offset;
    public GameObject canvasInteract;
    public TextMeshProUGUI textInteract;
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
            canvasInteract.transform.position = transform.position + offset;
            textInteract.SetText("Start");
            canvasInteract.SetActive(true);
            canInteract = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            canvasInteract.SetActive(false);
            canInteract = false;
        }
    }

    void TakeBarque()
    {
        SceneManager.LoadScene("Demo technique 1");
    }
}
