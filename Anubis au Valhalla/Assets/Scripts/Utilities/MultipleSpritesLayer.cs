using UnityEngine;
using UnityEngine.Rendering;

public class MultipleSpritesLayer : LayerManager
{
    // Start is called before the first frame update
    private SortingGroup _srProps;

    public override void Start()
    {
        player = GameObject.Find("Personnage Spine");
        emptyLayerPlayer = GameObject.Find("PlayerEmptyLayer");
        srPlayer = player.GetComponent<MeshRenderer>();
        emptyLayer = GetComponentInChildren<Transform>();
        _srProps = GetComponent<SortingGroup>();
    }
    
    public override void Update()
    {
        if (emptyLayerPlayer.transform.position.y > emptyLayer.position.y)
        {
            _srProps.sortingOrder = srPlayer.sortingOrder + 1;
        }
        else
        {
            _srProps.sortingOrder = srPlayer.sortingOrder - 1;
        }
    }
}
