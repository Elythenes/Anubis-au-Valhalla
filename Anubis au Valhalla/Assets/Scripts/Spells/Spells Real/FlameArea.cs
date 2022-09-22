using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FlameArea : MonoBehaviour
{
    [Header("FlameArea")] 
    public SpellStaticAreaType sOFlameArea;
    public float tempsReloadHitFlameAreaTimer;
    public bool stopAttack;

    private void OnTriggerStay2D(Collider2D col)
    {
        for (int i = 0; i < sOFlameArea.nombreOfDot; i++)
        {
            if (tempsReloadHitFlameAreaTimer <= sOFlameArea.espacementDoT && stopAttack == false)
            {
                tempsReloadHitFlameAreaTimer += Time.deltaTime;
            }

            if (tempsReloadHitFlameAreaTimer > sOFlameArea.espacementDoT && col.gameObject.tag == "Monstre")
            {
                Debug.Log("touché");
                col.GetComponent<IA_Monstre1>().TakeDamage(sOFlameArea.puissanceAttaque);
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


