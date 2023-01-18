using System;
using System.Collections;
using DG.Tweening;
using GenPro;
using Pathfinding;
using TMPro;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class MonsterLifeManager : MonoBehaviour
{
    public EnemyData data;
    public GameObject textDamage;
    public GameObject textCriticalDamage;
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
    public bool elite;
    public IA_Monstre1 IALoup;
    public IA_Guerrier IAGuerrier;
    public IA_Corbeau IACorbeau;
    
    public bool isDisolve;
    public SpriteRenderer sprite2DRend;
    public float disolveValue;
    public float disolveValueAmount;
    public GameObject spawnCircle;
    public GameObject child;
    public GameObject emptyLayers;

    [Header("Alterations d'état")] 
    public bool FlameInfected;
    public float FlameInfectedTimer;
    public float FlameInfectedTimerMax;
    public float spawnTimer;
    public float spawnTimerMax;
    public NewPowerManager flameManager;
    
    public float InvincibleTime;
    public float InvincibleTimeTimer;
    public bool isInvincible;
    
    public float MomifiedTime = 3;
    public float MomifiedTimeTimer;
    public bool isMomified;
    public Material momieShader;
    private bool activeBandelettes;
    
    public bool isEnvased;
    public float EnvasedTime = 5;
    public float EnvasedTimeTimer;
    private float demiSpeed;
    
    [Header("Challenges")] 
    public bool eliteChallenge = false;
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

    private void FixedUpdate()
    {
        if (isDisolve)
        {
            if (CharacterController.instance.transform.position.x > transform.position.x)
            {
                //Debug.Log("droite");
                var localRotation = sprite2DRend.transform.localRotation;
                localRotation = Quaternion.Euler(localRotation.x, 0, localRotation.z);
                sprite2DRend.transform.localRotation = localRotation;
            }
            else if (CharacterController.instance.transform.position.x < transform.position.x)
            {
                //Debug.Log("gauche");
                var localRotation = sprite2DRend.transform.localRotation;
                localRotation = Quaternion.Euler(localRotation.x, -180, localRotation.z);
                sprite2DRend.transform.localRotation = localRotation;
            }
            disolveValue += disolveValueAmount;
            sprite2DRend.material.SetFloat("_Step", disolveValue);
            if (disolveValue >= 1)
            {
                isDisolve = false;
                //Destroy(sprite2DRend.gameObject);
                sprite2DRend.enabled = false;
            }
        }
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
            sprite2DRend.gameObject.transform.position = child.transform.position;
            if (CharacterController.instance.transform.position.x > transform.position.x)
            {
                //Debug.Log("droite");
                var localRotation = sprite2DRend.transform.localRotation;
                localRotation = Quaternion.Euler(localRotation.x, 0, localRotation.z);
                sprite2DRend.transform.localRotation = localRotation;
            }
            else if (CharacterController.instance.transform.position.x < transform.position.x)
            {
                //Debug.Log("gauche");
                var localRotation = sprite2DRend.transform.localRotation;
                localRotation = Quaternion.Euler(localRotation.x, -180, localRotation.z);
                sprite2DRend.transform.localRotation = localRotation;
            }
            
            MomifiedTimeTimer += Time.deltaTime;
            ai.canMove = false;
            sprite2DRend.enabled = true;
            sprite2DRend.material = momieShader;
            if (MomifiedTimeTimer >= MomifiedTime)
            {
                activeBandelettes = true;
                isMomified = false;
                sprite2DRend.enabled = false;
                ai.canMove = true;
                MomifiedTimeTimer = 0;
            }
        }

        if (FlameInfected)
        {
            FlameInfectedTimer += Time.deltaTime;
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnTimerMax)
            {
                GameObject miniflame = Instantiate(flameManager.p2DashTrailHitbox,child.transform.position,Quaternion.identity);
                miniflame.GetComponent<FlameArea>().isMiniFlame = true;
                miniflame.transform.localScale = new Vector2(flameManager.p2DashTrailSize /3,flameManager.p2DashTrailSize /3);
                spawnTimer = 0;
            }

            if (FlameInfectedTimer >= FlameInfectedTimerMax)
            {
                FlameInfectedTimer = 0;
                FlameInfected = false;
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
                GlyphManager.Instance.doCrit = true;
                textCriticalDamage.GetComponentInChildren<TextMeshPro>().SetText((Mathf.RoundToInt(damage * AnubisCurrentStats.instance.criticalBonusDamage)).ToString());
                GameObject textOBJ = Instantiate(textCriticalDamage, new Vector3(child.transform.position.x,child.transform.position.y + 1,-5), Quaternion.identity);
                textOBJ.transform.localScale *= 2;
                textOBJ.GetComponentInChildren<DamagePopUp>().isCritique = true;

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
            GlyphManager.Instance.didAttack = true;
            if (criticalPick <= AttaquesNormales.instance.criticalRate)
            {
                damage = Mathf.RoundToInt(damage * AnubisCurrentStats.instance.criticalBonusDamage);
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

    private IEnumerator HitScanReset()
    {
        gotHit = true;
        yield return null;
        gotHit = false;
    }
    
    public virtual void Die()
    {
        isInvincible = true;
        InvincibleTime = 99;
        
        if (IACorbeau is not null)
        {
            IACorbeau.audioSource.pitch = 1;
            IACorbeau.audioSource.PlayOneShot(IACorbeau.audioClipArray[3]);
            IACorbeau.isDead = true;
        }
        else if (IALoup is not null)
        {
            IALoup.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            IALoup.audioSource.Stop();
            IALoup.audioSource.pitch = 1;
            IALoup.audioSource.PlayOneShot(IALoup.audioClipArray[3]);
            IALoup.isDead = true;
        }
        else if (IAGuerrier is not null)
        {
            animator.SetBool("isIdle",true);
            IAGuerrier.audioSource.pitch = 1;
            IAGuerrier.audioSource.PlayOneShot(IAGuerrier.audioClipArray[3]);
            IAGuerrier.isDead = true;
        }
       
        canvasLifeBar.SetActive(false);
        child.GetComponent<AIPath>().canMove = false;
        child.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        StartCoroutine(DelayedDeath());
    }

    private IEnumerator DelayedDeath()
    {
        yield return new WaitForSeconds(1f);
        child.SetActive(false);
        if (SalleGenerator.Instance.currentRoom.parasites && !isParasite)
        {
            var parasite = Instantiate(SalleGenerator.Instance.parasiteToSpawn, ai.transform.position, Quaternion.identity);
            var parasiteScript = parasite.GetComponent<MonsterLifeManager>();
            SalleGenerator.Instance.currentRoom.currentEnemies.Add(parasiteScript);
            parasite.GetComponent<MonsterLifeManager>().isParasite = true;
            parasite.GetComponent<MonsterLifeManager>().soulValue = 1; //Mathf.RoundToInt(parasite.GetComponent<MonsterLifeManager>().soulValue * 0.5f);
            parasite.transform.localScale /= 2;
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
            yield return new WaitForSeconds(0.5f);
            canvasLifeBar.SetActive(true);
        }
        else
        {
            isDisolve = true;
            yield return new WaitForSeconds(1.9f);
            canvasLifeBar.SetActive(true);
        }
        child.SetActive(true);
    }
}
