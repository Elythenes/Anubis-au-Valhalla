using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class AttaquesNormales : MonoBehaviour
{
    
    private InputManager controls;
    public static AttaquesNormales instance;

    [Header("Stats Attaque 1")] 
    public GameObject hitboxC1;
    public Vector2 rangeAttaque1;
    public bool IsC1;
    public int damageC1;
    public float dureeHitbox1;
    public float StunDuration1;
    public float StunDurationTimer1;
    public float dashImpulseC1;

    [Header("Stats Attaque 2")] 
    public GameObject hitboxC2;
    public Vector2 rangeAttaque2;
    public bool IsC2;
    public int damageC2;
    public float dureeHitbox2;
    public float StunDuration2;
    public float StunDurationTimer2;
    public float dashImpulseC2;
    
    [Header("Stats Attaque 3")] 
    public GameObject hitboxC3;
    public Vector2 rangeAttaque3;
    public bool IsC3;
    public int damageC3;
    public float dureeHitbox3;
    public float StunDuration3;
    public float StunDurationTimer3;
    public float dashImpulseC3;
    
    [Header("Général")]
    public bool abandonOn;
    public bool canAttack;
    public int comboActuel;
    public float cooldownAbandonCombo;
    public float cooldownAbandonComboTimer;
    public bool buffer;

    public List<GameObject> hitBoxC = new List<GameObject>();
    public List<Vector2> rangeAttaque = new List<Vector2>();
    public List<bool> isC = new List<bool>();
    public List<int> damage = new List<int>();
    public List<float> dureeHitbox = new List<float>();
    public List<float> stunDuration = new List<float>();
    public List<float> stunDurationMax = new List<float>();
    public List<float> dashImpulse = new List<float>();
    public List<float> timeForCanDash = new List<float>();





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
                        abandonOn = false;
                        cooldownAbandonComboTimer = 0;
                        //buffer = false;
                        comboActuel++;
                        Combo1();     
                    }
                    
                    break;
                
                case 1:
                    if (canAttack)
                    {
                        abandonOn = false;
                        cooldownAbandonComboTimer = 0;
                       // buffer = false;
                        comboActuel++;
                        Combo2();
                    }
                    
                    break;
                
                case 2:
                    if (canAttack)
                    {
                        abandonOn = false;
                        cooldownAbandonComboTimer = 0;
                        //buffer = false;
                        comboActuel = 0;
                        Combo3();
                    }
                    break;
                    
            }
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
        
        
        
        // ------------------ Gestion Combo 1-------------
      
        /*if (IsC1 && !canAttack && StunDurationTimer1 <= StunDuration1 && abandonOn)
        {
            if (mouse.leftButton.wasPressedThisFrame)
            {
                buffer = true;
            }
        }*/
        for (int i = 0; i < hitBoxC.Count; i++)
        {
            if (stunDuration[i] >= stunDurationMax[i])
            {
                
            }

            if (isC[i])
            {
                
            }
        }
        if (StunDurationTimer1 >= StunDuration1)
        { 
            canAttack = true;
            CharacterController.instance.isAttacking = false;
            IsC1 = false;
            StunDurationTimer1 = 0;
        }
        
        if (IsC1)
        {
            StunDurationTimer1 += Time.deltaTime;
        }
        // ------------------ Gestion Combo 1-------------
    }

    

    public void Combo1() // Dash légèrement vers l'avant puis crée une hitbox devant le perso et touche les ennemis
    {
        abandonOn = true;
        IsC1 = true;
        canAttack = false;
        CharacterController.instance.isAttacking = true;
        Vector2 charaPos = CharacterController.instance.transform.position;
        Vector2 mousePos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
        Vector3 moveDirection = (mousePos - charaPos);
        moveDirection.z = 0;
        moveDirection.Normalize();
        CharacterController.instance.rb.AddForce(moveDirection * dashImpulseC1, ForceMode2D.Impulse);
        
        GameObject swordObj = Instantiate(hitboxC1, new Vector3(999,99,0),Quaternion.identity);
        swordObj.transform.position = CharacterController.instance.transform.position;
        swordObj.transform.localRotation = Quaternion.AngleAxis(angle,Vector3.forward);
    }
    
    public void Combo2() // La même chose mais la hitbox est plus alongée et le dash plus long et rapide
    {
        abandonOn = true;
        IsC2 = true;
        canAttack = false;
        CharacterController.instance.isAttacking = true;
        Vector2 charaPos = CharacterController.instance.transform.position;
        Vector2 mousePos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
        Vector3 moveDirection = (mousePos - charaPos);
        moveDirection.z = 0;
        moveDirection.Normalize();
        CharacterController.instance.rb.AddForce(moveDirection * dashImpulseC2, ForceMode2D.Impulse);
        
        
        GameObject swordObj = Instantiate(hitboxC2, new Vector3(999,99,0),Quaternion.identity);
        swordObj.transform.position = CharacterController.instance.transform.position;
        swordObj.transform.localRotation = Quaternion.AngleAxis(angle,Vector3.forward);
    }
    
    public void Combo3() // La même chose mais la hitbox est plus alongée et le dash plus long et rapide
    {
        abandonOn = true;
        IsC3 = true;
        canAttack = false;
        CharacterController.instance.isAttacking = true;
        Vector2 charaPos = CharacterController.instance.transform.position;
        Vector2 mousePos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
        Vector3 moveDirection = (mousePos - charaPos);
        moveDirection.z = 0;
        moveDirection.Normalize();
        CharacterController.instance.rb.AddForce(moveDirection * dashImpulseC3, ForceMode2D.Impulse);
        
        
        GameObject swordObj = Instantiate(hitboxC3, new Vector3(999,99,0),Quaternion.identity);
        swordObj.transform.position = CharacterController.instance.transform.position;
        swordObj.transform.localRotation = Quaternion.AngleAxis(angle,Vector3.forward);
    }
}

