using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Door : MonoBehaviour
{
    
    public SalleGennerator.Doortype doortype;
    public Salle roomToSpawn;


    public void ChooseRoomToSpawn(int room)
    {
        //Debug.Log("ca va spawn"+SalleGennerator.instance.roomPrefab[room]);
        roomToSpawn = SalleGennerator.instance.roomPrefab[room];
    }
}
