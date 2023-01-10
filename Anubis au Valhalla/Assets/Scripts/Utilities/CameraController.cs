using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController cameraInstance;
    public Transform cameraTarget;

    public float smoothMove;

    public Vector2 minPos;
    public Vector2 maxPos;
    // Start is called before the first frame update
    void Awake()
    {
        if (cameraInstance != null)
        {
           
            return;
        }

        cameraInstance = this;
    }

    private void Start()
    {
        cameraTarget = CharacterController.instance.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position != cameraTarget.position)
        {
            Vector3 targetPos = new Vector3(cameraTarget.position.x, cameraTarget.position.y, transform.position.z);

            targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);
            targetPos.y = Mathf.Clamp(targetPos.y, minPos.y, maxPos.y);

            transform.position = Vector3.Lerp(transform.position, targetPos, smoothMove);
        }
    }
}
