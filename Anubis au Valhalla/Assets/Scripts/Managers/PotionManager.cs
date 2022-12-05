using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor.Rendering;
using UnityEngine.UI;

public class PotionManager : MonoBehaviour
{
   public static PotionManager Instance;
   
   [Header("MANAGER")]
   public KeyCode usePotion = KeyCode.A;
   [Expandable] public PotionObject currentPotion;
   public bool isPotionSlotFill;
   [NaughtyAttributes.ReadOnly] public bool revokePotionEarly = false;

   [Header("SHOP")]
   [Expandable] public List<PotionObject> potionsForShop = new();


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
         
         if (currentPotion == null)
         {
            Debug.Log("il n'y a pas de potions");
         }
         else
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

      if (currentPotion != null)
      {
         VerifyForSpecificPotion(currentPotion.index);
      }
   }

   void DrinkPotion(PotionObject glou)
   {
      if (glou.type == PotionObject.PotionType.StatBasicPotion 
          || glou.type == PotionObject.PotionType.StatSpecificPotion)
      {
         UiManager.instance.panelPotion.SetActive(true);
         
         for (int i = 0; i < 3; i++)
         {
            AnubisCurrentStats.instance.comboDamage[i] += glou.damage;
         }
         AnubisCurrentStats.instance.thrustDamage += glou.damage;
         //DamageManager.instance.Heal(glouglou.heal);
         AnubisCurrentStats.instance.damageReduction += glou.armor;
         if (glou.wArmor != 0)
         {
            AnubisCurrentStats.instance.damageReduction /= glou.wArmor;
         }
         if (glou.speed != 0)
         {
            AnubisCurrentStats.instance.speedX *= glou.speed;
            AnubisCurrentStats.instance.speedY *= glou.speed;
         }
         //Fonction avec la MagicForce
         
         Debug.Log("drink potion et ajout de stat");
         StartCoroutine(CoroutinePotion(glou));

      }
      else if (glou.type == PotionObject.PotionType.SpecialPotion)
      {
         UseSpecialPotion(glou.index);
      }
      
   }

   
   void VerifyForSpecificPotion(int num)
   {
      Debug.Log("verify for spe");
      switch (num)
      {
         case 1:
            if (Input.GetKeyDown(KeyCode.L)) //pour les autres cases remplacer le Input par la condition que vous voulez
            {
               Debug.Log("lol");
               revokePotionEarly = true;
            }
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

   void RevokePotion12(PotionObject glou)
   {
      UiManager.instance.panelPotion.SetActive(false);
      UiManager.instance.spritePotion.GetComponent<RawImage>().texture = null;
      
      if (glou.type == PotionObject.PotionType.StatBasicPotion 
          || glou.type == PotionObject.PotionType.StatSpecificPotion)
      {
         for (int i = 0; i < 3; i++)
         {
            AnubisCurrentStats.instance.comboDamage[i] -= glou.damage;
         }
         AnubisCurrentStats.instance.thrustDamage -= glou.damage;
         //DamageManager.instance.Heal(glouglou.heal*-1);
         AnubisCurrentStats.instance.damageReduction -= glou.armor;
         if (glou.wArmor != 0)
         {
            AnubisCurrentStats.instance.damageReduction *= glou.wArmor;
         }
         if (glou.speed != 0)
         {
            AnubisCurrentStats.instance.speedX /= glou.speed;
            AnubisCurrentStats.instance.speedY /= glou.speed;
         }
         //Fonction avec la MagicForce

      }
   }

   private IEnumerator CoroutinePotion(PotionObject glouglou)
   {
      float compteurDuration = 0f;
      while (compteurDuration < glouglou.effectDuration)
      {
         if (revokePotionEarly == true)
         {
            RevokePotion12(glouglou);
            revokePotionEarly = false;
            yield break;
         }
         else
         {
            yield return new WaitForSecondsRealtime(0.1f);
            compteurDuration += 0.1f;
            //Debug.Log("compteur de duration potion est à " + compteurDuration);
         }
      }
      RevokePotion12(glouglou);
   }
   
}
