using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
/*
TRUCS A FAIRE POUR LA GEN PRO:
        INCLURE UNE TRANSITION VERS UN ECRAN DE VICTOIRE QUAND IL N'Y A PLUS DE SALLES A GENERER
*/
public class SalleGennerator : MonoBehaviour
{
        [Header("CETTE VARIABLE MODIFIE LA TAILLE DU DONJON QU'ON VEUX GENERER")]
        public int dungeonSize;
        
        [Header("REFERENCES")]
        public CharacterController player;
        public static SalleGennerator instance;
        public List<Door> s_doors;
        [SerializeField] private Salle startRoom;
        [SerializeField] private Salle EndRoom;

        [Header("CONTENU DU DONJON")]
        public List<Salle> roomPrefab = new List<Salle>();
        public List<Salle> specialRooms;
        [Header("PATTERNES")]
        public List<SalleContent_Ennemies> spawnGroups = new List<SalleContent_Ennemies>();



        [Header("VARIABLES INTERNES POUR DEBUG")]
        [SerializeField] private int roomsDone = -1;
        public DoorOrientation fromDoor = DoorOrientation.West;
        [SerializeField] private DoorOrientation toDoor;
        public DoorOrientation spawnDoor;
        [SerializeField] private Salle currentRoom;
        public int chosenPattern;

        private readonly Queue<Salle> roomsQueue = new Queue<Salle>();

        public enum DoorOrientation
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
                roomsDone = -1;
                TransitionToNextRoom(DoorOrientation.West);
        }

        // Update is called once per frame
        void Update()
        {
                if(!currentRoom.roomDone) return;

                for (int i = 0; i < (int)DoorOrientation.West + 1; i++)
                {
                        if(fromDoor == (DoorOrientation) i ) continue;
                        
                        OpenDoors((DoorOrientation)i,true);
                }
        }

        public Salle GenerateDungeon2()
        {
                if (roomsDone == 0)
                {
                        EnableDoors(DoorOrientation.East,true);
                        OpenDoors(DoorOrientation.East, true);
                        s_doors[(int) DoorOrientation.East].ChooseRoomToSpawn(Random.Range(0, roomPrefab.Count));
                        return Instantiate(startRoom);
                }
                for (int i = 0; i < (int)DoorOrientation.West + 1; i++)
                {
                        if (i == (int) fromDoor) continue;
                        bool enabled = Random.value > 0.4f;
                        EnableDoors((DoorOrientation) i,enabled);
                        s_doors[i].ChooseRoomToSpawn(Random.Range(0, roomPrefab.Count));
                }

                if (roomsDone == 4)
                {
                        Door removedDoor = s_doors[(int)fromDoor];
                        s_doors.RemoveAt((int)fromDoor);
                        Door doorToSpecial = s_doors[Random.Range(0, s_doors.Count)];
                        doorToSpecial.currentDoorType = Door.DoorType.ToChallenge1;
                        //if (doorToSpecial.doorOrientation == fromDoor) doorToSpecial.doorOrientation = toDoor;
                        doorToSpecial.ChooseSpecialToSpawn(0);
                        s_doors.Insert((int)fromDoor, removedDoor);
                        //return Instantiate(specialRooms[0]);
                }
                if (roomsDone == dungeonSize)
                {
                        for (int i = 0; i < (int)DoorOrientation.West + 1; i++)
                        {
                                EnableDoors((DoorOrientation) i ,false);
                        }
                        EnableDoors(fromDoor,true);
                        return Instantiate(EndRoom);
                }
                EnableDoors(fromDoor,true);
                EnableDoors(toDoor,true);
                return (Salle)FindObjectOfType(typeof(Salle));
        }

        public Salle BeginGeneration()
        {
                if (currentRoom != null) Destroy(currentRoom.gameObject);
                roomsDone++;
                if (roomsDone == 0)
                {
                        return GenerateDungeon2();
                }
                if (roomsDone == dungeonSize)
                {
                        return GenerateDungeon2();
                }
                chosenPattern = Random.Range(0, spawnGroups.Count);
                Instantiate(s_doors[(int)spawnDoor].roomToSpawn);
                return GenerateDungeon2();
        }



        public void TransitionToNextRoom(DoorOrientation door)
        {
                toDoor = door;
                fromDoor = door switch
                {
                        DoorOrientation.West => DoorOrientation.East,
                        DoorOrientation.East => DoorOrientation.West,
                        DoorOrientation.North => DoorOrientation.South,
                        DoorOrientation.South => DoorOrientation.North,
                        _ => fromDoor
                };
                if(currentRoom != null) Destroy(currentRoom.gameObject);
                currentRoom = BeginGeneration();
                if(roomsDone != 0)MovePlayerToDoor(fromDoor);
                ClearRoom();
                currentRoom.GetSpawnPoints(0);

        }

        public void MovePlayerToDoor(DoorOrientation doorOrientation)
        {
                EnableDoors((DoorOrientation)Random.Range(0,4),true);
                player.rb.velocity = Vector2.zero;
                switch (doorOrientation)
                {
                        case DoorOrientation.North:
                                CharacterController.instance.transform.position = new Vector3(s_doors[(int) DoorOrientation.North].transform.position.x,s_doors[(int) DoorOrientation.North].transform.position.y -1,s_doors[(int) DoorOrientation.North].transform.position.z);
                                break;
                        case DoorOrientation.West:
                                CharacterController.instance.transform.position = new Vector3(s_doors[(int) DoorOrientation.West].transform.position.x +1,s_doors[(int) DoorOrientation.West].transform.position.y,s_doors[(int) DoorOrientation.West].transform.position.z);
                                break;
                        case DoorOrientation.South:
                                CharacterController.instance.transform.position = new Vector3(s_doors[(int) DoorOrientation.South].transform.position.x,s_doors[(int) DoorOrientation.South].transform.position.y +1,s_doors[(int) DoorOrientation.South].transform.position.z);
                                break;
                        case DoorOrientation.East:
                                CharacterController.instance.transform.position = new Vector3(s_doors[(int) DoorOrientation.East].transform.position.x-1,s_doors[(int) DoorOrientation.East].transform.position.y,s_doors[(int) DoorOrientation.East].transform.position.z);
                                break;
                        default:
                                throw new ArgumentOutOfRangeException(nameof(doorOrientation), doorOrientation, null);
                }
                AdjustCameraSettings();
        }

        public void EnableDoors(DoorOrientation index, bool state)
        {
                
                s_doors[(int) index].gameObject.SetActive(state);
                OpenDoors(index,false);
        }

        public void OpenDoors(DoorOrientation index, bool state)
        {
                s_doors[(int)index].GetComponent<BoxCollider2D>().enabled = state;
        }
        
        public void AdjustCameraSettings()
        {
                var cam = CameraController.cameraInstance;
                
                cam.transform.position = new Vector3(cam.cameraTarget.position.x,cam.cameraTarget.position.y,cam.transform.position.z);
        }

        public void ClearRoom()
        {
                List<GameObject> amount = CharacterController.instance.GetComponent<GhostDash>().tousLesSprites;
                for (int i = 0; i < amount.Count; i++)
                {
                        Destroy(CharacterController.instance.GetComponent<GhostDash>().tousLesSprites[i].gameObject);
                }
                amount.Clear();
        }
        
        //------------------------------------------CODE OBSOLETE----------------------------------------//
        
        public void GenerateDungeon() //génération de la map
        {
                if (currentRoom != null) Destroy(currentRoom.gameObject);
                roomsDone = -1;
                var maps = new List<Salle>(roomPrefab);
                roomsQueue.Clear();
                bool special0 = Random.value > 0.5f;
                if(special0) maps.Add(specialRooms[0]);
                for (int i = 0; i < dungeonSize; i++)
                {
                        var j = Random.Range(0, maps.Count);
                        roomsQueue.Enqueue(maps[j]);
                        maps.RemoveAt(j);
                }
        }
        public Salle GetNextRoom()
        {
                roomsDone++;
                if (roomsDone == 0)
                {
                        EnableDoors(DoorOrientation.East,true);
                        OpenDoors(DoorOrientation.East, true);
                        return Instantiate(startRoom);
                }
                for (int i = 0; i < (int)DoorOrientation.West + 1; i++)
                {
                        bool enabled = Random.value > 0.4f;
                        EnableDoors((DoorOrientation) i,enabled);
                }
                if (roomsDone == dungeonSize)
                {
                        for (int i = 0; i < (int)DoorOrientation.West + 1; i++)
                        {
                                EnableDoors((DoorOrientation) i ,false);
                        }
                        EnableDoors(fromDoor,true);
                        return Instantiate(EndRoom);
                }
                EnableDoors(fromDoor,true);
                return Instantiate(roomsQueue.Dequeue());

        }
}


