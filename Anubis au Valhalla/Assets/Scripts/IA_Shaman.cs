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

    [Header("Déplacements")] 
    private Rigidbody2D rb;
    private GameObject player;
    private Seeker seeker;
    private AIPath aipath;
    private Path path;
    public SpriteRenderer sr;
    private IAstarAI ai;
    private AIDestinationSetter playerFollow;
    public float radiusWondering;
    public bool isWondering;
    public bool isFleeing;
    public float distanceMaxPlayer;


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
       // playerFollow.enabled = true;
        //playerFollow.target = player.transform;
        
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
                Debug.Log("wondering");
                playerFollow.enabled = false;
                ai.destination = PickRandomPoint();
                ai.SearchPath();
            }
        }
        
        if(Vector3.Distance(player.transform.position, transform.position) <= distanceMaxPlayer)
        {
            Vector3 distancePlayer = new Vector3(CharacterController.instance.transform.position.x - transform.position.x,
                CharacterController.instance.transform.position.y - transform.position.y,0);

            rb.AddForce(transform.position - distancePlayer, ForceMode2D.Force);
            isFleeing = true;
            isWondering = false;
        }
        else
        {
            isFleeing = false;
            isWondering = true;
        }

        StartUpSummonTimeTimer += Time.deltaTime;
        if (StartUpSummonTimeTimer >= StartUpSummonTime)
        {
            isWondering = false;
            SummoningTimeTimer += Time.deltaTime;
        }

        if (SummoningTimeTimer >= SummoningTime)
        {
            Instantiate(corbeau, transform.position + new Vector3(0,3,0), Quaternion.identity);
            StartUpSummonTimeTimer = 0;
            SummoningTimeTimer = 0;
            isWondering = true;
        }
    }
    
    Vector2 PickRandomPoint() 
    {
        var point = Random.insideUnitCircle * radiusWondering;
        point.x += ai.position.x;
        point.y += ai.position.y;
        return point;
    }
}
