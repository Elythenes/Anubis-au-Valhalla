using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireCircle : MonoBehaviour
{
  public NewPowerManager manager;
  public GameObject sableExplosion;
  public GameObject vFXChild;
  

    private void Start()
    {
      manager = GameObject.Find("NewPowerManager").GetComponent<NewPowerManager>();
      Destroy(gameObject,manager.p2ComboWaveDuration);
      transform.DOScale(transform.localScale * manager.p2ComboWaveRadiuses[manager.currentLevelPower2 - 1], 0.5f);
      vFXChild.transform.DOScale(vFXChild.transform.localScale * manager.p2ComboWaveRadiuses[manager.currentLevelPower2 - 1], 0.5f);
    }

    private void Update()
  {
    /*if (transform.localScale.x < manager.p2ComboWaveRadiuses[manager.currentLevelPower2 - 1])
    {
      transform.localScale += new Vector3(0.008f,0.008f,0);
    }*/
  }


  private void OnTriggerEnter2D(Collider2D col)
  {
    if (col.gameObject.tag == "Monstre")
    {
      if (manager.p2ComboWaveSoul)
      {
        Souls.instance.CreateSouls(col.transform.position,Random.Range(2,5));
      }
      col.GetComponentInParent<MonsterLifeManager>().TakeDamage(manager.p2ComboWaveDamages[manager.currentLevelPower2 -1] + (int)AnubisCurrentStats.instance.magicForce, 0.5f);
    }
  }
}
