using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DashTracker : MonoBehaviour
{
    public float distance = 22;
    public GameObject player;
    public float diagonalMultiplier;
    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        if (CharacterController.instance is null) return;
        switch (CharacterController.instance.facing)
        {
            case CharacterController.LookingAt.Nord:
                transform.position = player.transform.position + Vector3.up * distance;
                transform.rotation = Quaternion.Euler(0,0,90);
                break;
            case CharacterController.LookingAt.Est:
                transform.position = player.transform.position + Vector3.right * distance - new Vector3(0,0.75f,0);
                transform.rotation = Quaternion.Euler(0,0,0);
                break;
            case CharacterController.LookingAt.Ouest:
                transform.position = player.transform.position + Vector3.left * distance - new Vector3(0,0.75f,0);
                transform.rotation = Quaternion.Euler(0,0,180);
                break;
            case CharacterController.LookingAt.Sud:
                transform.position = player.transform.position + new Vector3(0,-3,0) + Vector3.down * distance;
                transform.rotation = Quaternion.Euler(0,0,270);
                break;
            case CharacterController.LookingAt.NordEst:
                transform.position = player.transform.position + new Vector3(0.5f,0.5f,0) * distance * diagonalMultiplier;
                transform.rotation = Quaternion.Euler(0,0,45);
                break;
            case CharacterController.LookingAt.NordOuest:
                transform.position = player.transform.position + new Vector3(-0.5f,0.5f,0) * distance * diagonalMultiplier;
                transform.rotation = Quaternion.Euler(0,0,135);
                break;
            case CharacterController.LookingAt.SudEst:
                transform.position = player.transform.position + new Vector3(0,-3,0) + new Vector3(0.5f,-0.5f,0) * distance * diagonalMultiplier;
                transform.rotation = Quaternion.Euler(0,0,315);
                break;
            case CharacterController.LookingAt.SudOuest:
                transform.position = player.transform.position + new Vector3(0,-3,0) + new Vector3(-0.5f,-0.5f,0) * distance * diagonalMultiplier;
                transform.rotation = Quaternion.Euler(0,0,235);
                break;
        }
    }

    // Update is called once per frame
    void OnDisable()
    {
        transform.position = CharacterController.instance.transform.position;
        transform.rotation = Quaternion.Euler(0,0,0);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("MurInfranchissable"))
        {
            //Debug.Log("mais what");
            gameObject.SetActive(false);
            CharacterController.instance.playerCol.enabled = true;
            CharacterController.instance.canBuffer = false;
            return;
        }
        CharacterController.instance.playerCol.enabled = false;
        CharacterController.instance.canBuffer = true;
        gameObject.SetActive(false);
    }
}
