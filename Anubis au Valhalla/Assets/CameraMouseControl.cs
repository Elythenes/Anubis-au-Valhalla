using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseControl : MonoBehaviour
{
    public Camera camera;
    public float newY;
    public float newX;
    public float multiplierMouse;
    public InputManager controls;
    void Update()
    {
        Vector3 charaPos = CharacterController.instance.transform.position;
        Salle activeRoom = SalleGennerator.instance.currentRoom;

        Salle limites = activeRoom.GetComponent<Salle>();


        float height = camera.orthographicSize;
        float width = height * camera.aspect;


        if (limites.maxPos.y > charaPos.y + height && 
            limites.minPos.y < charaPos.y - height)
        {
            newY =  charaPos.y;
        }

        else
        {
            if (limites.maxPos.y <= charaPos.y + height)
            {
                newY = limites.maxPos.y - height;
            }
            else
            {
                newY = limites.minPos.y + height;
            }
        }


        if (limites.minPos.x < charaPos.x - width && 
            limites.maxPos.x > charaPos.x + width)
        {
            newX = charaPos.x;
        }

        else
        {
            if (limites.minPos.x >= charaPos.x - width)
            {
                newX = limites.minPos.x + width;
            }
            else
            {
                newX = limites.maxPos.x - width;
            }
        }


        Vector2 mousePos = camera.ScreenToViewportPoint(Input.mousePosition);

        Vector2 newPos = new Vector2( mousePos.x * multiplierMouse - multiplierMouse / 2,  mousePos.y * multiplierMouse- multiplierMouse / 2);

        transform.position = new Vector3(newX + newPos.x, newY + newPos.y, transform.position.z);
    }
}
