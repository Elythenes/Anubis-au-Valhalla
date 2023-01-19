using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CinematiqueBoss : MonoBehaviour
{
    
    [Header("Volume")]
    public Volume volumeCinematique;
    public float disolveValue;
    public float timeToLastStep;
    
    [Header("Volume")]
    public float smoothCine;
    public GameObject camera;
    public GameObject valkyrie;

    [Header("Autre")] 
    public GameObject gameUI;
    
    void Start()
    {
        camera = GameObject.Find("Main Camera");
    }

    public void Update()
    {
        volumeCinematique.weight = disolveValue;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            CharacterController.instance.isCinematic = true;
            CharacterController.instance.allowMovements = false;
            CharacterController.instance.rb.velocity = Vector2.zero;
            CharacterController.instance.canDash = false;
            gameUI.SetActive(false);
            GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(Cinematic());
        }
    }

    IEnumerator Cinematic()
    {
        CameraController.cameraInstance.smoothMove = smoothCine;
        StartCoroutine(VolumeStart());
        StartCoroutine(CameraController.cameraInstance.TansitionCamera(valkyrie));
        yield return new WaitForSeconds(10f);
        //StartCoroutine(camera.GetComponent<CameraController>().TansitionCamera(CharacterController.instance.gameObject,timeToGo));
        
        
        /*if (timeToGo > smoothMove) pour reset la cam sur player
       {
           stopMove = false;
       }*/
    }
    
   /* public void ouioui()
    {
        CamraController.cameraInstance = 
    }*/
    IEnumerator VolumeStart()
    {
        camera.transform.DOShakePosition(3f, 0.1f);
        Time.timeScale = 1;
        float timeElapsed = 0;
        while (timeElapsed < timeToLastStep)
        {
            disolveValue = Mathf.Lerp(0, 0.7f, timeElapsed / timeToLastStep);
            timeElapsed += 1.5f * Time.deltaTime;
            yield return null;
        }
    }
}
