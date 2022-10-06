using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using TMPro;
using UnityEngine;

public class IA_Guerrier : MonoBehaviour
{
    [Header("Vie et visuels")]
    public GameObject emptyLayers;
    public bool isElite;

    [Header("Déplacements")]
    public GameObject player;
    public Seeker seeker;
    public AIPath aipath;
    private Path path;
    private SpriteRenderer sr;
    IAstarAI ai;
    public AIDestinationSetter playerFollow;
    public float radiusWondering;
    public bool isWondering;

    [Header("Attaque")] 
    public bool isAttacking;
    public Transform pointAttaque;
    public LayerMask HitboxPlayer;
    public float rangeAttaque;
    public int puissanceAttaque;
    public float StartUpAttackTime;
    public float StartUpAttackTimeTimer;
    public float WonderingTime;
    public float WonderingTimeTimer;
    public float rangeElite;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        seeker = GetComponent<Seeker>();
        sr = GetComponent<SpriteRenderer>();
        ai = GetComponent<IAstarAI>();
        playerFollow.enabled = true;
        playerFollow.target = player.transform;

        if (isElite)
        {
            rangeAttaque = rangeElite;
        }
    }


    public void Update()
    {
        if (player.transform.position.y > emptyLayers.transform.position.y) // Faire en sorte que le perso passe derrière ou devant l'ennemi.
        {
            sr.sortingOrder = 2;
        }
        else
        {
            sr.sortingOrder = 1;
        }

        if (!isAttacking)
        {
            if (transform.position.x < player.transform.position.x) // Permet d'orienter le monstre vers la direction dans laquelle il se déplace
            {
                transform.localScale = new Vector3(-1, 2.2909f, 1);
            }
            else if (transform.position.x > player.transform.position.x)
            {
                transform.localScale = new Vector3(1, 2.2909f, 1);
            }
        }
       

        if (aipath.reachedDestination) // Quand le monstre arrive proche du joueur, il commence à attaquer
        {
            if (isWondering)
            {
                StartCoroutine(WaitMove());
            }
            else
            {
                isAttacking = true;
            }

        }

        if (isAttacking)
        {
            aipath.canMove = false;
            StartUpAttackTimeTimer += Time.deltaTime;
        }
        
        IEnumerator WaitMove()
        {
            aipath.canMove = false;
            yield return new WaitForSeconds(1f);
            aipath.canMove = true;
        }

        if (StartUpAttackTimeTimer >= StartUpAttackTime)
        {
            Collider2D[] toucheJoueur = Physics2D.OverlapCircleAll(pointAttaque.position, rangeAttaque, HitboxPlayer);

            foreach (Collider2D joueur in toucheJoueur)
            {
                Debug.Log("touché");
                joueur.GetComponent<DamageManager>().TakeDamage(puissanceAttaque);
            }

            aipath.canMove = true;
            isWondering = true;
            isAttacking = false;
            StartUpAttackTimeTimer = 0;
        }

        if (isWondering)
        {
            WonderingTimeTimer += Time.deltaTime;
            if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath)) 
            {
                playerFollow.enabled = false;
                ai.destination = PickRandomPoint();
                ai.SearchPath();
            }
        }
        
        if (WonderingTimeTimer >= WonderingTime)
        {
            isAttacking = false;
            isWondering = false;
            playerFollow.enabled = true;
            ai.SearchPath();
            WonderingTimeTimer = 0;
            StartUpAttackTimeTimer = 0;
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
