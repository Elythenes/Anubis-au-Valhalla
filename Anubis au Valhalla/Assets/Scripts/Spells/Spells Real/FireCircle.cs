using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class FireCircle : MonoBehaviour
{
    public PouvoirFeuObject soPouvoirFeu;

    private void Start()
    {
      Destroy(gameObject,soPouvoirFeu.circleDuration);
    }

    private void Update()
  {
    if (transform.localScale.x < soPouvoirFeu.circleMaxScale && transform.localScale.y < soPouvoirFeu.circleMaxScale)
    {
      transform.localScale += new Vector3(soPouvoirFeu.circleScaleSpeed,soPouvoirFeu.circleScaleSpeed,0);
    }
  }


  private void OnTriggerEnter2D(Collider2D col)
  {
    if (col.gameObject.tag == "Monstre")
    {
      //Vector2 pushForce = col.transform.position - transform.position;
      //col.GetComponent<AIPath>().canMove = false;
      //col.GetComponentInParent<MonsterLifeManager>().Reset(0.2f);
      col.GetComponentInParent<MonsterLifeManager>().DamageText(soPouvoirFeu.attaqueNormaleDamage);
      col.GetComponentInParent<MonsterLifeManager>().TakeDamage(soPouvoirFeu.attaqueNormaleDamage, soPouvoirFeu.stagger);
      //col.GetComponent<Rigidbody2D>().AddRelativeForce(pushForce.normalized*soPouvoirFeu.pushForce,ForceMode2D.Impulse);
    }
  }
}
