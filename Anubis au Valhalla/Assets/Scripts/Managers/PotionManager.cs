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
   public bool addStartingPotion;
   [Expandable] public PotionObject startingPotion;
   
   [Header("DEBUG")]
   [NaughtyAttributes.ReadOnly] public bool revokePotionEarly = false;
   public bool showBools;
   [ShowIf("showBools")] public float baseDamageBeforePotion;
   [ShowIf("showBools")] public float baseDamageForSoulBeforePotion;
   [ShowIf("showBools")] public int armorBeforePotion;
   [ShowIf("showBools")] public float speedXBeforePotion;
   [ShowIf("showBools")] public float speedYBeforePotion;
   [ShowIf("showBools")] public float magicForceBeforePotion;
   
   [ShowIf("showBools")] public float hpBeforePotion;
   [ShowIf("showBools")] public bool tookDamage;
   
   [Header("SHOP")]
   [Expandable] public List<PotionObject> potionsForShop = new();
   
   [Header("Audio")]
   public AudioSource audioSource;
   public AudioClip[] audioClipArray;

   [Header("Visuel")] 
   public GameObject defenceVFX;
   public GameObject attaqueVFX;
   private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
      }

      if (addStartingPotion)
      {
         currentPotion = startingPotion;
         isPotionSlotFill = true;
         UiManager.instance.CollectPotionDebug();
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
      //Debug.Log("tookDamage is = " + tookDamage);
   }

   void DrinkPotion(PotionObject glou)
   {
      audioSource.pitch = 1;
      audioSource.PlayOneShot(audioClipArray[0]);
      Debug.Log("Drink " + glou.nom);
      SaveStatBeforePotion();
      
      UiManager.instance.spritePotion.GetComponent<Image>().color = new Color(255,255,255,0);
      UiManager.instance.boxPotionImage.GetComponent<Image>().color = new Color(255,255,255,0);
      //UiManager.instance.globalBoxPotionImage.GetComponent<Image>().color = new Color(255,255,255,0);
      //UiManager.instance.FillDescriptionPotion();
      //UiManager.instance.panelPotion.SetActive(true); //à enlever quand on aura les vfx
      
      if (glou.type == PotionObject.PotionType.StatBasicPotion 
          || glou.type == PotionObject.PotionType.StatSpecificPotion)
      {
         if (glou.damage > 0)
         {
            GameObject atkVFX = Instantiate(attaqueVFX, DamageManager.instance.transform.position - new Vector3(0,1,0), Quaternion.identity);
            atkVFX.transform.parent = DamageManager.instance.transform;
            AnubisCurrentStats.instance.totalBaseBonusDamage += AnubisCurrentStats.instance.totalBaseDamage * glou.damage/100;
         }
         

         DamageManager.instance.Heal(glou.heal);

         if (glou.armor > 0)
         {
            GameObject defVFX = Instantiate(defenceVFX, DamageManager.instance.transform.position - new Vector3(0,1,0), Quaternion.identity);
            defVFX.transform.parent = DamageManager.instance.transform;
            AnubisCurrentStats.instance.damageReduction *= glou.armor;
         }
         
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
         StartCoroutine(CoroutinePotion(glou));

      }
      else if (glou.type == PotionObject.PotionType.SpecialPotion)
      {
         UseSpecialPotion(glou.index);
      }

      if (glou.type == PotionObject.PotionType.StatSpecificPotion)
      {
         switch (glou.index)
         {
            case 6: //Potion fragile de force
               //hpBeforePotion = Mathf.RoundToInt(AnubisCurrentStats.instance.vieActuelle);
               tookDamage = false;
               break;
         }
      }
      currentPotion = null;
   }

   
   void VerifyForSpecificPotion(int num)
   {
      switch (num)
      {
         case 1:
            if (Input.GetKeyDown(KeyCode.L)) //pour les autres cases remplacer le Input par la condition que vous voulez
            {
               Debug.Log("lol");
               revokePotionEarly = true;
            }
            break;
         
         case 6: //Potion fragile de force
            if (tookDamage)  //Anubis prend des dégâts
            {
               Debug.Log("perte pot fragile");
               revokePotionEarly = true;
               tookDamage = false;
            }
            break;
         
         
      }
   }
   
   void UseSpecialPotion(int num)
   {
      SaveStatBeforePotion();
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

      if (glou.type == PotionObject.PotionType.StatBasicPotion 
          || glou.type == PotionObject.PotionType.StatSpecificPotion)
      {
         AnubisCurrentStats.instance.totalBaseBonusDamage = baseDamageBeforePotion;
         AnubisCurrentStats.instance.baseDamageForSoul = baseDamageForSoulBeforePotion;
         AnubisCurrentStats.instance.damageReduction = armorBeforePotion;
         AnubisCurrentStats.instance.speedX = speedXBeforePotion;
         AnubisCurrentStats.instance.speedY = speedYBeforePotion;
         AnubisCurrentStats.instance.magicForce = magicForceBeforePotion;
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


   void SaveStatBeforePotion()
   {
      baseDamageBeforePotion = AnubisCurrentStats.instance.totalBaseBonusDamage;
      baseDamageForSoulBeforePotion = AnubisCurrentStats.instance.baseDamageForSoul;
      armorBeforePotion = AnubisCurrentStats.instance.damageReduction;
      speedXBeforePotion = AnubisCurrentStats.instance.speedX;
      speedYBeforePotion = AnubisCurrentStats.instance.speedY;
   }
   
   
}
