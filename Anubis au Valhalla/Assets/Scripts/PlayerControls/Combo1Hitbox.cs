using DG.Tweening;
using Pathfinding;
using UnityEngine;


public class Combo1Hitbox : MonoBehaviour
{
    [Range(0, 2)] public int comboNumber;
    public float stagger = 0.2f;
    private bool isShaking;
    private bool isWaiting;
    public Camera mainCamera;
    public GameObject bloodEffect;
    public bool isStop;
    

    public virtual void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        transform.parent = CharacterController.instance.transform;
        Destroy(gameObject, AttaquesNormales.instance.dureeHitbox[comboNumber]);
       transform.localScale *= AttaquesNormales.instance.rangeAttaque[comboNumber];
    }

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Monstre"))
        {
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


            mainCamera.transform.DOShakePosition(0.1f,0.2f);
            col.gameObject.GetComponent<AIPath>().canMove = false;
            col.gameObject.GetComponentInParent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(AttaquesNormales.instance.damage[comboNumber]), stagger);
            
            
            //col.gameObject.GetComponent<Rigidbody2D>().AddForce(angleNormalized*AttaquesNormales.instance.forceKnockback[comboNumber],ForceMode2D.Impulse);
            //col.GetComponentInParent<MonsterLifeManager>().ai.Move(angleNormalized*AttaquesNormales.instance.forceKnockback[comboNumber]);
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
