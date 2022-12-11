using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    [HideInInspector]public GameObject player;
    [HideInInspector]public GameObject emptyLayerPlayer;
    [HideInInspector]public MeshRenderer srPlayer;
    [HideInInspector]public SpriteRenderer srProps;
    [HideInInspector]public Transform emptyLayer;
    public virtual void Start()
    {
        player = GameObject.Find("Personnage Spine");
        emptyLayerPlayer = GameObject.Find("PlayerEmptyLayer");
        srPlayer = player.GetComponent<MeshRenderer>();
        srProps = GetComponent<SpriteRenderer>();
        emptyLayer = GetComponentInChildren<Transform>();
    }

    
    public virtual void Update()
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
