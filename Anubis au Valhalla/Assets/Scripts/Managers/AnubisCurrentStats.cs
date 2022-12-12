using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnubisCurrentStats : MonoBehaviour
{
   public static AnubisCurrentStats instance;
   public AttaquesNormales atk;
   public DamageManager life;
   public CharacterController move;
   
   [Header("ATTAQUE")] 
   public List<GameObject> hitBoxC = new List<GameObject>();
   public List<Vector2> rangeAttaque = new List<Vector2>();
   public List<bool> isC = new List<bool>();

   //BaseDamage = dégât de base du personnage à la run t=0sec
   public float totalBaseDamage = 20;
   public List<int> comboBaseDamage = new List<int>();
   public int thrustBaseDamage;
   
   //BonusDamage = valeur qu'on ajoute au BaseDamage et qu'on modifie en cas de bonus (via glyphs)
   public float totalBaseBonusDamage;
   public List<int> comboBaseBonusDamage = new List<int>();
   public int thrustBaseBonusDamage;

   //DamageForStat = somme de BaseDamage + BonusDamage, permet de donner les dégâts du personnage pendant toute la run
   [NaughtyAttributes.ReadOnly] public float totalBaseDamageForStat;
   [NaughtyAttributes.ReadOnly] public List<int> comboDamageForStat = new List<int>();
   [NaughtyAttributes.ReadOnly] public int thrustDamageForStat;
   [NaughtyAttributes.ReadOnly] public int soulBonusDamageForStat;

   public float baseDamageForSoul;
   public List<float> multiplicateurDamage = new(4){.5f,.75f,1,1.3f};
   
   public List<int> comboDamage = new List<int>();
   public int thrustDamage = 10;
   
   
   public int criticalRate = 5;
   public List<float> dureeHitbox = new List<float>();
   public List<float> stunDuration = new List<float>();
   public List<float> forceKnockback = new List<float>();
   public List<float> stunDurationMax = new List<float>();
   public List<float> dashImpulse = new List<float>();
   public List<float> timeForCanDash = new List<float>();
   public List<float> dashTimers = new List<float>();
   public float magicForce = 30;
   
   [Header("DEFENSE")] 
   public int vieActuelle;
   public int vieMax = 100;
   public int damageReduction;
   public float tempsInvinsbleAfterHit = 2f;
   public float stunAfterHit = 0.2f;

   [Header("DEPLACEMENT")]
   public float speedX = 37f;
   public float speedY = 28f;
   public float dashCooldown = 0.3f;

   private void Awake()
   {
      /*atk = AttaquesNormales.instance;
      move = CharacterController.instance;
      life = DamageManager.instance;*/
      
      if (instance == null)
      {
         instance = this;
      }
   }

   private void Start()
   {
      // pour les attaques
      StartDamageForStat();
      atk.hitBoxC = hitBoxC;
      atk.rangeAttaque = rangeAttaque;
      atk.isC = isC;
      atk.criticalRate = criticalRate;
      atk.dureeHitbox = dureeHitbox;
      atk.forceKnockback = forceKnockback;
      atk.stunDurationMax = stunDurationMax;
      atk.dashImpulse = dashImpulse;
      atk.timeForCanDash = timeForCanDash;
      atk.dashTimers = dashTimers;
      atk.specialDmg = thrustDamage;

      // pour la vie
      life.vieActuelle = vieActuelle;
      life.vieMax = vieMax;
      life.damageReduction = damageReduction;
      life.tempsInvinsibleAfterHit = tempsInvinsbleAfterHit;
      life.stunAfterHit = stunAfterHit;

      // pour les mouvements
      move.speedX = speedX;
      move.speedY = speedY;
      move.dashCooldown = dashCooldown;
   }

   private void Update()
   {
      // pour les attaques
      UpdateDamageForStat();
      atk.hitBoxC = hitBoxC;
      atk.rangeAttaque = rangeAttaque;
      atk.isC = isC;
      atk.criticalRate = criticalRate;
      atk.dureeHitbox = dureeHitbox;
      atk.forceKnockback = forceKnockback;
      atk.stunDurationMax = stunDurationMax;
      atk.dashImpulse = dashImpulse;
      atk.timeForCanDash = timeForCanDash;
      atk.dashTimers = dashTimers;

      // pour la vie
      life.vieActuelle = vieActuelle;
      life.vieMax = vieMax;
      life.damageReduction = damageReduction;
      life.tempsInvinsibleAfterHit = tempsInvinsbleAfterHit;
      life.stunAfterHit = stunAfterHit;

      // pour les mouvements
      move.speedX = speedX;
      move.speedY = speedY;
      move.dashCooldown = dashCooldown;
   }


   public void StartDamageForStat()
   {
      AddBonusDamage(0,0);
      for (int i = 0; i < 3; i++)
      {
         comboDamageForStat[i] = Mathf.RoundToInt(totalBaseDamage * multiplicateurDamage[i]);
      }
      thrustDamageForStat = Mathf.RoundToInt(totalBaseDamage * multiplicateurDamage[3]);
   }
   
   public void UpdateDamageForStat() //fonction souvent appelée pour faire le point au niveau des dégâts d'Anubis
   {
      totalBaseDamageForStat = totalBaseDamage + totalBaseBonusDamage;
      for (int i = 0; i < 3; i++)
      {
         comboBaseDamage[i] = Mathf.RoundToInt(totalBaseDamageForStat * multiplicateurDamage[i]);
      }
      thrustBaseDamage = Mathf.RoundToInt(totalBaseDamageForStat * multiplicateurDamage[3]);
      
      for (int i = 0; i < 3; i++)
      {
         comboDamageForStat[i] = comboBaseDamage[i] + comboBaseBonusDamage[i] + soulBonusDamageForStat;
      }
      thrustDamageForStat = thrustBaseDamage + thrustBaseBonusDamage + soulBonusDamageForStat;
   }


   public void AddBonusDamage(int tague, float value)
   {
      switch (tague)
      {
         case 0: //reset toutes les bonus Damage pour le Start, donc il faut que value = 0
            totalBaseBonusDamage = value;
            for (int i = 0; i < 3; i++)
            {
               comboBaseBonusDamage[i] = Mathf.RoundToInt(value);
            }
            thrustBaseBonusDamage = Mathf.RoundToInt(value);
            break;
         
         case 1: //Total Base Damage
            totalBaseBonusDamage += value;
            break;
         
         case 2: //All Combo Base Damage
            for (int i = 0; i < 3; i++)
            {
               comboBaseBonusDamage[i] += Mathf.RoundToInt(value);
            }
            thrustBaseBonusDamage += Mathf.RoundToInt(value);
            break;
         
         case 3: //Swing 1 Base Damage
            comboBaseBonusDamage[0] += Mathf.RoundToInt(value);
            break;

         case 4: //Swing 2 Base Damage
            comboBaseBonusDamage[1] += Mathf.RoundToInt(value);
            break;
         
         case 5: //Smash Base Damage
            comboBaseBonusDamage[2] += Mathf.RoundToInt(value);
            break;
         
         case 6: //Thrust Base Damage
            thrustBaseBonusDamage += Mathf.RoundToInt(value);
            break;
      }
   }

}
