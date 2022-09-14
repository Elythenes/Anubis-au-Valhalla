using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
  public float speed;
  public float dashSpeed;

  public Rigidbody2D rb;

  private Vector2 movement;

  private void Update()
  {
    movement.x = Input.GetAxisRaw("Horizontal");
    movement.y = Input.GetAxisRaw("Vertical");

    if (Input.GetButtonDown("Dash"))
    {
      Debug.Log("oui");
      rb.MovePosition(rb.position + movement * dashSpeed * Time.fixedDeltaTime);
    }
  }

  private void FixedUpdate()
  {
    rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
  }
}
