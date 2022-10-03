using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public float radiusWondering;
    public bool isWondering;

    [Header("Attaque")] 
    public bool isAttacking;
    public float rotationSpeed; 
    public int puissanceAttaque;
    public float StartUpAttackTime;
    public float StartUpAttackTimeTimer;
    public GameObject projectilPlume;


    private void Start()
    {
        seeker = GetComponent<Seeker>();
        sr = GetComponent<SpriteRenderer>();
        ai = GetComponent<IAstarAI>();
        playerFollow.enabled = true;
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
            
            if (StartUpAttackTimeTimer >= StartUpAttackTime)
            {
                GameObject projPlume = Instantiate(projectilPlume, player.transform.position, Quaternion.identity);
               /* Vector2 dir = new Vector2(CharacterController.instance.transform.position.x - projPlume.transform.position.x,
                    CharacterController.instance.transform.position.y - projPlume.transform.position.y);
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                projPlume.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward)*/
                StartUpAttackTimeTimer = 0;
            }
        }
        else
        {
            aipath.canMove = true;
        }
    }
}
