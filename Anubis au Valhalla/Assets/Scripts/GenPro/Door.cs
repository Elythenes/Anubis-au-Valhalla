using System;
using System.Collections.Generic;
using GenPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    
    public SalleGenerator.DoorOrientation doorOrientation;
    public Salle roomToSpawn;
    public DoorType currentDoorType;
    public SpriteRenderer currentSprite;
    public List<Sprite> doorSprites;
    public bool willChooseSpecial;
    public BoxCollider2D doorCollider;
    public GameObject normalDoor1;
    public GameObject shopDoor1;
    public GameObject challengeDoor1;
    public GameObject normalDoor2;
    public GameObject shopDoor2;
    public GameObject challengeDoor2;
    public GameObject bossDoor;
    public GameObject broken;

    public enum DoorType
    {
        Normal,
        ToShop,
        ToChallenge1,
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
    bossDoor.SetActive(false);
    broken.SetActive(false);
    }

    private void Awake()
    {
        currentSprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        
        switch (currentDoorType)
        {
            case DoorType.Normal:
                break;
            case DoorType.ToShop:
                currentSprite.enabled = true;
                willChooseSpecial = false;
                if (SalleGenerator.Instance.zone2)
                {
                    shopDoor2.SetActive(true);
                    break;
                }
                shopDoor1.SetActive(true);
                currentSprite.sprite = doorSprites[1];
                break;
            case DoorType.ToChallenge1:
                currentSprite.sprite = doorSprites[2];
                break;
            case DoorType.ToBoss:
                bossDoor.SetActive(true);
                break;
            case DoorType.Broken:
                broken.SetActive(true);
                break;
        }

        if (willChooseSpecial && currentDoorType != DoorType.ToShop)
        {
            if (SalleGenerator.Instance.zone2)
            {
                challengeDoor2.SetActive(true);
                return;
            }
            challengeDoor1.SetActive(true);
            currentSprite.enabled = true;
            currentSprite.sprite = doorSprites[2];
        }
        else if(currentDoorType == DoorType.Normal)
        {
            currentSprite.sprite = doorSprites[0];
            if (SalleGenerator.Instance.zone2)
            {
                normalDoor2.SetActive(true);
                return;
            }
            normalDoor1.SetActive(true);
            currentSprite.enabled = false;
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
