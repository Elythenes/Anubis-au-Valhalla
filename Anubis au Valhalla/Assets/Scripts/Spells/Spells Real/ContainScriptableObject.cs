using System.Collections.Generic;
using DG.Tweening;
using TMPro;
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
    
    [Header("Interaction")]
    public bool canInteract;
    public GameObject CanvasInteraction;
    public Vector3 offset;
    public TextMeshProUGUI TextInteraction;
    
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
        
        CanvasInteraction = GameObject.FindWithTag("CanvasInteraction");
        TextInteraction = GameObject.Find("TexteAction").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (timer >= 0)
        {
            rb.velocity -= rb.velocity * 0.01f;
            timer -= Time.deltaTime;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            canInteract = true;
            CanvasInteraction.SetActive(true);
            CanvasInteraction.transform.position = transform.transform.position + offset;
            CanvasInteraction.transform.localScale = new Vector3(0, 0, CanvasInteraction.transform.localScale.z);
            CanvasInteraction.transform.DOScale(new Vector3(1, 1, CanvasInteraction.transform.localScale.z), 0.25f);
            TextInteraction.SetText("Prendre");
        }
    }
    
    
    private void OnTriggerExit2D(Collider2D other) //c'est du Debug, ne sert pas vraiment
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanvasInteraction.SetActive(false);
            canInteract = false;
        }
    }
}
