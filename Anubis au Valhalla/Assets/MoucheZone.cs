using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoucheZone : MonoBehaviour
{
    [Header("FlameArea")] 
    public PouvoirAmeObject sOPouvoirAme;

    public GameObject mouche;
    public List<GameObject> moucheList;
    public float tempsReloadHitFlameAreaTimer;
    public bool stopAttack;
    public int moucheAmount;

    private void Start()
    {
        moucheAmount = sOPouvoirAme.moucheAmount;
        for (int i = 0; i < moucheAmount; i++)
        {
            GameObject moucheObj = Instantiate(mouche, transform.position + new Vector3(Random.Range(-3, 3), Random.Range(-3, 3)), Quaternion.identity);
            moucheList.Add(moucheObj);
        }
        Destroy(gameObject,sOPouvoirAme.hitboxDashDuration);
        transform.localScale = sOPouvoirAme.zoneScale;
    }

    private void Update()
    {
        if (moucheAmount == 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre" && moucheAmount > 0)
        {
            stopAttack = false;
            for (int i = 0; i < sOPouvoirAme.nombreOfDot; i++)
            {
                if (tempsReloadHitFlameAreaTimer <= sOPouvoirAme.espacementDoT && stopAttack == false)
                {
                    tempsReloadHitFlameAreaTimer += Time.deltaTime;
                }

                if (tempsReloadHitFlameAreaTimer > sOPouvoirAme.espacementDoT && col.gameObject.tag == "Monstre")
                {
                    Debug.Log("touché");
                    col.GetComponentInParent<MonsterLifeManager>().DamageText(sOPouvoirAme.dashPuissanceAttaque);
                    col.GetComponentInParent<MonsterLifeManager>().TakeDamage(sOPouvoirAme.dashPuissanceAttaque,sOPouvoirAme.stagger);
                    tempsReloadHitFlameAreaTimer = 0;
                    moucheAmount--;
                    Destroy(moucheList[0]);
                }
             
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            stopAttack = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            
            tempsReloadHitFlameAreaTimer = 0;
        }
    }
}
