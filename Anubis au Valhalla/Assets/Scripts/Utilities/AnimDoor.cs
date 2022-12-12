
using UnityEngine;

public class AnimDoor : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SalleGennerator.instance.currentRoom.roomDone)
        {
            anim.SetBool("Open",true);
        }
    }
}