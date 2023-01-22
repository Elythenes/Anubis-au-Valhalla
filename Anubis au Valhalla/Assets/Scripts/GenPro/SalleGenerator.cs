using System;
using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GenPro
{
        public class SalleGenerator : MonoBehaviour
        {
                [Header("CETTE VARIABLE MODIFIE LA TAILLE DU DONJON QU'ON VEUX GENERER")]
                public int dungeonSize;
                [SerializeField] public int roomsDone = -1;

                [Header("REFERENCES")] 
                public CharacterController player;
                public static SalleGenerator Instance;
                public GameObject amphores;
                public List<Door> sDoors;
                [SerializeField] private Salle startRoom;
                [SerializeField] public Salle endRoom;
                [SerializeField] public Salle startRoom2;
                [SerializeField] private Salle endRoom2;
                public GameObject challengeCoffre;

                [Header("CONTENU DU DONJON")]
                public List<Salle> roomPrefab = new List<Salle>();
                public List<Salle> roomPrefab2 = new List<Salle>();
                public List<Salle> specialRooms;
                private List<GameObject> _itemList;

                [Header("SHOP")]
                [NaughtyAttributes.MinMaxSlider(0.0f, 1.0f)] public Vector2 minMaxShopThreshold = new(.2f,.4f);
                [Range(0, 1)] public float shopSpawnChance = .3f;
                public int spawnedShops;
                public List<float> chancePharaon = new List<float>(7) { 40, 45, 60, 65, 75, 90, 90};
                public List<float> chanceDivinité = new List<float>(7) { 0, 0, 20, 30, 40, 50, 60};
                public List<int> nbMaxPharaon = new List<int>(7) { 3, 4, 5, 6, 7, 8, 9 };
                public List<int> nbMaxDivinite = new List<int>(7) { 0, 0, 1, 2, 3, 4, 5 };
        
                [Header("PATTERNES")]
                public List<SalleContent_Ennemies> spawnGroups = new List<SalleContent_Ennemies>();
                public List<SalleContent_Ennemies> spawnGroupsLevel2 = new List<SalleContent_Ennemies>();
                public SalleContent_Ennemies eliteChallenge;
                public int maxEnemiesPerSmallWave;
                public int maxEnemiesPerBigWave;
                public float timeBetweenWaves;
                public List<int> maxElitesPerRoom = new List<int>();

                [Header("VARIABLES INTERNES POUR DEBUG")]
                public DoorOrientation fromDoor = DoorOrientation.West;
                [SerializeField] private DoorOrientation toDoor;
                public bool isTuto;
        
                public int globalBank = 14;
                public bool testBankSystem;
                public List<int> extraMoneyPerRoom = new List<int>(){0,0,3,3,0,5,5,6,0,0,7,0,8,9,9,0,12,14,16,0,0};
                public int inflation = 1;
        

                public CanvasGroup transitionCanvas;
                public int shopsVisited;
                private ProceduralGridMover _moveGrid;
                [HideInInspector] public DoorOrientation spawnDoor;
                public Salle currentRoom;
                public int chosenPattern;
                public int challengeChooser;
                public GameObject timer;
                public GameObject parasiteToSpawn;
                public bool zone2;
                public bool morbinTime;
                public bool isHub;
                

                public enum DoorOrientation
                {
                        North = 0,
                        East = 1,
                        South = 2,
                        West = 3
                }
                private void Awake()
                {
                        if (Instance != null)
                        {
                                DestroyImmediate(gameObject);
                                return;
                        }
                        Instance = this;
                        if (!isTuto)
                        { 
                                _moveGrid = AstarPath.active.gameObject.GetComponent<ProceduralGridMover>();
                        }
                        timer = GameObject.FindWithTag("Timer");
                }

                void Start()
                {
                        if (zone2)
                        {
                                NewZone(DoorOrientation.West, false, GameObject.Find("East").GetComponent<Door>());
                                return;
                        }

                        if (!isTuto)
                        {
                                TransitionToNextRoom(DoorOrientation.West, false, GameObject.Find("East").GetComponent<Door>());
                        }
                }
                /// <summary>
                /// Méthode qui ouvre toutes les portes actives
                /// </summary>
                public void UnlockDoors()
                {
                        for (int i = 0; i < (int)DoorOrientation.West + 1; i++)
                        {
                                if(fromDoor == (DoorOrientation) i ) continue;
                        
                                OpenDoors((DoorOrientation)i,true);
                        }
                        roomsDone++;
                        if (!isHub)
                        {
                                currentRoom.roomDone = true;
                        }
                }

                /// <summary>
                /// Première partie de la génération, check s'il doit générer le début ou la fin du donjon avant de générer une salle
                /// </summary>

                public void NewZone(DoorOrientation door, bool switchDoor, Door type)
                {
                        SoundManager.instance.PlayZone2();
                        SoundManager.instance.ChangeToZone2();
                        zone2 = true;
                        startRoom = startRoom2;
                        endRoom = endRoom2;
                        roomPrefab.Clear();
                        roomPrefab.AddRange(roomPrefab2);
                        extraMoneyPerRoom.RemoveRange(0,roomsDone);
                        maxElitesPerRoom.RemoveRange(0,roomsDone);
                        roomsDone = 0;
                        dungeonSize -= shopsVisited;
                        shopsVisited = 0;
                        spawnedShops = 0;
                        for (int i = 0; i < sDoors.Count; i++)
                        {
                                sDoors[i].ResetDoorState();
                        }

                        TransitionToNextRoom(door,switchDoor,type);

                }

                private Salle BeginGeneration()
                {
                        //if (currentRoom != null) Destroy(currentRoom.gameObject);
                        if (roomsDone == 0)
                        {
                                return GenerateDungeon2();
                        }
                        if (roomsDone == dungeonSize)
                        {
                                return GenerateDungeon2();
                        }
                        chosenPattern = Random.Range(0, spawnGroups.Count);
                        Instantiate(sDoors[(int)spawnDoor].roomToSpawn);
                        return GenerateDungeon2();
                }
                /// <summary>
                /// Active X portes puis leurs assigne une salle a spawn
                /// </summary>
                private Salle GenerateDungeon2()
                {
                        for (int i = 0; i < (int)DoorOrientation.West + 1; i++)
                        {
                                sDoors[i].currentDoorType = Door.DoorType.Normal;
                        }
                        if (roomsDone == 0 )
                        {
                                if (zone2)
                                {
                                        for (int i = 0; i < (int)DoorOrientation.West + 1; i++)
                                        {
                                                if (i == (int) fromDoor) continue;
                                                bool state = Random.value > 0.4f;
                                                EnableDoors((DoorOrientation) i,state);
                                                bool special = Random.value > 0.3f;
                                                if (special)
                                                {
                                                        sDoors[i].currentDoorType = Door.DoorType.ToChallenge1;
                                                }
                                                sDoors[i].ChooseRoomToSpawn(Random.Range(0, roomPrefab.Count));
                                        }
                                }
                                else
                                {
                                        EnableDoors(DoorOrientation.East,true);
                                        OpenDoors(DoorOrientation.East, true);
                                        sDoors[(int) DoorOrientation.East].ChooseRoomToSpawn(Random.Range(0, roomPrefab.Count));

                                }
                                return Instantiate(startRoom);
                        }

                        if (roomsDone == 2 && spawnedShops < 1)
                        {
                                Debug.Log("spawn un shop");
                                spawnedShops++;
                                Door removedDoor = sDoors[(int)fromDoor];
                                sDoors.RemoveAt((int)fromDoor);
                                Door doorToShop = sDoors[Random.Range(0, sDoors.Count)];
                                doorToShop.currentDoorType = Door.DoorType.ToShop;
                                doorToShop.ChooseSpecialToSpawn(0);
                                EnableDoors((DoorOrientation)sDoors.IndexOf(doorToShop),true);
                                sDoors.Insert((int)fromDoor, removedDoor);
                                for (int i = 0; i < (int)DoorOrientation.West + 1; i++)
                                {
                                        if (i == (int) fromDoor)continue;
                                        if (i == sDoors.IndexOf(doorToShop))continue;
                                        sDoors[i].currentDoorType = Door.DoorType.ToChallenge1;
                                        sDoors[i].ChooseRoomToSpawn(Random.Range(0, roomPrefab.Count));
                                        EnableDoors((DoorOrientation)i,true);
                                }
                                EnableDoors(fromDoor,true);
                                sDoors[(int)fromDoor].currentDoorType = Door.DoorType.Broken;
                                return (Salle)FindObjectOfType(typeof(Salle));
                        }


                        
                        if (roomsDone == dungeonSize - 2)
                        {
                                Debug.Log("spawn garanti du shop");
                                for (int i = 0; i < (int)DoorOrientation.West + 1; i++)
                                {
                                        if (i == (int) fromDoor) continue;
                                        EnableDoors((DoorOrientation) i,true);
                                        if(i == (int)DoorOrientation.South)EnableDoors(DoorOrientation.South,false);
                                        sDoors[i].currentDoorType = Door.DoorType.ToShop;
                                        sDoors[i].ChooseSpecialToSpawn(0);
                                }
                                spawnedShops++;
                                EnableDoors(fromDoor,true);
                                sDoors[(int)fromDoor].currentDoorType = Door.DoorType.Broken;
                                return (Salle)FindObjectOfType(typeof(Salle));
                        }
                        if (roomsDone == dungeonSize - 1)
                        {
                                Debug.Log("spawn porte boss");
                                for (int i = 0; i < (int)DoorOrientation.West + 1; i++)
                                {
                                        
                                        sDoors[i].currentDoorType = Door.DoorType.Normal;
                                        EnableDoors((DoorOrientation)i,false);
                                        Debug.Log(sDoors[i].isActiveAndEnabled);
                                        if (i == (int)DoorOrientation.North)
                                        {
                                                EnableDoors(DoorOrientation.North, true);
                                                sDoors[i].currentDoorType = Door.DoorType.ToBoss;
                                        }
                                }
                                EnableDoors(fromDoor,true);
                                sDoors[(int)fromDoor].currentDoorType = Door.DoorType.Broken;
                                return (Salle)FindObjectOfType(typeof(Salle));
                        }
                        if (roomsDone == dungeonSize)
                        {
                                Debug.Log("spawn salle boss");
                                for (int i = 0; i < (int)DoorOrientation.West + 1; i++)
                                {
                                        EnableDoors((DoorOrientation) i ,false);
                                        sDoors[i].currentDoorType = Door.DoorType.Normal;
                                }

                                if (zone2)
                                {
                                        EnableDoors(fromDoor,true);
                                        sDoors[(int)fromDoor].currentDoorType = Door.DoorType.Broken;
                                        return Instantiate(endRoom2);
                                }
                                EnableDoors(fromDoor,true);
                                sDoors[(int)fromDoor].currentDoorType = Door.DoorType.Broken;
                                EnableDoors(toDoor,true);
                                sDoors[(int)toDoor].currentDoorType = Door.DoorType.Transition;
                                return Instantiate(endRoom);
                        }
                        for (int i = 0; i < (int)DoorOrientation.West + 1; i++)
                        {
                                if (i == (int) fromDoor) continue;
                                bool state = Random.value > 0.4f;
                                EnableDoors((DoorOrientation) i,state);
                                bool special = Random.value > 0.7f;
                                if (special)
                                {
                                        sDoors[i].currentDoorType = Door.DoorType.ToChallenge1;
                                }
                                sDoors[i].ChooseRoomToSpawn(Random.Range(0, roomPrefab.Count));
                        }

                        EnableDoors(fromDoor,true);
                        sDoors[(int)fromDoor].currentDoorType = Door.DoorType.Broken;
                        EnableDoors(toDoor,true);
                        return (Salle)FindObjectOfType(typeof(Salle));
                }
        
                /// <summary>
                /// Méthode qui enclenche le changement de salle
                /// </summary>
                public void TransitionToNextRoom(DoorOrientation door, bool switchDoor, Door type)
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
                        transitionCanvas.DOFade(1, 0.25f).OnComplete(() =>
                        {
                                if (currentRoom != null) currentRoom.gameObject.SetActive(false);
                                if(switchDoor) SwapDoorType(type);
                                currentRoom = BeginGeneration();
                                if (roomsDone != 0 && !zone2)
                                {
                                        MovePlayerToDoor(fromDoor);
                                }
                                else if (zone2 && roomsDone != 0)
                                {
                                        MovePlayerToDoor(fromDoor);
                                }
                                Souls.instance.ClearOfSouls();
                                ClearRoom(); 
                                if(currentRoom != startRoom)currentRoom.GetSpawnPoints(Random.Range(0, 3));
                                _moveGrid.target = currentRoom.AstarRef;
                                transitionCanvas.DOFade(0, 0.25f);
                        });
                }
                /// <summary>
                /// Bouge le joueur selon la direction de la porte qu'il a emprunté
                /// </summary>
                public void MovePlayerToDoor(DoorOrientation doorOrientation)
                {
                        switch (doorOrientation)
                        {
                                case DoorOrientation.North:
                                        CharacterController.instance.transform.position = new Vector3(sDoors[(int) DoorOrientation.North].transform.position.x,sDoors[(int) DoorOrientation.North].transform.position.y -1,sDoors[(int) DoorOrientation.North].transform.position.z);
                                        break;
                                case DoorOrientation.West:
                                        CharacterController.instance.transform.position = new Vector3(sDoors[(int) DoorOrientation.West].transform.position.x +1,sDoors[(int) DoorOrientation.West].transform.position.y,sDoors[(int) DoorOrientation.West].transform.position.z);
                                        break;
                                case DoorOrientation.South:
                                        CharacterController.instance.transform.position = new Vector3(sDoors[(int) DoorOrientation.South].transform.position.x,sDoors[(int) DoorOrientation.South].transform.position.y +2,sDoors[(int) DoorOrientation.South].transform.position.z);
                                        break;
                                case DoorOrientation.East:
                                        CharacterController.instance.transform.position = new Vector3(sDoors[(int) DoorOrientation.East].transform.position.x-1,sDoors[(int) DoorOrientation.East].transform.position.y,sDoors[(int) DoorOrientation.East].transform.position.z);
                                        break;
                                default:
                                        throw new ArgumentOutOfRangeException(nameof(doorOrientation), doorOrientation, null);
                        }
                        AdjustCameraSettings();
                }
                /// <summary>
                /// Gère si les portes sont la de base 
                /// </summary>
                public void EnableDoors(DoorOrientation index, bool state)
                {
                
                        sDoors[(int) index].gameObject.SetActive(state);
                        sDoors[(int) index].ResetDoorState();
                        OpenDoors(index,false);
                }
                /// <summary>
                /// Méthode pour ouvrir les portes
                /// </summary>
                public void OpenDoors(DoorOrientation index, bool state)
                {
                        if (!isTuto)
                        {
                                sDoors[(int)index].doorCollider.enabled = state;
                        }
                       
                }
                /// <summary>
                /// TP la caméra au joueur
                /// </summary>
                public void AdjustCameraSettings()
                {
                        var cam = CameraController.cameraInstance;
                        var transform1 = cam.transform;
                        var camTransform = transform1.position;
                        var camTarget = cam.cameraTarget.position;
                        camTransform = new Vector3(camTarget.x,camTarget.y,camTransform.z);
                        transform1.position = camTransform;
                }
                /// <summary>
                /// Méthode pour clean la salle(sprites de ghostdash, projectiles, spells etc...).
                /// 
                /// A utiliser si les objets a destroy ne sont pas en enfant d'une salle
                /// </summary>
                public void ClearRoom()
                {
                        player.trail.Clear(); 
                        var amount = CharacterController.instance.ghost.tousLesSprites;
                        for (int i = 0; i < amount.Count; i++)
                        {
                                Destroy(CharacterController.instance.ghost.tousLesSprites[i].gameObject);
                        }


                        amount.Clear();

                        if (TothBehiavour.instance != null && roomsDone != 0)
                        {
                                DestroyImmediate(TothBehiavour.instance.gameObject);
                        }

                }

                public void SwapDoorType(Door type)
                {
                        type.currentDoorType = Door.DoorType.Normal;
                        type.willChooseSpecial = false;
                }
        }
}


