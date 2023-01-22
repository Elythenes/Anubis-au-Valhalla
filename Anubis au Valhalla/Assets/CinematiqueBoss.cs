
using System.Collections;

using DG.Tweening;
using Pathfinding;

using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

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
    public GameObject valkyrieRoot;
    public IA_Valkyrie valkyrieIA;


    [Header("Autre")]
    public GameObject cameraPoint;
    public bool isLifeUp;
    public GameObject gameUI;
    public CanvasGroup UILifeBar;
    public bool isDead;
    public GameObject bloodParticles;
   


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
        gameUI = GameObject.Find("GameUI");
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

        if (isDead)
        {
            SoundManager.instance.audioSource.Stop();
            StartCoroutine(CinematicDead());
            isDead = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            SoundManager.instance.audioSource.Stop();
            CharacterController.instance.isCinematic = true;
            CharacterController.instance.allowMovements = false;
            CharacterController.instance.isDashing = false;
            CharacterController.instance.anim.SetBool("isIdle",true);
            CharacterController.instance.anim.SetBool("isDashing",false);
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
        SoundManager.instance.ChangeToBoss();
        SoundManager.instance.PlayBoss();
        valkyrieRoot.GetComponent<MonsterLifeManager>().animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(3f);
        valkyrieRoot.GetComponent<MonsterLifeManager>().animator.SetBool("isIdle", true);
        valkyrieRoot.GetComponent<MonsterLifeManager>().animator.SetBool("isAttacking", false);
        StartCoroutine(camera.GetComponent<CameraController>().BackTansitionCamera(CharacterController.instance.gameObject));
        yield return new WaitForSeconds(1.5f);
        gameUI.SetActive(true);
        isLifeUp = true;
        CharacterController.instance.isCinematic = false;
        CharacterController.instance.allowMovements = true;
        CharacterController.instance.canDash = true;
        valkyrieRoot.GetComponentInChildren<IA_Valkyrie>().enabled = true;
    }

    IEnumerator CinematicDead()
    {
        DamageManager.instance.invinsible = true;
        cameraPoint.transform.position = valkyrieIA.gameObject.transform.position - new Vector3(0, 1, 0);
        CharacterController.instance.isCinematic = true;
        CharacterController.instance.allowMovements = false;
        CharacterController.instance.isDashing = false;
        SoundManager.instance.audioSource.Stop();
        CameraController.cameraInstance.smoothMove = 3;
        valkyrieRoot.GetComponentInChildren<AIPath>().enabled = false;
        valkyrieRoot.transform.DOShakePosition(5, 2);
        CameraController.cameraInstance.transform.DOShakePosition(5, 0.2f);
        StartCoroutine( CameraController.cameraInstance.TansitionCamera(cameraPoint));
        CameraController.cameraInstance.stopMove = false;
        for (int i = 0; i < 10; i++)
        {
            Instantiate(bloodParticles, valkyrieIA.transform.position + new Vector3(Random.Range(1,1),Random.Range(1,1)), Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(3);
        LokiDialoguesManager.instance.DialogueUP();
        LokiDialoguesManager.instance.index = 6;
        LokiDialoguesManager.instance.NextDialogue();
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
