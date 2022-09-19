using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public GameObject player;
    public static DamageManager instance;

    public float vieActuelle;
    public float vieMax;
    public bool invinsible;
    public float TempsInvinsbleAfterHit;
    public float StunAfterHit;
    public bool stun;
    
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
    
    public void TakeDamage(int damage)
    {
        if (!invinsible)
        {
            vieActuelle -= damage;
            StartCoroutine(TempsInvinsibilité());
            StartCoroutine(TempsStun());
        }

        if (vieActuelle <= 0)
        {
            Die();
        }
    }

    IEnumerator TempsStun()
    {
        stun = true;
        yield return new WaitForSeconds(StunAfterHit);
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
