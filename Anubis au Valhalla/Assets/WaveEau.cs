using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEau : MonoBehaviour
{
    public PouvoirEauObject sOPouvoirEau;
    public float damageTimer;

    private void Start()
    {
        damageTimer = 0;
        Destroy(gameObject,sOPouvoirEau.durationWave);
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        //damageTimer += Mathf.RoundToInt(Time.deltaTime * sOPouvoirEau.durationDamageScale);
        damageTimer += Time.deltaTime * sOPouvoirEau.durationDamageScale;
        
        if (transform.localScale.x < sOPouvoirEau.maxScaleWave.x && transform.localScale.y < sOPouvoirEau.maxScaleWave.x)
        {
            transform.localScale += new Vector3(0.01f, 0.01f, 0);
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("touché");
        col.GetComponentInParent<MonsterLifeManager>().DamageText(Mathf.RoundToInt(sOPouvoirEau.dammageWave * damageTimer) + (AnubisCurrentStats.instance.vieActuelle /10));
        col.GetComponentInParent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(sOPouvoirEau.dammageWave * damageTimer),sOPouvoirEau.staggerRayon);
        //DamageManager.instance.Heal(Mathf.RoundToInt((sOPouvoirEau.dammageWave * damageTimer)/10));
    }
    
    /*private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            stopAttack = false;
            for (int i = 0; i < 1; i++)
            {
                if (tempsReloadHitTimer <= 0.1f && stopAttack == false)
                {
                    tempsReloadHitTimer += Time.deltaTime;
                }

                if (tempsReloadHitTimer > 0.1f && col.gameObject.tag == "Monstre")
                {
                    Debug.Log("touché");
                    col.GetComponentInParent<MonsterLifeManager>().DamageText(sOPouvoirEau.dammageWave * damageTimer);
                    col.GetComponentInParent<MonsterLifeManager>().TakeDamage(sOPouvoirEau.rayonDamage + (AnubisCurrentStats.instance.vieActuelle /25),sOPouvoirEau.staggerRayon);
                    DamageManager.instance.Heal((sOPouvoirEau.dammageWave * damageTimer)/10);
                    tempsReloadHitTimer = 0;
                }
            }
        }
    }*/
}
