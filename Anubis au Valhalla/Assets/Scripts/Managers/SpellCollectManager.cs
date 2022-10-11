using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCollectManager : MonoBehaviour
{
    public List<SpellObject> containerSlot1 = new List<SpellObject>(2);
    public List<SpellObject> containerSlot2 = new List<SpellObject>(2);

    void Update()
    {
        SpellReplacement(containerSlot1);
        SpellReplacement(containerSlot2);
    }

    void SpellReplacement(List<SpellObject> list)
    {
        if (list.Count >= 2)
        {
            list.RemoveRange(0,1);
        }
    }
    
    /*public List<ScriptableObject> spellSlot1;
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
    }*/
}
