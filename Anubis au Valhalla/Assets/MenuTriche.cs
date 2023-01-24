using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTriche : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
            UiManager.instance.ActivatePause();
            UiManager.instance.isSousMenu = false;
            UiManager.instance.PlayButtonSound();
        }
    }

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
        for (int i = 0; i < 20; i++)
        {
            if (NewPowerManager.Instance.currentLevelPower1 < 10)
            {
                NewPowerManager.Instance.currentLevelPower1++;
                NewPowerManager.Instance.PowerLevelUnlock();
            }
            else if (NewPowerManager.Instance.currentLevelPower2 < 10)
            {
                NewPowerManager.Instance.currentLevelPower2++;
                NewPowerManager.Instance.PowerLevelUnlock();
            }
            else
            {
                return;
            }
        }
    }
}
