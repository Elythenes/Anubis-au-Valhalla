using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouvoirFoudre : MonoBehaviour
{
     public PouvoirFoudreObject soPouvoirFoudre;
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
         secondesRestantes = soPouvoirFoudre.duration -0.4f;
      }
   
      public void Update()
      {
         if (isActive && !lockCast)
         {
            secondesRestantes -= Time.deltaTime;
            if (anubis.isDashing)
            {
               timerSpawn += Time.deltaTime;
               if (timerSpawn >= soPouvoirFoudre.dashSpawnRate)
               {
                  GameObject paralysieHitbox = Instantiate(soPouvoirFoudre.hitboxDash, anubis.transform.position, Quaternion.identity);
                  paralysieHitbox.transform.parent = anubis.transform;
                  Destroy(paralysieHitbox,anubis.dashDuration-anubis.timerDash);
               }
            }
            if (anubisAtk.attaque3)
            {
               Instantiate(soPouvoirFoudre.hitboxAttaqueNormale, anubis.transform.position, Quaternion.identity);
            }

            if (anubisAtk.attaqueSpe) // Attaque Puissante
            {
               Vector2 mousePos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
               Vector2 charaPos = CharacterController.instance.transform.position;
               float angle = Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
               Instantiate(soPouvoirFoudre.hitboxThrust, anubis.transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
            }
         }
         
         if (!isActive)
         {
            if (secondesRestantes <= soPouvoirFoudre.duration)
            {
               secondesRestantes += Time.deltaTime;
            }
         }
         
         if (secondesRestantes >= soPouvoirFoudre.duration)
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
