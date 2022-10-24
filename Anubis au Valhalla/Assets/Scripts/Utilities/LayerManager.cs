using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    private GameObject player;
    private GameObject emptyLayerPlayer;
    private SpriteRenderer srPlayer;
    private SpriteRenderer srProps;
    private Transform emptyLayer;
    void Start()
    {
        player = GameObject.Find("Personnage");
        emptyLayerPlayer = GameObject.Find("PlayerEmptyLayer");
        srPlayer = player.GetComponent<SpriteRenderer>();
        srProps = GetComponent<SpriteRenderer>();
        emptyLayer = GetComponentInChildren<Transform>();
    }

    
    void Update()
    {
        if (emptyLayerPlayer.transform.position.y > emptyLayer.position.y)
        {
            srProps.sortingOrder = srPlayer.sortingOrder + 1;
        }
        else
        {
            srProps.sortingOrder = srPlayer.sortingOrder - 1;
        }
    }
}
