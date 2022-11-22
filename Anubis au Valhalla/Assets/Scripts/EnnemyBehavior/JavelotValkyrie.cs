using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JavelotValkyrie : MonoBehaviour
{
    public IA_Valkyrie ia;
    public Vector2 dir;
    public float angle;
    

    private void Start()
    {
        dir = ia.dir;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        dir.Normalize();
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x + dir.x * ia.javelotSpeed, transform.position.y + dir.y * ia.javelotSpeed, 0);
        Destroy(gameObject,5f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            DamageManager.instance.TakeDamage(ia.puissanceAttaqueJavelot, gameObject);
            Destroy(gameObject);
        }
    }
}
