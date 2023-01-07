using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireCircle : MonoBehaviour
{
  public NewPowerManager manager;
  public GameObject sableExplosion;
  

    private void Start()
    {
      manager = GameObject.Find("NewPowerManager").GetComponent<NewPowerManager>();
      Destroy(gameObject,manager.p2ComboWaveDuration);
    }

    private void Update()
  {
    if (transform.localScale.x < manager.p2ComboWaveRadiuses[manager.currentLevelPower2])
    {
      transform.localScale += new Vector3(0.008f,0.008f,0);
    }
  }


  private void OnTriggerEnter2D(Collider2D col)
  {
    if (col.gameObject.tag == "Monstre")
    {
      if (manager.p2ComboWaveSoul)
      {
        Souls.instance.CreateSouls(col.transform.position,Random.Range(2,5));
      }
      col.GetComponentInParent<MonsterLifeManager>().TakeDamage(manager.p2ComboWaveDamages[manager.currentLevelPower2], 0.5f);
      if (col.GetComponentInParent<MonsterLifeManager>().vieActuelle <= 0)
      {
        if (manager.p2ComboWaveDeathExplosion)
        {
          Instantiate(sableExplosion, col.transform.position, Quaternion.identity);
        }
      }
     
    }
  }
}
