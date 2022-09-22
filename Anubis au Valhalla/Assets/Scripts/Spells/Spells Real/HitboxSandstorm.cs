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
      for (int i = 0; i < 3; i++)
      {
         if (tempsReloadHitSandstorm <= sOSandstorm.espacementDoT && stopAttack == false)
         {
            tempsReloadHitSandstorm += Time.deltaTime;
         }

         if (tempsReloadHitSandstorm > sOSandstorm.espacementDoT && col.gameObject.tag == "Monstre")
         {
            Debug.Log("touché");
            col.GetComponent<IA_Monstre1>().TakeDamage(sOSandstorm.puissanceAttaque);
            //yield return new WaitForSeconds(tempsReloadHitSandstormMax);
            tempsReloadHitSandstorm = 0;
         }
      } 
   }
   
   private void OnTriggerExit2D(Collider2D col)
   {
      stopAttack = true;
      tempsReloadHitSandstorm = 0;
   }
}
