using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElecticBallBehiavour : MonoBehaviour
{
    public PouvoirFoudreObject soPouvoirFoudre;
    private Rigidbody2D rb;
    public GameObject[] monsterList;
    public List<GameObject> alreadyDone;
    public GameObject target;
    public int bounce;
    private bool noTarget = true;


    private void Start()
    {
        target = null;
        rb = gameObject.GetComponent<Rigidbody2D>();
        Destroy(gameObject,soPouvoirFoudre.bulletDuration);
        transform.localScale = soPouvoirFoudre.bulletScale;
    }

    void Update()
    {
        if (noTarget)
        {
            rb.velocity = transform.right * soPouvoirFoudre.bulletSpeed;
        }
       
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre" && bounce <= soPouvoirFoudre.maxBounce)
        {
            alreadyDone.Add(col.gameObject);
            bounce++;
            noTarget = false;
            monsterList = GameObject.FindGameObjectsWithTag("Monstre").Where(e => !e.Equals(col.gameObject)).ToArray();
            col.GetComponentInParent<MonsterLifeManager>().TakeDamage(soPouvoirFoudre.thrustDamage, soPouvoirFoudre.stagger);
            GetClosestEnemy(monsterList);
            Vector2 dir = target.transform.position - transform.position;
            rb.velocity = Vector2.zero;
            rb.velocity += dir*(soPouvoirFoudre.bulletSpeed*1.3f);
            Debug.Log(dir);
        }
        else if(bounce > soPouvoirFoudre.maxBounce)
        {
            Destroy(gameObject);
        }
    }
    
      
    Transform GetClosestEnemy (GameObject[] enemies)
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

        if (bestTarget.gameObject.layer == 6)
        {
            target = bestTarget.gameObject;
        }
        return bestTarget;
    }
    
}
