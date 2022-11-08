using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Camera cam;

    public float zoomSpeed;

    private bool open = false;

    public CanvasGroup shopUI;

    public GameObject weaponShop;
    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (open && cam.orthographicSize >= 2)
        {
            OpenShop();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            open = true;
            CharacterController.instance.controls.Disable();
            CharacterController.instance.canDash = false;
            AttaquesNormales.instance.canAttack = false;
            shopUI.DOFade(1, 1);
        }
    }

    void OpenShop()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 1.9f, zoomSpeed);
    }

    public void MoveCam(int buttonNumber)
    {
        cam.orthographicSize = 1;
        weaponShop.transform.localScale = Vector3.one * 3;
        if (buttonNumber == 0)
        {
            weaponShop.transform.Translate(230,0,0);
        }
        if (buttonNumber == 1)
        {
            weaponShop.transform.Translate(0,0,0);
        }
        if (buttonNumber == 2)
        {
            weaponShop.transform.Translate(-210,0,0);
        }

    }
}
