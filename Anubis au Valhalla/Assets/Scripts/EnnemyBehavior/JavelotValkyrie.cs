using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class JavelotValkyrie : MonoBehaviour
{
    public IA_Valkyrie ia;
    public Rigidbody2D rb;
    public BoxCollider2D col;
    public Vector2 dir;
    public float angle;
    private Vector2 capturedPlayerPos;
    public int travelDistance;
    public float travelTime;
    public float TurningSpeed;
    public float timeForAim;
    public float timeForIndic;
    public float timeForLaunch;
    [BoxGroup("Timers")] [SerializeField] private float aimTimer;
    [BoxGroup("Timers")] [SerializeField] private float indicTimer;
    [BoxGroup("Timers")] [SerializeField] private float launchTimer;
    public bool showingIndic;
    public bool launching;
    public Transform restingPos;
    public float restingPosLatency;
    public GameObject indicationJavelot;
    public GameObject indicHolder;

    private void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (aimTimer < timeForAim)
        {
            aimTimer += Time.deltaTime;
            MoveTowardsRestingPos();
            if (aimTimer > 1f) AimAtPlayer();
        }
        /*else if (indicTimer < timeForIndic)
        {
            indicTimer += Time.deltaTime;

        }*/
        else if(!launching)
        {
            ShowIndicator();
            launching = true;
            indicHolder.transform.DOShakePosition(timeForLaunch, 0.5f, 30,0).OnComplete(Launch);
            launchTimer += Time.deltaTime;
        }
    }

    void MoveTowardsRestingPos()
    {
        transform.position = Vector3.Lerp(transform.position, restingPos.position, restingPosLatency);
    }

    void AimAtPlayer()
    {
        dir = new Vector2(CharacterController.instance.transform.position.x - transform.position.x,CharacterController.instance.transform.position.y - transform.position.y);
        dir.Normalize();
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation,Quaternion.AngleAxis(angle, Vector3.forward), TurningSpeed);
    }

    void ShowIndicator()
    {
        if (showingIndic)return;
        indicationJavelot.SetActive(true);
        Destroy(indicationJavelot,timeForLaunch + 0.1f);
        showingIndic = true;
    }

    void Launch()
    {
        col.enabled = true;
        rb.velocity = new Vector2(dir.x * ia.javelotSpeed, dir.y * ia.javelotSpeed);
        Destroy(gameObject,travelTime);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            DamageManager.instance.TakeDamage(ia.puissanceAttaqueJavelot, gameObject);
            Destroy(gameObject);
        }
    }
}
