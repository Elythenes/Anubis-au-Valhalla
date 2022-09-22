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

    [Header("FlameArea")]
    public GameObject flameArea;
    public int timerSpell1 = 2;
    public int puissanceAttaqueFlameArea;
    public float espacementDoTFlameArea;
    private float cooldownFlameAreaTimer;
    public float cooldownFlameArea;
    public bool canCastFlameArea;
    
    [Header("SandStorm")]
    public GameObject sandstormArea;
    public int timerSpell2 = 2;
    public int puissanceAttaqueSandstorm;
    public float espacementDoTSandstorm;
    private float tempsReloadHitSandstorm;
    private float cooldownSandstormTimer;
    public float cooldownSandstorm;
    public bool canCastSandstorm;

    [Header("FireBall")] 
    public GameObject fireBall;
    public int timerSpell3 = 2;
    public float bulletSpeed;
    public int puissanceAttaqueFireBall;
    private float cooldownFireballTimer;
    public float cooldownFireball;
    public bool canCastFireBall;

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

            if (canCastFlameArea)
            {
                TimeLimitedSpell(flameArea, timerSpell1);
            }
            
            /*if (canCastFireBall)
            {
                ThrowingSpell(fireBall);
            }*/
        }
        
        if (Input.GetKeyDown(spell2))
        {
            if (canCastSandstorm)
            {
                FollowingSpell(sandstormArea);
            }
        }
        
        if (cooldownFlameAreaTimer < cooldownFlameArea && !canCastFlameArea) //cooldown de la FlameArea
        {
            cooldownFlameAreaTimer += Time.deltaTime;
        }
        else if (cooldownFlameAreaTimer > cooldownFlameArea)
        {
            canCastFlameArea = true;
            cooldownFlameAreaTimer = 0;
        }
        
        if (cooldownSandstormTimer < cooldownSandstorm && !canCastSandstorm) //cooldown du Sandstorm
        {
            cooldownSandstormTimer += Time.deltaTime;
        }
        else if (cooldownSandstormTimer > cooldownSandstorm)
        {
            canCastSandstorm = true;
            cooldownSandstormTimer = 0;
        }
        
        if (cooldownFireballTimer < cooldownFireball && !canCastFireBall) //cooldown de la Fireball
        {
            cooldownFireballTimer += Time.deltaTime;
        }
        else if (cooldownFireballTimer > cooldownFireball)
        {
            canCastFireBall = true;
            cooldownFireballTimer = 0;
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
        canCastFlameArea = false;
        var gbInstance = Instantiate(gb, new Vector3(targetUser.transform.position.x, targetUser.transform.position.y/*-(targetUser.transform.localScale.y/2)*/, 0), Quaternion.identity);
        Debug.Log("Spell1 used");
        StartCoroutine(TimeLimitedGb(gbInstance, timerSpell1));
    }

    
    
    //Spell qui apparaît, disparaît après une durée et qui reste sur du joueur
    //(ici int déclarée au début "timerSpell2")
    void FollowingSpell(GameObject gb)
    {
        canCastSandstorm = false;
        var gbInstance = Instantiate(gb,new Vector3(targetUser.transform.position.x, targetUser.transform.position.y/*-(targetUser.transform.localScale.y/2)*/, 0), Quaternion.identity, targetUser.transform);

        Debug.Log("Spell2 used");
        StartCoroutine(TimeLimitedGb(gbInstance, timerSpell2));
    }

    
    //Pour un Spell qui apparaît (et disparaît après avoir touché qqc (ennemi ou mur))
    void ThrowingSpell(GameObject gb)
    {
        canCastFireBall = false;
        Vector2 mousePos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 charaPos = CharacterController.instance.transform.position;
        float angle = Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
        
        var gbInstance = Instantiate(gb, new Vector3(targetUser.transform.position.x,
            targetUser.transform.position.y+targetUser.transform.localScale.y/2, 0), Quaternion.AngleAxis(angle, Vector3.forward));
        StartCoroutine(TimeLimitedGb(gbInstance, timerSpell3));
    }
}

