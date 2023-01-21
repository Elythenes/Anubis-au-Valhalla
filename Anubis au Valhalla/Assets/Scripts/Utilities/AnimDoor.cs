using GenPro;
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
        if (!SalleGenerator.Instance.currentRoom) return;
        if (SalleGenerator.Instance.currentRoom.roomDone)
        {
            anim.SetBool("Open",true);
        }
        else
        {
            anim.SetBool("Open",false);
        }
    }

    private void OnDisable()
    {
        anim.SetBool("Open",false);
        //anim.
    }
}
