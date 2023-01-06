using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class FireCircle : MonoBehaviour
{
  public NewPowerManager manager;
  public GameObject sableExplosion;
  public SpriteRenderer exploSR;
  public CircleCollider2D exploCollider;
  public bool isExplosion;
  public bool DoOnce;
  

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

    if (isExplosion)
    {
     
    }
  }


  private void OnTriggerEnter2D(Collider2D col)
  {
    if (col.gameObject.tag == "Monstre")
    {
      col.GetComponentInParent<MonsterLifeManager>().TakeDamage(manager.p2ComboWaveDamages[manager.currentLevelPower2], 0.5f);
      if (col.GetComponentInParent<MonsterLifeManager>().vieActuelle <= 0)
      {
        if (manager.p2ComboWaveDeathExplosion)
        {
          isExplosion = true;
          GameObject hitboxExplosion = Instantiate(sableExplosion, col.transform.position, Quaternion.identity);
     
          if (hitboxExplosion.transform.localScale.x < 5)
          {
            hitboxExplosion.transform.localScale += new Vector3(0.05f, 0.05f, 0);
            Vector2 S = exploSR.sprite.bounds.size;
            exploCollider.radius = exploSR.transform.localScale.y / 2;
          }
          else
          {
            Destroy(gameObject);
          }
        }
      }
     
    }
  }
}
