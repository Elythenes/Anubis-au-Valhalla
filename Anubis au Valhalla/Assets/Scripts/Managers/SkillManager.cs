using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SkillManager : MonoBehaviour
{
    public KeyCode spell1;
    public KeyCode spell2;
    
    public GameObject targetUser;

    public GameObject flameArea;
    public int timerSpell1 = 2;
    
    public GameObject sandstormArea;
    public int timerSpell2 = 2;

    public GameObject fireBall;
    public float launchVelocity = 100f;

    void Update()
    {
        if (Input.GetKeyDown(spell1))
        {
            //TimeLimitedSpell(flameArea, timerSpell1);
            ThrowingSpell(fireBall);
        }
        if (Input.GetKeyDown(spell2))
        {
            FollowingSpell(sandstormArea, timerSpell2);
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
    void TimeLimitedSpell(GameObject gb, int timer) 
    {
        var gbInstance = Instantiate(gb, new Vector3(targetUser.transform.position.x, targetUser.transform.position.y/*-(targetUser.transform.localScale.y/2)*/, 0), Quaternion.identity);
        Debug.Log("Spell1 used");
        StartCoroutine(TimeLimitedGb(gbInstance, timerSpell1));
    }

    //Spell qui apparaît, disparaît après une durée et qui reste sur du joueur
    //(ici int déclarée au début "timerSpell2")
    void FollowingSpell(GameObject gb, int timer)
    {
        var gbInstance = Instantiate(gb,new Vector3(targetUser.transform.position.x, targetUser.transform.position.y/*-(targetUser.transform.localScale.y/2)*/, 0), Quaternion.identity, targetUser.transform);
        Debug.Log("Spell2 used");
        StartCoroutine(TimeLimitedGb(gbInstance, timerSpell2));
    }
    
    //Pour un Spell qui apparaît (et disparaît après avoir touché qqc (ennemi ou mur))
    void ThrowingSpell(GameObject gb)
    {
        var gbInstance = Instantiate(gb, new Vector3(targetUser.transform.position.x, targetUser.transform.position.y+targetUser.transform.localScale.y/2, 0), Quaternion.identity);
        gbInstance.GetComponent<Rigidbody2D>().AddForce(Camera.main.ScreenToWorldPoint(Input.mousePosition)*launchVelocity);
    }
}

