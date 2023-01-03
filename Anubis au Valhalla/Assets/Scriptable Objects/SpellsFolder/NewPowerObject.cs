using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPowerObject : ScriptableObject
{
    public int upgradeValue = 1;
    public NewPowerType newPowerType = NewPowerType.None;

}

public enum NewPowerType
{
    Power1,
    Power2,
    None,
}
