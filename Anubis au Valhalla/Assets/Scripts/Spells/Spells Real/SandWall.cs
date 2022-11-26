using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;

public class SandWall : MonoBehaviour
{
    public PouvoirPlaieObject soPouvoirPlaiePlaie;
    private BoxCollider2D collider;
    private Rigidbody2D rb;
    public float timerStep2Time;
    public float speed;
    public float tempsReloadHitFlameAreaTimer;
    public bool stopAttack;
    public bool activeHitbox;


    private void Start()
    {
        Destroy(gameObject,soPouvoirPlaiePlaie.wallDuration);
        Vector2 charaPos = CharacterController.instance.transform.position;
        float angle = Mathf.Atan2(transform.position.y - charaPos.y, transform.position.x - charaPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        timerStep2Time += Time.deltaTime;

        if (timerStep2Time >= soPouvoirPlaiePlaie.timeToStep2)
        { 
            activeHitbox = true;
            collider.isTrigger = true;
            transform.Translate(transform.right*speed,Space.World);
        }
    }
    
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre" && activeHitbox)
        {
            stopAttack = false;
            for (int i = 0; i < 3; i++)
            {
                if (tempsReloadHitFlameAreaTimer <= 0.2 && stopAttack == false)
                {
                    tempsReloadHitFlameAreaTimer += Time.deltaTime;
                }

                if (tempsReloadHitFlameAreaTimer > 0.2 && col.gameObject.tag == "Monstre")
                {
                    Debug.Log("touché");
                    col.GetComponentInParent<MonsterLifeManager>().DamageText(soPouvoirPlaiePlaie.dashDamage);
                    col.GetComponentInParent<MonsterLifeManager>().TakeDamage(soPouvoirPlaiePlaie.dashDamage,soPouvoirPlaiePlaie.staggerDash);
                    tempsReloadHitFlameAreaTimer = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            stopAttack = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            
            tempsReloadHitFlameAreaTimer = 0;
        }
    }

    IEnumerator WaitToCollide()
    {
        yield return new WaitForSeconds(0.3f);
        collider.enabled = true;
    }
    
    
}
