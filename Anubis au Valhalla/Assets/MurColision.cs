using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MurColision : MonoBehaviour
{
    private void Update()
    {
        var g = CharacterController.instance;
        if (g.canBoost && !g.isDashing)
        {
            if (!g.rb.IsTouchingLayers(128))
            {
                g.canBoost = false;
                g.canBuffer = false;
                g.rb.velocity *= 0.5f;
                g.QuitDash();
            }
        }
        if (g.canBuffer && g.timerDash > g.dashDuration * 0.3f)
        {
            if (!g.rb.IsTouchingLayers(128))
            {
                g.canBoost = false;
                g.canBuffer = false;
                //g.rb.velocity *= 0.5f;
                g.QuitDash();
            }
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        var g = CharacterController.instance;
        if (col.gameObject.layer == 7 && !g.isDashing && (g.canBuffer || g.canBoost))
        {
            if (g.rb.GetContacts(new List<Collider2D>()) >= 1)
            {
                g.canBoost = true;
                g.stopDash = false;

                g.playerCol.enabled = false; 
                g.Dashing();
                g.allowMovements = false;
                //Debug.Log("Givin' it a little push");
                
            }
            else
            {
                g.canBoost = false;
                g.canBuffer = false;
                g.QuitDash();
            }
        }
    }
}
