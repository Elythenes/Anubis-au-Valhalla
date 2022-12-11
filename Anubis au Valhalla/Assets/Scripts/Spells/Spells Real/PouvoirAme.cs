using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PouvoirAme : MonoBehaviour
{
   public static PouvoirAme instance;
    public PouvoirAmeObject soPouvoirAme;
   public float secondesRestantes;
   public bool isActive;
   public bool lockCast;
   [HideInInspector] public bool spawnHitboxDash;
   private CharacterController anubis;
   private AttaquesNormales anubisAtk;

   private void Awake()
   {
      if (instance == null)
      {
         instance = this;
      }
   }

   private void Start()
   {
      anubis = CharacterController.instance;
      anubisAtk = AttaquesNormales.instance;
      secondesRestantes = soPouvoirAme.duration -0.5f;
   }
   public void Update()
      {
         if (isActive && !lockCast)
         {
            secondesRestantes -= Time.deltaTime;
            
            if (anubis.isDashing)  // ATTAQUE DASH
            {
               DamageManager.instance.isAmePowered = true;
            }
            else
            {
               DamageManager.instance.isAmePowered = false;
            }

            if (spawnHitboxDash)
            {
               Instantiate(soPouvoirAme.hitboxDash, anubis.transform.position, Quaternion.identity);
               spawnHitboxDash = false;
            }
   
            if (anubisAtk.attaque3)
            {
               Instantiate(soPouvoirAme.hitboxAttaqueNormale, anubis.transform.position, Quaternion.identity);
            }
            
            if (anubisAtk.attaqueSpeSpell) //PLACEHOLDER - REMPLACER PAR LE THRUST
            {
               Vector2 mousePos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
               Vector2 charaPos = CharacterController.instance.transform.position;
               float angle = Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
               for (int i = 0; i < soPouvoirAme.zoneAmount; i++)
               { 
                  Instantiate(soPouvoirAme.hitboxThrust, anubis.transform.position + new Vector3(Random.Range(-3,3),Random.Range(-3,3)), Quaternion.AngleAxis(angle,Vector3.forward));
               }
            }
         }
         
         if (!isActive)
         {
            if (secondesRestantes <= soPouvoirAme.duration)
            {
               secondesRestantes += Time.deltaTime;
            }
         }
         
         if (secondesRestantes >= soPouvoirAme.duration)
         {
            isActive = false;
            lockCast = false;
         }
   
         if (secondesRestantes <= 0)
         {
            lockCast = true;
            isActive = false;
         }
      }
}
