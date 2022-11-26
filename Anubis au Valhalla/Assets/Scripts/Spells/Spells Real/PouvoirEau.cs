using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouvoirEau : MonoBehaviour
{
     public PouvoirEauObject soPouvoirEau;
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
         secondesRestantes = soPouvoirEau.duration -0.4f;
      }
   
      public void Update()
      {
         if (isActive && !lockCast)
         {
            secondesRestantes -= Time.deltaTime;
            if (anubis.isDashing)
            {
               timerSpawn += Time.deltaTime;
               if (timerSpawn >= soPouvoirEau.dashSpawnRate)
               {
                  GameObject fireZone = Instantiate(soPouvoirEau.hitboxDash, anubis.transform.position, Quaternion.identity);
                  timerSpawn = 0;
               }
            }
   
            if (anubisAtk.attaque3)
            {
               Vector2 mousePos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
               Vector2 charaPos = CharacterController.instance.transform.position;
               float angle = Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
               Instantiate(soPouvoirEau.hitboxAttaqueNormale, anubis.transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
            }
         }
         
         if (!isActive)
         {
            if (secondesRestantes <= soPouvoirEau.duration)
            {
               secondesRestantes += Time.deltaTime;
            }
         }
         
         if (secondesRestantes >= soPouvoirEau.duration)
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
