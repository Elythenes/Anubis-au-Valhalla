using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class AttaquesNormales : MonoBehaviour
{
    public Animator anim;
    private InputManager controls;
    public static AttaquesNormales instance;

    [Header("Stats Attaques")]

    public List<GameObject> hitBoxC = new List<GameObject>();
    public List<Vector2> rangeAttaque = new List<Vector2>();
    public List<bool> isC = new List<bool>();
    public List<int> damage = new List<int>();
    public List<float> dureeHitbox = new List<float>();
    [HideInInspector] public List<float> stunDuration = new List<float>();
    public List<float> forceKnockback = new List<float>();
    public List<float> stunDurationMax = new List<float>();
    public List<float> dashImpulse = new List<float>();
    public List<float> timeForCanDash = new List<float>();
    public List<float> dashTimers = new List<float>();

    [Header("Général")]
    public bool abandonOn;
    public bool canAttack;
    public int comboActuel;
    public float cooldownAbandonCombo;
    public float cooldownAbandonComboTimer;
    public bool buffer;
    public GameObject swordObj;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        controls = new InputManager();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        Mouse mouse = InputSystem.GetDevice<Mouse>(); // Trouver input de la souris
        if (mouse.leftButton.wasPressedThisFrame && canAttack /*|| buffer*/) // Execute l'attaque selon l'avancement du combo
        {
            switch (comboActuel)
            {
                case 0:
                    
                    if (canAttack)
                    {
                        anim.SetBool("isAttacking1",true);
                        anim.SetBool("isAttacking2",false);
                        anim.SetBool("isIdle",false);
                        abandonOn = false;
                        cooldownAbandonComboTimer = 0;
                        //buffer = false;
                        comboActuel++;
                        Combo(0);     
                    }
                    
                    break;
                
                case 1:
                    if (canAttack)
                    {
                        anim.SetBool("isAttacking2",true);
                        anim.SetBool("isAttacking1",false);
                        anim.SetBool("isIdle",false);
                        abandonOn = false;
                        cooldownAbandonComboTimer = 0;
                       // buffer = false;
                        comboActuel++;
                        Combo(1);
                    }
                    
                    break;
                
                case 2:
                    if (canAttack)
                    {
                        abandonOn = false;
                        cooldownAbandonComboTimer = 0;
                        //buffer = false;
                        comboActuel = 0;
                        Combo(2);
                    }
                    break;
                    
            }
        }

        if (CharacterController.instance.isDashing)
        {
            Destroy(swordObj);
        }


        // ------------------ Gestion Abandon du Combo ---------------
        if (abandonOn)
        {
            cooldownAbandonComboTimer += Time.deltaTime;
        }
        
        if (cooldownAbandonComboTimer >= cooldownAbandonCombo) // Condition de retour à l'idle
        {
            abandonOn = false;
            comboActuel = 0;
            cooldownAbandonComboTimer = 0;
        }
        
        // ------------------ Gestion Abandon du Combo ---------------
        // ------------------ Gestion Combo -------------
      
        /*if (IsC1 && !canAttack && StunDurationTimer1 <= StunDuration1 && abandonOn)
        {
            if (mouse.leftButton.wasPressedThisFrame)
            {
                buffer = true;
            }
        }*/
        //ComboTimers(stunDurationMax,stunDuration);
        for (int i = 0; i < hitBoxC.Count; i++)
        {
            if (!isC[i])
            {
                continue;
            }
            if (stunDuration[i] >= stunDurationMax[i])
            {
                anim.SetBool("isAttacking1",false);
                anim.SetBool("isAttacking2",false);
                anim.SetBool("isIdle",true);
                Debug.Log(stunDuration[i] >= stunDurationMax[i]);
                canAttack = true;
                CharacterController.instance.isAttacking = false;
                isC[i] = false;
                stunDuration[i] = 0;
                dashTimers[i] = 0;
            }

            if (dashTimers[i] >= timeForCanDash[i])
            {
                CharacterController.instance.canDash = true;
            }
            stunDuration[i] += Time.deltaTime;
            dashTimers[i] += Time.deltaTime;
            

        }

        // ------------------ Gestion Combo-------------
    }
    //<Combo 1>/ Dash légèrement vers l'avant puis crée une hitbox devant le perso et touche les ennemis
    //<Combo 2>/ La même chose mais la hitbox est plus alongée et le dash plus long et rapide
    //<Combo 3>/ La même chose mais la hitbox est plus alongée et le dash plus long et rapide
    public void Combo(int index) 
    {
        abandonOn = true;
        isC[index] = true;
        canAttack = false;
        CharacterController.instance.isAttacking = true;
        Vector2 charaPos = CharacterController.instance.transform.position;
        Vector2 mousePos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
        Vector3 moveDirection = (mousePos - charaPos);
        moveDirection.z = 0;
        moveDirection.Normalize();
        if (moveDirection.x > 0)
        {
            CharacterController.instance.transform.localRotation = Quaternion.Euler(CharacterController.instance.transform.localRotation.x,0,CharacterController.instance.transform.localRotation.z);
        }
        else
        {
            CharacterController.instance.transform.localRotation = Quaternion.Euler(CharacterController.instance.transform.localRotation.x,-180,CharacterController.instance.transform.localRotation.z);
        }

        if (index == 0)
        {
            Slide(CharacterController.instance.facing);
        }
        else
        {
            CharacterController.instance.rb.AddForce(moveDirection * dashImpulse[index], ForceMode2D.Impulse);
        }
        Debug.Log(moveDirection);
        
        swordObj = Instantiate(hitBoxC[index], new Vector3(999,99,0),Quaternion.identity);
        swordObj.transform.position = CharacterController.instance.transform.position;
        swordObj.transform.localRotation = Quaternion.AngleAxis(angle,Vector3.forward);
    }
    public void Slide(CharacterController.LookingAt dir)
    {
        var rb = CharacterController.instance.rb;
        switch (dir)
        {
            case CharacterController.LookingAt.Nord:
                rb.velocity = (new Vector2(0,1) * rb.velocity.magnitude * 2.5f);
                break;
          
            case CharacterController.LookingAt.Sud:
                rb.velocity = (new Vector2(0,-1) * rb.velocity.magnitude * 2.5f);
                break;
          
            case CharacterController.LookingAt.Est:
                rb.velocity = (new Vector2(1,0) * rb.velocity.magnitude * 2.5f);
                break;
          
            case CharacterController.LookingAt.Ouest:
                rb.velocity = (new Vector2(-1,0) * rb.velocity.magnitude * 2.5f);
                break;
          
            case CharacterController.LookingAt.NordEst:
                rb.velocity = (new Vector2(1,1) * rb.velocity.magnitude * 2.5f);
                break;
          
            case CharacterController.LookingAt.NordOuest:
                rb.velocity = (new Vector2(-1,1) * rb.velocity.magnitude * 2.5f);
                break;
          
            case CharacterController.LookingAt.SudEst:
                rb.velocity = (new Vector2(1,-1) * rb.velocity.magnitude * 2.5f);
                break;
          
            case CharacterController.LookingAt.SudOuest:
                rb.velocity = (new Vector2(-1,-1) * rb.velocity.magnitude * 2.5f);
                break;
        }
    }
}

