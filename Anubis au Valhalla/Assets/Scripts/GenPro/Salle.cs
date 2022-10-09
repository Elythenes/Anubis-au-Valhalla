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
    public Vector2 minPos = Vector2.zero;
    public Vector2 maxPos = Vector2.zero;
    public int spawnBank = 10;
    public Tilemap tileMap;
    public TilemapRenderer renderer;
    public List<Vector3Int> availableTile;
    public List<Vector3> availableTilePos;
    public GameObject filledTile;
    [Header("POINTS DE SPAWNS")]
    public List<GameObject> availableSpawnA;
    public List<GameObject> availableSpawnB;
    public List<GameObject> availableSpawnC;
    public List<int> costList = new List<int>();


    // Start is called before the first frame update
    void Awake()
    {
        renderer.enabled = false;
        StartCoroutine(ExampleRoomCleared());
        RearrangeDoors();
        AdjustCameraConstraints();
    }

    private void Start()
    {
        
    }


    

    public IEnumerator ExampleRoomCleared()
    {
        yield return new WaitForSeconds(1);
        roomDone = true;
    }

    public void RearrangeDoors()
    {
        for (int i = 0; i < (int)SalleGennerator.DoorOrientation.West; i++)
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
        foreach (EnemyData t in SalleGennerator.instance.spawnGroups[SalleGennerator.instance.chosenPattern].enemiesToSpawn)
        {
            int cost = t.cost;
            costList.Add(cost);
        }
        int lowestCost = costList.Min();
        if (spawnBank > lowestCost)
        {
            Debug.Log("oui");
            var chosenValue = Random.Range(0, SalleGennerator.instance.enemySpawnData.Length);
            var chosenEnemy = SalleGennerator.instance.spawnGroups[SalleGennerator.instance.chosenPattern].enemiesToSpawn[chosenValue];
            Debug.Log(chosenEnemy.prefab);
            spawnBank -= costList[chosenValue];
            costList[chosenValue] *= 2;
            var chosenPoint = point[Random.Range(0, point.Count)];
            Instantiate(chosenEnemy.prefab, chosenPoint.transform);
            point.Remove(chosenPoint);
        }
    }

    public void GenerateTiles()
    {
 
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


