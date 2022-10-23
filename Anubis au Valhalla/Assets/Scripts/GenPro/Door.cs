using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Door : MonoBehaviour
{
    
    public SalleGennerator.DoorOrientation doorOrientation;
    public Salle roomToSpawn;
    public DoorType currentDoorType;
    public SpriteRenderer currentSprite;
    public List<Sprite> doorSprites;

    public enum DoorType
    {
        Normal,
        ToShop,
        ToChallenge1,
        ToChallenge2,
        ToChallenge3,
        ToBoss,
    }

    private void Update()
    {
        currentSprite = gameObject.GetComponent<SpriteRenderer>();
        switch (currentDoorType)
        {
            case DoorType.Normal:
                currentSprite.sprite = doorSprites[0];
                break;
            case DoorType.ToShop:
                currentSprite.sprite = doorSprites[1];
                break;
            case DoorType.ToChallenge1:
                currentSprite.sprite = doorSprites[2];
                break;
            case DoorType.ToChallenge2:
                currentSprite.sprite = doorSprites[3];
                break;
            case DoorType.ToChallenge3:
                currentSprite.sprite = doorSprites[4];
                break;
            case DoorType.ToBoss:
                currentSprite.sprite = doorSprites[5];
                break;
        }
    }


    public void ChooseRoomToSpawn(int room)
    {
        roomToSpawn = SalleGennerator.instance.roomPrefab[room];
    }
    public void ChooseSpecialToSpawn(int room)
    {
        roomToSpawn = SalleGennerator.instance.specialRooms[room];
    }
}
