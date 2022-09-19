using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
/*
TRUCS A FAIRE POUR LA GEN PRO:
        INCLURE LA GENERATION DE SALLES SPECIALES(COFFRES, DEFIS ETC...)
        DIFFERENCIER LES SALLES SPECIALLES POUR POUVOIR CONTROLLER QUAND ELLES APPARAISSENT
        INCLURE UNE SALLE DE FIN/BOSS
        INCLURE UNE TRANSITION VERS UN ECRAN DE VICTOIRE QUAND IL N'Y A PLUS DE SALLES A GENERER
*/
public class SalleGennerator : MonoBehaviour
{
        [Header("CETTE VARIABLE MODIFIE LA TAILLE DU DONJON QU'ON VEUX GENERER")]
        public int dungeonSize;
        
        [Header("REFERENCES")]
        public CharacterController player;
        public static SalleGennerator instance;
        public List<GameObject> s_doors;
        [SerializeField] private Salle startRoom;
        [SerializeField] private Salle EndRoom;

        [Header("CONTENU DU DONJON")]
        public List<Salle> roomPrefab = new List<Salle>();

        public List<Salle> specialRooms;



        [Header("VARIABLES INTERNES POUR DEBUG")]
        [SerializeField] private int roomsDone = -1;
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
                //camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
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
                bool special0 = Random.value > 0.5f;
                if(special0) maps.Add(specialRooms[0]);
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
                ClearRoom();

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
                        bool enabled = Random.value > 0.4f;
                        EnableDoors((Doortype) i,enabled);
                }
                if (roomsDone == dungeonSize)
                {
                        for (int i = 0; i < (int)Doortype.West + 1; i++)
                        {
                                EnableDoors((Doortype) i ,false);
                        }
                        EnableDoors(fromDoor,true);
                        return Instantiate(EndRoom);
                }
                EnableDoors(fromDoor,true);
                return Instantiate(roomsQueue.Dequeue());
                
        }

        public void MovePlayerToDoor(Doortype doortype)
        {
                EnableDoors((Doortype)Random.Range(0,4),true);
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
                AdjustCameraSettings();
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
}


