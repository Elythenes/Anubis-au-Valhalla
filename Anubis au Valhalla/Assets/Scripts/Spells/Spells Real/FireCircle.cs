using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class FireCircle : MonoBehaviour
{
  public NewPowerManager manager;

    private void Start()
    {
      manager = GameObject.Find("NewPowerManager").GetComponent<NewPowerManager>();
      Destroy(gameObject,manager.p2ComboWaveDuration);
    }

    private void Update()
  {
    if (transform.localScale.x < manager.p2ComboWaveRadiuses[manager.currentLevelPower2])
    {
      transform.localScale += new Vector3(0.5f,0.5f,0);
    }
  }


  private void OnTriggerEnter2D(Collider2D col)
  {
    if (col.gameObject.tag == "Monstre")
    {
      col.GetComponentInParent<MonsterLifeManager>().TakeDamage(manager.p2ComboWaveDamages[manager.currentLevelPower2], 0.5f);
    }
  }
}
