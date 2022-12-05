using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class PotionRepository : MonoBehaviour
{
    public static PotionRepository Instance;
    
    [Expandable] public List<PotionObject> potionsList;
    [Expandable] public PotionObject potionInside;
    
    [Header("Force Au Spawn")]
    public bool isMoving;
    [SerializeField] private float force = 3f;
    [SerializeField] private float deceleration = 0.3f;
    public float timer;
    public Rigidbody2D rb;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        int numberGot = Random.Range(0, potionsList.Count);
        potionInside = potionsList[numberGot];
        
        if (isMoving)
        {
            Vector2 explode = new Vector2(Random.Range(-force, force), Random.Range(-force, force));
            rb.AddForce(explode, ForceMode2D.Impulse);
            rb.drag = deceleration;
        }
    }
    
    private void Update()
    {

        isMoving = false;
        if (timer >= 0)
        {
            rb.velocity -= rb.velocity * 0.01f;
            timer -= Time.deltaTime;
        }
    }
    
}
