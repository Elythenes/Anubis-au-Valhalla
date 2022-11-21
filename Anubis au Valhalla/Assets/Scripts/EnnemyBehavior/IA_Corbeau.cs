using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEditor.Experimental.GraphView;

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
    public bool isFleeing;
    public float forceRepulse;
    public float radiusFleeing;


    [Header("Attaque")] public bool isAttacking;
    public float rotationSpeed;
    public float rotationSpeedSlown;
    public int puissanceAttaque;
    public float StartUpAttackTime;
    public float StartUpAttackTimeTimer;
    public GameObject projectilPlume;
    public float plumeSpeed;


    private void Start()
    {
        life = GetComponent<MonsterLifeManager>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        seeker = GetComponent<Seeker>();
        sr = GetComponent<SpriteRenderer>();
        ai = GetComponent<IAstarAI>();
        playerFollow.enabled = true;
        playerFollow.target = player.transform;
        
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

       

        if (aipath.reachedDestination && !life.isMomified) // Quand le monstre arrive proche du joueur, il commence à attaquer
        {
            aipath.canMove = false;
            if (life.isEnvased)
            {
                transform.RotateAround(player.transform.position, Vector3.forward, rotationSpeedSlown * Time.deltaTime);
                //rb.AddForce(Vector2.Perpendicular(transform.position - player.transform.position * rotationSpeedSlown ),ForceMode2D.Force);
            }
            else
            {
                //rb.AddForce(Vector2.Perpendicular(transform.position - player.transform.position* rotationSpeed),ForceMode2D.Force);
                transform.RotateAround(player.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
            }
          

            if (Vector3.Distance(player.transform.position, transform.position) <= radiusFleeing)
            {
                Debug.Log("close");
                isFleeing = true;
                Vector2 angle = transform.position - player.transform.position;
               rb.AddForce(angle.normalized*forceRepulse);
                          /*  if (Physics2D.Raycast(transform.position, Vector2.up, radiusFleeing, layerPlayer))
                            {
                                Debug.DrawRay(transform.position, Vector2.up * radiusFleeing, Color.red);
                                rb.AddForce(Vector2.down * forceRepulse);
                            }

                           
                            if (Physics2D.Raycast(transform.position, Vector2.down, radiusFleeing, layerPlayer))
                            {
                                Debug.DrawRay(transform.position, Vector2.down * radiusFleeing, Color.red);
                                rb.AddForce(Vector2.up * forceRepulse);
                            }

                            
                            if (Physics2D.Raycast(transform.position, Vector2.right, radiusFleeing, layerPlayer))
                            {
                                Debug.DrawRay(transform.position, Vector2.right * radiusFleeing, Color.red);
                                rb.AddForce(Vector2.left * forceRepulse);
                            }

                            
                            if (Physics2D.Raycast(transform.position, Vector2.left, radiusFleeing, layerPlayer))
                            {
                                Debug.DrawRay(transform.position, Vector2.left * radiusFleeing, Color.red);
                                rb.AddForce(Vector2.right * forceRepulse);
                            }

                           
                            if (Physics2D.Raycast(transform.position, new Vector2(1, 1), radiusFleeing, layerPlayer))
                            {
                                Debug.DrawRay(transform.position, new Vector2(1, 1) * radiusFleeing, Color.red);
                                rb.AddForce(new Vector2(-1, -1) * forceRepulse);
                            }

                           
                            if (Physics2D.Raycast(transform.position, new Vector2(-1, 1), radiusFleeing, layerPlayer))
                            {
                                Debug.DrawRay(transform.position, new Vector2(-1, 1) * radiusFleeing, Color.red);
                                rb.AddForce(new Vector2(1, -1) * forceRepulse);
                            }

                            
                            if (Physics2D.Raycast(transform.position, new Vector2(1, -1), radiusFleeing, layerPlayer))
                            {
                                Debug.DrawRay(transform.position, new Vector2(1, -1) * radiusFleeing, Color.red);
                                rb.AddForce(new Vector2(-1, 1) * forceRepulse);
                            }

                           
                            if (Physics2D.Raycast(transform.position, new Vector2(-1, -1), radiusFleeing,layerPlayer))
                            {
                                Debug.DrawRay(transform.position, new Vector2(-1, -1) * radiusFleeing, Color.red);
                                rb.AddForce(new Vector2(1, 1) * forceRepulse);
                            }*/
                
                            StartUpAttackTimeTimer += Time.deltaTime;
                        
                if (StartUpAttackTimeTimer >= StartUpAttackTime && !life.isMomified)
                {
                    GameObject projPlume = Instantiate(projectilPlume, transform.position, Quaternion.identity);
                    projPlume.GetComponent<ProjectileCorbeau>().ia = this;
                    StartUpAttackTimeTimer = 0;
                }
            }
            else if(Vector3.Distance(player.transform.position, transform.position) >= radiusFleeing)
            {
                Debug.Log("far");
                isFleeing = false;
            }
            
            
            if (!life.isMomified)
            {
                aipath.canMove = true;
            }
          
        }
    }
}