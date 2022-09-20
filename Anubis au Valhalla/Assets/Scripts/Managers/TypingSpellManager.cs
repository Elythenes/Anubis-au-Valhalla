using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypingSpellManager : MonoBehaviour
{
    public int spellSelection = 0;

    public KeyCode select;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(select))
        {
            Debug.Log("Enter pressé");
            SpellSelector();
        }
    }

    void SpellSelector()
    {
        switch (spellSelection)
        {
            case 1:
                Debug.Log("Spell 1 choisi");
                break;
        }
    }
}
