using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Akh : MonoBehaviour
{
    [Header("FlameArea")]
    public List<MonsterLifeManager> monsterList;
    public SpellStaticAreaObject soAkh;
    public float tempsReloadHitFlameAreaTimer;
    public bool stopAttack;

   /* private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            foreach (MonsterLifeManager VARIABLE in monsterList)
            {
                Debug.Log("touché");
                col.GetComponent<MonsterLifeManager>().DamageText(soAkh.puissanceAttaque + Mathf.RoundToInt(Mathf.Log(Souls.instance.soulBank + 1) *5));
                col.GetComponent<MonsterLifeManager>().TakeDamage(soAkh.puissanceAttaque + Mathf.RoundToInt(Mathf.Log(Souls.instance.soulBank + 1) *5),soAkh.stagger);
                tempsReloadHitFlameAreaTimer = 0;
            }
            stopAttack = false;
            for (int i = 0; i < soAkh.nombreOfDot; i++)
            {
                if (tempsReloadHitFlameAreaTimer <= soAkh.espacementDoT && stopAttack == false)
                {
                    tempsReloadHitFlameAreaTimer += Time.deltaTime;
                }

                if (tempsReloadHitFlameAreaTimer > soAkh.espacementDoT && col.gameObject.tag == "Monstre")
                {
                    Debug.Log("touché");
                    col.GetComponent<MonsterLifeManager>().DamageText(soAkh.puissanceAttaque + Mathf.RoundToInt(Mathf.Log(Souls.instance.soulBank + 1) *5));
                    col.GetComponent<MonsterLifeManager>().TakeDamage(soAkh.puissanceAttaque + Mathf.RoundToInt(Mathf.Log(Souls.instance.soulBank + 1) *5),soAkh.stagger);
                    tempsReloadHitFlameAreaTimer = 0;
                }
            }
        }
    }*/

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            monsterList.Add(col.gameObject.GetComponent<MonsterLifeManager>());
            stopAttack = false;
            foreach (MonsterLifeManager monstre in monsterList)
            {
                Debug.Log("touché");
                monstre.GetComponent<MonsterLifeManager>().DamageText(soAkh.puissanceAttaque + Mathf.RoundToInt(Mathf.Log(Souls.instance.soulBank + 1) *5));
                monstre.GetComponent<MonsterLifeManager>().TakeDamage(soAkh.puissanceAttaque + Mathf.RoundToInt(Mathf.Log(Souls.instance.soulBank + 1) *5),soAkh.stagger);
                tempsReloadHitFlameAreaTimer = 0;
                monsterList.Clear();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            monsterList.Remove(col.gameObject.GetComponent<MonsterLifeManager>());
            tempsReloadHitFlameAreaTimer = 0;
        }
    }
}
