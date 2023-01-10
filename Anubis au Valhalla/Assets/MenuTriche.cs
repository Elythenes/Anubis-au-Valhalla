using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTriche : MonoBehaviour
{
    public void ResetInvinsible()
    {
        DamageManager.instance.invinsible = false;
    }
    
    public void Invinsible()
    {
        DamageManager.instance.invinsible = true;
    }
    
    public void Ames()
    {
        Souls.instance.soulBank += 100;
        Souls.instance.UpdateSoulsCounter();
    }

    public void Force()
    {
        AnubisCurrentStats.instance.totalBaseBonusDamage += 100;
    }

    public void PouvoirLvlMax()
    {
        StartCoroutine(PowerUpgrade());
    }


    IEnumerator PowerUpgrade()
    {
        for (int i = 0; i < 10; i++)
        {
            if (NewPowerManager.Instance.currentLevelPower1 < 10)
            {
                NewPowerManager.Instance.currentLevelPower1 += 1;
                yield return new WaitForSeconds(0.01f);
            }
            
            if (NewPowerManager.Instance.currentLevelPower2 < 10)
            {
                NewPowerManager.Instance.currentLevelPower2 += 1;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
