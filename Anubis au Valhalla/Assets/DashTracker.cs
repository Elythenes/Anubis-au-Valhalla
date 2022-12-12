using System;
using DG.Tweening;
using UnityEngine;

public class DashTracker : MonoBehaviour
{
    public float distance = 22;
    public CharacterController player;

    public bool boost;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (CharacterController.instance is null) return;
        switch (CharacterController.instance.facing)
        {
            case CharacterController.LookingAt.Nord:
                transform.position = player.transform.position + Vector3.up * distance;
                transform.rotation = Quaternion.Euler(0,0,90);
                break;
            case CharacterController.LookingAt.Est:
                transform.position = player.transform.position + Vector3.right * distance;
                transform.rotation = Quaternion.Euler(0,0,0);
                break;
            case CharacterController.LookingAt.Ouest:
                transform.position = player.transform.position + Vector3.left * distance;
                transform.rotation = Quaternion.Euler(0,0,180);
                break;
            case CharacterController.LookingAt.Sud:
                transform.position = player.transform.position + new Vector3(0,-3,0) + Vector3.down * distance;
                transform.rotation = Quaternion.Euler(0,0,270);
                break;
            case CharacterController.LookingAt.NordEst:
                transform.position = player.transform.position + new Vector3(0.5f,0.5f,0) * distance;
                transform.rotation = Quaternion.Euler(0,0,45);
                break;
            case CharacterController.LookingAt.NordOuest:
                transform.position = player.transform.position + new Vector3(-0.5f,0.5f,0) * distance;
                transform.rotation = Quaternion.Euler(0,0,135);
                break;
            case CharacterController.LookingAt.SudEst:
                transform.position = player.transform.position + new Vector3(0,-3,0) + new Vector3(0.5f,-0.5f,0) * distance;
                transform.rotation = Quaternion.Euler(0,0,315);
                break;
            case CharacterController.LookingAt.SudOuest:
                transform.position = player.transform.position + new Vector3(0,-3,0) + new Vector3(-0.5f,-0.5f,0) * distance;
                transform.rotation = Quaternion.Euler(0,0,235);
                break;
        }
    }

    // Update is called once per frame
    void OnDisable()
    {
        transform.position = CharacterController.instance.transform.position;
        transform.rotation = Quaternion.Euler(0,0,0);
        boost = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        boost = true;
        CharacterController.instance.playerCol.enabled = false;
        Debug.Log("CA TOUCHE");
    }

    /*private void Update()
    {
        if (boost)
        {
                    switch (CharacterController.instance.facing)
        {
            case CharacterController.LookingAt.Nord:
                transform.position = CharacterController.instance.transform.position + Vector3.up * distance;
                transform.rotation = Quaternion.Euler(0,0,90);
                break;
            case CharacterController.LookingAt.Est:
                transform.position = CharacterController.instance.transform.position + Vector3.right * distance;
                transform.rotation = Quaternion.Euler(0,0,0);
                break;
            case CharacterController.LookingAt.Ouest:
                transform.position = CharacterController.instance.transform.position + Vector3.left * distance;
                transform.rotation = Quaternion.Euler(0,0,180);
                break;
            case CharacterController.LookingAt.Sud:
                transform.position = CharacterController.instance.transform.position + new Vector3(0,-3,0) + Vector3.down * distance;
                transform.rotation = Quaternion.Euler(0,0,270);
                break;
            case CharacterController.LookingAt.NordEst:
                transform.position = CharacterController.instance.transform.position + new Vector3(0.5f,0.5f,0) * distance;
                transform.rotation = Quaternion.Euler(0,0,45);
                break;
            case CharacterController.LookingAt.NordOuest:
                transform.position = CharacterController.instance.transform.position + new Vector3(-0.5f,0.5f,0) * distance;
                transform.rotation = Quaternion.Euler(0,0,135);
                break;
            case CharacterController.LookingAt.SudEst:
                transform.position = CharacterController.instance.transform.position + new Vector3(0,-3,0) + new Vector3(0.5f,-0.5f,0) * distance;
                transform.rotation = Quaternion.Euler(0,0,315);
                break;
            case CharacterController.LookingAt.SudOuest:
                transform.position = CharacterController.instance.transform.position + new Vector3(0,-3,0) + new Vector3(-0.5f,-0.5f,0) * distance;
                transform.rotation = Quaternion.Euler(0,0,235);
                break;
        }
        }
    }*/
}
