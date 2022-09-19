using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public KeyCode Spell1;
    public KeyCode Spell2;
    
    public GameObject TargetUser;

    public GameObject FlameArea;
    public GameObject SandstormArea;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(Spell1))
        {
            SkillUseTargetBottom(FlameArea);
        }
    }

    void SkillUseTargetBottom(GameObject gb)
    {
        Instantiate(gb, new Vector3(TargetUser.transform.position.x, TargetUser.transform.position.y-(TargetUser.transform.localScale.y/2), 0), quaternion.identity);
    }
}

