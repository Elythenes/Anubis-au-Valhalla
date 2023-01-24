using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public SpriteRenderer[] sprite;

    public Color endColor;

    public float timeToFade;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var oui in sprite)
        {
            oui.color = Color.Lerp(oui.color,endColor,timeToFade);
        }
    }
}
