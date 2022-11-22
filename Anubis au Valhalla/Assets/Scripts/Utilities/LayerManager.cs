using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    private GameObject player;
    private GameObject emptyLayerPlayer;
    private MeshRenderer srPlayer;
    private SpriteRenderer srProps;
    private Transform emptyLayer;
    void Start()
    {
        player = GameObject.Find("Personnage Spine");
        emptyLayerPlayer = GameObject.Find("PlayerEmptyLayer");
        srPlayer = player.GetComponent<MeshRenderer>();
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
