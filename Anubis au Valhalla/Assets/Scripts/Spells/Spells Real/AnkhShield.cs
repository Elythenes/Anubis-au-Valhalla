using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnkhShield : MonoBehaviour
{
    public SpellDefenceObject sOAnkhShield;
    public static AnkhShield instance;
    public bool isActive;
    public float secondesRestantes;
    

    private void Start()
    {
        if (instance = null)
        {
            instance = this;
        }

        DamageManager.instance.isAnkh = true;
        secondesRestantes = sOAnkhShield.secondesTotales;
    }

    public void Update()
    {
        secondesRestantes -= Time.deltaTime;

        if (secondesRestantes <= 0)
        {
            gameObject.SetActive(false);
            DamageManager.instance.isAnkh = false;
        }
    }
}
