using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skyfall : MonoBehaviour
{
    public BoxCollider2D col;

    public IA_Valkyrie ia;
    // Start is called before the first frame update
    void OnEnable()
    {
        ia = IA_Valkyrie.instance;
        transform.position = GetPoint();
        while (col.GetContacts(new List<ContactPoint2D>()) > 0)
        {
            transform.position = GetPoint();
        }
    }

    Vector3 GetPoint()
    {
        var point = Random.insideUnitCircle * ia.radiusWondering;
        point.x += CharacterController.instance.transform.position.x;
        point.y += CharacterController.instance.transform.position.y;
        return point;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
