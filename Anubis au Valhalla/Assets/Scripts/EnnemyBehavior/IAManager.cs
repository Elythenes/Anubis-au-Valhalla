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
                    a.hasShaked = false;
                    sr.enabled = false;
                    a.IndicationTimeTimer += Time.deltaTime;
            
                }
        
                if (a.IndicationTimeTimer >= a.IndicationTime)
                {
                    indicatorAndFall();
                    a.FallTimeTimer += Time.deltaTime;
                }
        
                if (a.StartUpJavelotTimeTimer >= a.StartUpJavelotTime) // Attaque javelot
                {
                    attaqueJavelot();
                }
        
                if (!a.isFleeing) // Déplacements
                {
                    Roam();
                }
                break;
        }
    }
        void DetectPlayerRelativePos()
    {
        RaycastHit hitUp;
        if(Physics2D.Raycast(transform.position, Vector2.up, a.radiusWonderingShaman,layerPlayer))
        {
            Debug.DrawRay(transform.position,Vector2.up * a.radiusWonderingShaman,Color.red);
            rb.AddForce(Vector2.down * a.forceRepulseShaman);
            a.isWondering = false;
        }
                        
        RaycastHit2D hitDown;
        if(Physics2D.Raycast(transform.position, Vector2.down, a.radiusWonderingShaman,layerPlayer))
        {
            Debug.DrawRay(transform.position,Vector2.down * a.radiusWonderingShaman,Color.red);
            rb.AddForce(Vector2.up * a.forceRepulseShaman);
            a.isWondering = false;
        }
                        
        RaycastHit hitRight;
        if(Physics2D.Raycast(transform.position, Vector2.right, a.radiusWonderingShaman,layerPlayer))
        {
            Debug.DrawRay(transform.position,Vector2.right * a.radiusWonderingShaman,Color.red);
            rb.AddForce(Vector2.left * a.forceRepulseShaman);
            a.isWondering = false;
        }
                        
        RaycastHit hitLeft;
        if(Physics2D.Raycast(transform.position, Vector2.left, a.radiusWonderingShaman,layerPlayer))
        {
            Debug.DrawRay(transform.position,Vector2.left * a.radiusWonderingShaman,Color.red);
            rb.AddForce(Vector2.right * a.forceRepulseShaman);
            a.isWondering = false;
        }
                        
        RaycastHit hitUpLeft;
        if(Physics2D.Raycast(transform.position, new Vector2(1,1), a.radiusWonderingShaman,layerPlayer))
        {
            Debug.DrawRay(transform.position,new Vector2(1,1) * a.radiusWonderingShaman,Color.red);
            rb.AddForce(new Vector2(-1,-1) * a.forceRepulseShaman);
            a.isWondering = false;
        }
                        
        RaycastHit hitUpRight;
        if(Physics2D.Raycast(transform.position, new Vector2(-1,1), a.radiusWonderingShaman,layerPlayer))
        {
            Debug.DrawRay(transform.position,new Vector2(-1,1) * a.radiusWonderingShaman,Color.red);
            rb.AddForce(new Vector2(1,-1) * a.forceRepulseShaman);
            a.isWondering = false;
        }
                        
        RaycastHit hitDownLeft;
        if(Physics2D.Raycast(transform.position, new Vector2(1,-1), a.radiusWonderingShaman,layerPlayer))
        {
            Debug.DrawRay(transform.position,new Vector2(1,-1) * a.radiusWonderingShaman,Color.red);
            rb.AddForce(new Vector2(-1,1) * a.forceRepulseShaman);
            a.isWondering = false;
        }
                        
        RaycastHit hitDownRight;
        if(Physics2D.Raycast(transform.position, new Vector2(-1,-1), a.radiusWonderingShaman,layerPlayer))
        {
            Debug.DrawRay(transform.position,new Vector2(-1,-1) * a.radiusWonderingShaman,Color.red);
            rb.AddForce(new Vector2(1,1) * a.forceRepulseShaman);
            a.isWondering = false;
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
            ai.destination = a.pointToGOFleeing;
            ai.SearchPath();
        }
    }
    #region ShamanIA

    void CompareOwnPosToPlayer()
    {
        if(Vector3.Distance(player.transform.position, transform.position) <= a.distanceMaxPlayerShaman)
        {
            if (!a.isAttacking)
            {
                a.isFleeing = true;
                a.isWondering = false;
                
                if (a.isFleeing)
                {
                    a.timeFleeingShamanTimer += Time.deltaTime;
                    
                    if (a.timeFleeingShamanTimer <= a.timeFleeingShaman)
                    {
                        DetectPlayerRelativePos();
                    }
                    else
                    {
                        a.timeFleeingShamanTimer = 0;
                        a.isFleeing = false;
                        a.isWondering = true;
                    }
                }
            }
        }
        else
        {
            a.isFleeing = false;
            a.isWondering = true;
        }
    }

    void Summon()
    {
        Instantiate(a.corbeau, transform.position + new Vector3(0,3,0), Quaternion.identity);
        a.StartUpSummonTimeTimer = 0;
        a.SummoningTimeTimer = 0;
        a.isWondering = true;
        a.isAttacking = false;
    }
    void PickRandomPoint() 
    {
        var point = Random.insideUnitCircle * a.radiusWonderingShaman;
        point.x += ai.position.x;
        point.y += ai.position.y;
        
        if (Vector3.Distance((Vector3)player.transform.position, point) !<= a.radiusWonderingShaman)
        {
            PickRandomPoint();
        }
        else
        {
            a.pointToGOFleeing =point; 
            return;
        }
    }

    #endregion
   
    #region ValkyrieIA
    
     void TriggerSaut()
    {
        a.FallTimeTimer = 0;
        a.hasFallen = false;
        a.isAttacking = true;
        ai.canMove = false;
            
        if (!a.hasShaked)
        {
            transform.DOShakePosition(1f, 1);
            a.hasShaked = true;
        }
    }
    void indicatorAndFall()
    {
        a.JumpTimeTimer = 0;
        if (!a.hasFallen)
        {
            a.fallPos = player.transform.position;
            a.hasFallen = true;
            GameObject indicationObj = Instantiate(a.indicationFall, player.transform.position, Quaternion.identity);
            Destroy(indicationObj,a.FallTime);
        }
        
        if (a.FallTimeTimer >= a.FallTime)
        {
            a.IndicationTimeTimer = 0;
            a.FallTimeTimer = 0;
            transform.position = a.fallPos;
            sr.enabled = true;
            StartCoroutine(LagFall());
        }             
    }

    void attaqueJavelot()
    {
        StartCoroutine(StartUpJavelot());
        transform.DOShakePosition(1,1);
        a.isAttacking = true;
        a.StartUpJavelotTimeTimer = 0;
    }
    IEnumerator StartUpJavelot() // Au début de l'attaque du javelot
    {
        yield return new WaitForSeconds(1f);
        GameObject projJavelot = Instantiate(a.projectilJavelot, transform.position, Quaternion.identity);
        projJavelot.GetComponent<JavelotValkyrie>().ia = this;
        a.isAttacking = false;
    }
    
    IEnumerator LagFall() // A la fin de l'attaque du saut
    {
        Debug.Log("oui");
        GameObject hitboxObj = Instantiate(a.hitboxFall, transform.position, Quaternion.identity);
        hitboxObj.GetComponent<HitBoxFallValkyrie>().ia = this;
        yield return new WaitForSeconds(1);
        Destroy(hitboxObj);
        ai.canMove = true;
        a.IndicationTimeTimer = 0;
        a.isAttacking = false;
    }
    #endregion
}
