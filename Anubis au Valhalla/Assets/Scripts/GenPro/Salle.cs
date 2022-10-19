using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


public class Salle : MonoBehaviour
{
    public bool roomDone = false;
    public Transform[] transformReferences;
    public Transform AstarRef;
    public GameObject player;
    public GameObject coffre;
    public Vector2 minPos = Vector2.zero;
    public Vector2 maxPos = Vector2.zero;
    public int spawnBank = 0;
    public int propsAmount = 5;
    public List<GameObject> currentEnemies = new List<GameObject>();
    public Tilemap tileMap;
    public TilemapRenderer renderer;
    public List<Vector3> availableTilePos;
    [Header("POINTS DE SPAWNS")]
    public List<GameObject> availableSpawnA;
    public List<GameObject> availableSpawnB;
    public List<GameObject> availableSpawnC;
    public List<int> costList = new List<int>();

    public PropSize propSizes = new PropSize();
    [Serializable]
    public class Props
    {
        public List<GameObject> props;
    }
    
    [Serializable]
    public class PropSize
    {
        public List<Props> PropsList;
    }
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        AstarRef = GameObject.Find("A* Ref").GetComponent<Transform>();
        renderer.enabled = false;
        spawnBank = SalleGennerator.instance.GlobalBank;
        SalleGennerator.instance.GlobalBank = Mathf.RoundToInt(SalleGennerator.instance.GlobalBank * 1.1f);
        
        AstarPath.active.Scan(AstarPath.active.data.graphs);
        RearrangeDoors();
        AdjustCameraConstraints();
        GetAvailableTiles();
    }

    private void Start()
    {
        if (currentEnemies.Count == 0)
        {
            roomDone = true;
            SalleGennerator.instance.roomsDone++;
        }
    }

    private void Update()
    {

    }

    public void RearrangeDoors()
    {
        for (int i = 0; i < (int)SalleGennerator.DoorOrientation.West + 1; i++)
        {
            SalleGennerator.instance.s_doors[i].transform.position = transformReferences[i].position;
        }
    }

    public void AdjustCameraConstraints()
    {
        var cam = CameraController.cameraInstance;
        cam.minPos = minPos;
        cam.maxPos = maxPos;
    }

    public void GetSpawnPoints(int spawnPoints)
    {
        if (spawnPoints == 0)
        {
         SpawnEnemies(availableSpawnA);   
        }
        if (spawnPoints == 1)
        {
            SpawnEnemies(availableSpawnB);
        }
        if (spawnPoints == 2)
        {
            SpawnEnemies(availableSpawnC);
        }
    }
    public void SpawnEnemies(List<GameObject> point)
    {
        if (point.Count == 0)
        {
            return;
        }
        var pattern = SalleGennerator.instance.chosenPattern;
        foreach (EnemyData t in SalleGennerator.instance.spawnGroups[pattern].enemiesToSpawn)
        {
            int cost = t.cost;
            costList.Add(cost);
        }
        while (spawnBank > costList.Min()) //tries to buy enemies as long as it can afford at least one of them
        {
            var chosenValue = Random.Range(0, costList.Count);
            if(spawnBank < costList.Max()) chosenValue = costList.IndexOf(costList.Min());//if it cant afford the most expensive enemy, it will buy the cheapest one
            Debug.Log(chosenValue);
            var chosenEnemy = SalleGennerator.instance.spawnGroups[pattern].enemiesToSpawn[chosenValue];
            spawnBank -= costList[chosenValue];
            costList[chosenValue] += 3;
            var chosenPoint = point[Random.Range(0, point.Count)];
            currentEnemies.Add(Instantiate(chosenEnemy.prefab, chosenPoint.transform.position,quaternion.identity,chosenPoint.transform));
            point.Remove(chosenPoint);
        }
    }
    
    public void GenerateProps()
    {
        if (propSizes.PropsList.Count == 0)
        {
            return;
        }
        for (int i = 0; i < propsAmount; i++)
        {
            int randomSize = Random.Range(0, propSizes.PropsList.Count);
            var propane = propSizes.PropsList[randomSize];
            if (randomSize == 0)
            {
                BlockTiles(propane,randomSize);
            }
            if (randomSize == 1)
            {
                BlockTiles(propane,randomSize);
            }
            if (randomSize == 2)
            {
                BlockTiles(propane,randomSize);
            }
            if (randomSize == 3)
            {
                BlockTiles(propane,randomSize);
            }
        }
    }

    public void BlockTiles(Props propane, int randomSize)
    {
        var chosenTile = availableTilePos[Random.Range(0, availableTilePos.Count)];
        Instantiate(propane.props[Random.Range(0, propane.props.Count)], chosenTile,quaternion.identity, gameObject.transform);
        List<Vector3> tilesToRemove = new List<Vector3>();
        foreach (var tile in availableTilePos)
        {
            if (tile.x >= chosenTile.x - (randomSize + 1) &&
                tile.x <= chosenTile.x + (randomSize + 1) &&
                tile.y >= chosenTile.y - (randomSize + 1) &&
                tile.y >= chosenTile.y + (randomSize + 1))
            {
                tilesToRemove.Add(tile);
            }
        }

        foreach (var tile in tilesToRemove)
        {
            availableTilePos.Remove(tile);
        }
        tilesToRemove.Clear();
    }

    public void GetAvailableTiles()
    {
        //var doorRef = SalleGennerator.instance.fromDoor;
        //var doorTransformRef = transformReferences[(int)doorRef];
        availableTilePos = new List<Vector3>();
        for (int i = tileMap.cellBounds.xMin; i < tileMap.cellBounds.xMax; i++)
        {
            for (int j = tileMap.cellBounds.yMin; j < tileMap.cellBounds.yMax; j++)
            {
                Vector3Int localTile = new Vector3Int(i, j, (int)tileMap.transform.position.z);
                Vector3 place = tileMap.CellToWorld(localTile);
                if (tileMap.HasTile(localTile))
                {
                    availableTilePos.Add(place);
                    foreach (var doorTransformRef in transformReferences)
                    {
                        Vector3Int door = new Vector3Int(Mathf.RoundToInt(doorTransformRef.position.x),Mathf.RoundToInt(doorTransformRef.position.y),Mathf.RoundToInt(doorTransformRef.position.z));
                        if ((localTile.x >= door.x - 2 &&
                             localTile.x <= door.x + 2)&&
                            (localTile.y >= door.y - 2 &&
                             localTile.y <= door.y + 2))
                        {
                                Debug.Log("true");
                                //Instantiate(fill, localTile, quaternion.identity);
                                availableTilePos.Remove(localTile); 
                        }
                    }
                }
            }
        }
        GenerateProps();
    }

    public void CheckForEnemies()
    {
        
        Debug.Log("oui");
        if (currentEnemies.Count == 0)
        {
            roomDone = true;
            SalleGennerator.instance.roomsDone++;
            //int coffreSpawnChance = Random.Range(1, 2);
            //Debug.Log(coffreSpawnChance);
           // if (coffreSpawnChance == 2)
            //{
                Debug.Log("coffre");
                Instantiate(coffre,player.transform.position,Quaternion.identity);
           // }
        }
    }
    

}


/*foreach (Transform door in transformReferences)
{
    door.position = new Vector3Int((int)door.position.x, (int)door.position.y, 0);
    for (int i = 0; i < availableTile.Count; i++)
    {
        if ((localTile.x >= door.position.x - 2 &&
                             localTile.x <= door.x + 2)&&
                            (localTile.y >= door.y - 2 &&
                             localTile.y <= door.y + 2))
        {
            availableTile.Remove(availableTile[i]); 
            Instantiate(filledTile, new Vector3(availableTile[i].x +0.5f,availableTile[i].y +0.5f,availableTile[i].z),Quaternion.identity);
        }
        if (localTile.magnitude >= door.magnitude + 2 &&
                            localTile.magnitude <= door.magnitude - 2)
        {
            availableTile.Remove(availableTile[i]); 
            Instantiate(filledTile, new Vector3(availableTile[i].x +0.5f,availableTile[i].y +0.5f,availableTile[i].z),Quaternion.identity);
        }
    }
}
Debug.Log(availableTile.Count);
                    foreach (Door door in SalleGennerator.instance.s_doors)
*/


