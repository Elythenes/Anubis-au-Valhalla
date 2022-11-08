using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMonstreManager : MonoBehaviour
{
    public Transform monstreAsuivre;
    public RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (monstreAsuivre != null)
        {
            rectTransform.anchoredPosition = monstreAsuivre.localPosition;
        }
    }
}
