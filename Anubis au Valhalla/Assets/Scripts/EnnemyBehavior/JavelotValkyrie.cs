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
    public bool skyFall;
    public bool falling;
    public int javelotNumber;

    private void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (!skyFall)
        {
            if (aimTimer < timeForAim)
            {
                aimTimer += Time.deltaTime;
                MoveTowardsRestingPos();
                if (aimTimer > 1f) AimAtPlayer(Vector2.zero, TurningSpeed);
            }
            else if(!launching)
            {
                ShowIndicator();
                launching = true;
                indicHolder.transform.DOShakePosition(timeForLaunch, 0.5f, 30,0).OnComplete(Launch);
                launchTimer += Time.deltaTime;
            }
        }
        else
        {
            AimAtPlayer(Vector2.up,TurningSpeed/2);
            if (aimTimer < 1.3f)
            {
                MoveTowardsRestingPos();
                aimTimer += Time.deltaTime;
            }
            else if(!launching)
            {
                launching = true;
                transform.DOMove(transform.position + Vector3.up * 100, ia.jumpSpeed);
            }
            else
            {
                indicTimer += Time.deltaTime;
                if (indicTimer > 0.4f)
                {
                    AimAtPlayer(Vector2.down,TurningSpeed*2);
                }
                if (timeForIndic < indicTimer && !falling)
                {
                    falling = true;
                    ia.skyfallIndic[javelotNumber].gameObject.SetActive(true);
                    transform.position = ia.skyfallIndic[javelotNumber].transform.position + Vector3.up * 100;
                    transform.DOMove(ia.skyfallIndic[javelotNumber].transform.position + Vector3.up, ia.jumpSpeed).OnComplete(() =>
                    {
                        ia.skyfallIndic[javelotNumber].gameObject.SetActive(false);
                        var hitbox = Instantiate(ia.hitboxFall, transform.position + Vector3.down, Quaternion.identity);
                        transform.DOShakePosition(ia.FallTime, 0.2f, 50).OnComplete((() =>
                        {
                            Destroy(hitbox);
                            Destroy(gameObject);
                        }));
                    });
                }
            }

        }
    }

    void MoveTowardsRestingPos()
    {
        transform.position = Vector3.Lerp(transform.position, restingPos.position, restingPosLatency);
    }

    void AimAtPlayer(Vector2 direction, float turn)
    {
        if (skyFall)
        {
            dir = direction;
            dir.Normalize();
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation,Quaternion.AngleAxis(angle, Vector3.forward), turn);
            return;
        }
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
