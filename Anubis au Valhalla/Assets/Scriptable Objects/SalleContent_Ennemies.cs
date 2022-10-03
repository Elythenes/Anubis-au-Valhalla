using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Room Content/Enemies")]
public class SalleContent_Ennemies : ScriptableObject
{
    public int spawnAmount;
    public GameObject[] enemiesToSpawn;
    public int[] spawnWeight;
    [HideInInspector] public int totalWeight = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
