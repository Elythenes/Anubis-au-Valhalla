using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
  [Header("Déplacements")]
  public InputManager controls;
  private CharacterController instance;
  public float speedX;
  public float speedY;
  
  [Header("Dash")]
  public float dashSpeed;
  private float timerDash;
  public float dashDuration;
  private float timerdashCooldown;
  public float dashCooldown;
  public bool isDashing;
  public bool canDash;
  public GhostDash ghost;
  
  private Rigidbody2D rb;
  private Vector2 movement;



  private void Awake()
  {

    if (instance == null)
    {
      instance = this;
    }

    rb = gameObject.GetComponent<Rigidbody2D>();
    
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
    Keyboard kb = InputSystem.GetDevice<Keyboard>();
    
    if (isDashing == false)
    {
       movement = controls.Player.Movement.ReadValue<Vector2>();
     // movement.x = Input.GetAxisRaw("Horizontal");
     // movement.y = Input.GetAxisRaw("Vertical");
    }
  
    if (isDashing == false) 
    {
      rb.velocity = new Vector2(movement.x * speedX, movement.y * speedY);
    }

    if (kb.spaceKey.wasPressedThisFrame && isDashing == false && canDash)
    {
      ghost.activerEffet = true;
      isDashing = true;
    }
    
    if (isDashing) // Déplacement lors du dash
    {
      timerDash += Time.deltaTime;
      if (movement.x == 0 && movement.y == 0)
      {
        rb.velocity = (new Vector2(1,0) * dashSpeed);
      }
      else
      {
        rb.velocity = (movement * dashSpeed);
      }

      if (rb.velocity.x > 0) // Le personnage fait volte face.
      {
        float face = transform.localScale.x;
        face = -1;
      }
      else
      {
        float face = transform.localScale.x;
        face = 1;
      }

    }
    if (timerDash > dashDuration)
    {
      ghost.activerEffet = false;
      rb.velocity = (Vector2.zero);
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
  
}
