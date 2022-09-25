using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Salle : MonoBehaviour
{
    public bool roomDone = false;
    public Transform[] transformReferences;
    public bool[] enableDoors;
    public Vector2 minPos = Vector2.zero;
    public Vector2 maxPos = Vector2.zero;


    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(ExampleRoomCleared());
        RearrangeDoors();
        AdjustCameraConstraints();
    }
    public IEnumerator ExampleRoomCleared()
    {
        yield return new WaitForSeconds(1);
        roomDone = true;
    }

    public void RearrangeDoors()
    {
        for (int i = 0; i < (int)SalleGennerator.DoorOrientation.West; i++)
        {
            SalleGennerator.instance.s_doors[i].transform.position = transformReferences[i].position;
            //SalleGennerator.instance.s_doors[i].SetActive(enableDoors[i]);
        }
    }

    public void AdjustCameraConstraints()
    {
        var cam = CameraController.cameraInstance;
        cam.minPos = minPos;
        cam.maxPos = maxPos;
    }

}
