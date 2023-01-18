using System;
using System.Collections;
using System.Collections.Generic;
using GenPro;
using UnityEngine;

public class SpawnCircle : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip audioCast;

    private float ouiouiTransparanceDeMesCouilles = 0;
    // Start is called before the first frame update
    void Start()
    {
        audioSource.PlayOneShot(audioCast);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, 1.9f);
        _spriteRenderer.color = new Color(_spriteRenderer.color.r,_spriteRenderer.color.g,_spriteRenderer.color.b,0);
    }

    private void FixedUpdate()
    {
        ouiouiTransparanceDeMesCouilles = Mathf.Lerp(ouiouiTransparanceDeMesCouilles, 1, 0.05f);
        _spriteRenderer.color = new Color(_spriteRenderer.color.r,_spriteRenderer.color.g,_spriteRenderer.color.b,ouiouiTransparanceDeMesCouilles);
    }
}
