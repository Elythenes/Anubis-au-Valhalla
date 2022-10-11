using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

public class IA_Shaman : MonoBehaviour
{
    [Header("Vie et visuels")]
    public GameObject emptyLayers;
    public bool isElite;
    public LayerMask layerPlayer;

    [Header("Déplacements")] 
    private Rigidbody2D rb;
    private GameObject player;
    private Seeker seeker;
    private AIPath aipath;
    private Path path;
    private SpriteRenderer sr;
    private IAstarAI ai;
    private AIDestinationSetter playerFollow;
    public float radiusWondering;
    public bool isWondering;
    public bool isFleeing;
    public float distanceMaxPlayer;
    public float timeFleeing;
    public float timeFleeingTimer;
    public float forceRepulse;
    public Vector3 pointToGO;


    [Header("Attaque")] 
    public bool isAttacking;
    public float StartUpSummonTime;
    public float StartUpSummonTimeTimer;
    public float SummoningTime;
    public float SummoningTimeTimer;
    private bool hasShaked;
    public GameObject corbeau;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        seeker = GetComponent<Seeker>();
        sr = GetComponent<SpriteRenderer>();
        ai = GetComponent<IAstarAI>();
        playerFollow = GetComponent<AIDestinationSetter>();
    }


    public void Update()
    {
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
        
        
        if (isWondering && !isFleeing)
        {
            if (!ai.pathPending && ai.reachedEndOfPath || !ai.hasPath) 
            {
                playerFollow.enabled = false;
                PickRandomPoint();
                ai.destination = pointToGO;
                ai.SearchPath();
            }
        }
        
        if(Vector3.Distance(player.transform.position, transform.position) <= distanceMaxPlayer)
        {
            
            if (!isAttacking)
            {
                isFleeing = true;
                isWondering = false;
                
                if (isFleeing)
                {
                    timeFleeingTimer += Time.deltaTime;
                    
                    if (timeFleeingTimer <= timeFleeing)
                    {
                        RaycastHit hitUp;
                        if(Physics2D.Raycast(transform.position, Vector2.up, radiusWondering,layerPlayer))
                        {
                            Debug.DrawRay(transform.position,Vector2.up * radiusWondering,Color.red);
                            rb.AddForce(Vector2.down * forceRepulse);
                            isWondering = false;
                        }
                        
                        RaycastHit hitDown;
                        if(Physics2D.Raycast(transform.position, Vector2.down, radiusWondering,layerPlayer))
                        {
                            Debug.DrawRay(transform.position,Vector2.down * radiusWondering,Color.red);
                            rb.AddForce(Vector2.up * forceRepulse);
                            isWondering = false;
                        }
                        
                        RaycastHit hitRight;
                        if(Physics2D.Raycast(transform.position, Vector2.right, radiusWondering,layerPlayer))
                        {
                            Debug.DrawRay(transform.position,Vector2.right * radiusWondering,Color.red);
                            rb.AddForce(Vector2.left * forceRepulse);
                            isWondering = false;
                        }
                        
                        RaycastHit hitLeft;
                        if(Physics2D.Raycast(transform.position, Vector2.left, radiusWondering,layerPlayer))
                        {
                            Debug.DrawRay(transform.position,Vector2.left * radiusWondering,Color.red);
                            rb.AddForce(Vector2.right * forceRepulse);
                            isWondering = false;
                        }
                        
                        RaycastHit hitUpLeft;
                        if(Physics2D.Raycast(transform.position, new Vector2(1,1), radiusWondering,layerPlayer))
                        {
                            Debug.DrawRay(transform.position,new Vector2(1,1) * radiusWondering,Color.red);
                            rb.AddForce(new Vector2(-1,-1) * forceRepulse);
                            isWondering = false;
                        }
                        
                        RaycastHit hitUpRight;
                        if(Physics2D.Raycast(transform.position, new Vector2(-1,1), radiusWondering,layerPlayer))
                        {
                            Debug.DrawRay(transform.position,new Vector2(-1,1) * radiusWondering,Color.red);
                            rb.AddForce(new Vector2(1,-1) * forceRepulse);
                            isWondering = false;
                        }
                        
                        RaycastHit hitDownLeft;
                        if(Physics2D.Raycast(transform.position, new Vector2(1,-1), radiusWondering,layerPlayer))
                        {
                            Debug.DrawRay(transform.position,new Vector2(1,-1) * radiusWondering,Color.red);
                            rb.AddForce(new Vector2(-1,1) * forceRepulse);
                            isWondering = false;
                        }
                        
                        RaycastHit hitDownRight;
                        if(Physics2D.Raycast(transform.position, new Vector2(-1,-1), radiusWondering,layerPlayer))
                        {
                            Debug.DrawRay(transform.position,new Vector2(-1,-1) * radiusWondering,Color.red);
                            rb.AddForce(new Vector2(1,1) * forceRepulse);
                            isWondering = false;
                        }
                        
                        
                    }
                    else
                    {
                        timeFleeingTimer = 0;
                        isFleeing = false;
                        isWondering = true;
                    }
                }
            }
        }
        else
        {
            isFleeing = false;
            isWondering = true;
        }
        

        StartUpSummonTimeTimer += Time.deltaTime;
        if (StartUpSummonTimeTimer >= StartUpSummonTime)
        {
            isAttacking = true;
            isWondering = false;
            SummoningTimeTimer += Time.deltaTime;
        }

        if (SummoningTimeTimer >= SummoningTime)
        {
            Instantiate(corbeau, transform.position + new Vector3(0,3,0), Quaternion.identity);
            StartUpSummonTimeTimer = 0;
            SummoningTimeTimer = 0;
            isWondering = true;
            isAttacking = false;
        }
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
            pointToGO =point; 
            return;
        }
    }
}
