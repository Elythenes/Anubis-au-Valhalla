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
    public LayerMask groundLayer;

    private void Start()
    {

        if(Physics2D.Raycast(transform.position,new Vector3(0,0,1),10,groundLayer))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y-5);
        }
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
        if (Input.GetKeyDown(KeyCode.F) && CanOpen && !isOpened)
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
