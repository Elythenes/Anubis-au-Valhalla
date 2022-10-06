using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class IA_Corbeau : MonoBehaviour
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
    public float distanceMaxPlayer;


    [Header("Attaque")] 
    public bool isAttacking;
    public float rotationSpeed; 
    public int puissanceAttaque;
    public float StartUpAttackTime;
    public float StartUpAttackTimeTimer;
    public GameObject projectilPlume;
    public float plumeSpeed;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        seeker = GetComponent<Seeker>();
        sr = GetComponent<SpriteRenderer>();
        ai = GetComponent<IAstarAI>();
        playerFollow.enabled = true;
        playerFollow.target = player.transform;
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
            aipath.canMove = false;
            transform.RotateAround(player.transform.position,Vector3.forward,rotationSpeed*Time.deltaTime);
            StartUpAttackTimeTimer += Time.deltaTime;

            if (Vector3.Distance(player.transform.position, transform.position) <= distanceMaxPlayer)
            {
                Vector3 distancePlayer = new Vector3(CharacterController.instance.transform.position.x - transform.position.x,
                    CharacterController.instance.transform.position.y - transform.position.y,0);

                transform.DOMove(transform.position - distancePlayer*2, 5, false);
            }
            
            if (StartUpAttackTimeTimer >= StartUpAttackTime)
            {
                GameObject projPlume = Instantiate(projectilPlume, transform.position, Quaternion.identity);
                projPlume.GetComponent<ProjectileCorbeau>().ia = this;
                StartUpAttackTimeTimer = 0;
            }
        }
        else
        {
            aipath.canMove = true;
        }
    }
}
