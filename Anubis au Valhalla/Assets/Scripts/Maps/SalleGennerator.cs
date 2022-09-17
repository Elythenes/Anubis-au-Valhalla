using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class SalleGennerator : MonoBehaviour
{
        [Header("CETTE VARIABLE MODIFIE LA TAILLE DU DONJON QU'ON VEUX GENERER")]
        public int dungeonSize;
        
        [Header("REFERENCES")]
        public CharacterController player;
        public static SalleGennerator instance;
        public GameObject[] s_doors;
        
        [Header("CONTENU DU DONJON")]
        public List<Salle> roomPrefab = new List<Salle>();


 
        [Header("VARIABLES INTERNES POUR DEBUG")]
        [SerializeField] private int roomsDone = -1;
        [SerializeField] private Salle startRoom;
        [SerializeField] private Doortype fromDoor = Doortype.West;
        [SerializeField] private Salle currentRoom;
        private readonly Queue<Salle> roomsQueue = new Queue<Salle>();

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
                if(!currentRoom.roomDone) return;

                for (int i = 0; i < (int)Doortype.West + 1; i++)
                {
                        if(fromDoor == (Doortype) i ) continue;
                        
                        OpenDoors((Doortype)i,true);
                }
        }

        public void GenerateRoom()
        {
                if (currentRoom != null) Destroy(currentRoom.gameObject);
                roomsDone = -1;
                var maps = new List<Salle>(roomPrefab);
                roomsQueue.Clear();
                
                for (int i = 0; i < dungeonSize; i++)
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
                if(roomsDone != 0)MovePlayerToDoor(fromDoor);

        }

        public Salle GetNextRoom()
        {
                roomsDone++;
                if (roomsDone == 0)
                {
                        EnableDoors(Doortype.East,true);
                        OpenDoors(Doortype.East, true);
                        return Instantiate(startRoom);
                }
                for (int i = 0; i < (int)Doortype.West + 1; i++)
                {
                        EnableDoors((Doortype) i,true);
                }
                return Instantiate(roomsQueue.Dequeue());
                
        }

        public void MovePlayerToDoor(Doortype doortype)
        {
                player.rb.velocity = Vector2.zero;

                switch (doortype)
                {
                        case Doortype.North:
                                CharacterController.instance.transform.position = new Vector3(s_doors[(int) Doortype.North].transform.position.x,s_doors[(int) Doortype.North].transform.position.y -1,s_doors[(int) Doortype.North].transform.position.z);
                                break;
                        case Doortype.West:
                                CharacterController.instance.transform.position = new Vector3(s_doors[(int) Doortype.West].transform.position.x +1,s_doors[(int) Doortype.West].transform.position.y,s_doors[(int) Doortype.West].transform.position.z);
                                break;
                        case Doortype.South:
                                CharacterController.instance.transform.position = new Vector3(s_doors[(int) Doortype.South].transform.position.x,s_doors[(int) Doortype.South].transform.position.y +1,s_doors[(int) Doortype.South].transform.position.z);
                                break;
                        case Doortype.East:
                                CharacterController.instance.transform.position = new Vector3(s_doors[(int) Doortype.East].transform.position.x-1,s_doors[(int) Doortype.East].transform.position.y,s_doors[(int) Doortype.East].transform.position.z);
                                break;
                        default:
                                throw new ArgumentOutOfRangeException(nameof(doortype), doortype, null);
                }
        }

        public void EnableDoors(Doortype index, bool state)
        {
                s_doors[(int) index].SetActive(state);
                OpenDoors(index,false);
        }

        public void OpenDoors(Doortype index, bool state)
        {
                s_doors[(int)index].GetComponent<BoxCollider2D>().enabled = state;
        }
}


