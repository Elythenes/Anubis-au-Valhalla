using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
  public float speed;

  public bool isDashing;
  public bool canDash;
  public float dashSpeed;
  public float timerDash;
  public float timerDashMax;
  public float dashCooldown;
  public float dashCooldownMax;

  public Rigidbody2D rb;

  private Vector2 movement;
  private Vector2 positionActuelle;

  private void Update()
  {
    movement.x = Input.GetAxisRaw("Horizontal");
    movement.y = Input.GetAxisRaw("Vertical");

    positionActuelle = new Vector2(transform.position.x, transform.position.y);

    if (Input.GetButtonDown("Dash") && isDashing == false && canDash)
    {
      isDashing = true;
    }
    
    if (isDashing)
    {
      timerDash += Time.deltaTime;
      rb.MovePosition(positionActuelle + movement * (dashSpeed * Time.deltaTime));
      
    }
    if (timerDash > timerDashMax)
    {
      isDashing = false;
      timerDash = 0;
      canDash = false;
    }

    if (canDash == false)
    {
      dashCooldown += Time.deltaTime;
    }
    
    if (dashCooldown >= dashCooldownMax)
    {
      canDash = true;
      dashCooldown = 0;
    }
  }

      
  private void FixedUpdate()
  {
    if (isDashing == false)
    {
      rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
  }
}
