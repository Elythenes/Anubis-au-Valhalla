using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpellCollectManager : MonoBehaviour
{
    public List<ScriptableObject> spellSlot1;
    public List<ScriptableObject> spellSlot2;
    
    void Update()
    {
        SpellReplacement(spellSlot1);
        SpellReplacement(spellSlot2);
    }

    void SpellReplacement(List<ScriptableObject> list)
    {
        if (list.Count >= 2)
        {
            list.RemoveRange(0,1);
        }
    }
}
