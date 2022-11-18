using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
  [Header("Déplacements")]
  public Animator anim;
  public InputManager controls;
  public static CharacterController instance; //jai besion de l'instance pour bouger le joueur au changements de salles
  public float speedX;
  public float speedY;
  public bool isAttacking;
  public LookingAt facing;

  public enum LookingAt { Nord,NordEst,Est,SudEst,Sud,SudOuest,Ouest,NordOuest }
  
  [Header("Dash")]
  public float dashSpeed;
  private float timerDash;
  public float dashDuration;
  private float timerdashCooldown;
  public float dashCooldown;
  public bool isDashing;
  public bool canDash;
  public GhostDash ghost;
  
  [HideInInspector]public Rigidbody2D rb; // ca aussi
  private Vector2 movement;
  public float astarPathTimer = 0f;
  public float astarPathTimerMax = 1f;

  [Header("Utilitaires")] 
  public KeyCode interaction;
  public GameObject indicationDirection;


  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }

    rb = gameObject.GetComponent<Rigidbody2D>();
    controls = new InputManager();
    PivotTo(transform.position);
  }

  private void OnEnable()
  {
    controls.Enable();
  }
  private void OnDisable()
  {
    controls.Disable();
  }
  
  public void PivotTo(Vector3 position)
  {
    Vector3 offset = transform.position - position;
    foreach (Transform child in transform)
      child.transform.position += offset;
    transform.position = position;
  }
  
  private void Update()
  {
    Keyboard kb = InputSystem.GetDevice<Keyboard>();

    Vector2 directionIndic = Camera.main.ScreenToWorldPoint(Input.mousePosition) - indicationDirection.transform.position;
    float angleIndic = Mathf.Atan2(directionIndic.y, directionIndic.x) * Mathf.Rad2Deg;
    Quaternion rotationIndic = Quaternion.AngleAxis(angleIndic, Vector3.forward);
    indicationDirection.transform.rotation = rotationIndic;
    
    
    if (isDashing == false)
    {
      if (DamageManager.instance.stun == false)
      {
        movement = controls.Player.Movement.ReadValue<Vector2>(); // Read les input de déplacement 
      }

      if (movement.x != 0 || movement.y != 0)
      {
        anim.SetBool("isIdle", false);
        anim.SetBool("isWalking", true);
      }
      else if (movement == Vector2.zero)
      {
        anim.SetBool("isIdle", true);
        anim.SetBool("isWalking", false);
      }
    }

    if (isDashing == false && !isAttacking) // Déplacments hors dash.
    {
      rb.AddForce(new Vector2(movement.x * speedX, movement.y * speedY));
      //rb.velocity = new Vector2(movement.x * speedX, movement.y * speedY);
    }

    if (kb.spaceKey.wasPressedThisFrame && isDashing == false && canDash)
    {
      ghost.lastPlayerPos = transform.position;
      AttaquesNormales.instance.canAttack = false;
      ghost.enabled = true;
      isDashing = true;
    }
    
    if (isDashing && !isAttacking) // Déplacement lors du dash selon la direction du regard du perso
    { 
      Dashing();
    }
    Flip();

    if (timerDash > dashDuration) // A la fin du dash...
    {
      rb.velocity *= 0.5f;
      AttaquesNormales.instance.canAttack = true;
      isDashing = false;
      timerDash = 0;
      canDash = false;
    }

    if (canDash == false)
    {
      timerdashCooldown += Time.deltaTime;
    }
    
    if (timerdashCooldown >= dashCooldown) // Cooldown dash
    {
      canDash = true;
      timerdashCooldown = 0;
    }
  }

  void Dashing()
  {
    timerDash += Time.deltaTime;
    if (movement.x != 0 && movement.y != 0)
    {
      rb.AddForce(new Vector2(movement.x,movement.y) * dashSpeed * 2);
    }
    else
    {
      switch (facing)
      {
        case LookingAt.Nord:
          rb.velocity = (new Vector2(0,1) * dashSpeed);
          break;
          
        case LookingAt.Sud:
          rb.velocity = (new Vector2(0,-1) * dashSpeed);
          break;
          
        case LookingAt.Est:
          rb.velocity = (new Vector2(1,0) * dashSpeed);
          break;
          
        case LookingAt.Ouest:
          rb.velocity = (new Vector2(-1,0) * dashSpeed);
          break;
          
        case LookingAt.NordEst:
          rb.velocity = (new Vector2(1,1) * dashSpeed);
          break;
          
        case LookingAt.NordOuest:
          rb.velocity = (new Vector2(-1,1) * dashSpeed);
          break;
          
        case LookingAt.SudEst:
          rb.velocity = (new Vector2(1,-1) * dashSpeed);
          break;
          
        case LookingAt.SudOuest:
          rb.velocity = (new Vector2(-1,-1) * dashSpeed);
          break;
      }
    }
  }

  void Flip()
  {
    if (movement.x > 0 && !isAttacking) // Le personnage s'oriente vers la direction où il marche. 
    {
      facing = LookingAt.Est;
      transform.localRotation = Quaternion.Euler(0, 0,0);
      //transform.localScale = new Vector3(1, 1, 0);
    }

    if (movement.x < 0 && !isAttacking)
    {
      facing = LookingAt.Ouest;
      transform.localRotation = Quaternion.Euler(0, 180,0);
      //transform.localScale = new Vector3(-1, 1, 0);
    }
    
    if (movement.y < 0 && !isAttacking)
    {
      facing = LookingAt.Sud;
      float face = transform.localScale.x;
      face = 1;
    }
    
    if (movement.y > 0 && !isAttacking)
    {
      facing = LookingAt.Nord;
      float face = transform.localScale.x;
      face = 1;
    }
  }
  
  // ---TRUC POUR GENERER LA PROCHAINE SALLE---

  private void OnTriggerEnter2D(Collider2D col)
  {
    if (col.gameObject.CompareTag("Door"))
    {
      ghost.activerEffet = false;
      isDashing = false;
      canDash = true;
      timerdashCooldown = 0;
      var hitDoor = col.GetComponent<Door>();
      SalleGennerator.instance.spawnDoor = col.gameObject.GetComponent<Door>().doorOrientation;
      if (hitDoor.currentDoorType == Door.DoorType.ToShop)
      {
        SalleGennerator.instance.shopsVisited++;
        SalleGennerator.instance.TransitionToNextRoom(col.gameObject.GetComponent<Door>().doorOrientation, true, hitDoor);
      }
      else if (hitDoor.currentDoorType != Door.DoorType.Normal)
      {
        SalleGennerator.instance.TransitionToNextRoom(col.gameObject.GetComponent<Door>().doorOrientation, true, hitDoor);
      }
      else
      {
        SalleGennerator.instance.TransitionToNextRoom(col.gameObject.GetComponent<Door>().doorOrientation, false, hitDoor);
      }
    }
  }
}
