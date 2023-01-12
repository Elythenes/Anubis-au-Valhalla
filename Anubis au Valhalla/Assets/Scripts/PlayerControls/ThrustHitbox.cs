using System.Collections;
using Pathfinding;
using UnityEngine;

public class ThrustHitbox : Combo1Hitbox
{
    public override void Start()
    {
        transform.parent = CharacterController.instance.transform;
        Destroy(gameObject, AttaquesNormales.instance.dureeHitbox[3]);
        transform.localScale *= AttaquesNormales.instance.rangeAttaque[3];
    }
    
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monstre"))
        {
            if (CharacterController.instance.isHiting == false && other.gameObject.GetComponentInParent<MonsterLifeManager>().isInvincible == false)
            {
                CharacterController.instance.isHiting = true; 
            }
            
            StartCoroutine(ResetTracking());
            Vector3 angleKnockback = other.transform.position - transform.parent.position;
            Vector3 angleNormalized = angleKnockback.normalized;
            float angle = Mathf.Atan2(transform.parent.position.y - other.transform.position.y,transform.parent.position.x - other.transform.position.x ) * Mathf.Rad2Deg;
            GameObject effetSang = Instantiate(bloodEffect, other.transform.position, Quaternion.identity);
            effetSang.transform.rotation = Quaternion.Euler(0,0,angle);
            
            
            
            if (other.GetComponent<PuppetHealth>())
            {
                other.gameObject.GetComponent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(AttaquesNormales.instance.specialDmg), stagger);
                return;
            }
            
            
            
            other.gameObject.GetComponent<AIPath>().canMove = false;
            other.gameObject.GetComponentInParent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(AttaquesNormales.instance.specialDmg), stagger);
        }
    }
    
    IEnumerator ResetTracking()
    {
        yield return null;
        CharacterController.instance.isHiting = false;
    }
}
