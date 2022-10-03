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
