using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class DamageManager : MonoBehaviour
{
    [Header("Objects")]
    public GameObject textDamage;
    public GameObject textHealDamage;
    public Camera mainCamera;
    public GameObject player;
    public static DamageManager instance;
    public Volume gVolume;
    public Volume gVolumeMiss;
    public Volume gVolumeMort;
    private ColorAdjustments ca;
    public SpellDefenceObject ankhShieldData;

    [Header("Alterations d'Etat")] 
    public float knockbackAmount;
    public bool stun;
    public bool invinsible;
    public bool isAnkh;
    public Animator animPlayer;
    
    [Header("Feedbacks")]
    public float timeHitStop;
    public float timeRedScreen;
    public bool EffectMiss;
    public float t1;
    public float t2;
    public float t3;

    [Header("Stats")]
    [NaughtyAttributes.ReadOnly] public int vieActuelle = AnubisCurrentStats.instance.vieActuelle;
    [NaughtyAttributes.ReadOnly] public int vieMax = AnubisCurrentStats.instance.vieMax;
    [NaughtyAttributes.ReadOnly] public int damageReduction = AnubisCurrentStats.instance.damageReduction;
    [NaughtyAttributes.ReadOnly] public float tempsInvinsibleAfterHit = AnubisCurrentStats.instance.tempsInvinsbleAfterHit;
    [NaughtyAttributes.ReadOnly] public float stunAfterHit = AnubisCurrentStats.instance.stunAfterHit;
    private bool stopWaiting;

    [Header("Variables de tracking")] 
    public bool isHurt;
    
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
    }
    
    void Start()
    {
        vieActuelle = vieMax;
        LifeBarManager.instance.SetMaxHealth(vieMax);
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.01F * Time.timeScale;
        stopWaiting = false;
    }

    private void Update()
    {
        if (vieActuelle > vieMax)
        {
            vieActuelle = vieMax;
        }
    }

    public void TakeDamage(int damage, GameObject enemy)
    {
        if (!invinsible)
        {
            if (!CharacterController.instance.isDashing)
            {
                isHurt = true;
                isHurt = false;
                var angle = CharacterController.instance.transform.position - enemy.transform.position;
                angle.Normalize();
                CharacterController.instance.rb.AddForce(damage*angle*knockbackAmount, ForceMode2D.Impulse);
                StartCoroutine(RedScreenStart(timeRedScreen));
                HitStop(timeHitStop,false);
                vieActuelle -= damage / damageReduction;
                GameObject textObj = Instantiate(textDamage, new Vector3(transform.position.x,transform.position.y + 1,-5), Quaternion.identity);
                textObj.GetComponentInChildren<TextMeshPro>().SetText((damage / damageReduction).ToString());
                
                
                if (vieActuelle <= 0)
                {
                    Debug.Log("il semblerait qu'il soit mort");
                    StopCoroutine(TempsStun());
                    vieActuelle = 0;
                    Die();
                }
                else
                {
                    animPlayer.SetBool("isHurt",true);
                    StartCoroutine(TempsInvinsibilité(2f));
                    StartCoroutine(TempsStun());
                }
                
                LifeBarManager.instance.SetHealth(vieActuelle);
                
            }
            else if(CharacterController.instance.isDashing)
            {
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
        vieActuelle += healAmount;
        GameObject textHealObj = Instantiate(textHealDamage, new Vector3(transform.position.x,transform.position.y + 1,-5), Quaternion.identity);
        textHealObj.GetComponentInChildren<TextMeshPro>().SetText((healAmount).ToString());
        if (vieActuelle >= 100)
        {
            vieActuelle = 100;
        }
        LifeBarManager.instance.SetHealth(vieActuelle);
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
        Debug.Log("enterons le");
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
        CharacterController.instance.allowMovements = false;
        GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
        Debug.Log("vrai");
        invinsible = true;
        animPlayer.SetBool("isDead",true);
        stun = true;
        StartCoroutine(ReloadScene());
        StartCoroutine(EffetMort());
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
}
