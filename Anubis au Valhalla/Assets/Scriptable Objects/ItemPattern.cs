using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/ItemPreset")]
public class ItemPatten : ScriptableObject
{
    public int soulAmount;
    public List<GameObject> chestContent;
}
