using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceRange : MonoBehaviour
{
    public List<GameObject> monsterList;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Monstre")
        {
            monsterList.Add(col.gameObject);
        }
    }
}
