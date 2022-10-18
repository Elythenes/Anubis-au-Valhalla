using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionBehaviour : MonoBehaviour
{
   public bool canPickUp;
   private void OnTriggerStay2D(Collider2D col)
   {
      if (col.CompareTag("Player"))
      {
         canPickUp = true;
      }

      if (canPickUp && Input.GetKeyDown(KeyCode.A))
      {
         Potion();
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
      DamageManager.instance.
   }
}
