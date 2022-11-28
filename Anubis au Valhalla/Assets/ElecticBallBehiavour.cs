using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ElecticBallBehiavour : MonoBehaviour
{
    public PouvoirFoudreObject soPouvoirFoudre;
    private Rigidbody2D rb;
    public BounceRange bounceData;
    public List<GameObject> monsterList;
    public GameObject target;


    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        Destroy(gameObject,soPouvoirFoudre.bulletDuration);
        transform.localScale = soPouvoirFoudre.bulletScale;
    }

    void Update()
    {
        monsterList = bounceData.monsterList;
        rb.velocity = transform.right * soPouvoirFoudre.bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            monsterList.Remove(col.gameObject);
            col.GetComponentInParent<MonsterLifeManager>().DamageText(soPouvoirFoudre.thrustDamage);
            col.GetComponentInParent<MonsterLifeManager>().TakeDamage(soPouvoirFoudre.thrustDamage, soPouvoirFoudre.stagger);
            GetClosestEnemy(monsterList);
            if (target is not null)
            {
                transform.DOMove(target.transform.position, 0.2f);
            }
            else if(target is null)
            {
                Destroy(gameObject);
            }
        }
    }
    
      
    Transform GetClosestEnemy (List<GameObject> enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(GameObject potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }

        if (bestTarget is not null)
        {
            target = bestTarget.gameObject;
        }
        return bestTarget;
    }
    
}
