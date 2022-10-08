using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo3Hitbox : MonoBehaviour
{
    private void Start()
    {
        transform.parent = CharacterController.instance.transform;
        Destroy(gameObject, AttaquesNormales.instance.dureeHitbox3);
        transform.localScale *= AttaquesNormales.instance.rangeAttaque3;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Monstre"))
        {
            col.gameObject.GetComponent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(AttaquesNormales.instance.damageC1));
            col.gameObject.GetComponent<MonsterLifeManager>().DamageText(Mathf.RoundToInt(AttaquesNormales.instance.damageC1));
        }
    }
}
