using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{
    public static DamagePopUp instance;
    public bool isCritique;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        

        if (isCritique)
        {
            transform.DOShakePosition(1, 1.5f);
        }
    }
    
    private void Update()
    {
        Destroy(transform.parent.gameObject,1f);
    }
}
