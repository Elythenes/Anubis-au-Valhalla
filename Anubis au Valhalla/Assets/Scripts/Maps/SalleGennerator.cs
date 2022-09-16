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
        public List<Salle> roomPrefab = new List<Salle>();

        public readonly Queue<Salle> roomsQueue = new Queue<Salle>();

        public Salle currentRoom;
        public int roomsDone = -1;

        public Transform transformCurrentRoom => currentRoom.transform;

        public enum Doortype
        {
                North = 0,
                East = 1,
                South = 2,
                West = 3
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
                GenerateRoom();
                TransitionToNextRoom(Doortype.West);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void GenerateRoom()
        {
                if (currentRoom != null) Destroy(currentRoom.gameObject);
                roomsDone = -1;
                var maps = new List<Salle>(roomPrefab);
                roomsQueue.Clear();

                var roomsToGenerate = 3;
                for (int i = 0; i < roomsToGenerate; i++)
                {
                        var j = Random.Range(0, maps.Count);
                        roomsQueue.Enqueue(maps[j]);
                        maps.RemoveAt(j);
                }
        }

        public void TransitionToNextRoom(Doortype door)
        {
                fromDoor = door switch
                {
                        Doortype.West => Doortype.East,
                        Doortype.East => Doortype.West,
                        Doortype.North => Doortype.South,
                        Doortype.South => Doortype.North,
                        _ => fromDoor
                };
                if(currentRoom != null) Destroy(currentRoom.gameObject);
                currentRoom = GetNextRoom();
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
                                CharacterController.instance.transform.position = new Vector3(0,4,0);
                                break;
                        case Doortype.West:
                                CharacterController.instance.transform.position = new Vector3(-17.5f,-0.5f,0);
                                break;
                        case Doortype.South:
                                CharacterController.instance.transform.position = new Vector3(0,-3.9f,0);
                                break;
                        case Doortype.East:
                                CharacterController.instance.transform.position = new Vector3(17.5f,-0.5f,0);
                                break;
                        default:
                                throw new ArgumentOutOfRangeException(nameof(doortype), doortype, null);
                }
        }
}


