using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEau : MonoBehaviour
{
    public NewPowerManager manager;
    public Sprite spriteHalfCircle;

    private void Start()
    {
        manager = GameObject.Find("NewPowerManager").GetComponent<NewPowerManager>();
        Destroy(gameObject,manager.p1ComboConeDurations[manager.currentLevelPower1]);
        transform.localScale = Vector3.zero;
        if (manager.p1ComboConeHalfSphere)
        {
            GetComponent<SpriteRenderer>().sprite = spriteHalfCircle;
            Destroy(GetComponent<PolygonCollider2D>());
            PolygonCollider2D p = gameObject.AddComponent<PolygonCollider2D>();
            p.isTrigger = true;
        }
    }

    private void Update()
    {
        if (transform.localScale.x < manager.p1ComboConeReaches[manager.currentLevelPower1])
        {
            transform.localScale += new Vector3(0.01f, 0.01f, 0);
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("touché");
        if(col.gameObject.tag == "Monstre")
        {
            if (!manager.p1ComboConeStagger)
            {
                col.GetComponentInParent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(manager.p1ComboConeDamages[manager.currentLevelPower1] + AnubisCurrentStats.instance.magicForce),0.5f);
            }
            else
            {
                col.GetComponentInParent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(manager.p1ComboConeDamages[manager.currentLevelPower1]+ AnubisCurrentStats.instance.magicForce),9f);
            }
          
        }
    }
}
