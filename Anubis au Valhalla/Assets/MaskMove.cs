using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MaskMove : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        var ia = IA_Valkyrie.instance;
        transform.DOScale(Vector3.one, ia.windUpSpeed[(int)ia.currentState]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
