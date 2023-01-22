using System.Collections;
using DG.Tweening;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;


public class Combo1Hitbox : MonoBehaviour
{
    [Range(0, 2)] public int comboNumber;
    public float stagger = 0.2f;
    private bool isShaking;
    private bool isWaiting;
    public GameObject mainCamera;
    public GameObject bloodEffect;
    public bool isStop;
    public ParticleSystem SlashVFX;
    public GameObject Impact;



    public virtual void Start()
    {
        Vector2 charaPos = CharacterController.instance.transform.position;
        Vector2 mousePos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
       SlashVFX.transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);
       if (SlashVFX.transform.position.x - CharacterController.instance.transform.position.x > 0)
       {
           var transform1 = SlashVFX.transform;
           var localScale = transform1.localScale;
           localScale = new Vector3(localScale.x, localScale.y, localScale.z);
           transform1.localScale = localScale;
       }
       else
       {
           var transform1 = SlashVFX.transform;
           var localScale = transform1.localScale;
           localScale = new Vector3(localScale.x, -localScale.y, localScale.z);
           transform1.localScale = localScale;
       }
        mainCamera = GameObject.Find("CameraHolder");
        transform.parent = CharacterController.instance.transform;
        if (AttaquesNormales.instance.buffer)
        {
            Vector2 dashBoost = new Vector2(transform.position.x + angle * 100,transform.position.y + angle * 100);
            transform.position = dashBoost;
        }
        Destroy(gameObject, AttaquesNormales.instance.dureeHitbox[comboNumber]);
       transform.localScale *= AttaquesNormales.instance.rangeAttaque[comboNumber];
       SlashVFX.transform.localScale *=AttaquesNormales.instance.rangeAttaque[comboNumber];
    }

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.CompareTag("Monstre") && col.GetComponentInParent<MonsterLifeManager>().isInvincible == false)
        {
            
            if (/*CharacterController.instance.isHiting == false &&*/ col.gameObject.GetComponentInParent<MonsterLifeManager>().isInvincible == false)
            {
                CharacterController.instance.isHiting = true; 
            }

            Instantiate(Impact, col.transform.position, Quaternion.identity);
            StartCoroutine(ResetTracking());
            float angle = Mathf.Atan2(transform.parent.position.y - col.transform.position.y,transform.parent.position.x - col.transform.position.x ) * Mathf.Rad2Deg;
            GameObject effetSang = Instantiate(bloodEffect, col.transform.position, Quaternion.identity);
            effetSang.transform.rotation = Quaternion.Euler(0,0,angle);
            Vector3 angleKnockback = col.transform.position - transform.parent.position;
            Vector3 angleNormalized = angleKnockback.normalized;
            
            
            if (col.GetComponent<PuppetHealth>())
            {
                //col.gameObject.GetComponent<MonsterLifeManager>().DamageText(AnubisCurrentStats.instance.comboDamage[comboNumber]);
                col.gameObject.GetComponent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(AttaquesNormales.instance.damage[comboNumber]), stagger);
                return;
            }


            mainCamera.transform.DOShakePosition(0.2f,1f);
            col.gameObject.GetComponent<AIPath>().canMove = false;
            col.gameObject.GetComponentInParent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(AttaquesNormales.instance.damage[comboNumber]), stagger);

            if (!col.gameObject.GetComponentInParent<MonsterLifeManager>().elite)
            {
                col.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                col.gameObject.GetComponent<Rigidbody2D>().AddForce(angleNormalized*AttaquesNormales.instance.forceKnockback[comboNumber],ForceMode2D.Impulse);
            }
            else
            {
                col.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                col.gameObject.GetComponent<Rigidbody2D>().AddForce(angleNormalized*AttaquesNormales.instance.forceKnockback[comboNumber]/1.5f,ForceMode2D.Impulse);
            }
          
        }
    } 

    IEnumerator ResetTracking()
    {
        yield return null;
        CharacterController.instance.isHiting = false;
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
