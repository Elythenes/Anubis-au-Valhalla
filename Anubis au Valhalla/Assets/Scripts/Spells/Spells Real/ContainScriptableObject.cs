using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ContainScriptableObject : MonoBehaviour
{
    public static ContainScriptableObject instance;
    public List<SpellObject> SpellObjectsList;
    public List<GameObject> prefabInsideList;
    public SpellObject spellInside;
    public GameObject prefabInside;
    public bool isMoving;
    
    [Header("Force Au Spawn")]
    [SerializeField] private float force = 3f;
    [SerializeField] private float deceleration = 0.3f;
    public float timer;
    public Rigidbody2D rb;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        int numberGot = Random.Range(0, SpellObjectsList.Count);
        spellInside = SpellObjectsList[numberGot];
        prefabInside = prefabInsideList[numberGot];

        if (isMoving)
        {
            Vector2 explode = new Vector2(Random.Range(-force, force), Random.Range(-force, force));
            rb.AddForce(explode, ForceMode2D.Impulse);
            rb.drag = deceleration;
        }
    }

    private void Update()
    {
        if (timer >= 0)
        {
            rb.velocity -= rb.velocity * 0.01f;
            timer -= Time.deltaTime;
        }
    }
}
