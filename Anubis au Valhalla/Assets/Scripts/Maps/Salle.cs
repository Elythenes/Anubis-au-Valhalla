using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salle : MonoBehaviour
{
    public bool roomDone = false;

    // Start is called before the first frame update
    void Awake()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ExampleRoomCleared()
    {
        yield return new WaitForSeconds(1);
        roomDone = true;
    }
}
