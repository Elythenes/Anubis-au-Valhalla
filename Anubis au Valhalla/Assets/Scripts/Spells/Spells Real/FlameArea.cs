using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FlameArea : MonoBehaviour
{
  
    [Header("FlameArea")]
    public int puissanceAttaqueFlameArea; 
    public float tempsReloadHitFlameAreaMax;
    public float tempsReloadHitFlameAreaTimer;
    public bool stopAttack;

    private void Start()
    {
        puissanceAttaqueFlameArea = SkillManager.instance.puissanceAttaqueFlameArea;
        tempsReloadHitFlameAreaMax = SkillManager.instance.espacementDoTFlameArea;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        for (int i = 0; i < 4; i++)
        {
            if (tempsReloadHitFlameAreaTimer <= tempsReloadHitFlameAreaMax && stopAttack == false)
            {
                tempsReloadHitFlameAreaTimer += Time.deltaTime;
            }

            if (tempsReloadHitFlameAreaTimer > tempsReloadHitFlameAreaMax && col.gameObject.tag == "Monstre")
            {
                Debug.Log("touché");
                col.GetComponent<IA_Monstre1>().TakeDamage(puissanceAttaqueFlameArea);
                col.GetComponent<IA_Monstre1>().DamageText(puissanceAttaqueFlameArea.ToString());
                //yield return new WaitForSeconds(tempsReloadHitSandstormMax);
                tempsReloadHitFlameAreaTimer = 0;
            }
        } 
    }
   
    private void OnTriggerExit2D(Collider2D col)
    {
        stopAttack = true;
        tempsReloadHitFlameAreaTimer = 0;
    }
}


