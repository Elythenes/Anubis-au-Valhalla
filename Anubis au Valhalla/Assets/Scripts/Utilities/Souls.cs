using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Souls : MonoBehaviour
{

    public GameObject soul;

    public static Souls instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CreateSouls(Vector3 ennemyPos, int soulAmount)
    {
        for (int i = 0; i <= soulAmount; i++)
        {
            Instantiate(soul, ennemyPos, Quaternion.Euler(0,0,0));
        }
    }
}
