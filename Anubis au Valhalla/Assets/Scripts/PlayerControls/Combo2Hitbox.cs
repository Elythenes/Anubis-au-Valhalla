using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo2Hitbox : MonoBehaviour
{
    private void Start()
    {
        transform.parent = CharacterController.instance.transform;
        Destroy(gameObject, AttaquesNormales.instance.dureeHitbox2);
        transform.localScale *= AttaquesNormales.instance.rangeAttaque2;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Monstre"))
        {
            col.gameObject.GetComponent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(AttaquesNormales.instance.damageC2));
            col.gameObject.GetComponent<MonsterLifeManager>().DamageText(Mathf.RoundToInt(AttaquesNormales.instance.damageC2));
        }
    }
}
