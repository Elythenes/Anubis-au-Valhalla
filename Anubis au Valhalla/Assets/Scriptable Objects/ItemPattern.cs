using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/ItemPattern")]
public class ItemPattern : ScriptableObject
{
    public int soulAmount;
    public List<GameObject> chestContent;
}
