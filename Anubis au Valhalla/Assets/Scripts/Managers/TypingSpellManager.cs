using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class TypingSpellManager : MonoBehaviour
{
    public KeyCode spell1;
    //public KeyCode spell2;
    
    public int spellSelection = 0;

    public KeyCode select;

    [Header("Text Input")]
    public bool interactable = false;
    public InputField iField;
    public TextMeshPro numberSpellOutput;
    public int characterLimit = 3;
    public string myText;

    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(select))
        {
            Debug.Log(iField.text);
            interactable = true;
            Debug.Log("Enter pressé");
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
