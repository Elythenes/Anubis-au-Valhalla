using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WaveEau : MonoBehaviour
{
    public NewPowerManager manager;
    public Sprite spriteHalfCircle;
    public GameObject VFXCone;
    public GameObject VFXDemiCercle;

    private void Start()
    {
        VFXCone.GetComponent<VisualEffect>().Play();
        manager = GameObject.Find("NewPowerManager").GetComponent<NewPowerManager>();
        Destroy(gameObject,manager.p1ComboConeDurations[manager.currentLevelPower1 - 1]);
        transform.localScale = Vector3.zero;
        if (manager.p1ComboConeHalfSphere)
        {
            VFXCone.SetActive(false);
            VFXDemiCercle.SetActive(true);
            GetComponent<SpriteRenderer>().sprite = spriteHalfCircle;
            Destroy(GetComponent<PolygonCollider2D>());
            PolygonCollider2D p = gameObject.AddComponent<PolygonCollider2D>();
            p.isTrigger = true;
        }
    }

    private void Update()
    {
        if (transform.localScale.x < manager.p1ComboConeReaches[manager.currentLevelPower1 - 1])
        {
            transform.localScale += new Vector3(0.01f, 0.015f, 0);
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("touché");
        if(col.gameObject.tag == "Monstre")
        {
            if (!manager.p1ComboConeStagger)
            {
                col.GetComponentInParent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(manager.p1ComboConeDamages[manager.currentLevelPower1 - 1] + AnubisCurrentStats.instance.magicForce),0.5f);
            }
            else
            {
                col.GetComponentInParent<MonsterLifeManager>().TakeDamage(Mathf.RoundToInt(manager.p1ComboConeDamages[manager.currentLevelPower1 - 1]+ AnubisCurrentStats.instance.magicForce),9f);
            }
          
        }
    }
}
