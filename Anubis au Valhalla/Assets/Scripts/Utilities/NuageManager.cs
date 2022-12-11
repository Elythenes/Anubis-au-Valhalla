using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NuageManager : MonoBehaviour
{
    public List<Transform> nuageList = new List<Transform>();
    public float nuageSpeed;
    public float startPos;
    public float endPos;
    
    void Start()
    {
        foreach (Transform child in transform)
        {
            nuageList.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform nuage in nuageList)
        {
            nuage.transform.position += new Vector3(nuageSpeed, 0,0);
            
            if (nuage.transform.position.x < endPos)
            {
                nuage.transform.position = new Vector3(startPos,nuage.transform.position.y,nuage.transform.position.z);
            }
        }
        
        
    }
}
