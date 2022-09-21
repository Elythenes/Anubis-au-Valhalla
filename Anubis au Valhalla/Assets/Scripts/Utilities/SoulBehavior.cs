using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBehavior : MonoBehaviour
{
    public Vector3 playerPos;
    [SerializeField] private float force = 3f;
    [SerializeField] private float timer = 1;
    [SerializeField] private float deceleration = 0.3f;

    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb.AddForce(new Vector2(Random.Range(-force,force),Random.Range(-force,force)), ForceMode2D.Impulse);
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
            timer -= Time.deltaTime;
        }
        else
        {
            rb.AddForce(dirNormalised * force,ForceMode2D.Force);
        }
    }
}
