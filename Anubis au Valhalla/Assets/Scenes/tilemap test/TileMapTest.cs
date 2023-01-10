using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapTest : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tile;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int x = Random.Range(tilemap.cellBounds.xMin -1,tilemap.cellBounds.xMax +1);
            int y = Random.Range(-tilemap.cellBounds.yMin -1,tilemap.cellBounds.yMax +1);
            tilemap.BoxFill(new Vector3Int(x,y,0),tile,-1,-1,1,1);
        }
    }
}
