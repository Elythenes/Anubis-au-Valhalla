using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouvoirFeu : MonoBehaviour
{
   public PouvoirFeuObject soPouvoirFeu;
   public float secondesRestantes;
   public bool isActive;
   private CharacterController anubis;
   private AttaquesNormales anubisAtk;
   private float timerSpawn;
   public bool lockCast;

   private void Start()
   {
      anubis = CharacterController.instance;
      anubisAtk = AttaquesNormales.instance;
      //DamageManager.instance.isAnkh = true;
      secondesRestantes = soPouvoirFeu.duration -0.5f;
   }

   public void Update()
   {
      if (isActive && !lockCast)
      {
         secondesRestantes -= Time.deltaTime;
         if (anubis.isDashing)
         {
            timerSpawn += Time.deltaTime;
            if (timerSpawn >= soPouvoirFeu.dashSpawnRate)
            {
               GameObject fireZone = Instantiate(soPouvoirFeu.hitboxDash, anubis.transform.position, Quaternion.identity);
               fireZone.GetComponent<FlameArea>().sOPouvoirFeu = soPouvoirFeu;
               timerSpawn = 0;
            }
         }

         if (anubisAtk.attaque3)
         {
            Vector2 mousePos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 charaPos = CharacterController.instance.transform.position;
            float angle = Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
            Instantiate(soPouvoirFeu.hitboxThrust, anubis.transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
         }
      }
      
      if (!isActive)
      {
         if (secondesRestantes <= soPouvoirFeu.duration)
         {
            secondesRestantes += Time.deltaTime;
         }
      }
      
      if (secondesRestantes >= soPouvoirFeu.duration)
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