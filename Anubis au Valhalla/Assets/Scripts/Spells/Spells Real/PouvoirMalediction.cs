using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouvoirMalediction : MonoBehaviour
{
     public PouvoirMaledictionObject soPouvoirMalediction;
      public float secondesRestantes;
      public bool isActive;
      private CharacterController anubis;
      private AttaquesNormales anubisAtk;
      public bool lockCast;
   
      private void Start()
      {
         anubis = CharacterController.instance;
         anubisAtk = AttaquesNormales.instance;
         secondesRestantes = soPouvoirMalediction.duration -0.4f;
      }
   
      public void Update()
      {
         if (secondesRestantes > soPouvoirMalediction.duration)
         {
            secondesRestantes = soPouvoirMalediction.duration;
         }
         
         if (isActive && !lockCast)
         {
            secondesRestantes -= Time.deltaTime;
            
            if (anubis.debutDash)
            {
               GameObject hitboxDash = Instantiate(soPouvoirMalediction.hitboxDash, anubis.transform.position, Quaternion.identity);
               hitboxDash.transform.parent = anubis.transform;
            }
            
            if (anubisAtk.attaque3)
            {
               Vector2 mousePos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
               Vector2 charaPos = CharacterController.instance.transform.position;
               float angle = Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
               Instantiate(soPouvoirMalediction.hitboxAttaqueNormale, anubis.transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
            }

            if (anubisAtk.attaqueSpeSpell)
            {
               GameObject plumeMaat = Instantiate(soPouvoirMalediction.hitboxThrust, anubis.transform.position, Quaternion.identity);
            }
         }
         
         if (!isActive)
         {
            if (secondesRestantes <= soPouvoirMalediction.duration)
            {
               secondesRestantes += Time.deltaTime;
            }
         }
         
         if (secondesRestantes >= soPouvoirMalediction.duration)
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
