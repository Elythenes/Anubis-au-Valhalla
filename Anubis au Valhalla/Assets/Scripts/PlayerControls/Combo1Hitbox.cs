using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DG.Tweening;
using Pathfinding;
using UnityEngine;
using Weapons;
using Debug = UnityEngine.Debug;

public class Combo1Hitbox : MonoBehaviour
{
    [Range(0, 2)] public int comboNumber;
    public float stagger = 0.2f;
    public GameObject camera;
    private bool isShaking;
    private bool isWaiting;
    public GameObject bloodEffect;
    public bool isStop;
    

    public virtual void Start()
    {
        camera = GameObject.Find("CameraHolder");
        transform.parent = CharacterController.instance.transform;
        Destroy(gameObject, AttaquesNormales.instance.dureeHitbox[comboNumber]);
       transform.localScale *= AttaquesNormales.instance.rangeAttaque[comboNumber];
    }

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Monstre"))
        {
            /*if (!isShaking)
            {
                isShaking = true;
                camera.transform.DOShakePosition(0.15f, 0.8f).OnComplete((() => isShaking = false));
            }*/

            /*if (!isWaiting)
            {
                Debug.Log("stop");
                StartCoroutine(HitStop(AttaquesNormales.instance.hitStopDuration[comboNumber]));
            }*/
            Vector3 angleKnockback = col.transform.position - transform.parent.position;
            Vector3 angleNormalized = angleKnockback.normalized;
            float angle = Mathf.Atan2(transform.parent.position.y - col.transform.position.y,transform.parent.position.x - col.transform.position.x ) * Mathf.Rad2Deg;
            GameObject effetSang = Instantiate(bloodEffect, col.transform.position, Quaternion.identity);
            effetSang.transform.rotation = Quaternion.Euler(0,0,angle);
            col.gameObject.GetComponent<AIPath>().canMove = false;
            col.gameObject.GetComponentInParent<MonsterLifeManager>().DamageText(Mathf.RoundToInt(AttaquesNormales.instance.damage[comboNumber]));
            col.gameObject.GetComponentInParent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(AttaquesNormales.instance.damage[comboNumber]), stagger);
            //col.gameObject.GetComponent<Rigidbody2D>().AddForce(angleNormalized*AttaquesNormales.instance.forceKnockback[comboNumber],ForceMode2D.Impulse);
            col.GetComponentInParent<MonsterLifeManager>().ai.Move(angleNormalized*AttaquesNormales.instance.forceKnockback[comboNumber]);
        }
    }
    

    /*IEnumerator HitStop(float duration)
    {
        isStop = true;
        Time.timeScale = 0.0f;
        isWaiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        isWaiting = false;
        isStop = false;
    }*/
    

    
}
