using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBehavior : MonoBehaviour
{
    public Vector3 playerPos;
    [SerializeField] private float force = 3f;
    [SerializeField] private float minForce = 1f;
    [SerializeField] private float timer = 1;
    [SerializeField] private float deceleration = 0.3f;
    [SerializeField] private float poofForce = 3f;
    private bool haspoofed = false;

    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
        Vector2 explode = new Vector2(Random.Range(-force, force), Random.Range(-force, force));
        rb.AddForce(explode, ForceMode2D.Impulse);
        rb.drag = deceleration;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = CharacterController.instance.transform.position;
        Vector3 dir = playerPos - transform.position;
        Vector3 dirNormalised = dir.normalized;

        if (timer >= 0)
        {
            rb.velocity -= rb.velocity * 0.01f;
            timer -= Time.deltaTime;
        }
        else
        {
            if (!haspoofed)
            {
                PoofAway(dirNormalised);
                haspoofed = true;
            } 
            rb.AddForce(dirNormalised + dirNormalised * (force * deceleration * (1/Vector2.Distance(transform.position,playerPos))),ForceMode2D.Force);
            //transform.position.magnitude >= playerPos.magnitude - 0.2f
            if (Vector3.Distance(playerPos, transform.position) <= 1) 
            {
                Souls.instance.CollectSouls(gameObject, 1);
            }
        }
        
    }

    public void PoofAway(Vector2 dir)
    {
        rb.AddForce(-dir * Random.Range(3,poofForce+1),ForceMode2D.Impulse);
    }
}
