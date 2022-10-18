using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
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
    private ColorAdjustments ca;

    [Header("Alterations d'Etat")]
    public bool stun;
    public bool invinsible;
    public Animator animPlayer;
    
    [Header("Feedbacks")]
    public float timeHitStop;
    public float timeRedScreen;

    [Header("Stats")]
    public float vieActuelle;
    public float vieMax;
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
    }

    public void TakeDamage(int damage)
    {
        if (!invinsible)
        {
            StartCoroutine(RedScreen(timeRedScreen));
            HitStop(timeHitStop*(damage/10));
            vieActuelle -= damage;
            StartCoroutine(TempsInvinsibilité());
            StartCoroutine(TempsStun());
            textDamage.GetComponentInChildren<TextMeshPro>().SetText(damage.ToString());
            Instantiate(textDamage, new Vector3(transform.position.x,transform.position.y + 1,-5), Quaternion.identity);
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
        Time.timeScale = 0.0f;
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
        Color newColor = new Color32(255, 0, 0,0); 
        Color originalColor = new Color32(255, 255, 255,0);
        gVolume.profile.TryGet(out ca);
        ca.colorFilter.value = newColor;
        yield return new WaitForSeconds(timeRedScreenC);
        ca.colorFilter.value = originalColor;
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
