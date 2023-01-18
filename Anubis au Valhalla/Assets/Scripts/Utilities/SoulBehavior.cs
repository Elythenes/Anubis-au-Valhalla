using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBehavior : MonoBehaviour
{
    public Vector3 playerPos;
    [SerializeField] private float force = 3f;
    [SerializeField] private float forceSpawn = 3f;
    [SerializeField] private float timer = 1;
    [SerializeField] private float deceleration = 0.3f;
    [SerializeField] private float poofForce = 3f;
    private bool haspoofed = false;

  

    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
        Vector2 explode = new Vector2(Random.Range(-forceSpawn, forceSpawn), Random.Range(-forceSpawn, forceSpawn));
        rb.AddForce(new Vector2(explode.x,explode.y), ForceMode2D.Impulse);
        rb.drag = deceleration;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPos = CharacterController.instance.transform.position;
        Vector3 dir = playerPos - transform.position;
        Vector3 dirNormalised = dir.normalized;
        
        
        if (timer >= 0)
        {
            rb.velocity -= rb.velocity * 0.03f;
            timer -= Time.deltaTime;
        }
        else
        {
            if (!haspoofed)
            {
                PoofAway(dirNormalised);
                haspoofed = true;
            }
            if (Vector3.Distance(playerPos, transform.position) >= 5)
            {
                rb.AddForce(dirNormalised + dirNormalised * (force * deceleration * (1/Vector2.Distance(transform.position,playerPos))),ForceMode2D.Force);
            }
            else
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(dirNormalised * forceSpawn,ForceMode2D.Impulse);
                if (Vector3.Distance(playerPos, transform.position) <= 1) 
                {
                    Souls.instance.CollectSouls(gameObject, 1);
                }
            }
        }
        
    }

    public void PoofAway(Vector2 dir)
    {
        rb.AddForce(-dir * Random.Range(3,poofForce+1),ForceMode2D.Impulse);
    }
}
