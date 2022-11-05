using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChestBehaviour : MonoBehaviour
{
    public bool CanOpen;
    public bool isOpened;
    public List<ItemPattern> patternList;
    public ItemPattern patternLooted;
    private Rigidbody2D rbItem;
    private SpriteRenderer sr;
    public Sprite spriteNormal;
    public Sprite spriteOutline;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CanOpen = true;
            //sr.sprite = spriteOutline;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CanOpen = false;
            //sr.sprite = spriteNormal;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && CanOpen && !isOpened)
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        isOpened = true;
        patternLooted = patternList[Random.Range(0, patternList.Count)];
        Souls.instance.CreateSouls(transform.position, patternLooted.soulAmount);
        
        for (int i = 0; i < patternLooted.chestContent.Count; i++)
        {
           Instantiate(patternLooted.chestContent[i], transform.position, Quaternion.identity);
        }
    }
}
