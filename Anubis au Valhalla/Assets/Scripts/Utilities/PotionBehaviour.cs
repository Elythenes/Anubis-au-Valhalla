using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class PotionBehaviour : MonoBehaviour
{
   [SerializeField] private float force = 3f;
   [SerializeField] private float deceleration = 0.3f;
   public float timer;
   public bool canPickUp;
   public int vieHealed;
   public Rigidbody2D rb;

   private void Awake()
   {
      Vector2 explode = new Vector2(Random.Range(-force, force), Random.Range(-force, force));
      rb.AddForce(explode, ForceMode2D.Impulse);
      rb.drag = deceleration;
   }

   private void Update()
   {
      if (timer >= 0)
      {
         rb.velocity -= rb.velocity * 0.01f;
         timer -= Time.deltaTime;
      }
      
      if (canPickUp && Input.GetKeyDown(KeyCode.A))
      {
         Potion();
      }
   }

   private void OnTriggerEnter2D(Collider2D col)
   {
      if (col.CompareTag("Player"))
      {
         canPickUp = true;
      }
   }
   
   private void OnTriggerExit2D(Collider2D col)
   {
      if (col.CompareTag("Player"))
      {
         canPickUp = false;
      }
      
   }


   void Potion()
   {
      DamageManager.instance.vieActuelle += vieHealed;
      if (DamageManager.instance.vieActuelle >= DamageManager.instance.vieMax)
      {
         DamageManager.instance.vieActuelle =  DamageManager.instance.vieMax;
      }
      LifeBarManager.instance.SetHealth(DamageManager.instance.vieActuelle);
      
      Destroy(gameObject);
   }
}
