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
   private SpriteRenderer sr;
   public Sprite spriteNormal;
   public Sprite spriteOutline;

   private void Awake()
   {
      Vector2 explode = new Vector2(Random.Range(-force, force), Random.Range(-force, force));
      rb.AddForce(explode, ForceMode2D.Impulse);
      rb.drag = deceleration;
      sr = GetComponent<SpriteRenderer>();
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
         //sr.sprite = spriteOutline;
      }
   }
   
   private void OnTriggerExit2D(Collider2D col)
   {
      if (col.CompareTag("Player"))
      {
         canPickUp = false;
        // sr.sprite = spriteNormal;
      }
      
   }


   void Potion()
   {
      DamageManager.instance.Heal(vieHealed);
      Destroy(gameObject);
   }
}
