using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ExplosionAmeDash : MonoBehaviour
{
   public PouvoirAmeObject soPouvoirAme;

   private void Start()
   {
      Destroy(gameObject,soPouvoirAme.hitboxthrustDuration);
      transform.localScale = Vector3.zero;
   }

   private void Update()
   {
      if (transform.localScale.x < soPouvoirAme.explosionScale.x && transform.localScale.y < soPouvoirAme.explosionScale.y)
      {
         transform.localScale += new Vector3(0.05f,0.05f,0);
      }
    
   }
   
   private void OnTriggerEnter2D(Collider2D col)
   {
      if (col.gameObject.tag == "Monstre")
      {
         col.GetComponentInParent<MonsterLifeManager>().TakeDamage(soPouvoirAme.thrustDamage, soPouvoirAme.stagger);
      }
   }
}
