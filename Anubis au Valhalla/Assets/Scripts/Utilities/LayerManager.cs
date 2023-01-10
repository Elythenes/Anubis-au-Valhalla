using UnityEngine;

public class LayerManager : MonoBehaviour
{
    public GameObject player;
    public GameObject emptyLayerPlayer;
    public MeshRenderer srPlayer;
    public SpriteRenderer srProps;
    public Transform emptyLayer;
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
