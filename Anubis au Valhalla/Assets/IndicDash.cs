using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class IndicDash : MonoBehaviour
{
    public GameObject endPointRef;

    public IA_Valkyrie ia;
    private Color baseColor = new(2.85756063f,14.8503885f,2.79902625f,1);
    private Color interColor = new(13.1758127f,14.8503885f,2.79902625f,1);
    private Color endColor = new Color(14.9633474f,2.25140452f,1.76454532f,1);

    public SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        ia = IA_Valkyrie.instance;
        sr.material.color = baseColor;
        sr.material.DOColor(endColor, ia.windUpSpeed[(int)ia.currentState]);
        var refe = Instantiate(endPointRef, Vector3.forward * ia.chargeDistance, Quaternion.identity,transform);
        refe.transform.localScale = new Vector3(1/transform.localScale.x,1,1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
