using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxSandstorm : MonoBehaviour
{
   [Header("SandStorm")] 
   public SpellFollowingAreaObject sOSandstorm;
   public float tempsReloadHitSandstorm;
   public bool stopAttack;
   public int nbDoT;

   private void OnTriggerStay2D(Collider2D col)
   {
      if (col.gameObject.tag == "Monstre" && !stopAttack)
      {
         for (int i = 0; i < nbDoT; i++)
         {
            if (tempsReloadHitSandstorm <= sOSandstorm.espacementDoT && stopAttack == false)
            {
               tempsReloadHitSandstorm += Time.deltaTime;
            }

            if (tempsReloadHitSandstorm > sOSandstorm.espacementDoT)
            {
               Debug.Log("touché");
               col.GetComponentInParent<MonsterLifeManager>().DamageText(sOSandstorm.puissanceAttaque);
               col.GetComponentInParent<MonsterLifeManager>().TakeDamage(sOSandstorm.puissanceAttaque,sOSandstorm.stagger);
               tempsReloadHitSandstorm = 0;
            }
         } 
      }
   }
   
   private void OnTriggerExit2D(Collider2D col)
   {
      if (col.gameObject.tag == "Monstre")
      {
         //stopAttack = true;
         tempsReloadHitSandstorm = 0;
      }
   }
}
