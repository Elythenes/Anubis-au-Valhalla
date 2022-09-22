using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxSandstorm : MonoBehaviour
{
   

   [Header("SandStorm")]
   public int puissanceAttaqueSandstorm;
   public float tempsReloadHitSandstormMax;
   public float tempsReloadHitSandstorm;
   public bool stopAttack;

   private void Start()
   {
      puissanceAttaqueSandstorm = SkillManager.instance.puissanceAttaqueSandstorm;
      tempsReloadHitSandstormMax = SkillManager.instance.espacementDoTSandstorm;
   }

   private void OnTriggerStay2D(Collider2D col)
   {
      for (int i = 0; i < 3; i++)
      {
         if (tempsReloadHitSandstorm <= tempsReloadHitSandstormMax && stopAttack == false)
         {
            tempsReloadHitSandstorm += Time.deltaTime;
         }

         if (tempsReloadHitSandstorm > tempsReloadHitSandstormMax && col.gameObject.tag == "Monstre")
         {
            Debug.Log("touché");
            col.GetComponent<IA_Monstre1>().TakeDamage(puissanceAttaqueSandstorm);
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
