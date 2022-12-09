using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

public class IA_Valkyrie : MonoBehaviour
{
   [Header("Général")]
    public bool isElite;
    public GameObject emptyLayers;
    public MonsterLifeManager life;
    public SpriteRenderer[] spriteArray;


    [Header("Déplacements")]
    public GameObject player;
    public Seeker seeker;
    public AIPath aipath;
    private Path path;
    public GameObject canvasLifeBar;
    private Rigidbody2D rb;
    private Collider2D collider;
    IAstarAI ai;
    public AIDestinationSetter playerFollow;
    public bool isFleeing;
    public float radiusWondering;
    public Vector2 pointToGo;



    [Header("Attaque - Javelot")] 
    public bool isAttacking;
    public int puissanceAttaqueJavelot;
    public float javelotSpeed;
    public float StartUpJavelotTime;
    public float StartUpJavelotTimeTimer;
    public bool isJavelotIndic;
    public float IndicJavelotTime;
    public float IndicJavelotTimeTimer;
    public GameObject projectilJavelot;
    public GameObject indicationJavelot;
    [HideInInspector] public Vector2 dir;
    public Gradient gradientIndic;
    public GameObject indicHolder;

    [Header("Attaque - Jump")]
    public GameObject indicationFall;
    public GameObject hitboxFall;
    private Vector2 fallPos;
    public int FallDamage;
    public float pushForce;
    public bool hasShaked;
    public bool hasFallen;
    public float TriggerJumpTime;
    public float TriggerJumpTimeTimer;     // Le temps que met l'attaque à se tick
    public float JumpTime;
    public float JumpTimeTimer;            // Le temps que met la valkyrie à sauter et disparaitre
    public float IndicationTime;
    public float IndicationTimeTimer;           // Le temps que met la valkyrie entre l'indication de l'attaque (zone rouge) et la retombée
    public float FallTime;
    public float FallTimeTimer;           // Le temps que met la valkyrie entre la retombée et le retour à son etat normal.
    
    
    private void Start()
    {
        StartUpJavelotTime = Random.Range(5, 9);
        TriggerJumpTime = Random.Range(8, 13);
        spriteArray = GetComponentsInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        seeker = GetComponent<Seeker>();
        collider = GetComponent<BoxCollider2D>();
        ai = GetComponent<IAstarAI>();
        playerFollow.enabled = true;
        playerFollow.target = player.transform;
        
        if (life.elite)
        {
            isElite = true;
        }

        if (isElite)
        {
            puissanceAttaqueJavelot *= 2;
                FallDamage *= 2;
        }
        if (life.overdose || SalleGennerator.instance.currentRoom.overdose)
        {
            ai.maxSpeed *= 2;
            javelotSpeed *= 1.5f;
            StartUpJavelotTime *= 0.4f;
            JumpTime *= 0.4f;
            IndicationTime *= 0.5f;
            FallTime *= 0.5f;
        }
    }


    public void Update()
    {
        //SortEnemies();

       /* if (!isAttacking&& !life.isMomified)
        {
            Flip();
        }*/
        
        if(!isAttacking&& !life.isMomified) // Cooldwn des attaques;
        {
            StartUpJavelotTimeTimer += Time.deltaTime;
            TriggerJumpTimeTimer += Time.deltaTime;
        }
        
        if (TriggerJumpTimeTimer >= TriggerJumpTime) // Attaque saut
        {
            JumpTimeTimer += Time.deltaTime;
            TriggerSaut();
        }
        
        if (JumpTimeTimer >= JumpTime)
        {
                TriggerJumpTimeTimer = 0;
                hasShaked = false;
                canvasLifeBar.SetActive(false);
                foreach (SpriteRenderer sprite in spriteArray)
                {
                    sprite.enabled = false;
                }
                collider.enabled = false;
                IndicationTimeTimer += Time.deltaTime;
            
        }
        
        if (IndicationTimeTimer >= IndicationTime)
        {
            indicatorAndFall();
            FallTimeTimer += Time.deltaTime;
        }
        
        if (StartUpJavelotTimeTimer >= StartUpJavelotTime) // Attaque javelot
        {
            attaqueJavelot();
        }

        if (isJavelotIndic)
        {
            IndicJavelotTimeTimer += Time.deltaTime;
            if (indicHolder is not null)
            {
                Debug.Log("ouiqdqdd");
                indicHolder.GetComponent<SpriteRenderer>().color = gradientIndic.Evaluate(IndicJavelotTimeTimer);
            }
            
            if (IndicJavelotTimeTimer >= IndicJavelotTime)
            {
                StartUpJavelot();
                IndicJavelotTimeTimer = 0;
            }
        }
        
        if (!isFleeing&& !life.isMomified) // Déplacements
        {
            deplacement();
        }

        if (life.vieActuelle <= 0)
        {
            if (indicHolder is not null)
            {
                Destroy(indicHolder.gameObject);
            }
        }
    }

    void TriggerSaut()
    {
        FallTimeTimer = 0;
        hasFallen = false;
        isAttacking = true;
        ai.canMove = false;
            
        if (!hasShaked)
        {
            transform.DOShakePosition(1f, 1);
            hasShaked = true;
        }
    }
    void indicatorAndFall()
    {
        JumpTimeTimer = 0;
        if (!hasFallen)
        {
            fallPos = player.transform.position;
            hasFallen = true;
            GameObject indicationObj = Instantiate(indicationFall, player.transform.position, Quaternion.identity);
            Destroy(indicationObj,FallTime);
        }
        
        if (FallTimeTimer >= FallTime)
        {
            IndicationTimeTimer = 0;
            FallTimeTimer = 0;
            transform.position = fallPos;
            canvasLifeBar.SetActive(true);
            foreach (SpriteRenderer sprite in spriteArray)
            {
                sprite.enabled = true;
            }
            collider.enabled = true;
            StartCoroutine(LagFall());
        }             
    }
    
    void deplacement()
    {
        if (!ai.pathPending && ai.reachedEndOfPath || !ai.hasPath) 
        {
            playerFollow.enabled = false;
            PickRandomPoint();
            ai.destination = pointToGo;
            ai.SearchPath();
        }
    }

    void attaqueJavelot()
    {
        dir = new Vector2(CharacterController.instance.transform.position.x - transform.position.x,
            CharacterController.instance.transform.position.y - transform.position.y);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        GameObject indicJavelot = Instantiate(indicationJavelot,transform.position,  Quaternion.Euler(0,0,angle));
        Destroy(indicJavelot,IndicJavelotTime+0.1f);
        indicHolder = indicJavelot;
        isJavelotIndic = true;
        transform.DOShakePosition(IndicJavelotTime,1);
        isAttacking = true;
        StartUpJavelotTimeTimer = 0;
    }
    
  
    void StartUpJavelot() // Au début de l'attaque du javelot
    {
        GameObject projJavelot = Instantiate(projectilJavelot, transform.position, Quaternion.identity);
        projJavelot.GetComponent<JavelotValkyrie>().ia = this;
        isAttacking = false;
        isJavelotIndic = false;
    }

    IEnumerator LagFall() // A la fin de l'attaque du saut
    {
        Debug.Log("oui");
        GameObject hitboxObj = Instantiate(hitboxFall, transform.position, Quaternion.identity);
        hitboxObj.GetComponent<HitBoxFallValkyrie>().ia = this;
        yield return new WaitForSeconds(1);
        Destroy(hitboxObj);
        ai.canMove = true;
        IndicationTimeTimer = 0;
        isAttacking = false;
    }    
    void PickRandomPoint() 
    {
        var point = Random.insideUnitCircle * radiusWondering;
        point.x += ai.position.x;
        point.y += ai.position.y;
        
        if (Vector3.Distance((Vector3)player.transform.position, point) !<= radiusWondering)
        {
            PickRandomPoint();
        }
        else
        {
            pointToGo = point; 
            return;
        }
    }
}

