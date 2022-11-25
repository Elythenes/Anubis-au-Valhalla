using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PotionManager : MonoBehaviour
{
   public static PotionManager Instance;

   public KeyCode usePotion = KeyCode.A;
   [Expandable] public PotionObject currentPotion;


   private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
      }
   }

   private void Update()
   {
      if (Input.GetKeyDown(usePotion))
      {
         if (currentPotion.type == PotionObject.PotionType.SpecialItem)
         {
            Debug.Log("Je ne peux pas boire une chose qui n'est pas une potion");
         }
         else
         {
            DrinkPotion(currentPotion);
         }
      }
   }

   void DrinkPotion(PotionObject glouglou)
   {
      if (glouglou.type == PotionObject.PotionType.StatBasicPotion 
          || glouglou.type == PotionObject.PotionType.StatSpecificPotion)
      {
         //float compteurDuration = 0f;
         //while (compteurDuration < glouglou.effectDuration)                //faire un truc pour faire la duraction
         {
            for (int i = 0; i < 3; i++)
            {
               AnubisCurrentStats.instance.comboDamage[i] += glouglou.damage;
            }
            AnubisCurrentStats.instance.thrustDamage += glouglou.damage;
            //DamageManager.instance.Heal(glouglou.heal);
            AnubisCurrentStats.instance.damageReduction += glouglou.armor;
            if (glouglou.wArmor != 0)
            {
               AnubisCurrentStats.instance.damageReduction /= glouglou.wArmor;
            }
            if (glouglou.speed != 0)
            {
               AnubisCurrentStats.instance.speedX *= glouglou.speed;
               AnubisCurrentStats.instance.speedY *= glouglou.speed;
            }
            //Fonction avec la MagicForce
            Debug.Log("drink potion et ajout de stat");
            VerifyForSpecificPotion(glouglou.index);
         }
      }
      else if (glouglou.type == PotionObject.PotionType.SpecialPotion)
      {
         UseSpecialPotion(glouglou.index);
      }
   }

   
   void VerifyForSpecificPotion(int num)
   {
      switch (num)
      {
         case 1:
            /*if (DamageManager.instance.isHurt)
            {
               currentPotion. = currentPotion.effectDuration;
            }*/
            break;
      }
   }
   
   void UseSpecialPotion(int num)
   {
      switch (num)
      {
         case 0:
            break;
      }
   }

   void TriggerSpecialItem(int num)
   {
      
   }
   
   
   
   
}