using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CinematiqueBoss : MonoBehaviour
{
    public Volume volumeCinematique;
    public float disolveValue;
    public float timeToLastStep;
    public GameObject camera;
    public GameObject valkyrie;

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
            StartCoroutine(VolumeStart());
            CameraController.cameraInstance.TansitionCamera(valkyrie);
        }
    }
    
    IEnumerator VolumeStart()
    {
        Time.timeScale = 1;
        float timeElapsed = 0;
        while (timeElapsed < timeToLastStep)
        {
            disolveValue = Mathf.Lerp(0, 0.4f, timeElapsed / timeToLastStep);
            timeElapsed += 0.2f * Time.deltaTime;
            yield return null;
        }
    }
}
