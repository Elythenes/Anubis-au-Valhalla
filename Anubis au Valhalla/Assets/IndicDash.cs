using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicDash : MonoBehaviour
{
    public GameObject endPointRef;

    public IA_Valkyrie ia;
    // Start is called before the first frame update
    void Start()
    {
        ia = IA_Valkyrie.instance;
        var refe = Instantiate(endPointRef, Vector3.forward * ia.chargeDistance, Quaternion.identity,transform);
        refe.transform.localScale = new Vector3(1/transform.localScale.x,1,1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
