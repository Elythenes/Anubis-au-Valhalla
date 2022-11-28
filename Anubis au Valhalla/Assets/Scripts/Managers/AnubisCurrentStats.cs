using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
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
   public List<int> comboDamage = new List<int>();
   public int thrustDamage = 10;
   public int criticalRate = 5;
   public List<float> dureeHitbox = new List<float>();
   [NaughtyAttributes.ReadOnly] public List<float> stunDuration = new List<float>();
   public List<float> forceKnockback = new List<float>();
   public List<float> stunDurationMax = new List<float>();
   public List<float> dashImpulse = new List<float>();
   public List<float> timeForCanDash = new List<float>();
   public List<float> dashTimers = new List<float>();
   
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
      atk.hitBoxC = hitBoxC;
      atk.rangeAttaque = rangeAttaque;
      atk.isC = isC;
      atk.damage = comboDamage;
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
      life.vieMax += 1;
   }

   private void Update()
   {
      // pour les attaques
      atk.hitBoxC = hitBoxC;
      atk.rangeAttaque = rangeAttaque;
      atk.isC = isC;
      atk.damage = comboDamage;
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
}
