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

                [Header("REFERENCES")] 
                public CharacterController player;
                public static SalleGenerator Instance;
                public GameObject amphores;
                public List<Door> sDoors;
                [SerializeField] private Salle startRoom;
                [SerializeField] public Salle endRoom;
                [SerializeField] public Salle startRoom2;
                [SerializeField] private Salle endRoom2;

                [Header("CONTENU DU DONJON")]
                public List<Salle> roomPrefab = new List<Salle>();
                public List<Salle> roomPrefab2 = new List<Salle>();
                public List<Salle> specialRooms;
                private List<GameObject> _itemList;

                [Header("SHOP")]
                [NaughtyAttributes.MinMaxSlider(0.0f, 1.0f)] public Vector2 minMaxShopThreshold = new(.2f,.4f);
                [Range(0, 1)] public float shopSpawnChance = .3f;
        
                [Header("PATTERNES")]
                public List<SalleContent_Ennemies> spawnGroups = new List<SalleContent_Ennemies>();
                public List<SalleContent_Ennemies> spawnGroupsLevel2 = new List<SalleContent_Ennemies>();
        
                [Header("VARIABLES INTERNES POUR DEBUG")]
                [SerializeField] public int roomsDone = -1;
                public DoorOrientation fromDoor = DoorOrientation.West;
                [SerializeField] private DoorOrientation toDoor;
        
                public int globalBank = 14;
                public bool testBankSystem;
                public List<int> extraMoneyPerRoom = new List<int>(){0,0,3,3,0,5,5,6,0,0,7,0,8,9,9,0,12,14,16,0,0};
                public int inflation = 1;
        
                public float timeBetweenWaves;
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
                        _moveGrid = AstarPath.active.gameObject.GetComponent<ProceduralGridMover>();
                        timer = GameObject.FindWithTag("Timer");
                }

                void Start()
                {
                        TransitionToNextRoom(DoorOrientation.West, false, GameObject.Find("East").GetComponent<Door>());
                        //Debug.Log(GameObject.Find("East").GetComponent<Door>());
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
                }

                /// <summary>
                /// Première partie de la génération, check s'il doit générer le début ou la fin du donjon avant de générer une salle
                /// </summary>

                public void NewZone(DoorOrientation door, bool switchDoor, Door type)
                {
                        zone2 = true;
                        startRoom = startRoom2;
                        endRoom = endRoom2;
                        roomPrefab.Clear();
                        roomPrefab.AddRange(roomPrefab2);
                        roomsDone = 0;
                        dungeonSize -= shopsVisited;
                        shopsVisited = 0;
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
                                                        sDoors[i].willChooseSpecial = true;
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
                        for (int i = 0; i < (int)DoorOrientation.West + 1; i++)
                        {
                                if (i == (int) fromDoor) continue;
                                bool state = Random.value > 0.4f;
                                EnableDoors((DoorOrientation) i,state);
                                bool special = Random.value > 0.7f;
                                if (special)
                                {
                                        sDoors[i].willChooseSpecial = true;
                                }
                                sDoors[i].ChooseRoomToSpawn(Random.Range(0, roomPrefab.Count));
                        }

                        if (shopsVisited < 1 && roomsDone >= Mathf.RoundToInt(dungeonSize * minMaxShopThreshold.x) && roomsDone <= Mathf.RoundToInt(dungeonSize * minMaxShopThreshold.y))
                        {
                                var shopspawn = Random.value;
                                if (shopspawn >= shopSpawnChance)
                                {
                                        Door removedDoor = sDoors[(int)fromDoor];
                                        sDoors.RemoveAt((int)fromDoor);
                                        Door doorToShop = sDoors[Random.Range(0, sDoors.Count)];
                                        doorToShop.currentDoorType = Door.DoorType.ToShop;
                                        doorToShop.ChooseSpecialToSpawn(0);
                                        doorToShop.willChooseSpecial = false;
                                        sDoors.Insert((int)fromDoor, removedDoor);
                                        for (int i = 0; i < (int)DoorOrientation.West + 1; i++)
                                        {
                                                if (i == (int) fromDoor)continue;
                                                if (i == sDoors.IndexOf(doorToShop))continue;
                                                sDoors[i].willChooseSpecial = true;
                                                sDoors[i].ChooseRoomToSpawn(Random.Range(0, roomPrefab.Count));
                                        }
                                }

                        }
                        if (roomsDone == dungeonSize - 2)
                        {
                                for (int i = 0; i < (int)DoorOrientation.West + 1; i++)
                                {
                                        if (i == (int) fromDoor) continue;
                                        bool state = Random.value > 0.4f;
                                        EnableDoors((DoorOrientation) i,state);
                                        if(i == (int)DoorOrientation.South)EnableDoors(DoorOrientation.South,false);
                                        sDoors[i].currentDoorType = Door.DoorType.ToShop;
                                        sDoors[i].ChooseSpecialToSpawn(0);
                                        sDoors[i].willChooseSpecial = false;
                                }
                        }
                        if (roomsDone == dungeonSize - 1)
                        {
                                for (int i = 0; i < (int)DoorOrientation.West + 1; i++)
                                {
                                        if (i == (int) fromDoor) continue;
                                        sDoors[i].willChooseSpecial = false;
                                        EnableDoors((DoorOrientation)i,false);
                                        if(i == (int)DoorOrientation.North)EnableDoors(DoorOrientation.North, true);
                                }
                        }
                        if (roomsDone == dungeonSize)
                        {
                                for (int i = 0; i < (int)DoorOrientation.West + 1; i++)
                                {
                                        EnableDoors((DoorOrientation) i ,false);
                                        sDoors[i].willChooseSpecial = false;
                                }

                                if (zone2)
                                {
                                        Debug.Log("el cringo");
                                        return Instantiate(endRoom2);
                                }
                                EnableDoors(fromDoor,true);
                                EnableDoors(toDoor,true);
                                return Instantiate(endRoom);
                        }
                        EnableDoors(fromDoor,true);
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
                                if (roomsDone != 0)
                                {
                                        MovePlayerToDoor(fromDoor);
                                }
                                else if (zone2)
                                {
                                        MovePlayerToDoor(fromDoor);
                                }
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
                        EnableDoors((DoorOrientation)Random.Range(0,4),true);
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
                        sDoors[(int)index].collider.enabled = state;
                        //s_doors[(int)index].GetComponentInChildren<Animator>().SetBool("Open",state);
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
                        foreach(GameObject item in GameObject.FindGameObjectsWithTag("Item"))
                        {
                                Destroy(item);
                        }
                
                        foreach(GameObject item in GameObject.FindGameObjectsWithTag("CollectableSpell"))
                        {
                                Destroy(item);
                        }

                        List<GameObject> amount = CharacterController.instance.ghost.tousLesSprites;
                        for (int i = 0; i < amount.Count; i++)
                        {
                                Destroy(CharacterController.instance.ghost.tousLesSprites[i].gameObject);
                        }

                        foreach (var soul in Souls.instance.soulsInScene)
                        {
                                Souls.instance.CollectSouls(soul,1);
                                
                        }
                        Souls.instance.soulsInScene.Clear();
                        amount.Clear();
                        if (TothBehiavour.instance != null && roomsDone != 0)
                        {
                                DestroyImmediate(TothBehiavour.instance.gameObject);
                        }

                }

                public void SwapDoorType(Door type)
                {
                        type.currentSprite.sprite = type.doorSprites[0];
                        type.currentDoorType = Door.DoorType.Normal;
                }
        }
}


