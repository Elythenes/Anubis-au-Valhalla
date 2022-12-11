using UnityEngine;
using UnityEngine.Rendering;

public class MultipleSpritesLayer : LayerManager
{
    // Start is called before the first frame update
    private SortingGroup srProps;

    public override void Start()
    {
        player = GameObject.Find("Personnage Spine");
        emptyLayerPlayer = GameObject.Find("PlayerEmptyLayer");
        srPlayer = player.GetComponent<MeshRenderer>();
        emptyLayer = GetComponentInChildren<Transform>();
        srProps = GetComponent<SortingGroup>();

    }
    public override void Update()
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
