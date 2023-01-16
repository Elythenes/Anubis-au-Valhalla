using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenPro;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
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
    public List<MonsterLifeManager> currentEnemies = new List<MonsterLifeManager>();
    public List<GameObject> discardedPoints = new List<GameObject>();
    public Tilemap tileMap;
    public TilemapRenderer renderer;
    public List<Vector3> availableTilePos;
    [Header("POINTS DE SPAWNS")]
    public List<GameObject> availableSpawnA;
    public List<GameObject> availableSpawnB;
    public List<GameObject> availableSpawnC;
    public List<int> costList = new List<int>();
    public PropSize propSizes = new PropSize();

    private int challengeChosen;
    private bool hasElited = false;
    private bool infiniteBank = false;
    private TimerChallenge timer;
    public bool parasites = false;
    public bool overdose = true;
    public int spawnedElites = 0;

    private float waveTimer;
    [HideInInspector] public TextMove text;
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
        timer = SalleGenerator.Instance.timer.GetComponent<TimerChallenge>();
        spawnBank = SalleGenerator.Instance.globalBank;
        if (SalleGenerator.Instance.testBankSystem)
        {
            SalleGenerator.Instance.globalBank += SalleGenerator.Instance.extraMoneyPerRoom[SalleGenerator.Instance.roomsDone];
        }
        else
        {
            SalleGenerator.Instance.globalBank = Mathf.RoundToInt(SalleGenerator.Instance.globalBank * 1.1f);
        }
        RearrangeDoors();
        AdjustCameraConstraints();
        GetAvailableTiles();
        AstarPath.active.Scan(AstarPath.active.data.graphs);
    }

    private void Start()
    {
        if (SalleGenerator.Instance.challengeChooser == 2) infiniteBank = true;
        challengeChosen = SalleGenerator.Instance.challengeChooser;
        text = TextMove.instance;
        if (currentEnemies.Count == 0)
        {
            roomDone = true;
            SalleGenerator.Instance.roomsDone++;
        }

        if (SalleGenerator.Instance.morbinTime)
        {
            Debug.Log("Gros challenge spécial");
            overdose = true;
            infiniteBank = true;
            challengeChosen = 2;
            C2_Survive();
        }
        else if(!roomDone)
        {
            switch (challengeChosen)
            {
                case 1:
                    C1_AllElites();
                    break;
                case 2:
                    C2_Survive();
                    break;
                case 3:
                    C3_TimeAttack();
                    break;
                case 4:
                    C4_Parasites();
                    break;
                case 5:
                    C5_Overdose();
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (infiniteBank && timer.enabled)
        {
            if (timer.internalTimer < 0)
            {
                Instantiate(SalleGenerator.Instance.challengeCoffre,player.transform.position - new Vector3(0,1,0),Quaternion.identity, transform);
                foreach (var enemy in currentEnemies)
                {
                    enemy.soulValue = 1;
                    enemy.Die();
                }
                timer.enabled = false;
                timer.timer.enabled = false;
                roomDone = true;
                SalleGenerator.Instance.roomsDone++;
                SalleGenerator.Instance.UnlockDoors();
                text.FadeOut(text.titleAlpha,text.titleEndPos,text.title);
                SalleGenerator.Instance.morbinTime = false;
                return;
            }
            
            waveTimer -= Time.deltaTime;
            if (waveTimer < 0)
            {
                waveTimer = SalleGenerator.Instance.timeBetweenWaves;
                var spawnPoints = Random.Range(0, 3);
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
        }
    }

    private void C1_AllElites()
    {
        text.Appear(text.titleAlpha,text.titleStartPos,text.title);
        text.Appear(text.descAlpha,text.descStartPos,text.description);
        StartCoroutine(DelayedFade());
        text.title.text = "Ennemis vicieux";
        text.description.text = "Tous les ennemis sont des élites";
    }

    private void C2_Survive()
    {
        Debug.Log("mais fais le challenge connard");
        text.Appear(text.titleAlpha,text.titleStartPos,text.title);
        text.Appear(text.descAlpha,text.descStartPos,text.description);
        timer.timer.enabled = true;
        timer.enabled = true;
        waveTimer = SalleGenerator.Instance.timeBetweenWaves;
        if (overdose)
        {
            text.title.text = "Survie";
            text.description.text = "Survivez à la horde!";
            timer.internalTimer = 30;
        }
        else
        {
            text.title.text = "Survie";
            text.description.text = "Survivez jusqu'au temps imparti";
            timer.internalTimer = 20;
        }
        StartCoroutine(DelayedFade());

    }

    private void C3_TimeAttack()
    {
        text.Appear(text.titleAlpha,text.titleStartPos,text.title);
        text.Appear(text.descAlpha,text.descStartPos,text.description);
        text.title.text = "Course contre la montre";
        text.description.text = "Tuer tous les ennemis avant la fin du temps imparti donnera une meilleure récompense";
        StartCoroutine(DelayedFade());
        timer.timer.enabled = true;
        timer.enabled = true;
    }

    private void C4_Parasites()
    {
        text.Appear(text.titleAlpha,text.titleStartPos,text.title);
        text.Appear(text.descAlpha,text.descStartPos,text.description);
        text.title.text = "Parasites";
        text.description.text = "Des corbeaux naissent de la carcasse de vos ennemis";
        StartCoroutine(DelayedFade());
        parasites = true;
    }

    private void C5_Overdose()
    {
        text.Appear(text.titleAlpha,text.titleStartPos,text.title);
        text.Appear(text.descAlpha,text.descStartPos,text.description);
        text.title.text = "Overdose";
        text.description.text = "Les ennemis sont bien plus rapides, mais aussi plus fragile";
        StartCoroutine(DelayedFade());
        overdose = true;
    }

    public void RearrangeDoors()
    {
        for (int i = 0; i < (int)SalleGenerator.DoorOrientation.West + 1; i++)
        {
            SalleGenerator.Instance.sDoors[i].transform.position = transformReferences[i].position;
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
            SpawnAmphores(availableSpawnB);
        }
        if (spawnPoints == 1)
        {
            SpawnEnemies(availableSpawnB);
            SpawnAmphores(availableSpawnC);
        }
        if (spawnPoints == 2)
        {
            SpawnEnemies(availableSpawnC);
            SpawnAmphores(availableSpawnA);
        }
    }
    public void SpawnEnemies(List<GameObject> point)
    {
        if (point.Count == 0) return;
        var pattern = SalleGenerator.Instance.chosenPattern;
        if (spawnedElites >= SalleGenerator.Instance.maxElitesPerRoom[SalleGenerator.Instance.roomsDone])
        {
            pattern = SalleGenerator.Instance.spawnGroups.IndexOf(SalleGenerator.Instance.spawnGroups.Last());
            costList.Clear();
        }
        if (SalleGenerator.Instance.challengeChooser != 1)
        {
            if (costList.Count == 0)
            {    
                foreach (var t in SalleGenerator.Instance.spawnGroups[pattern].enemiesToSpawn)
                {
                    int cost = t.cost;
                    costList.Add(cost);
                }
            }
        }
        else if(SalleGenerator.Instance.challengeChooser == 1)
        {
            if (costList.Count == 0)
            {
                foreach (var t in SalleGenerator.Instance.eliteChallenge.enemiesToSpawn)
                {
                    int cost = t.cost;
                    costList.Add(cost);
                }
            }
            
            while (spawnBank > costList.Min()) //tries to buy enemies as long as it can afford at least one of them
            {
                var chosenValue = Random.Range(0, costList.Count);
                if(spawnBank < costList.Max()) chosenValue = costList.IndexOf(costList.Min());//if it cant afford the most expensive enemy, it will buy the cheapest one
                var chosenEnemy = SalleGenerator.Instance.eliteChallenge.enemiesToSpawn[chosenValue];
                spawnBank -= costList[chosenValue];
                costList[chosenValue] += SalleGenerator.Instance.inflation;
                var chosenPoint = point[Random.Range(0, point.Count)];
                var enemyObject =Instantiate(chosenEnemy.prefab, chosenPoint.transform.position,quaternion.identity,chosenPoint.transform);
                var enemyScript = enemyObject.GetComponent<MonsterLifeManager>();
                currentEnemies.Add(enemyScript);
                discardedPoints.Add(chosenPoint);
                point.Remove(chosenPoint); // Get the spawner to spawn in waves if theres too many enemies to to spawn
                if (point.Count == 0)
                {
                    point.AddRange(discardedPoints);
                    discardedPoints.Clear();
                    return;
                }
            }
            return;
        }
        if(infiniteBank)
        {
            if(timer.internalTimer > 0 && (currentEnemies.Count == 0|| !hasElited))
            {
                hasElited = true;
                for (int i = 0; i < SalleGenerator.Instance.maxEnemiesPerBigWave; i++)
                {
                    var chosenValue = Random.Range(0, costList.Count);
                    var chosenEnemy = SalleGenerator.Instance.spawnGroups.Last().enemiesToSpawn[chosenValue];
                    if (chosenEnemy.isElite) spawnedElites++;
                    var chosenPoint = point[Random.Range(0, point.Count)];
                    var enemyObject =Instantiate(chosenEnemy.prefab, chosenPoint.transform.position,quaternion.identity,chosenPoint.transform);
                    var enemyScript = enemyObject.GetComponent<MonsterLifeManager>();
                    currentEnemies.Add(enemyScript);
                    discardedPoints.Add(chosenPoint);
                    point.Remove(chosenPoint);
                }
                point.AddRange(discardedPoints);
                discardedPoints.Clear();
                return;
            }
            if (timer.internalTimer > 0 && currentEnemies.Count > 0)
            {
                for (int i = 0; i < SalleGenerator.Instance.maxEnemiesPerSmallWave; i++)
                {
                    var chosenValue = Random.Range(0, costList.Count);
                    var chosenEnemy = SalleGenerator.Instance.spawnGroups.Last().enemiesToSpawn[chosenValue];
                    var chosenPoint = point[Random.Range(0, point.Count)];
                    if (chosenEnemy.isElite) spawnedElites++;
                    var enemyObject =Instantiate(chosenEnemy.prefab, chosenPoint.transform.position,quaternion.identity,chosenPoint.transform);
                    var enemyScript = enemyObject.GetComponent<MonsterLifeManager>();
                    currentEnemies.Add(enemyScript);
                    discardedPoints.Add(chosenPoint);
                    point.Remove(chosenPoint);
                }
                point.AddRange(discardedPoints);
                discardedPoints.Clear();
                return;
            }

            return;
        }
        while (spawnBank > costList.Min())
        {
            var chosenValue = Random.Range(0, costList.Count);
            if(spawnBank < costList.Max()) chosenValue = costList.IndexOf(costList.Min());
            var chosenEnemy = SalleGenerator.Instance.spawnGroups[pattern].enemiesToSpawn[chosenValue];
            if (chosenEnemy.isElite) spawnedElites++;
            spawnBank -= costList[chosenValue];
            var chosenPoint = point[Random.Range(0, point.Count)];
            var enemyObject =Instantiate(chosenEnemy.prefab, chosenPoint.transform.position,quaternion.identity,chosenPoint.transform);
            var enemyScript = enemyObject.GetComponent<MonsterLifeManager>();
            currentEnemies.Add(enemyScript);
            if (overdose)
            {
                enemyScript.overdose = true;
            }
            discardedPoints.Add(chosenPoint);
            point.Remove(chosenPoint);
            if (point.Count == 0)
            {
                point.AddRange(discardedPoints);
                discardedPoints.Clear();
                return;
            }
        }
    }

    public void SpawnAmphores(List<GameObject> point)
    {
        if (point.Count == 0)
        {
            return;
        }
        for (int i = 0; i < Random.Range(1,6); i++)
        {
            var chosenPoint = point[Random.Range(0, point.Count)];
            Instantiate(SalleGenerator.Instance.amphores,chosenPoint.transform);
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
        //var doorRef = SalleGenerator.instance.fromDoor;
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
        if (spawnBank > costList.Min() && currentEnemies.Count == 0)
        {
            var spawnPoints = Random.Range(0, 3);
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
            return;
        }

        if (currentEnemies.Count != 0) return;
        if (infiniteBank && timer.internalTimer > 0) return;
        roomDone = true;
        SalleGenerator.Instance.roomsDone++;
        SalleGenerator.Instance.UnlockDoors();
        text.FadeOut(text.titleAlpha,text.titleEndPos,text.title);
        switch (SalleGenerator.Instance.challengeChooser)
        {
            case  0:
                Instantiate(coffre,player.transform.position - new Vector3(0,1,0),Quaternion.identity, transform);
                break;
            case 1:
                Instantiate(SalleGenerator.Instance.challengeCoffre,player.transform.position - new Vector3(0,1,0),Quaternion.identity, transform);
                break;
            case 2:
                break;
            case 3:
                if (timer.internalTimer > 0)
                {
                    Instantiate(SalleGenerator.Instance.challengeCoffre,player.transform.position - new Vector3(0,1,0),Quaternion.identity, transform);
                }
                else
                {
                    Instantiate(coffre,player.transform.position - new Vector3(0,1,0),Quaternion.identity, transform);
                }
                timer.enabled = false;
                timer.timer.enabled = false;
                break;
            case 4:
                Instantiate(SalleGenerator.Instance.challengeCoffre,player.transform.position - new Vector3(0,1,0),Quaternion.identity, transform);
                break;
            case 5:
                Instantiate(SalleGenerator.Instance.challengeCoffre,player.transform.position - new Vector3(0,1,0),Quaternion.identity, transform);
                break;
        }

        if (SalleGenerator.Instance.roomsDone == SalleGenerator.Instance.dungeonSize)
        {
            Debug.Log("will spawn boss");
            timer.enabled = false;
            timer.timer.enabled = false;
            text.FadeOut(text.descAlpha, text.descEndPos, text.description);
        }
    }
    public IEnumerator DelayedSpawns()
    {
        yield return new WaitForSeconds(SalleGenerator.Instance.timeBetweenWaves);
        if (spawnBank > costList.Min())
        {
            var spawnPoints = Random.Range(0, 3);
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
    }

    public IEnumerator DelayedFade()
    {
        yield return new WaitForSeconds(text.textDuration);
        text.FadeOut(text.descAlpha, text.descEndPos, text.description);
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
                    foreach (Door door in SalleGenerator.instance.s_doors)
*/


