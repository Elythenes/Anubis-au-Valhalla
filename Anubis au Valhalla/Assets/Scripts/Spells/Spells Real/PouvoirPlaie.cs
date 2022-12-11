using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouvoirPlaie : MonoBehaviour
{
   public PouvoirPlaieObject soPouvoirPlaie;
   public float secondesRestantes;
   public bool isActive;
   public bool lockCast;
   private CharacterController anubis;
   private AttaquesNormales anubisAtk;
   private float timerSpawn;

   private void Start()
   {
      anubis = CharacterController.instance;
      anubisAtk = AttaquesNormales.instance;
      secondesRestantes = soPouvoirPlaie.duration -0.5f;
   }
   public void Update()
      {
         if (isActive && !lockCast)
         {
            secondesRestantes -= Time.deltaTime;
            if (anubis.isDashing)  // ATTAQUE DASH
            {
               timerSpawn += Time.deltaTime;
               if (timerSpawn >= soPouvoirPlaie.dashSpawnRate)
               {
                  float angle = Mathf.Atan2(CharacterController.instance.movement.x,CharacterController.instance.movement.y) * Mathf.Rad2Deg;
                  GameObject murDeSable = Instantiate(soPouvoirPlaie.hitboxDash, anubis.transform.position, Quaternion.AngleAxis(angle,Vector3.forward));
                  timerSpawn = 0;
               }
            }
   
            if (anubisAtk.attaque3) // ATTAQUE NORMALE
            {
               Instantiate(soPouvoirPlaie.hitboxAttaqueNormale, anubis.transform.position, Quaternion.identity);
            }
            
            if (anubisAtk.attaqueSpe2) //PLACEHOLDER - REMPLACER PAR LE THRUST
            {
               for (int i = 0; i < soPouvoirPlaie.nuberOfBullets; i++)
               {
                  Vector2 mousePos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
                  Vector2 charaPos = CharacterController.instance.transform.position;
                  float angle = Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
                  if (i == 0)
                  {
                     angle -= 20;
                  }
                  else if(i == 2)
                  {
                     angle += 20;
                  }
                  Instantiate(soPouvoirPlaie.hitboxThrust,transform.position,Quaternion.AngleAxis(angle,Vector3.forward));
               }
            }
         }
         
         if (!isActive)
         {
            if (secondesRestantes <= soPouvoirPlaie.duration)
            {
               secondesRestantes += Time.deltaTime;
            }
         }
         
         if (secondesRestantes >= soPouvoirPlaie.duration)
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
