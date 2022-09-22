using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    
    public SalleGennerator.Doortype doortype;



    public Salle ChooseRoomToSpawn(int room)
    {
        Debug.Log("ca va spawn"+SalleGennerator.instance.roomPrefab[room]);
        return Instantiate(SalleGennerator.instance.roomPrefab[room]);
    }
}
