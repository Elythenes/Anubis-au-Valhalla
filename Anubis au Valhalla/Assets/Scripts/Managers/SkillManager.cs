using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SkillManager : MonoBehaviour
{

    [Header("Général")] 
    public GameObject player;
    public KeyCode spell1;
    public KeyCode spell2;
    public GameObject targetUser;
    public LayerMask layerMonstres;

    [Header("FlameArea")]
    public GameObject flameArea;
    public int timerSpell1 = 2;
    public float rangeAttaqueFlameArea;
    public int puissanceAttaqueFlameArea;
    public int tempsReloadHitFlameArea;
    
    [Header("SandStorm")]
    public GameObject sandstormArea;
    public int timerSpell2 = 2;

    [Header("FireBall")]
    public GameObject fireBall;
    public float launchVelocity = 100f;
    public float rangeAttaqueFireBall;
    public int puissanceAttaqueFireBall;

    void Update()
    {
        if (Input.GetKeyDown(spell1))
        {
            //TimeLimitedSpell(flameArea, timerSpell1);
            ThrowingSpell(fireBall);
        }
        if (Input.GetKeyDown(spell2))
        {
            FollowingSpell(sandstormArea);
        }
    }
    
    //Coroutine pour les spells qui doivent disparaître
    IEnumerator TimeLimitedGb(GameObject gbInstance, int timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gbInstance);
        Debug.Log("destroyed");
    }
    
    //Pour un Spell qui apparaît et disparaît après une durée
    //(ici int déclarée au début "timerSpell1")
    void TimeLimitedSpell(GameObject gb, float timerReload) 
    {
        var gbInstance = Instantiate(gb, new Vector3(targetUser.transform.position.x, targetUser.transform.position.y/*-(targetUser.transform.localScale.y/2)*/, 0), Quaternion.identity);
        Debug.Log("Spell1 used");
        StartCoroutine(TimeLimitedGb(gbInstance, timerSpell1));
    }

    
    
    //Spell qui apparaît, disparaît après une durée et qui reste sur du joueur
    //(ici int déclarée au début "timerSpell2")
    void FollowingSpell(GameObject gb)
    {
        var gbInstance = Instantiate(gb,new Vector3(targetUser.transform.position.x, targetUser.transform.position.y/*-(targetUser.transform.localScale.y/2)*/, 0), Quaternion.identity, targetUser.transform);

        Debug.Log("Spell2 used");
        StartCoroutine(TimeLimitedGb(gbInstance, timerSpell2));
    }

    
    //Pour un Spell qui apparaît (et disparaît après avoir touché qqc (ennemi ou mur))
    void ThrowingSpell(GameObject gb)
    {
        var gbInstance = Instantiate(gb, new Vector3(targetUser.transform.position.x, targetUser.transform.position.y+targetUser.transform.localScale.y/2, 0), Quaternion.identity);
        
        Collider2D[] toucheMonstre = Physics2D.OverlapCircleAll(player.transform.position, rangeAttaqueFireBall, layerMonstres);

        foreach (Collider2D monstre in toucheMonstre)
        {
            Debug.Log("touché");
            monstre.GetComponent<IA_Monstre1>().TakeDamage(puissanceAttaqueFireBall);
        }
        
        gbInstance.GetComponent<Rigidbody2D>().AddForce(Camera.main.ScreenToWorldPoint(Input.mousePosition)*launchVelocity);
    }
    
    
}

