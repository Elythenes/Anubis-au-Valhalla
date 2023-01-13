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
    public Animator anim;
    public bool MoveIn;

    public KeyCode keySkipThot = KeyCode.V;

    public float tempsRestantsThot;

    public string generalAmorce;

    [Header("DEBUG")]
    public bool isThotHere;
    
    
    //Fonctions : Systèmes ************************************************************************************************************************
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (MoveIn)
        {
            SummonThot();
        }

      
    }
    
    
    //Fonctions ************************************************************************************************************************
    
    public void SummonThot() //appelé quand on monte de level
    {
        boxTextThot.text = generalAmorce;
        anim.SetBool("MoveOut",false);
        anim.SetBool("MoveIn",true);
        isThotHere = true;
        StartCoroutine(ThotReste());
    }

    private IEnumerator ThotReste()
    {
        yield return new WaitForSecondsRealtime(tempsRestantsThot);
        anim.SetBool("MoveOut",true);
        anim.SetBool("MoveIn",false);
        isThotHere = false;
        MoveIn = false;
    }
    
}
