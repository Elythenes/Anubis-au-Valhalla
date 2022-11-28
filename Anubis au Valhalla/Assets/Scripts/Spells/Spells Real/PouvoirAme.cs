using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouvoirAme : MonoBehaviour
{
    public PouvoirAmeObject soPouvoirAme;
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
      secondesRestantes = soPouvoirAme.duration -0.5f;
   }
   public void Update()
      {
         if (isActive && !lockCast)
         {
            secondesRestantes -= Time.deltaTime;
            if (anubis.isDashing)  // ATTAQUE DASH
            {
               timerSpawn += Time.deltaTime;
               if (timerSpawn >= soPouvoirAme.dashSpawnRate)
               {
                  Vector2 mousePos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
                  Vector2 charaPos = CharacterController.instance.transform.position;
                  float angle = Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
                  GameObject locustre = Instantiate(soPouvoirAme.hitboxDash, anubis.transform.position + new Vector3(Random.Range(-3,3),Random.Range(-3,3)), Quaternion.AngleAxis(angle,Vector3.forward));
                  timerSpawn = 0;
               }
            }
   
            if (anubisAtk.attaque3)
            {
               Instantiate(soPouvoirAme.hitboxAttaqueNormale, anubis.transform.position, Quaternion.identity);
            }
            
            if (anubisAtk.attaque1) //PLACEHOLDER - REMPLACER PAR LE THRUST
            {
               
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
