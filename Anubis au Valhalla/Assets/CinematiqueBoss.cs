using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CinematiqueBoss : MonoBehaviour
{
    public static CinematiqueBoss instance;
    public bool canMoveController;
    
    [Header("Volume")]
    public Volume volumeCinematique;
    public float disolveValue;
    public float timeToLastStep;
    
    [Header("Volume")]
    public float smoothCine;
    public GameObject camera;
    public GameObject valkyrie;


    [Header("Autre")]
    public GameObject cameraPoint;
    public bool isLifeUp;
    public GameObject gameUI;
    public CanvasGroup UILifeBar;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        camera = GameObject.Find("Main Camera");
    }

    public void Update()
    {
        volumeCinematique.weight = disolveValue;

        if (!canMoveController)
        {
            CharacterController.instance.rb.velocity = Vector2.zero;
        }
      
        
        if (isLifeUp)
        {
            if (UILifeBar.alpha < 1)
            {
                UILifeBar.alpha += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            CharacterController.instance.isCinematic = true;
            CharacterController.instance.allowMovements = false;
            
            CharacterController.instance.canDash = false;
            gameUI.SetActive(false);
            GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(CinematicStartDialoges());
        }
    }

    public void CotoutineStartCombat()
    {
        StartCoroutine(CinematicStartCombat());
    }

    public IEnumerator CinematicStartDialoges()
    {
        CameraController.cameraInstance.smoothMove = smoothCine;
        StartCoroutine(VolumeStart());
        StartCoroutine(CameraController.cameraInstance.TansitionCamera(cameraPoint));
        yield return new WaitForSeconds(4f);
        LokiDialoguesManager.instance.DialogueUP();
        LokiDialoguesManager.instance.NextDialogue();
        
    }

    IEnumerator CinematicStartCombat()
    {
        valkyrie.GetComponent<MonsterLifeManager>().animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(3f);
        valkyrie.GetComponent<MonsterLifeManager>().animator.SetBool("isIdle", true);
        valkyrie.GetComponent<MonsterLifeManager>().animator.SetBool("isAttacking", false);
        StartCoroutine(camera.GetComponent<CameraController>().BackTansitionCamera(CharacterController.instance.gameObject));
        yield return new WaitForSeconds(1.5f);
        isLifeUp = true;
        CharacterController.instance.isCinematic = false;
        CharacterController.instance.allowMovements = true;
        CharacterController.instance.canDash = true;
        // Lancer l'IA de la valkyrie;
    }
 
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
