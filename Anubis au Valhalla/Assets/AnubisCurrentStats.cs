using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class AnubisCurrentStats : MonoBehaviour
{
   public static AnubisCurrentStats instance;
   
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
      if (instance == null)
      {
         instance = this;
      }
   }
   
}
