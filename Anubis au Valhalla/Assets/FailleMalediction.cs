using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailleMalediction : MonoBehaviour
{
    public PouvoirMaledictionObject soMalediction;
    private Rigidbody2D rb;

    private void Start()
    {
        transform.localScale = soMalediction.ScaleYAttaqueNormale;
        Destroy(gameObject,soMalediction.dureeAttaqueNormale);
    }

    void Update()
    {
        transform.localScale += new Vector3(0.5f, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            col.GetComponentInParent<MonsterLifeManager>().DamageText(soMalediction.damageAttaqueNormale);
            col.GetComponentInParent<MonsterLifeManager>().TakeDamage(soMalediction.damageAttaqueNormale, soMalediction.staggerAttaqueNormale);
        }
    }
}
