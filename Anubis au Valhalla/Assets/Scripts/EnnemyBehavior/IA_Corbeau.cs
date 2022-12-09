using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class IA_Corbeau : MonoBehaviour
{
    [Header("Vie et visuels")] public GameObject emptyLayers;
    public bool isElite;
    private Rigidbody2D rb;
    public LayerMask layerPlayer;
    public MonsterLifeManager life;

    [Header("Déplacements")] public GameObject player;
    public Seeker seeker;
    public AIPath aipath;
    private Path path;
    private SpriteRenderer sr;
    IAstarAI ai;
    public AIDestinationSetter playerFollow;
    public bool canMove = true;
    public bool isFleeing;
    public bool isRotating;
    public bool isChasing;
    public float forceRepulse;
    public float radiusFleeing;
    public float speedTowardPlayer;


    [Header("Attaque")] public bool isAttacking;
    public float rotationSpeed;
    public float rotationSpeedSlown;
    [NaughtyAttributes.ReadOnly] public int puissanceAttaque;
    public float StartUpAttackTime;
    public float StartUpAttackTimeTimer;
    public float AttackTime;
    public float AttackTimeTimer;
    public GameObject projectilPlume;
    public GameObject indicationAttaque;
    private GameObject holder;
    private bool indic = true;
    public float plumeSpeed;
    public Vector2 directionProj;
    public Gradient gradientIndic;
    
    
    //Fonctions ******************************************************************************************************************************************************
    
    private void Awake()
    {
        puissanceAttaque = GetComponentInParent<MonsterLifeManager>().data.damage;
    }

    private void Start()
    {
        StartUpAttackTime = Random.Range(3, 6);
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        seeker = GetComponent<Seeker>();
        sr = GetComponent<SpriteRenderer>();
        ai = GetComponent<IAstarAI>();
        playerFollow.enabled = true;
        playerFollow.target = player.transform;
        if (life.elite)
        {
            isElite = true;
        }

       if (life.overdose || SalleGennerator.instance.currentRoom.overdose)
        {
            speedTowardPlayer *= 150;
            forceRepulse *= 1.5f;
            rotationSpeed *= 2;
            rotationSpeedSlown *= 2;
            StartUpAttackTime *= 0.25f;
            AttackTime *= 0.3f;
        }
        
    }


    public void Update()
    {
        if (life.vieActuelle < 0)
        {
            Destroy(holder);
        }
        
        if (player.transform.position.y >
            emptyLayers.transform.position.y) // Faire en sorte que le perso passe derrière ou devant l'ennemi.
        {
            sr.sortingOrder = 2;
        }
        else
        {
            sr.sortingOrder = 1;
        }

        if (!isAttacking)
        {
            if (transform.position.x <
                player.transform.position
                    .x) // Permet d'orienter le monstre vers la direction dans laquelle il se déplace
            {
                transform.localScale = new Vector3(-1, 2.2909f, 1);
            }
            else if (transform.position.x > player.transform.position.x)
            {
                transform.localScale = new Vector3(1, 2.2909f, 1);
            }
        }

        if (StartUpAttackTimeTimer >= StartUpAttackTime && !life.isMomified)
        {
            AttackTimeTimer += Time.deltaTime;
            
            if (indic)
            {
                isFleeing = false;
                isChasing = false;
                isRotating = false;
                aipath.canMove = false;
                canMove = false;
                rb.velocity = Vector2.zero;
                directionProj = new Vector2(CharacterController.instance.transform.position.x - transform.position.x,
                    CharacterController.instance.transform.position.y - transform.position.y);
                float angle = Mathf.Atan2(directionProj.y, directionProj.x) * Mathf.Rad2Deg;
               GameObject indicOBJ = Instantiate(indicationAttaque,transform.position,  Quaternion.Euler(0,0,angle));
               holder = indicOBJ;
               Destroy(indicOBJ,AttackTime+0.1f);
               indic = false;
            }
            else if(life.gotHit)
            {
                if (holder is not null)
                {
                    Destroy(holder.gameObject);
                }
                StartUpAttackTimeTimer = 0;
                AttackTimeTimer = 0;
                canMove = true;
                indic = true;
            }

            holder.GetComponent<SpriteRenderer>().color = gradientIndic.Evaluate(AttackTimeTimer);
            
            if (AttackTimeTimer >= AttackTime && !life.isMomified)
            {
                aipath.canMove = true;
                GameObject projPlume = Instantiate(projectilPlume, transform.position, Quaternion.identity);
                projPlume.GetComponent<ProjectileCorbeau>().ia = this;
                StartUpAttackTimeTimer = 0;
                AttackTimeTimer = 0;
                indic = true;
                canMove = true;
            }
        }

        if (Vector3.Distance(player.transform.position, transform.position) <= radiusFleeing * 3 && !life.isMomified && !isFleeing && canMove) // Quand le monstre arrive proche du joueur, il commence à attaquer
        {
            isRotating = true;
            isChasing = false;
            StartUpAttackTimeTimer += Time.deltaTime;
                        
           
            
            if (life.isEnvased && canMove)
            {
                transform.RotateAround(player.transform.position, Vector3.forward, rotationSpeedSlown * Time.deltaTime);
                //rb.AddForce(Vector2.Perpendicular(transform.position - player.transform.position * rotationSpeedSlown ),ForceMode2D.Force);
            }
            else if(canMove)
            {
                //rb.AddForce(Vector2.Perpendicular(transform.position - player.transform.position* rotationSpeed),ForceMode2D.Force);
                transform.RotateAround(player.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
            }

        }
        else
        {
            isRotating = false;
        }
        
        /*if (Vector3.Distance(player.transform.position, transform.position) <= radiusFleeing && !isChasing && canMove)
        {
            isChasing = false;
            isRotating = false;
            isFleeing = true;
            Vector2 angle = transform.position - player.transform.position;
            rb.AddForce(angle.normalized * forceRepulse);
        }
        else
        {
            isFleeing = false;
        }*/
            
        if(Vector3.Distance(player.transform.position, transform.position) >= radiusFleeing*3 && !isFleeing && !isRotating && canMove)
        {
            isChasing = true;
            Vector2 angleTowardPlayer = player.transform.position - transform.position;
            rb.AddForce(angleTowardPlayer*speedTowardPlayer);
            isFleeing = false;
        }
        else
        {
            isChasing = false;
        }
    }
    
}