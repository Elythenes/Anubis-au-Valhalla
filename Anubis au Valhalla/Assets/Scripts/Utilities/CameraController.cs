using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController cameraInstance;
    public Transform cameraTarget;

    public float smoothMove;
    public bool stopMove;

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
        if (transform.position != cameraTarget.position && !stopMove)
        {
            Vector3 targetPos = new Vector3(cameraTarget.position.x, cameraTarget.position.y, transform.position.z);
            targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);
            targetPos.y = Mathf.Clamp(targetPos.y, minPos.y, maxPos.y);
            transform.position = targetPos;
        }
    }

    public IEnumerator TansitionCamera(GameObject targetPos)
    {
        stopMove = true;
        Time.timeScale = 1;
        float timeElapsed = 0;
        while (timeElapsed < smoothMove)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos.transform.position, smoothMove);
            timeElapsed += 0.5f * Time.deltaTime;
            yield return null;
        }

        if (timeElapsed > smoothMove)
        {
            stopMove = false;
        }
    }
}
