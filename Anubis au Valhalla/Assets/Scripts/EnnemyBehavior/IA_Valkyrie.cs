using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        
        if(!isAttacking) // Cooldwn des attaques;
        {
            StartUpJavelotTimeTimer += Time.deltaTime;
            TriggerJumpTimeTimer += Time.deltaTime;
        }

       
        if (TriggerJumpTimeTimer >= TriggerJumpTime) // Attaque saut
        {
            FallTimeTimer = 0;
            hasFallen = false;
            isAttacking = true;
            ai.canMove = false;
            JumpTimeTimer += Time.deltaTime;
            
            if (!hasShaked)
            {
                transform.DOShakePosition(1f, 1);
                hasShaked = true;
            }

        }
        
        if (JumpTimeTimer >= JumpTime)
        {
                TriggerJumpTimeTimer = 0;
                hasShaked = false;
                sr.enabled = false;
                IndicationTimeTimer += Time.deltaTime;
                
        }
        
        if (IndicationTimeTimer >= IndicationTime)
        {
            JumpTimeTimer = 0;
                    if (!hasFallen)
                    {
                        fallPos = player.transform.position;
                        hasFallen = true;
                        GameObject indicationObj = Instantiate(indicationFall, player.transform.position, Quaternion.identity);
                        Destroy(indicationObj,FallTime);
                    }
                    FallTimeTimer += Time.deltaTime;
                    
                    if (FallTimeTimer >= FallTime)
                    {
                        IndicationTimeTimer = 0;
                        FallTimeTimer = 0;
                        transform.position = fallPos;
                        sr.enabled = true;
                        StartCoroutine(LagFall());
                    }             
        }
       
        
        if (StartUpJavelotTimeTimer >= StartUpJavelotTime) // Attaque javelot
        {
            StartCoroutine(StartUpJavelot());
            transform.DOShakePosition(1,1);
            isAttacking = true;
            StartUpJavelotTimeTimer = 0;
            
        }
        
        if (!isFleeing) // Déplacements
        {
            if (!ai.pathPending && ai.reachedEndOfPath || !ai.hasPath) 
            {
                playerFollow.enabled = false;
                PickRandomPoint();
                ai.destination = pointToGo;
                ai.SearchPath();
            }
        }
    }

    IEnumerator StartUpJavelot() // Au début de l'attaque du javelot
    {
        yield return new WaitForSeconds(1f);
        GameObject projJavelot = Instantiate(projectilJavelot, transform.position, Quaternion.identity);
        projJavelot.GetComponent<JavelotValkyrie>().ia = this;
        isAttacking = false;
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

