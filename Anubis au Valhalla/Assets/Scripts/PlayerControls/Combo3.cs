using System.Collections;
using DG.Tweening;
using Pathfinding;
using UnityEngine;


public class Combo3 : MonoBehaviour
{
    [Range(0, 2)] public int comboNumber;
    public float stagger = 0.2f;
    private bool isShaking;
    private bool isWaiting;
    public GameObject mainCamera;
    public GameObject bloodEffect;
    public GameObject parent;

    [Header("Effet Smash")]
    public ParticleSystem cracksMeshRenderer;
    public Material particulesMaterial;
    public float disolveValue = 1;
    public float disolveGainedByFrame;
    public GameObject collider;
    public float scaleGainedByFrame;


    public virtual void Start()
    {
        Destroy(parent,AnubisCurrentStats.instance.dureeHitbox[2]);
        disolveValue = 0;
        particulesMaterial = cracksMeshRenderer.GetComponent<Renderer>().material;
        mainCamera = GameObject.Find("CameraHolder");
        mainCamera.transform.DOShakePosition(0.2f,1.5f);
       transform.localScale *= AttaquesNormales.instance.rangeAttaque[3];
       StartCoroutine(StopHitbox());
    }

    void FixedUpdate()
    {
        if (disolveValue < 0.99f)
        {
            disolveValue += disolveGainedByFrame;
            particulesMaterial.SetFloat("_Step", disolveValue);
        }
        else
        {
            Destroy(gameObject);
        }

        if (collider.transform.localScale.x < 0.15f && collider.transform.localScale.y < 0.15f)
        { 
            collider.transform.localScale += new Vector3(scaleGainedByFrame, scaleGainedByFrame, 0); 
        }
    }
    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Monstre") && col.GetComponentInParent<MonsterLifeManager>().isInvincible == false)
        {
         
            float angle = Mathf.Atan2(transform.position.y - col.transform.position.y,transform.position.x - col.transform.position.x ) * Mathf.Rad2Deg;
            if (col.GetComponentInParent<MonsterLifeManager>().isInvincible == false && col.GetComponentInParent<MonsterLifeManager>().isSmashInvinsible == false)
            {
                GameObject effetSang = Instantiate(bloodEffect, col.transform.position, Quaternion.identity);
                effetSang.transform.rotation = Quaternion.Euler(0,0,angle);
                col.gameObject.GetComponentInParent<MonsterLifeManager>()
                    .TakeDamage(Mathf.RoundToInt(AttaquesNormales.instance.damage[comboNumber]), 0);
            }
            Vector3 angleKnockback = col.transform.position - transform.position;
            Vector3 angleNormalized = angleKnockback.normalized;
            
            
            
            if (col.GetComponent<PuppetHealth>())
            {
                col.gameObject.GetComponent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(AttaquesNormales.instance.damage[comboNumber]), stagger);
                return;
            }
            
            
            
            col.gameObject.GetComponent<AIPath>().canMove = false;
         
            
            if (!col.gameObject.GetComponentInParent<MonsterLifeManager>().elite && !col.gameObject.GetComponentInParent<MonsterLifeManager>().isMomified)
            {
                col.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                col.gameObject.GetComponent<Rigidbody2D>().AddForce(
                    angleNormalized * AttaquesNormales.instance.forceKnockback[comboNumber], ForceMode2D.Impulse);
            }
            else if(!col.gameObject.GetComponentInParent<MonsterLifeManager>().isMomified)
            {
                col.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                col.gameObject.GetComponent<Rigidbody2D>().AddForce(angleNormalized*AttaquesNormales.instance.forceKnockback[comboNumber]/1.5f,ForceMode2D.Impulse);
            }
            col.gameObject.GetComponentInParent<MonsterLifeManager>().isSmashInvinsible = true;
        }
    }

    IEnumerator StopHitbox()
    {
        yield return new WaitForSeconds(0.1f);
    }
}
