using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.VFX;

public class AttaquesNormales : MonoBehaviour
{
    public Animator anim;
    private InputManager controls;
    public static AttaquesNormales instance;

    [Header("Stats Attaques")]
    //public List<float> hitStopDuration = new List<float>();
    [NaughtyAttributes.ReadOnly] public List<GameObject> hitBoxC;
    [NaughtyAttributes.ReadOnly] public List<Vector2> rangeAttaque;
    [NaughtyAttributes.ReadOnly] public List<bool> isC;
    [NaughtyAttributes.ReadOnly] public List<int> damage;
    [NaughtyAttributes.ReadOnly] public int criticalRate;
    [NaughtyAttributes.ReadOnly] public List<float> dureeHitbox;
    [NaughtyAttributes.ReadOnly] public List<float> stunDuration;
    [NaughtyAttributes.ReadOnly] public List<float> forceKnockback;
    [NaughtyAttributes.ReadOnly] public List<float> stunDurationMax;
    [NaughtyAttributes.ReadOnly] public List<float> dashImpulse;
    [NaughtyAttributes.ReadOnly] public List<float> timeForCanDash;
    [NaughtyAttributes.ReadOnly] public List<float> dashTimers;
    [NaughtyAttributes.ReadOnly] public int specialDmg;

    [Header("Tracking valeurs pouvoirs")] 
    public bool attaque1;
    public bool attaque2;
    public bool attaque3;
    public bool attaqueSpeSpell;
    
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] audioClipArray;

    [Header("Général")] 
    public bool blockFlip;
    public bool attaqueSpe2;
    public bool abandonOn;
    public bool canAttack;
    public int comboActuel;
    public float cooldownAbandonCombo;
    public float cooldownAbandonComboTimer;
    public float offsetAtk3;
    public bool buffer;
    public bool buffer2;
    public GameObject swordObj;
    public List<VisualEffect> VFXobj;
    public GameObject VFXparent;
    public GameObject thrust;
    private float scaleYVFX1;
    private float scaleYVFX2;
    
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

        #region Attaques Normalles
        
        if (mouse.leftButton.wasPressedThisFrame && canAttack && CharacterController.instance.allowMovements&& Time.timeScale != 0) // Execute l'attaque selon l'avancement du combo
        {
            ExecuteAttack();
        }

        if (mouse.leftButton.wasPressedThisFrame && (CharacterController.instance.isDashing || CharacterController.instance.canBoost))
        {
            buffer = true;
        }

        if (mouse.rightButton.wasPressedThisFrame && (CharacterController.instance.isDashing || CharacterController.instance.canBoost))
        {
            buffer2 = true;
        }

        // ------------------ Gestion Abandon du Combo ---------------
        if (abandonOn)
        {
            cooldownAbandonComboTimer += Time.deltaTime;
        }
        
        if (cooldownAbandonComboTimer >= cooldownAbandonCombo) // Condition de retour à l'idle
        {
            blockFlip = true;
            anim.SetBool("StopCombo",true);
            abandonOn = false;
            comboActuel = 0;
            cooldownAbandonComboTimer = 0;
        }

        if (comboActuel == 1 || comboActuel == 2)
        {
            if (CharacterController.instance.isDashing)
            {
                Destroy(swordObj);
            }
        }
        
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
                anim.SetBool("isAttacking3",false);
                anim.SetBool("isAttacking1Down",false);
                anim.SetBool("isAttacking2Down",false);
                anim.SetBool("isAttacking3Down",false);
                anim.SetBool("isIdle",true);
                canAttack = true;
                CharacterController.instance.isAttacking = false;
                isC[i] = false;
                stunDuration[i] = 0;
            }
            stunDuration[i] += Time.deltaTime;
        }

        #endregion

        if (mouse.rightButton.wasPressedThisFrame && canAttack && CharacterController.instance.allowMovements && !CharacterController.instance.isDashing && Time.timeScale != 0)
        {
            SpecialAttack();
        }

        if (attaqueSpe2)
        {
            stunDuration[2] += Time.deltaTime;
            if (stunDuration[2] >= stunDurationMax[3])
            {
                anim.SetBool("isAttackingSpe",false);
                anim.SetBool("isAttackingSpeDown",false);
                anim.SetBool("isWalking",false);
                anim.SetBool("isIdle",true);
                attaqueSpe2 = false;
                stunDuration[2] = 0;
                canAttack = true;
                CharacterController.instance.isAttacking = false;
            }
        }

        // ------------------ Gestion Combo-------------
    }

    public void ExecuteAttack()
    {
        CharacterController.instance.stopDash = true;
        anim.SetBool("StopCombo",false);
        blockFlip = false;
        CharacterController.instance.canBuffer = false;
        switch (comboActuel)
        {
            case 0:

                if (canAttack)
                {
                    attaque1 = true;
                    StartCoroutine(ResetTracking());
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
                    attaque2 = true;
                    StartCoroutine(ResetTracking());
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
                    attaque3 = true;
                    StartCoroutine(ResetTracking());
                    abandonOn = false;
                    cooldownAbandonComboTimer = 0;
                    //buffer = false;
                    comboActuel = 0;
                    Combo(2);
                }

                break;
        }
    }

    //<Combo 1>/ Glisse vers l'avant puis crée une hitbox devant le perso et touche les ennemis
    //<Combo 2>/ La même chose mais dash, et la hitbox est plus alongée et le dash plus long et rapide
    //<Combo 3>/ La même chose mais hitbox est plus alongée et le dash plus long et rapide
    public void Combo(int index) 
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(audioClipArray[index]);
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
        if (!CharacterController.instance.isDashing)
        {
            if (index == 0)
            {
                CharacterController.instance.rb.AddForce(moveDirection * dashImpulse[0], ForceMode2D.Force);
           
            }
            if (index == 1)
            {
                CharacterController.instance.rb.AddForce(moveDirection * dashImpulse[1], ForceMode2D.Force);
            }
            if (index == 2)
            {
                CharacterController.instance.rb.AddForce(moveDirection * dashImpulse[2], ForceMode2D.Force);
            }
        }
       

        #region Gestion des animations

        if (angle > 67.5f && angle < 112.5f) // Nord
        {
            if (CharacterController.instance.movement == Vector2.zero && !Input.GetKeyDown(KeyCode.S))
            {
                CharacterController.instance.facing = CharacterController.LookingAt.Nord;
            }
        }

        if (angle > -22.5f && angle < 22.5f) // Est
        {
            if (CharacterController.instance.movement == Vector2.zero && !Input.GetKeyDown(KeyCode.Q))
            {
                CharacterController.instance.facing = CharacterController.LookingAt.Est;
            }
        }


        if (angle > 157.5f && angle < 180 || angle > -180f && angle < -157.5f) // Ouest
        {
            if (CharacterController.instance.movement == Vector2.zero && !Input.GetKeyDown(KeyCode.D))    
            {
                CharacterController.instance.facing = CharacterController.LookingAt.Ouest;
            } 
        }

        if (angle > -112.5f && angle < -67.5f) // Sud
        {
            if (CharacterController.instance.movement == Vector2.zero && !Input.GetKeyDown(KeyCode.Z))       
            {
                CharacterController.instance.facing = CharacterController.LookingAt.Sud;
            }      
        }
        
        if (angle > 22.5f && angle < 67.5f) // Nord Est
        {
            if (CharacterController.instance.movement == Vector2.zero && !Input.GetKeyDown(KeyCode.S) && !Input.GetKeyDown(KeyCode.D))       
            {
                CharacterController.instance.facing = CharacterController.LookingAt.NordEst;
            }      
        }
        
        if (angle > 112.5f && angle < 157.5f) // Nord Ouest
        {
            if (CharacterController.instance.movement == Vector2.zero && !Input.GetKeyDown(KeyCode.S) && !Input.GetKeyDown(KeyCode.Q))       
            {
                CharacterController.instance.facing = CharacterController.LookingAt.NordOuest;
            }      
        }
        
        if (angle > -67.5f && angle < -22.5f) // Sud Est
        {
            if (CharacterController.instance.movement == Vector2.zero && !Input.GetKeyDown(KeyCode.S) && !Input.GetKeyDown(KeyCode.Z) && !Input.GetKeyDown(KeyCode.Q))       
            {
                CharacterController.instance.facing = CharacterController.LookingAt.SudEst;
            }      
        }
        
        if (angle > -157.5f && angle < -112.5f) // Sud Ouest
        {
            if (CharacterController.instance.movement == Vector2.zero && !Input.GetKeyDown(KeyCode.S) && !Input.GetKeyDown(KeyCode.Z) && !Input.GetKeyDown(KeyCode.D))       
            {
                CharacterController.instance.facing = CharacterController.LookingAt.SudOuest;
            }      
        }

        
        if (angle > 45 && angle < 135) // attaque vers le haut
        {
            // SetBool les animation vers le haut
        }
        else if (angle < 45 && angle > -45 /*droite*/|| angle > 135 && angle < 180 /*gauche-haut*/|| angle > -180 && angle < -135/*gauche-bas*/) // attaque à gauche ou à droite
        {
            if (index == 0)
            {
                anim.SetBool("isAttacking1",true);
                anim.SetBool("isAttacking2",false);
                anim.SetBool("isAttacking3",false);
                anim.SetBool("isIdle",false);
                anim.SetBool("isWalking",false);
            }
            else if(index == 1)
            {
                anim.SetBool("isAttacking1",false);
                anim.SetBool("isAttacking2",true);
                anim.SetBool("isAttacking3",false);
                anim.SetBool("isIdle",false);
                anim.SetBool("isWalking",false);
            }
            else if(index == 2)
            {
                anim.SetBool("isAttacking1",false);
                anim.SetBool("isAttacking2",false);
                anim.SetBool("isAttacking3",true);
                anim.SetBool("isIdle",false);
                anim.SetBool("isWalking",false);
            }
            
        }
        else if (angle > -135 && angle < -45) // attaque vers le bas
        {
            if (index == 0)
            {
                anim.SetBool("isAttacking1Down",true);
                anim.SetBool("isAttacking2Down",false);
                anim.SetBool("isAttacking3Down",false);
                anim.SetBool("isIdle",false);
                anim.SetBool("isWalking",false);
            }
            else if(index == 1)
            {
                anim.SetBool("isAttacking1Down",false);
                anim.SetBool("isAttacking2Down",true);
                anim.SetBool("isAttacking3Down",false);
                anim.SetBool("isIdle",false);
                anim.SetBool("isWalking",false);
            }
            else if(index == 2)
            {
                anim.SetBool("isAttacking1Down",false);
                anim.SetBool("isAttacking2Down",false);
                anim.SetBool("isAttacking3Down",true);
                anim.SetBool("isIdle",false);
                anim.SetBool("isWalking",false);
            }
        }

        #endregion

        if (moveDirection.x > 0)
        {
            CharacterController.instance.transform.localRotation = Quaternion.Euler(CharacterController.instance.transform.localRotation.x,0,CharacterController.instance.transform.localRotation.z);
        }
        else
        {
            CharacterController.instance.transform.localRotation = Quaternion.Euler(CharacterController.instance.transform.localRotation.x,-180,CharacterController.instance.transform.localRotation.z);
        }

        swordObj = Instantiate(hitBoxC[index], new Vector3(999,99,0),Quaternion.identity);
        swordObj.transform.position = CharacterController.instance.transform.position;
        if (index == 0)
        {
            swordObj.transform.localRotation = Quaternion.AngleAxis(angle,Vector3.forward);
            VFXobj[index].gameObject.SetActive(true);
            VFXobj[index].Play();
            VFXobj[index].transform.localScale = rangeAttaque[0]*3;
            VFXparent.transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);
            if (angle < 90 && angle > -90) // vers la droite
            {
              
              scaleYVFX1 = VFXobj[0].transform.localScale.y;
              if (scaleYVFX1 < 0)
              {
                  VFXobj[0].transform.localScale = new Vector3(VFXobj[0].transform.localScale.x , -scaleYVFX1, VFXobj[0].transform.localScale.z );
                 
              }
              scaleYVFX1 = VFXobj[0].transform.localScale.y;      
            }
            else  if (angle > 90 && angle < 180 || angle < -90 && angle > -180) // vers la gauche
            {
                scaleYVFX1 = VFXobj[0].transform.localScale.y;
                if (scaleYVFX1 > 0)
                {
                    VFXobj[0].transform.localScale = new Vector3(VFXobj[0].transform.localScale.x , -scaleYVFX1, VFXobj[0].transform.localScale.z );
                 
                }
                scaleYVFX1 = VFXobj[0].transform.localScale.y;      
            }
        }
        
        if (index == 1)
        {
            swordObj.transform.localRotation = Quaternion.AngleAxis(angle,Vector3.forward);
            VFXobj[index].gameObject.SetActive(true);
            VFXobj[index].Play();
            VFXobj[index].transform.localScale = rangeAttaque[1]*3;
            VFXparent.transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);
            if (angle < 90 && angle > -90) // vers la droite
            {
                scaleYVFX2 = VFXobj[1].transform.localScale.y;
                if (scaleYVFX2 < 0)
                {
                    VFXobj[1].transform.localScale = new Vector3(VFXobj[1].transform.localScale.x , -scaleYVFX2, VFXobj[1].transform.localScale.z );
                 
                }
                scaleYVFX2 = VFXobj[1].transform.localScale.y;      
            }
            else  if (angle > 90 && angle < 180 || angle < -90 && angle > -180) // vers la gauche
            {
                scaleYVFX2 = VFXobj[1].transform.localScale.y;
                if (scaleYVFX2 > 0)
                {
                    VFXobj[1].transform.localScale = new Vector3(VFXobj[1].transform.localScale.x , -scaleYVFX2, VFXobj[1].transform.localScale.z );
                 
                }
                scaleYVFX2 = VFXobj[1].transform.localScale.y;      
            }
        }

        if (index == 2)
        {
            swordObj.transform.position = transform.position + (moveDirection * offsetAtk3);
        }
    }

    public void SpecialAttack()
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(audioClipArray[3]);
        Vector2 charaPos = CharacterController.instance.transform.position;
        Vector2 mousePos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle2 = Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
        Vector3 moveDirection2 = (mousePos - charaPos);
        moveDirection2.z = 0;
        moveDirection2.Normalize();
        
        if (angle2 > 45 && angle2 < 135) // attaque vers le haut
        {
            if (CharacterController.instance.movement == Vector2.zero)
            {
                Debug.Log("attaqueSpéNorth");
                CharacterController.instance.facing = CharacterController.LookingAt.Nord;
            }
        }
        else if (angle2 < 45 && angle2 > -45 || angle2 > 135 && angle2 < 180 || angle2 > -180 && angle2 < -135) // attaque à gauche ou à droite
        {
            if (angle2 < 45 && angle2 > -45) // gauche = dash vers la gauche
            {
                if (CharacterController.instance.movement == Vector2.zero)               
                {
                    CharacterController.instance.facing = CharacterController.LookingAt.Est;
                }
            }
            else if (angle2 > 135 && angle2 < 180  || angle2 > -180 && angle2 < -135) // droite = dash vers la droite
            {
                if (CharacterController.instance.movement == Vector2.zero)         
                {
                    CharacterController.instance.facing = CharacterController.LookingAt.Ouest;
                }    
            }
            
            anim.SetBool("isAttackingSpe",true);
            anim.SetBool("isAttacking1",false);
            anim.SetBool("isAttacking2",false);
            anim.SetBool("isAttacking3",false);
            anim.SetBool("isIdle",false);
            anim.SetBool("isWalking",false);
        }
        else if (angle2 > -135 && angle2 < -45) // attaque vers le bas
        {
            if (CharacterController.instance.movement == Vector2.zero)       
            {
                CharacterController.instance.facing = CharacterController.LookingAt.Sud;
            }      
            anim.SetBool("isAttackingSpeDown",true);
            anim.SetBool("isAttacking1Down",false);
            anim.SetBool("isAttacking2Down",false);
            anim.SetBool("isAttacking3Down",false);
            anim.SetBool("isIdle",false);
            anim.SetBool("isWalking",false);
        }
    
       
        if (moveDirection2.x > 0)
        {
            CharacterController.instance.transform.localRotation = Quaternion.Euler(CharacterController.instance.transform.localRotation.x,0,CharacterController.instance.transform.localRotation.z);
        }
        else
        {
            CharacterController.instance.transform.localRotation = Quaternion.Euler(CharacterController.instance.transform.localRotation.x,-180,CharacterController.instance.transform.localRotation.z);
        }
        attaqueSpeSpell = true;
        attaqueSpe2 = true;
        StartCoroutine(ResetTracking());
        comboActuel = 0;
        canAttack = false;
        stunDuration[2] = 0;
        CharacterController.instance.isAttacking = true;
        CharacterController.instance.rb.AddForce(moveDirection2 * dashImpulse[3], ForceMode2D.Impulse);
        GameObject thrustObj = Instantiate(thrust, charaPos, Quaternion.identity);
        VFXobj[2].gameObject.SetActive(true);
        VFXobj[2].Play();
        thrustObj.transform.rotation = Quaternion.Euler(0,0,angle2);
        VFXobj[2].transform.rotation = Quaternion.Euler(0,0,angle2);
       /* if (angle2 < 45 && angle2 > -45) // gauche
        {
            thrustObj.transform.rotation = Quaternion.Euler(0,0,0);
            VFXobj[2].transform.rotation = Quaternion.Euler(0,0,0);
        }
        else if(angle2 > 135 && angle2 < 180 || angle2 > -180 && angle2 < -135) // droite
        {
            thrustObj.transform.rotation = Quaternion.Euler(0,0,180);
            VFXobj[2].transform.rotation = Quaternion.Euler(0,0,180);
        }
        else if (angle2 > 45 && angle2 < 135) // haut
        {
            thrustObj.transform.rotation = Quaternion.Euler(0,0,90);
            VFXobj[2].transform.rotation = Quaternion.Euler(0,0,90);
        }
        else if (angle2 > -135 && angle2 < -45) // bas
        {
            thrustObj.transform.rotation = Quaternion.Euler(0,0,-90);
            VFXobj[2].transform.rotation = Quaternion.Euler(0,0,-90);
        }*/
    }

    /*IEnumerator ResetState()
    {
        yield return new WaitForSeconds(stunDurationMax[3]);
        anim.SetBool("isAttackingSpe",false);
        anim.SetBool("isAttackingSpeDown",false);
        anim.SetBool("isWalking",false);
        anim.SetBool("isIdle",true);
        attaqueSpe2 = false;
        canAttack = true;
        CharacterController.instance.isAttacking = false;
    }*/
    
    public void Slide(CharacterController.LookingAt dir)
    {
        var rb = CharacterController.instance.rb;
        switch (dir)
        {
            case CharacterController.LookingAt.Nord:
                rb.velocity = (new Vector2(0,1) * (rb.velocity.magnitude * 2.5f));
                break;
          
            case CharacterController.LookingAt.Sud:
                rb.velocity = (new Vector2(0,-1) * (rb.velocity.magnitude * 2.5f));
                break;
          
            case CharacterController.LookingAt.Est:
                rb.velocity = (new Vector2(1,0) * (rb.velocity.magnitude * 2.5f));
                break;
          
            case CharacterController.LookingAt.Ouest:
                rb.velocity = (new Vector2(-1,0) * (rb.velocity.magnitude * 2.5f));
                break;
          
            case CharacterController.LookingAt.NordEst:
                rb.velocity = (new Vector2(1,1) * (rb.velocity.magnitude * 2.5f));
                break;
          
            case CharacterController.LookingAt.NordOuest:
                rb.velocity = (new Vector2(-1,1) * (rb.velocity.magnitude * 2.5f));
                break;
          
            case CharacterController.LookingAt.SudEst:
                rb.velocity = (new Vector2(1,-1) * (rb.velocity.magnitude * 2.5f));
                break;
          
            case CharacterController.LookingAt.SudOuest:
                rb.velocity = (new Vector2(-1,-1) * (rb.velocity.magnitude * 2.5f));
                break;
        }
    }

    IEnumerator ResetTracking()
    {
        yield return null;
        attaque1 = false;
        attaque2 = false;
        attaque3 = false;
        attaqueSpeSpell = false;
    }
}

