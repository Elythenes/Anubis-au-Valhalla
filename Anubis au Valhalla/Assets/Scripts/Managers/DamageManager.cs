using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    [Header("Objects")]
    public GameObject textDamage;
    public GameObject player;
    public static DamageManager instance;

    [Header("Alterations d'Etat")]
    public bool stun;
    public bool invinsible;
    public Animator animPlayer;
    
    [Header("Stats")]
    public float vieActuelle;
    public float vieMax;
    public float TempsInvinsbleAfterHit;
    public float StunAfterHit;
    
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
