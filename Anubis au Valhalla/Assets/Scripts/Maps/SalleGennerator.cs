using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class SalleGennerator : MonoBehaviour
{
        public CharacterController player;
        public GameObject[] s_doors;
        public Doortype fromDoor = Doortype.West;
        public static SalleGennerator instance;
        
        public readonly Queue<Salle> roomsQueue = new Queue<Salle>();

        public Salle currentRoom;
        public int roomsDone = -1;

        public enum Doortype
        {
                North = 1,
                East = 2,
                South = 3,
                West = 4
        }
        private void Awake()
        {
                if (instance != null)
                {
                        DestroyImmediate(gameObject);
                        return;
                }

                instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void GenerateNextRoom(Doortype door)
        {
                fromDoor = door switch
                {
                        Doortype.West => Doortype.East,
                        Doortype.East => Doortype.West,
                        Doortype.North => Doortype.South,
                        Doortype.South => Doortype.North,
                        _ => fromDoor
                };
                MovePlayerToDoor(fromDoor);
                
        }

        public Salle GetNextRoom()
        {
                roomsDone++;
                return Instantiate(roomsQueue.Dequeue());
        }

        public void MovePlayerToDoor(Doortype doortype)
        {
                player.rb.velocity = Vector2.zero;

                switch (doortype)
                {
                        case Doortype.North:
                                CharacterController.instance.transform.position = new Vector3(0,7.75f,0);
                                break;
                        case Doortype.West:
                                CharacterController.instance.transform.position = new Vector3(-17.5f,-0.5f,0);
                                break;
                        case Doortype.South:
                                CharacterController.instance.transform.position = new Vector3(0,-8.8f,0);
                                break;
                        case Doortype.East:
                                CharacterController.instance.transform.position = new Vector3(17.5f,-0.5f,0);
                                break;
                        default:
                                throw new ArgumentOutOfRangeException(nameof(doortype), doortype, null);
                }
        }
}


