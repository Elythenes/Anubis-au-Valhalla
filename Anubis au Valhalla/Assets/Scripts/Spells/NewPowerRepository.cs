
using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class NewPowerRepository : MonoBehaviour
{
    public NewPowerType newPowerType = NewPowerType.None;
    public Texture sprite;
    public GameObject orbeViolette;
    public GameObject orbeJaune;
    
      
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

    private void Awake()
    {
        if (NewPowerManager.Instance.currentLevelPower1 == 10)
        {
            newPowerType = NewPowerType.Power2;
        }
        else if (NewPowerManager.Instance.currentLevelPower2 == 10)
        {
            newPowerType = NewPowerType.Power1;
        }
        else
        {
            int range = Random.Range(0, 2);
            if (range == 0)
            {
                newPowerType = NewPowerType.Power1;
            }
            else
            {
                newPowerType = NewPowerType.Power2;
            }
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

    private void Start()
    {
        if (newPowerType == NewPowerType.Power1)
        {
            orbeViolette.SetActive(true);
        }
        
        if (newPowerType == NewPowerType.Power2)
        {
            orbeJaune.SetActive(true);

        }
    }

    private void Update()
    {
        if (NewPowerManager.Instance.currentLevelPower1 == 10)
        {
            newPowerType = NewPowerType.Power2;
        }
        else if (NewPowerManager.Instance.currentLevelPower2 == 10)
        {
            newPowerType = NewPowerType.Power1;
        }
        
        isMoving = false;
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
            CanvasInteraction.GetComponent<Canvas>().enabled = true;
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
            CanvasInteraction.GetComponent<Canvas>().enabled = false;
            //CanvasInteraction.SetActive(false);
            canInteract = false;
        }
    }
}

public enum NewPowerType
{
    Power1,
    Power2,
    None,
}

