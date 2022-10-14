using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
using UnityEngine;

public class IAManager : MonoBehaviour
{ 
    [Header("Vie et visuels")]
    public bool isElite;
    public GameObject emptyLayers;
    public LayerMask layerPlayer;
    public EnemyType enemyType;
    public EnemyData a;

    
    private Rigidbody2D rb;
    private GameObject player;
    private Seeker seeker;
    private AIPath aipath;
    private Path path;
    private SpriteRenderer sr;
    private IAstarAI ai;
    private AIDestinationSetter playerFollow;
    

    
    
    public enum EnemyType
    {
        Shaman = 0,
        Corbeau = 1,
        Loup = 2,
        Guerrier = 3,
        Valkyrie = 4
    }
    


    private void Start()
    {
        emptyLayers = GameObject.Find("emptyLayer");
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        seeker = GetComponent<Seeker>();
        sr = GetComponent<SpriteRenderer>();
        ai = GetComponent<IAstarAI>();
        playerFollow = GetComponent<AIDestinationSetter>();
        playerFollow.enabled = true;
        playerFollow.target = player.transform;
        
    }


    public void Update()
    {
        SortEnemies();
        if (!a.isAttacking)
        {
            Flip();
        }
        switch (enemyType)
        {
            case EnemyType.Shaman:

                if (a.isWondering && !a.isFleeing)
                {
                    Roam();
                }
                CompareOwnPosToPlayer();
        
                a.StartUpSummonTimeTimer += Time.deltaTime;
                if (a.StartUpSummonTimeTimer >= a.StartUpSummonTime)
                {
                    a.isAttacking = true;
                    a.isWondering = false;
                    a.SummoningTimeTimer += Time.deltaTime;
                }

                if (a.SummoningTimeTimer >= a.SummoningTime)
                {
                    Summon();
                }
                break;
            
            case EnemyType.Valkyrie:
                SortEnemies();

                if (!a.isAttacking)
                {
                    Flip();
                }
        
                if(!a.isAttacking) // Cooldwn des attaques;
                {
                    a.StartUpJavelotTimeTimer += Time.deltaTime;
                    a.TriggerJumpTimeTimer += Time.deltaTime;
                }
        
                if (a.TriggerJumpTimeTimer >= a.TriggerJumpTime) // Attaque saut
                {
                    a.JumpTimeTimer += Time.deltaTime;
                    TriggerSaut();
                }
        
                if (a.JumpTimeTimer >= a.JumpTime)
                {
                    a.TriggerJumpTimeTimer = 0;
                    hasShaked = false;
                    sr.enabled = false;
                    IndicationTimeTimer += Time.deltaTime;
            
                }
        
                if (IndicationTimeTimer >= IndicationTime)
                {
                    indicatorAndFall();
                    FallTimeTimer += Time.deltaTime;
                }
        
                if (a.StartUpJavelotTimeTimer >= StartUpJavelotTime) // Attaque javelot
                {
                    attaqueJavelot();
                }
        
                if (!isFleeing) // Déplacements
                {
                    Roam();
                }
                break;
        }
    }
        void DetectPlayerRelativePos()
    {
        RaycastHit hitUp;
        if(Physics2D.Raycast(transform.position, Vector2.up, radiusWonderingShaman,layerPlayer))
        {
            Debug.DrawRay(transform.position,Vector2.up * radiusWonderingShaman,Color.red);
            rb.AddForce(Vector2.down * forceRepulseShaman);
            isWondering = false;
        }
                        
        RaycastHit2D hitDown;
        if(Physics2D.Raycast(transform.position, Vector2.down, radiusWonderingShaman,layerPlayer))
        {
            Debug.DrawRay(transform.position,Vector2.down * radiusWonderingShaman,Color.red);
            rb.AddForce(Vector2.up * forceRepulseShaman);
            isWondering = false;
        }
                        
        RaycastHit hitRight;
        if(Physics2D.Raycast(transform.position, Vector2.right, radiusWonderingShaman,layerPlayer))
        {
            Debug.DrawRay(transform.position,Vector2.right * radiusWonderingShaman,Color.red);
            rb.AddForce(Vector2.left * forceRepulseShaman);
            isWondering = false;
        }
                        
        RaycastHit hitLeft;
        if(Physics2D.Raycast(transform.position, Vector2.left, radiusWonderingShaman,layerPlayer))
        {
            Debug.DrawRay(transform.position,Vector2.left * radiusWonderingShaman,Color.red);
            rb.AddForce(Vector2.right * forceRepulseShaman);
            isWondering = false;
        }
                        
        RaycastHit hitUpLeft;
        if(Physics2D.Raycast(transform.position, new Vector2(1,1), radiusWonderingShaman,layerPlayer))
        {
            Debug.DrawRay(transform.position,new Vector2(1,1) * radiusWonderingShaman,Color.red);
            rb.AddForce(new Vector2(-1,-1) * forceRepulseShaman);
            isWondering = false;
        }
                        
        RaycastHit hitUpRight;
        if(Physics2D.Raycast(transform.position, new Vector2(-1,1), radiusWonderingShaman,layerPlayer))
        {
            Debug.DrawRay(transform.position,new Vector2(-1,1) * radiusWonderingShaman,Color.red);
            rb.AddForce(new Vector2(1,-1) * forceRepulseShaman);
            isWondering = false;
        }
                        
        RaycastHit hitDownLeft;
        if(Physics2D.Raycast(transform.position, new Vector2(1,-1), radiusWonderingShaman,layerPlayer))
        {
            Debug.DrawRay(transform.position,new Vector2(1,-1) * radiusWonderingShaman,Color.red);
            rb.AddForce(new Vector2(-1,1) * forceRepulseShaman);
            isWondering = false;
        }
                        
        RaycastHit hitDownRight;
        if(Physics2D.Raycast(transform.position, new Vector2(-1,-1), radiusWonderingShaman,layerPlayer))
        {
            Debug.DrawRay(transform.position,new Vector2(-1,-1) * radiusWonderingShaman,Color.red);
            rb.AddForce(new Vector2(1,1) * forceRepulseShaman);
            isWondering = false;
        }
    }
    void SortEnemies()
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
    }
    void Flip()
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
    void Roam()
    {
        if (!ai.pathPending && ai.reachedEndOfPath || !ai.hasPath) 
        {
            playerFollow.enabled = false;
            PickRandomPoint();
            ai.destination = pointToGOFleeing;
            ai.SearchPath();
        }
    }
    #region ShamanIA

    void CompareOwnPosToPlayer()
    {
        if(Vector3.Distance(player.transform.position, transform.position) <= distanceMaxPlayerShaman)
        {
            if (!isAttacking)
            {
                isFleeing = true;
                isWondering = false;
                
                if (isFleeing)
                {
                    timeFleeingShamanTimer += Time.deltaTime;
                    
                    if (timeFleeingShamanTimer <= timeFleeingShaman)
                    {
                        DetectPlayerRelativePos();
                    }
                    else
                    {
                        timeFleeingShamanTimer = 0;
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
    }

    void Summon()
    {
        Instantiate(corbeau, transform.position + new Vector3(0,3,0), Quaternion.identity);
        StartUpSummonTimeTimer = 0;
        SummoningTimeTimer = 0;
        isWondering = true;
        isAttacking = false;
    }
    void PickRandomPoint() 
    {
        var point = Random.insideUnitCircle * radiusWonderingShaman;
        point.x += ai.position.x;
        point.y += ai.position.y;
        
        if (Vector3.Distance((Vector3)player.transform.position, point) !<= radiusWonderingShaman)
        {
            PickRandomPoint();
        }
        else
        {
            pointToGOFleeing =point; 
            return;
        }
    }

    #endregion
   
    #region ValkyrieIA
    
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
            sr.enabled = true;
            StartCoroutine(LagFall());
        }             
    }

    void attaqueJavelot()
    {
        StartCoroutine(StartUpJavelot());
        transform.DOShakePosition(1,1);
        isAttacking = true;
        StartUpJavelotTimeTimer = 0;
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
    #endregion
}
