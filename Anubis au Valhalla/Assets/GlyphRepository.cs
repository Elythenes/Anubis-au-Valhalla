using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class GlyphRepository : MonoBehaviour
{
    public static GlyphRepository Instance;

    [Expandable] public List<GlyphObject> glyphsList;
    [Expandable] public GlyphObject glyphInside;
    
    [Header("TESTING")] 
    public bool doGlyphTest;
    [Expandable] public GlyphObject glyphTest;
    
    [Header("Force Au Spawn")]
    public bool isMoving;
    [SerializeField] private float force = 3f;
    [SerializeField] private float deceleration = 0.3f;
    public float timer;
    public Rigidbody2D rb;
    
    [Header("Interaction")]
    public bool canInteract;
    public GameObject CanvasInteraction;
    public Vector3 offset;
    public TextMeshProUGUI TextInteraction;
    
    //Fonction : Systèmes *******************************************************************************************************************
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        if (doGlyphTest)
        {
            glyphInside = glyphTest;
        }
        else
        {
            int numberGot = Random.Range(0, glyphsList.Count);
            glyphInside = glyphsList[numberGot];
        }
        
        
        if (isMoving)
        {
            Vector2 explode = new Vector2(Random.Range(-force, force), Random.Range(-force, force));
            rb.AddForce(explode, ForceMode2D.Impulse);
            rb.drag = deceleration;
        }
        
        CanvasInteraction = GameObject.FindWithTag("CanvasInteraction");
        TextInteraction = GameObject.Find("TexteAction").GetComponent<TextMeshProUGUI>();
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = false;
        if (timer >= 0)
        {
            rb.velocity -= rb.velocity * 0.01f;
            timer -= Time.deltaTime;
        }
    }
    
    
    //Fonction *******************************************************************************************************************
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            CanvasInteraction.GetComponent<Canvas>().enabled = true;
            canInteract = true;
            CanvasInteraction.SetActive(true);
            CanvasInteraction.transform.position = transform.transform.position + offset;
            CanvasInteraction.transform.localScale = new Vector3(0, 0, CanvasInteraction.transform.localScale.z);
            CanvasInteraction.transform.DOScale(new Vector3(1, 1, CanvasInteraction.transform.localScale.z), 0.25f);
            TextInteraction.SetText("Prendre");
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanvasInteraction.GetComponent<Canvas>().enabled = false;
            //CanvasInteraction.SetActive(false);
            canInteract = false;
        }
    }
    
}
