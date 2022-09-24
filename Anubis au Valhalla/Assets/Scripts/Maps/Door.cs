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
        roomToSpawn = SalleGennerator.instance.roomPrefab[room];
    }
    public void ChooseSpecialToSpawn(int room)
    {
        roomToSpawn = SalleGennerator.instance.specialRooms[room];
    }
}
