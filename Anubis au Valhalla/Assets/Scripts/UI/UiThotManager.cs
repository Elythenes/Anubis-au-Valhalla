using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UiThotManager : MonoBehaviour
{
    public static UiThotManager Instance;

    public GameObject boxThot;
    public TextMeshProUGUI boxTextThot;

    public KeyCode keySkipThot = KeyCode.V;

    public float tempsRestantsThot;

    public string generalAmorce;

    [Header("DEBUG")]
    private IEnumerator Thot;
    public bool isThotHere;
    
    
    //Fonctions : Systèmes ************************************************************************************************************************
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        Thot = ThotReste();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(keySkipThot))
        {
            KillThotEarlier();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            SummonThot();
        }
    }
    
    
    //Fonctions ************************************************************************************************************************
    
    public void SummonThot() //appelé quand on monte de level
    {
        Debug.Log("Yé soui là");
        isThotHere = true;
        boxThot.SetActive(true);
        StartCoroutine(Thot);
    }

    public void KillThotEarlier()
    {
        if (isThotHere)
        {
            Debug.Log("ARG");
            boxThot.SetActive(false);
            isThotHere = false;
            StopCoroutine(Thot);
        }
        else
        {
            Debug.Log("Comment tu peux me tuer, je suis pas là");
        }
    }

    private IEnumerator ThotReste()
    {
        yield return new WaitForSecondsRealtime(tempsRestantsThot);
        Debug.Log("bye bye");
        boxThot.SetActive(false);
        isThotHere = false;
        
    }
    
}
