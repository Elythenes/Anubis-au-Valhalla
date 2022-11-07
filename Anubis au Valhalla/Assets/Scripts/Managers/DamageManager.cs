using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DamageManager : MonoBehaviour
{
    [Header("Objects")]
    public GameObject textDamage;
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
    }

    private void Update()
    {
        if (invinsible)
        {
            animPlayer.SetBool("IsInvinsible", true);
        }
        else
        {
            animPlayer.SetBool("IsInvinsible", false);
        }

        if (vieActuelle > vieMax)
        {
            vieActuelle = vieMax;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!invinsible)
        {
            if (!CharacterController.instance.isDashing)
            {
                Debug.Log("touché");
                StartCoroutine(RedScreen(timeRedScreen));
                HitStop(timeHitStop*(damage/10));
                Time.timeScale = 0.3f;
                if (isAnkh)
                {
                    damageReduction = ankhShieldData.reducteurDamage;
                    
                }
                else
                {
                    damageReduction = 1;
                }
                
                vieActuelle -= damage / damageReduction;
                textDamage.GetComponentInChildren<TextMeshPro>().SetText((damage / damageReduction).ToString());
                Instantiate(textDamage, new Vector3(transform.position.x,transform.position.y + 1,-5), Quaternion.identity);
                LifeBarManager.instance.SetHealth(vieActuelle);
                
                StartCoroutine(TempsInvinsibilité());
                StartCoroutine(TempsStun());
            }
            else
            {
                Debug.Log("miss");
                StartCoroutine(MissScreen(timeRedScreen));
                HitStop(timeHitStop*(damage/10));
                Time.timeScale = 0.3f;
            }
            
        }

        if (vieActuelle <= 0)
        {
            Die();
        }
    }

    public void HitStop(float duration)
    {
        if (stopWaiting)
            return;
        StartCoroutine(WaitStop(duration));
    }

    IEnumerator WaitStop(float duration)
    {
        stopWaiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        stopWaiting = false;
    }

    public IEnumerator RedScreen(float timeRedScreenC)
    {
        if (!CharacterController.instance.isDashing)
        {
            gVolume.weight = Mathf.Lerp(0, 1, timeRedScreen / Time.deltaTime);
            yield return new WaitForSeconds(timeRedScreen);
            gVolume.weight = Mathf.Lerp(1, 0, timeRedScreen / Time.deltaTime);
        }
    }
    
    public IEnumerator MissScreen(float timeRedScreenC)
    {
        if (CharacterController.instance.isDashing)
        {
            gVolumeMiss.weight = Mathf.Lerp(0, 1, timeRedScreen / Time.deltaTime);
            yield return new WaitForSeconds(timeRedScreen);
            gVolumeMiss.weight = Mathf.Lerp(1, 0, timeRedScreen / Time.deltaTime);
        }
    }
    
  
    
    IEnumerator TempsStun()
    {
        stun = true;
        AttaquesNormales.instance.canAttack = false;
        yield return new WaitForSeconds(StunAfterHit);
        AttaquesNormales.instance.canAttack = true;
        stun = false;
    }
    
    IEnumerator TempsInvinsibilité()
    {
        invinsible = true;
        yield return new WaitForSeconds(TempsInvinsbleAfterHit);
        invinsible = false;
    }
    
    void Die()
    {
        Destroy(player);
    }
}
