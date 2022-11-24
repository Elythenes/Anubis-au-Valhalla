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
    public float camBaseSize = 7f;

    public float zoomSpeed;

    private bool open = false;

    public CanvasGroup shopUI;

    public GameObject weaponShop;

    private GlyphInventory glyphUpdater;
    public UpragesList upgradesList;
    
    // Start is called before the first frame update
    [Serializable]
    public class UpragesList
    {
        public List<LameUps> UpsLame;
        public List<HampeUps> UpsHampe;
        public List<MancheUps> UpsManche;
    }
    [Serializable]
    public class LameUps
    {
        public List<ScriptableObject> Lames;
    }
    [Serializable]
    public class HampeUps
    {
        public List<ScriptableObject> Hampes;
    }
    [Serializable]
    public class MancheUps
    {
        public List<ScriptableObject> Manches;
    }   
    void Awake()
    {
        cam = Camera.main;
        glyphUpdater = GameObject.Find("GlyphManager").GetComponent<GlyphInventory>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (open && cam.orthographicSize > 2)
        {
            OpenShop();
        }

        if (!open)
        {
            QuitShop();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            shopUI.gameObject.SetActive(true);
            open = true;
            CharacterController.instance.controls.Disable();
            CharacterController.instance.canDash = false;
            AttaquesNormales.instance.canAttack = false;
            shopUI.DOFade(1, 1);
            SalleGennerator.instance.DisableOnShop.SetActive(false);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    void OpenShop()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 1.9f, zoomSpeed);
    }

    void QuitShop()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, camBaseSize, zoomSpeed);
    }

    public void CloseShop()
    {
        open = false;
        CharacterController.instance.controls.Enable();
        CharacterController.instance.canDash = true;
        AttaquesNormales.instance.canAttack = true;
        shopUI.DOFade(0, 1).OnComplete(() =>
        {
            shopUI.gameObject.SetActive(false);
        });
        SalleGennerator.instance.DisableOnShop.SetActive(true);

    }

    public void MoveCam(int buttonNumber)
    {
        cam.orthographicSize = 1;
        weaponShop.transform.localScale = Vector3.one * 3;
        if (buttonNumber == 0)
        {
            weaponShop.transform.Translate(230,20,0);
        }
        if (buttonNumber == 1)
        {
            weaponShop.transform.Translate(0,20,0);
        }
        if (buttonNumber == 2)
        {
            weaponShop.transform.Translate(-210,20,0);
        }

    }
}
