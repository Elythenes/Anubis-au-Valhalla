using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChestBehaviour : MonoBehaviour
{

    public bool isOpened;
    [Expandable] public List<ItemPattern> patternList;
    [Expandable] public ItemPattern patternLooted;
    private Rigidbody2D rbItem;
    public LayerMask groundLayer;
    public Animator openAnim;
    public ParticleSystem OuvertureVFX;
    
    [Header("Interaction")]
    public bool CanOpen;
    public GameObject CanvasInteraction;
    public Canvas canvasGné;
    public Vector3 offset;
    public TextMeshProUGUI TextInteraction;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip audioClipOpen;

    private void Start()
    {
        CanvasInteraction = GameObject.FindWithTag("CanvasInteraction");
        TextInteraction = GameObject.Find("TexteAction").GetComponent<TextMeshProUGUI>();
        canvasGné = CanvasInteraction.GetComponent<Canvas>();

        /*if(Physics2D.Raycast(transform.position,new Vector3(0,0,1),10,groundLayer))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y-5);
        }*/
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (!isOpened)
            {
                CanvasInteraction.SetActive(true);
                canvasGné.enabled = true;
                CanOpen = true;
                CanvasInteraction.transform.position = transform.position + offset;
                CanvasInteraction.transform.localScale = new Vector3(0,0,CanvasInteraction.transform.localScale.z);
                CanvasInteraction.transform.DOScale(new Vector3(1, 1, CanvasInteraction.transform.localScale.z),0.25f);
                TextInteraction.SetText("Ouvrir");
            }
         
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) //c'est du Debug, ne sert pas vraiment
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canvasGné.enabled = false;
            //CanvasInteraction.SetActive(false);
            CanOpen = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && CanOpen && !isOpened)
        {
            StartCoroutine(OpenChest());
            if (CanvasInteraction is not null)
            {
                audioSource.pitch = 1;
                audioSource.PlayOneShot(audioClipOpen);
                canvasGné.enabled = false;
                //CanvasInteraction.SetActive(false);
            }
        }
    }

    [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
    public IEnumerator OpenChest()
    {
        OuvertureVFX.Play();
        isOpened = true;
        openAnim.enabled = true;
        yield return new WaitForSeconds(1.3f);
        patternLooted = patternList[Random.Range(0, patternList.Count)];
        Souls.instance.CreateSouls(transform.position, patternLooted.soulAmount);
        
        for (int i = 0; i < patternLooted.chestContent.Count; i++)
        {
            Instantiate(patternLooted.chestContent[i], transform.position, Quaternion.identity, transform);
        }
    }
}
