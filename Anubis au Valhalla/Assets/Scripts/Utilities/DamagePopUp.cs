using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{
    private TextMeshPro textMesh;
    public static DamagePopUp instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        textMesh = transform.GetComponent<TextMeshPro>();
    }
    
    private void Update()
    {
        Destroy(transform.parent.gameObject,1f);
    }
}