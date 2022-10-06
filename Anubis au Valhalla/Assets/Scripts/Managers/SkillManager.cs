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
    public static SkillManager instance;
    public GameObject player;
    public KeyCode spell1;
    public KeyCode spell2;
    public GameObject targetUser;
    public LayerMask layerMonstres;

    [Header("SO FlameArea")]
    public GameObject flameArea;
    public SpellStaticAreaType sOFlameArea;
    
    [Header("SO SandStorm")]
    public GameObject sandstormArea;
    public SpellFollowingAreaType soSandstorm;
    
    [Header("SO Fireball")]
    public GameObject fireBall;
    public SpellThrowingType sOFireball;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(spell1))
        {
            Debug.Log("spell1");

            if (sOFlameArea.canCast)
            {
                TimeLimitedSpell(flameArea, sOFlameArea.duration);
            }
            
            /*if (sOFireball.canCast)
            {
                ThrowingSpell(fireBall);
            }*/
        }
        
        if (Input.GetKeyDown(spell2))
        {
            if (soSandstorm.canCast)
            {
                FollowingSpell(sandstormArea);
            }
        }
        
        if (sOFlameArea.cooldownTimer < sOFlameArea.cooldown && !sOFlameArea.canCast) //cooldown de la FlameArea
        {
            sOFlameArea.cooldownTimer += Time.deltaTime;
        }
        else if (sOFlameArea.cooldownTimer > sOFlameArea.cooldown)
        {
            sOFlameArea.canCast = true;
            sOFlameArea.cooldownTimer = 0;
        }
        
        if (soSandstorm.cooldownTimer < soSandstorm.cooldown && !soSandstorm.canCast) //cooldown du Sandstorm
        {
            soSandstorm.cooldownTimer += Time.deltaTime;
        }
        else if (soSandstorm.cooldownTimer > soSandstorm.cooldown)
        {
            soSandstorm.canCast = true;
            soSandstorm.cooldownTimer = 0;
        }
        
        if (sOFireball.cooldownTimer < sOFireball.cooldown && !sOFireball.canCast) //cooldown de la Fireball
        {
            sOFireball.cooldownTimer += Time.deltaTime;
        }
        else if (sOFireball.cooldownTimer > sOFireball.cooldown)
        {
            sOFireball.canCast = true;
            sOFireball.cooldownTimer = 0;
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
        sOFlameArea.canCast = false;
        var gbInstance = Instantiate(gb, new Vector3(targetUser.transform.position.x, targetUser.transform.position.y/*-(targetUser.transform.localScale.y/2)*/, 0), Quaternion.identity);
        Debug.Log("Spell1 used");
        StartCoroutine(TimeLimitedGb(gbInstance, sOFlameArea.duration));
    }

    
    
    //Spell qui apparaît, disparaît après une durée et qui reste sur du joueur
    //(ici int déclarée au début "timerSpell2")
    void FollowingSpell(GameObject gb)
    {
        soSandstorm.canCast = false;
        var gbInstance = Instantiate(gb,new Vector3(targetUser.transform.position.x, targetUser.transform.position.y/*-(targetUser.transform.localScale.y/2)*/, 0), Quaternion.identity, targetUser.transform);

        Debug.Log("Spell2 used");
        StartCoroutine(TimeLimitedGb(gbInstance, soSandstorm.duration));
    }

    
    //Pour un Spell qui apparaît (et disparaît après avoir touché qqc (ennemi ou mur))
    void ThrowingSpell(GameObject gb)
    {
        sOFireball.canCast = false;
        Vector2 mousePos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 charaPos = CharacterController.instance.transform.position;
        float angle = Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
        
        var gbInstance = Instantiate(gb, new Vector3(targetUser.transform.position.x,
            targetUser.transform.position.y+targetUser.transform.localScale.y/2, 0), Quaternion.AngleAxis(angle, Vector3.forward));
        StartCoroutine(TimeLimitedGb(gbInstance, sOFireball.duration));
    }
}
