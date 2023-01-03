using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NewPowerRepository : MonoBehaviour
{
    public NewPowerType newPowerType = NewPowerType.None;
    public Texture sprite;
    

    private void Awake()
    {
        if (NewPowerManager.Instance.currentLevelPower1 == 10)
        {
            newPowerType = NewPowerType.Power2;
        }
        else if (NewPowerManager.Instance.currentLevelPower2 == 10)
        {
            newPowerType = NewPowerType.Power1;
        }
        else
        {
            int range = Random.Range(0, 2);
            if (range == 0)
            {
                newPowerType = NewPowerType.Power1;
            }
            else
            {
                newPowerType = NewPowerType.Power2;
            }
        }
    }

    private void Update()
    {
        if (NewPowerManager.Instance.currentLevelPower1 == 10)
        {
            newPowerType = NewPowerType.Power2;
        }
        else if (NewPowerManager.Instance.currentLevelPower2 == 10)
        {
            newPowerType = NewPowerType.Power1;
        }
    }
}

public enum NewPowerType
{
    Power1,
    Power2,
    None,
}

