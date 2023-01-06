using System.Collections;
using DG.Tweening;
using GenPro;
using Pathfinding;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class MonsterLifeManager : MonoBehaviour
{
    public EnemyData data;
    public GameObject textDamage;
    public Animator animator;
    public Rigidbody2D rb;
    public HealthBarMonstre healthBar;
    public AIPath ai;
    [NaughtyAttributes.ReadOnly] public int vieMax;
    public int vieActuelle;
    [NaughtyAttributes.ReadOnly] public int soulValue = 4;
    public float delay;
    private float forceKnockBack = 10;
    public UnityEvent OnBegin, OnDone;
    public GameObject canvasLifeBar;
    public float criticalPick;
    public bool gotHit;
   
    

    public GameObject spawnCircle;
    public GameObject child;
    public GameObject emptyLayers;
    public SpriteRenderer sr;

    [Header("Alterations d'état")] 
    public float InvincibleTime;
    public float InvincibleTimeTimer;
    public bool isInvincible;
    public float MomifiedTime = 3;
    public float MomifiedTimeTimer;
    public bool isMomified;
    public GameObject bandelettesMomie;
    private bool activeBandelettes;
    public bool isEnvased;
    public float EnvasedTime = 5;
    public float EnvasedTimeTimer;
    private float demiSpeed;
    public bool elite = false;
    public bool isParasite = false;
    public bool overdose = false;
    

    private void Awake()
    {
        vieMax = data.maxHealth;
        soulValue = data.soulValue;
    }

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (overdose)
        {
            vieMax = Mathf.RoundToInt(vieMax * 0.3f);
        }
        vieActuelle = vieMax;
        demiSpeed = ai.maxSpeed / 2;
        child.SetActive(false);
        StartCoroutine(DelayedSpawn());

    }

    public virtual void Update()
    {
        transform.localRotation = Quaternion.identity;
        if (isInvincible)
        {
            InvincibleTimeTimer += Time.deltaTime;

            if (InvincibleTimeTimer >= InvincibleTime)
            {
                isInvincible = false;
                InvincibleTimeTimer = 0;
            }
        }


        if (isEnvased)
        {
            EnvasedTimeTimer += Time.deltaTime;
            ai.maxSpeed = demiSpeed;
            
            if (EnvasedTimeTimer >= EnvasedTime)
            {
                ai.maxSpeed *= 2;
                EnvasedTimeTimer = 0;
                isEnvased = false;
                
            }
        }
        
        if (isMomified)
        {
            MomifiedTimeTimer += Time.deltaTime;
            OnBegin.Invoke();
            ai.canMove = false; 
           bandelettesMomie.SetActive(true);
            
            if (MomifiedTimeTimer >= MomifiedTime)
            {
                OnDone.Invoke();
                activeBandelettes = true;
                isMomified = false;
                bandelettesMomie.SetActive(false);
                ai.canMove = true;
                MomifiedTimeTimer = 0;
            }
        }
    }

    public virtual void TakeDamage(int damage, float staggerDuration)
    {
        if (!isInvincible)
        {
            criticalPick = Random.Range(0,100);
            if (criticalPick <= AttaquesNormales.instance.criticalRate)
            {
                textDamage.GetComponentInChildren<TextMeshPro>().SetText((damage * 2).ToString());
                GameObject textOBJ = Instantiate(textDamage, new Vector3(child.transform.position.x,child.transform.position.y + 1,-5), Quaternion.identity);
                textOBJ.transform.localScale *= 2;

            }
            else
            {
                textDamage.GetComponentInChildren<TextMeshPro>().SetText(damage.ToString());
                Instantiate(textDamage, new Vector3(child.transform.position.x,child.transform.position.y + 1,-5), Quaternion.identity);
            }
            StartCoroutine(HitScanReset());
            gotHit = true;
            transform.DOShakePosition(staggerDuration, 0.5f, 50);/*.OnComplete(() =>
            {
                ai.canMove = true;
            });*/
            
            if (criticalPick <= AttaquesNormales.instance.criticalRate)
            {
                damage *= 2;
                vieActuelle -= damage; 
                healthBar.SetHealth(vieActuelle);
                isInvincible = true;
            }
            else
            {
                vieActuelle -= damage; 
                healthBar.SetHealth(vieActuelle);
                isInvincible = true;
            }
        
        }
        
        if (vieActuelle <= 0)
        {
            Die();
        }
    }
    

  /*  public virtual void OnTriggerEnter2D(Collider2D col)
    {
        Vector2 direction = (transform.position - col.transform.position);
        direction.Normalize();
        if (col.CompareTag("AttaqueNormale"))
        {
            //StopAllCoroutines();
            OnBegin?.Invoke();
            StartCoroutine(Reset(0.5f));
        }
    }*/

    public IEnumerator Reset(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector3.zero;
        OnDone?.Invoke();
    }

    private IEnumerator HitScanReset()
    {
        gotHit = true;
        yield return new WaitForSeconds(InvincibleTime);
        gotHit = false;
    }
    
    public virtual void Die()
    {
        canvasLifeBar.SetActive(false);
        child.GetComponent<AIPath>().canMove = false;
        child.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        child.GetComponent<Collider2D>().enabled = false;
        animator.SetBool("isDead",true);
        OnBegin.Invoke();
        StartCoroutine(DelayedDeath());
    }

    private IEnumerator DelayedDeath()
    {
        yield return new WaitForSeconds(0.5f);
        child.SetActive(false);
        if (SalleGenerator.Instance.currentRoom.parasites && !isParasite)
        {
            var parasite = Instantiate(SalleGenerator.Instance.parasiteToSpawn, transform.position, Quaternion.identity);
            var parasiteScript = parasite.GetComponent<MonsterLifeManager>();
            SalleGenerator.Instance.currentRoom.currentEnemies.Add(parasiteScript);
            parasite.GetComponent<MonsterLifeManager>().isParasite = true;
            parasite.GetComponent<MonsterLifeManager>().soulValue = Mathf.RoundToInt(parasite.GetComponent<MonsterLifeManager>().soulValue * 0.5f);
        }

        ScoreManager.instance.currentScore += data.score;
        Souls.instance.CreateSouls(child.transform.position, soulValue);
        SalleGenerator.Instance.currentRoom.currentEnemies.Remove(this);
        SalleGenerator.Instance.currentRoom.CheckForEnemies();
        Destroy(gameObject);
    }

    IEnumerator DelayedSpawn()
    {
        Instantiate(spawnCircle, transform.position, Quaternion.identity);
        if (isParasite)
        {
            yield return new WaitForSeconds(0.1f);
        }
        else
        {
            yield return new WaitForSeconds(SalleGenerator.Instance.timeBetweenWaves);
        }
        child.SetActive(true);
    }
}
