using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NewLayerManager : LayerManager
{
    private SortingGroup _srProps;

    public override void Start()
    {
        player = GameObject.Find("Personnage Spine");
        emptyLayerPlayer = GameObject.Find("PlayerEmptyLayer");
        srPlayer = player.GetComponent<MeshRenderer>();
        emptyLayer = GetComponentInChildren<Transform>();
        _srProps = GetComponent<SortingGroup>();
    }
    
    public void Update()
    {
        _srProps.sortingOrder = (int)transform.position.y;
        
        if (emptyLayerPlayer.transform.position.y > emptyLayer.position.y)
        {
           _srProps.sortingLayerName = "PropsFrontPlayer";
        }
        else if (emptyLayerPlayer.transform.position.y < emptyLayer.position.y)
        {
            _srProps.sortingLayerName = "PropsBackPlayer";
        }
    }
}
