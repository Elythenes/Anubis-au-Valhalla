using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class BoxInventory : MonoBehaviour
{

    [Range(1,21)] public int inventoryPosition;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public int GetButtonPosition()
    {
        return inventoryPosition;
    }
}
