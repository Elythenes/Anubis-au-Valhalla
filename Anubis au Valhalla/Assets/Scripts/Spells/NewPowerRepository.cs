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

public enum NewPowerType
{
    Power1,
    Power2,
    None,
}

