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
    public Vector2 oui;
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

        if (transform.position == cameraTarget.position)
        {
            StopAllCoroutines();
        }
    }

   
    public IEnumerator TansitionCamera(GameObject targetPos)
    {
        //Vector2 oui = Vector2.zero;
        cameraTarget = targetPos.transform;
        stopMove = true;
        float timeToGo = 0;
        while (timeToGo < smoothMove)
        {
            
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(7.75f, 8, timeToGo / smoothMove);
            oui = new Vector2(Mathf.Lerp(transform.position.x,cameraTarget.transform.position.x,timeToGo/ smoothMove),Mathf.Lerp(transform.position.y,cameraTarget.transform.position.y,timeToGo/ smoothMove));
            transform.position = new Vector3(oui.x,oui.y,-10);
            timeToGo += Time.deltaTime;
            yield return null;
        }
        Debug.Log("ouio");
        stopMove = false;
        transform.position = new Vector3(oui.x,oui.y,-10);
    }
}
