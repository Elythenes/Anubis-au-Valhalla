
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class DamageManager : MonoBehaviour
{
    public AnubisCurrentStats stats;
    
    [Header("Objects")]
    public GameObject textDamage;
    public GameObject textHealDamage;
    public GameObject deathMenu;
    public GameObject GameUI;
    public Camera mainCamera;
    public MeshRenderer playerMaterial;
    public Material deathDisolve;
    public static DamageManager instance;
    public Volume gVolume;
    public Volume gVolumeMiss;
    public Volume gVolumeMort;
    private ColorAdjustments ca;
    public TextMeshProUGUI compteurScore;
    public GameObject particulesHeal;

    [Header("Alterations d'Etat")] 
    public float knockbackAmount;
    public bool stun;
    public bool invinsible;
    public Animator animPlayer;
    public bool isAmePowered; // pour le pouvoir spéciale "âme" du dash 

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] audioClipArray;
    
    [Header("Tracking")] 
    public bool isDodging;
    
    [Header("Feedbacks")]
    public float timeHitStop;
    public float timeRedScreen;
    public bool EffectMiss;
    public float t1;
    public float t2;
    public float t3;

    [Header("Stats")]
    [NaughtyAttributes.ReadOnly] public int vieActuelle;
    [NaughtyAttributes.ReadOnly] public int vieMax;
    [NaughtyAttributes.ReadOnly] public int damageReduction;
    [NaughtyAttributes.ReadOnly] public float tempsInvinsibleAfterHit;
    [NaughtyAttributes.ReadOnly] public float stunAfterHit;
    private bool stopWaiting;

    [Header("Variables de tracking")] 
    public bool isHurt;

    private LayerMask murMask;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    void Start()
    {
        stats = AnubisCurrentStats.instance;
        vieActuelle = vieMax;
        Time.fixedDeltaTime = 0.01F * Time.timeScale;
        stopWaiting = false;
        murMask = LayerMask.GetMask("CollisionEnvironement");
    }

    private void Update()
    {
        if (stats.vieActuelle > stats.vieMax)
        {
            stats.vieActuelle = stats.vieMax;
        }
    }

    private void FixedUpdate()
    {
        var g = CharacterController.instance;
        if (g.canBoost && !g.isDashing)
        {
            if (!g.rb.IsTouchingLayers(128))
            {
                g.canBuffer = false;
                g.canBoost = false;
                g.playerCol.enabled = true;
                g.allowMovements = true;
                g.stopDash = false;
                g.anim.SetBool("isDashing",false);
                g.anim.SetBool("isWalking",true);
            }
        }
    }

    public void TakeDamage(int damage, GameObject enemy)
    {
        if (!invinsible)
        {
            if (!CharacterController.instance.isDashing)
            {
                isHurt = true;
                StartCoroutine(ResetTracking());
                var angle = CharacterController.instance.transform.position - enemy.transform.position;
                angle.Normalize();
                CharacterController.instance.rb.AddForce(angle * (damage * knockbackAmount), ForceMode2D.Impulse);
                StartCoroutine(RedScreenStart(timeRedScreen));
                HitStop(timeHitStop,false);
                stats.vieActuelle -= damage - damageReduction/100 * damage;
                PotionManager.Instance.tookDamage = true;
                GameObject textObj = Instantiate(textDamage, new Vector3(transform.position.x,transform.position.y + 1,-5), Quaternion.identity);
                textObj.GetComponentInChildren<TextMeshPro>().SetText((damage - damageReduction/100 * damage).ToString());
             
                
                if (stats.vieActuelle <= 0)
                {
                    StopCoroutine(TempsStun());
                    stats.vieActuelle = 0;
                    Die();
                }
                else
                {
                    animPlayer.SetBool("isHurt",true);
                    StartCoroutine(TempsInvinsibilité(2f));
                    StartCoroutine(TempsStun());
                }
            }
            else if(CharacterController.instance.isDashing)
            {
                isDodging = true;
                StartCoroutine(ResetTracking());
                if (isAmePowered &&  !PouvoirAme.instance.spawnHitboxDash)
                {
                    PouvoirAme.instance.spawnHitboxDash = true;
                }
                if (EffectMiss)
                {
                    StartCoroutine(TempsInvinsibilité(0.5f));
                    HitStop(timeHitStop,true);
                    StartCoroutine(MissScreen(timeRedScreen));
                    EffectMiss = false;
                }
            }
            
        }

    }

    public void Heal(int healAmount)
    {
        if (healAmount > 0)
        {
            GameObject particuleSoinOBJ = Instantiate(particulesHeal, transform.position, Quaternion.identity);
            particuleSoinOBJ.transform.parent = transform;
            stats.vieActuelle += healAmount;
            GameObject textHealObj = Instantiate(textHealDamage, new Vector3(transform.position.x,transform.position.y + 1,-5), Quaternion.identity);
            textHealObj.GetComponentInChildren<TextMeshPro>().SetText((healAmount).ToString());
            if (stats.vieActuelle >= 100)
            {
                stats.vieActuelle = 100;
            }
        }
    }

    public void HitStop(float duration, bool miss)
    {
        if (stopWaiting)
            return;
        StartCoroutine(WaitStop(duration,miss));
    }

    IEnumerator WaitStop(float duration,bool miss)
    {
        if (!miss)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 0.2f;
        }
        Time.fixedDeltaTime = 0.01F * Time.timeScale;
        stopWaiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
        if (miss)
        {
            Time.fixedDeltaTime = 0.01F * Time.timeScale;
        }

        if (!miss)
        {
            Time.fixedDeltaTime = 0.01f * Time.timeScale;
        }
        
        stopWaiting = false;
    }

    public IEnumerator RedScreenStart(float timeRedScreenC)
    {
        float timeElapsed = 0;
        while (timeElapsed < t1)
        {
            mainCamera.orthographicSize = Mathf.Lerp(7.75f, 7, timeElapsed / t1);
            gVolume.weight = Mathf.Lerp(0, 1, timeElapsed / t1);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        mainCamera.orthographicSize = 7;
        gVolume.weight = 1;
        
        t2 = 0;
        while (t2 < t1)
        {
            gVolume.weight = Mathf.Lerp(1, 0, t2 / t1);
            mainCamera.orthographicSize = Mathf.Lerp(7, 7.75f, t2 / t1);
            t2 += Time.deltaTime;
            yield return null;
        }
        mainCamera.orthographicSize = 7.75f;
        gVolume.weight = 0;
    }

    public IEnumerator ResetTracking()
    {
        yield return null;
        isHurt = false;
        isDodging = false;
    }

    public IEnumerator MissScreen(float timeRedScreenC)
    {
        float timeElapsed = 0;
        while (timeElapsed < t1)
        {
            mainCamera.orthographicSize = Mathf.Lerp(7.75f, 6, timeElapsed / t1);
            gVolumeMiss.weight = Mathf.Lerp(0, 1, timeElapsed / t1);
            timeElapsed += 10f * Time.deltaTime;
            yield return null;
        }
        mainCamera.orthographicSize = 6;
        gVolumeMiss.weight = 1;
        
        t2 = 0;
        while (t2 < t1)
        {
            gVolumeMiss.weight = Mathf.Lerp(1, 0, t2 / t1);
            mainCamera.orthographicSize = Mathf.Lerp(6, 7.75f, t2 / t1);
            t2 += 0.7f * Time.deltaTime;
            yield return null;
        }

        EffectMiss = true;
        mainCamera.orthographicSize = 7.75f;
        gVolumeMiss.weight = 0;
    }

    IEnumerator EffetMort()
    {
        mainCamera.transform.position = new Vector3(transform.position.x,transform.position.y,mainCamera.transform.position.z);
        float timeElapsed = 0;
        while (timeElapsed < t3)
        {
            mainCamera.orthographicSize = Mathf.Lerp(7.75f, 4.5f, timeElapsed / t3);
            gVolumeMort.weight = Mathf.Lerp(0, 1, timeElapsed / t3);
            timeElapsed += 1.5f * Time.deltaTime;
            yield return null;
        }
        mainCamera.orthographicSize = 4.5f;
        gVolumeMort.weight = 1;
    }
    
    IEnumerator TempsStun()
    {
        stun = true;
        AttaquesNormales.instance.canAttack = false;
        yield return new WaitForSeconds(stunAfterHit);
        AttaquesNormales.instance.canAttack = true;
        stun = false;
        CharacterController.instance.anim.SetBool("isHurt",false);
    }
    
    IEnumerator TempsInvinsibilité(float duration)
    {
        invinsible = true;
        yield return new WaitForSeconds(duration);
        invinsible = false;
    }
    
    void Die()
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(audioClipArray[0]);
        compteurScore.text = ScoreManager.instance.currentScore.ToString();
        CharacterController.instance.allowMovements = false;
        CharacterController.instance.movement = Vector2.zero;
        CharacterController.instance.rb.velocity = Vector2.zero;
        invinsible = true;
        animPlayer.SetBool("isIdle",true);
        animPlayer.SetBool("isDead",true);
        stun = true;
        StartCoroutine(ReloadScene());
        StartCoroutine(EffetMort());
    }

    IEnumerator ReloadScene()
    {
        GameUI.SetActive(false);
        yield return new WaitForSeconds(3f);
        deathMenu.SetActive(true);
        playerMaterial.material = deathDisolve;
        Time.timeScale = 0;
    }

    public void PlayDisolveSound()
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(audioClipArray[1]);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        var g = CharacterController.instance;
        if (col.gameObject.layer == 7 && !g.isDashing && (g.canBuffer || g.canBoost))
        {
            if (g.rb.GetContacts(new List<Collider2D>()) >= 1)
            {
                g.canBoost = true;

                g.playerCol.enabled = false; 
                g.Dashing();
                g.allowMovements = false;
                Debug.Log("Givin' it a little push");
                
            }
            else
            {
                g.canBuffer = false;
                g.canBoost = false;
                g.playerCol.enabled = true;
                g.allowMovements = true;
                g.stopDash = false;
            }
        }
    }
}
