using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxSandstorm : MonoBehaviour
{
   [Header("SandStorm")] 
   public SpellFollowingAreaType sOSandstorm;
   public float tempsReloadHitSandstorm;
   public bool stopAttack;

   private void OnTriggerStay2D(Collider2D col)
   {
      if (col.gameObject.tag == "Monstre")
      {
         for (int i = 0; i < 3; i++)
         {
            if (tempsReloadHitSandstorm <= sOSandstorm.espacementDoT && stopAttack == false)
            {
               tempsReloadHitSandstorm += Time.deltaTime;
            }

            if (tempsReloadHitSandstorm > sOSandstorm.espacementDoT)
            {
               Debug.Log("touché");
               col.GetComponent<IA_Monstre1>().TakeDamage(sOSandstorm.puissanceAttaque);
               col.GetComponent<IA_Monstre1>().DamageText(sOSandstorm.puissanceAttaque);
               tempsReloadHitSandstorm = 0;
            }
         } 
      }
   }
   
   private void OnTriggerExit2D(Collider2D col)
   {
      if (col.gameObject.tag == "Monstre")
      {
      stopAttack = true;
      tempsReloadHitSandstorm = 0;
      }
   }
}
