using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Akh : MonoBehaviour
{
    public List<MonsterLifeManager> monsterList;
    public PouvoirAmeObject soPouvoirAme;
    public float tempsReloadHitFlameAreaTimer;

    private void Start()
    {
        Destroy(gameObject,soPouvoirAme.attaqueNormaleDuration);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            monsterList.Add(col.gameObject.GetComponent<MonsterLifeManager>());
            foreach (MonsterLifeManager monstre in monsterList)
            {
                Debug.Log("touché");
                monstre.GetComponentInParent<MonsterLifeManager>().DamageText(soPouvoirAme.attaqueNormaleDamage + Mathf.RoundToInt(Mathf.Log(Souls.instance.soulBank + 1) *5));
                monstre.GetComponentInParent<MonsterLifeManager>().TakeDamage(soPouvoirAme.attaqueNormaleDamage + Mathf.RoundToInt(Mathf.Log(Souls.instance.soulBank + 1) *5),soPouvoirAme.stagger);
                monsterList.Clear();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            monsterList.Remove(col.gameObject.GetComponentInParent<MonsterLifeManager>());
            tempsReloadHitFlameAreaTimer = 0;
        }
    }
}
