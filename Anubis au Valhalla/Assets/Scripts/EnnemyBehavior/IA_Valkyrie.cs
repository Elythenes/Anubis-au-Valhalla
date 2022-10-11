using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

public class IA_Valkyrie : MonoBehaviour
{
   [Header("Vie et visuels")]
    public GameObject emptyLayers;
    public bool isElite;
    private Rigidbody2D rb;
    public LayerMask layerPlayer;

    [Header("Déplacements")]
    public GameObject player;
    public Seeker seeker;
    public AIPath aipath;
    private Path path;
    private SpriteRenderer sr;
    IAstarAI ai;
    public AIDestinationSetter playerFollow;
    public bool isFleeing;
    public float forceRepulse;
    public float distanceMinPlayer;
    public float radiusWondering;
    public float radiusFleeing;
    public Vector2 pointToGo;
    


    [Header("Attaque - Javelot")] 
    public bool isAttacking;
    public int puissanceAttaqueJavelot;
    public float javelotSpeed;
    public float StartUpJavelotTime;
    public float StartUpJavelotTimeTimer;
    public GameObject projectilJavelot;

    [Header("Attaque - Jump")]
    public GameObject indicationFall;
    public float StartUpJumpTime;
    public float StartUpJumpTimeTimer;
    public float JumpTime;
    public float JumpTimeTimer;
    public float Fall1Time;
    public float Fall1TimeTimer;
    public float Fall2Time;
    public float Fall2TimeTimer;
    
    
   


    private void Start()
    {
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
        
        if(!isAttacking)
        {
            StartUpJavelotTimeTimer += Time.deltaTime;
            StartUpJumpTimeTimer += Time.deltaTime;
        }

        if (StartUpJumpTimeTimer >= StartUpJumpTime)
        {
            isAttacking = true;
            ai.canMove = false;
            JumpTimeTimer += Time.deltaTime;
            
            if (JumpTimeTimer >= JumpTime)
            {
                StartUpJavelotTime = 0;
                sr.enabled = false;
                Fall1TimeTimer += Time.deltaTime;
                
                if (Fall1TimeTimer >= Fall1Time)
                {
                    transform.position = player.transform.position;
                    GameObject indicationFallObj = Instantiate(indicationFall, player.transform.position, Quaternion.identity);
                    Fall2TimeTimer += Time.deltaTime;
                    
                    if (Fall2TimeTimer >= Fall2Time)
                    {
                        Destroy(indicationFallObj);
                        sr.enabled = true;
                    }
                }
            }
        }
        
        if (StartUpJavelotTimeTimer >= StartUpJavelotTime)
        {
            isAttacking = true;
            GameObject projJavelot = Instantiate(projectilJavelot, transform.position, Quaternion.identity);
            projJavelot.GetComponent<JavelotValkyrie>().ia = this;
            StartUpJavelotTimeTimer = 0;
            isAttacking = false;
        }
        
        if (!isFleeing)
        {
            if (!ai.pathPending && ai.reachedEndOfPath || !ai.hasPath) 
            {
                playerFollow.enabled = false;
                PickRandomPoint();
                ai.destination = pointToGo;
                ai.SearchPath();
            }
        }
        
        if (Vector3.Distance(player.transform.position, transform.position) <= distanceMinPlayer) // Le monstre fuit quand il est trop proche du personnage.
        {
            isFleeing = true;
            RaycastHit hitUp;
                            if (Physics2D.Raycast(transform.position, Vector2.up, radiusFleeing, layerPlayer))
                            {
                                Debug.DrawRay(transform.position, Vector2.up * radiusFleeing, Color.red);
                                rb.AddForce(Vector2.down * forceRepulse);
                                ai.canMove = false;
                            }

                            RaycastHit hitDown;
                            if (Physics2D.Raycast(transform.position, Vector2.down, radiusFleeing, layerPlayer))
                            {
                                Debug.DrawRay(transform.position, Vector2.down * radiusFleeing, Color.red);
                                rb.AddForce(Vector2.up * forceRepulse);
                                ai.canMove = false;
                            }

                            RaycastHit hitRight;
                            if (Physics2D.Raycast(transform.position, Vector2.right, radiusFleeing, layerPlayer))
                            {
                                Debug.DrawRay(transform.position, Vector2.right * radiusFleeing, Color.red);
                                rb.AddForce(Vector2.left * forceRepulse);
                                ai.canMove = false;
                            }

                            RaycastHit hitLeft;
                            if (Physics2D.Raycast(transform.position, Vector2.left, radiusFleeing, layerPlayer))
                            {
                                Debug.DrawRay(transform.position, Vector2.left * radiusFleeing, Color.red);
                                rb.AddForce(Vector2.right * forceRepulse);
                                ai.canMove = false;
                            }

                            RaycastHit hitUpLeft;
                            if (Physics2D.Raycast(transform.position, new Vector2(1, 1), radiusFleeing, layerPlayer))
                            {
                                Debug.DrawRay(transform.position, new Vector2(1, 1) * radiusFleeing, Color.red);
                                rb.AddForce(new Vector2(-1, -1) * forceRepulse);
                                ai.canMove = false;
                            }

                            RaycastHit hitUpRight;
                            if (Physics2D.Raycast(transform.position, new Vector2(-1, 1), radiusFleeing, layerPlayer))
                            {
                                Debug.DrawRay(transform.position, new Vector2(-1, 1) * radiusFleeing, Color.red);
                                rb.AddForce(new Vector2(1, -1) * forceRepulse);
                                ai.canMove = false;
                            }

                            RaycastHit hitDownLeft;
                            if (Physics2D.Raycast(transform.position, new Vector2(1, -1), radiusFleeing, layerPlayer))
                            {
                                Debug.DrawRay(transform.position, new Vector2(1, -1) * radiusFleeing, Color.red);
                                rb.AddForce(new Vector2(-1, 1) * forceRepulse);
                                ai.canMove = false;
                            }

                            RaycastHit hitDownRight;
                            if (Physics2D.Raycast(transform.position, new Vector2(-1, -1), radiusFleeing,layerPlayer))
                            {
                                Debug.DrawRay(transform.position, new Vector2(-1, -1) * radiusFleeing, Color.red);
                                rb.AddForce(new Vector2(1, 1) * forceRepulse);
                                ai.canMove = false;
                            }
        }
        else
        {
            ai.canMove = true;
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
            pointToGo = point; 
            return;
        }
    }
}

