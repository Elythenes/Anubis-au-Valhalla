using System;
using System.Collections.Generic;
using GenPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    
    public SalleGenerator.DoorOrientation doorOrientation;
    public Salle roomToSpawn;
    public DoorType currentDoorType;
    public bool willChooseSpecial;
    public BoxCollider2D doorCollider;
    public GameObject normalDoor1;
    public GameObject shopDoor1;
    public GameObject challengeDoor1;
    public GameObject normalDoor2;
    public GameObject shopDoor2;
    public GameObject challengeDoor2;
    public GameObject transitionDoor;
    public GameObject bossDoor1;
    public GameObject bossDoor2;
    public GameObject broken;
    public GameObject broken2;

    public enum DoorType
    {
        Normal,
        ToShop,
        ToChallenge1,
        Transition,
        ToBoss,
        Broken
    }

    public void ResetDoorState()
    {
    normalDoor1.SetActive(false);
    shopDoor1.SetActive(false);
    challengeDoor1.SetActive(false);
    normalDoor2.SetActive(false);
    shopDoor2.SetActive(false);
    challengeDoor2.SetActive(false);
    bossDoor1.SetActive(false);
    bossDoor2.SetActive(false);
    broken.SetActive(false);
    broken2.SetActive(false);
    transitionDoor.SetActive(false);
    }

    public void OnEnable()
    {
        ResetDoorState();
    }


    private void Update()
    {
        if (currentDoorType != DoorType.ToChallenge1) willChooseSpecial = false;
        switch (currentDoorType)
        {
            case DoorType.Normal:
                if (SalleGenerator.Instance.zone2)
                {
                    normalDoor2.SetActive(true);
                    break;
                }
                normalDoor1.SetActive(true);
                break;
            case DoorType.ToShop:
                willChooseSpecial = false;
                if (SalleGenerator.Instance.zone2)
                {
                    shopDoor2.SetActive(true);
                    break;
                }
                shopDoor1.SetActive(true);
                break;
            case DoorType.ToChallenge1:
                willChooseSpecial = true;
                if (SalleGenerator.Instance.zone2)
                {
                    challengeDoor2.SetActive(true);
                    return;
                }
                challengeDoor1.SetActive(true);
                break;
            case DoorType.ToBoss:
                if (SalleGenerator.Instance.zone2)
                {
                    bossDoor2.SetActive(true);
                    break;
                }
                transitionDoor.SetActive(true);
                break;
            case DoorType.Broken:
                if (SalleGenerator.Instance.zone2)
                {
                    broken2.SetActive(true);
                    //normalDoor2.SetActive(false);
                    break;
                }
                broken.SetActive(true);
                break;
            case DoorType.Transition:
                transitionDoor.SetActive(true);
                break;
        }
    }

    public void ChooseRoomToSpawn(int room)
    {
        roomToSpawn = SalleGenerator.Instance.roomPrefab[room];
    }
    public void ChooseSpecialToSpawn(int room)
    {
        roomToSpawn = SalleGenerator.Instance.specialRooms[room];
    }
}
