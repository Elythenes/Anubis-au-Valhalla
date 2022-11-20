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
    public Camera mainCamera;
    public GameObject player;
    public static DamageManager instance;
    public Volume gVolume;
    public Volume gVolumeMiss;
    private ColorAdjustments ca;
    public SpellDefenceObject ankhShieldData;

    [Header("Alterations d'Etat")]
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

    [Header("Stats")]
    public int vieActuelle;
    public int vieMax;
    public int damageReduction;
    public float TempsInvinsbleAfterHit;
    public float StunAfterHit;
    private bool stopWaiting;
    
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
                Debug.Log("touché");
                var angle = CharacterController.instance.transform.position - enemy.transform.position;
                angle.Normalize();
                CharacterController.instance.rb.AddForce(damage*angle, ForceMode2D.Impulse);
                animPlayer.SetBool("isHurt",true);
                StartCoroutine(RedScreenStart(timeRedScreen));
                HitStop(timeHitStop,false);
                if (isAnkh)
                {
                    damageReduction = ankhShieldData.reducteurDamage;
                }
                else
                {
                    damageReduction = 1;
                }
                
                vieActuelle -= damage / damageReduction;
                GameObject textObj = Instantiate(textDamage, new Vector3(transform.position.x,transform.position.y + 1,-5), Quaternion.identity);
                textObj.GetComponentInChildren<TextMeshPro>().SetText((damage / damageReduction).ToString());
                LifeBarManager.instance.SetHealth(vieActuelle);
                if (vieActuelle <= 0)
                {
                    Die();
                }
                
                StartCoroutine(TempsInvinsibilité(2f));
                StartCoroutine(TempsStun());
            }
            else if(CharacterController.instance.isDashing)
            {
                if (EffectMiss)
                {
                    StartCoroutine(TempsInvinsibilité(0.5f));
                    Debug.Log("miss");
                    HitStop(timeHitStop,true);
                    StartCoroutine(MissScreen(timeRedScreen));
                    EffectMiss = false;
                }
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
    
  
    
    IEnumerator TempsStun()
    {
        stun = true;
        AttaquesNormales.instance.canAttack = false;
        yield return new WaitForSeconds(StunAfterHit);
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
        animPlayer.SetBool("isDead",true);
        stun = true;
        StartCoroutine(ReloadScene());
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
}
