using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnneauxCol : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            DamageManager.instance.TakeDamage(IA_Valkyrie.instance.ringDmg, gameObject);
            //collider2D.enabled = false;
        }
    }
}
