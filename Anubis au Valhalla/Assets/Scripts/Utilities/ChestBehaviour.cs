using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChestBehaviour : MonoBehaviour
{
    public bool CanOpen;
    public bool isOpened;
    [Expandable] public List<ItemPattern> patternList;
    [Expandable] public ItemPattern patternLooted;
    private Rigidbody2D rbItem;
    public LayerMask groundLayer;
    public Animator openAnim;
    public GameObject CanvasInteraction;
    public Vector3 offset;
    public TextMeshProUGUI TextInteraction;

    private void Start()
    {
        CanvasInteraction = GameObject.FindWithTag("CanvasInteraction");
        TextInteraction = GameObject.Find("TexteAction").GetComponent<TextMeshProUGUI>();

        if(Physics2D.Raycast(transform.position,new Vector3(0,0,1),10,groundLayer))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y-5);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isOpened)
            {
                CanOpen = true;
                CanvasInteraction.transform.position = transform.position + offset;
                CanvasInteraction.transform.localScale = new Vector3(0,0,CanvasInteraction.transform.localScale.z);
                CanvasInteraction.transform.DOScale(new Vector3(1, 1, CanvasInteraction.transform.localScale.z),0.25f);
                TextInteraction.SetText("Ouvrir");
                CanvasInteraction.SetActive(true); 
            }
         
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CanOpen = false;
            if (CanvasInteraction is not null)
            {
                CanvasInteraction.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && CanOpen && !isOpened)
        {
            StartCoroutine(OpenChest());
            if (CanvasInteraction is not null)
            {
                CanvasInteraction.SetActive(false);
            }
        }
    }

    public IEnumerator OpenChest()
    {
        isOpened = true;
        openAnim.enabled = true;
        yield return new WaitForSeconds(1.3f);
        patternLooted = patternList[Random.Range(0, patternList.Count)];
        Souls.instance.CreateSouls(transform.position, patternLooted.soulAmount);
        
        for (int i = 0; i < patternLooted.chestContent.Count; i++)
        {
           Instantiate(patternLooted.chestContent[i], transform.position, Quaternion.identity);
        }
    }
}
