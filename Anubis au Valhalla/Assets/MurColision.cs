using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MurColision : MonoBehaviour
{
    private void FixedUpdate()
    {
        var g = CharacterController.instance;
        if (g.canBoost && !g.isDashing)
        {
            Debug.Log(g.playerCol.enabled);
            if (!g.rb.IsTouchingLayers(128))
            {
                g.canBuffer = false;
                g.canBoost = false;
                g.playerCol.enabled = true;
                g.allowMovements = true;
                g.stopDash = false;
                g.anim.SetBool("isDashing",false);
                g.anim.SetBool("isWalking",true);
            }
        }
        if (g.canBuffer && g.timerDash > g.dashDuration * 0.3f)
        {
            if (!g.rb.IsTouchingLayers(128))
            {
                g.canBuffer = false;
                g.canBoost = false;
                g.playerCol.enabled = true;
                g.allowMovements = true;
                g.stopDash = false;
                g.anim.SetBool("isDashing",false);
                g.anim.SetBool("isWalking",true);
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

                g.playerCol.enabled = false; 
                g.Dashing();
                g.allowMovements = false;
                Debug.Log("Givin' it a little push");
                
            }
            else
            {
                g.canBuffer = false;
                g.canBoost = false;
                g.playerCol.enabled = true;
                g.allowMovements = true;
                g.stopDash = false;
            }
        }
    }
}
