using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class FenrirBoss : MonoBehaviour
{
    private CharacterController player;
    public static FenrirBoss instance;
    public bool center;
    public bool left;
    public bool right;
    public bool back;
    public List<PuppetHealth> parts = new List<PuppetHealth>();
    public float reduceScaleTimer;
    public float deactivationRecoil;
    public float stunTime;
    public Color deactivatedColor = new Color(0.3f, 0.3f, 0.3f);
    public Color vulnerableColor;
    [BoxGroup("COUP DE GRIFFE")] public Vector3 clawWindUpPos0;
    [BoxGroup("COUP DE GRIFFE")] public Vector3 clawWindUpRotation0;
    [BoxGroup("COUP DE GRIFFE")] public Vector3 headWindUpPos0;
    [BoxGroup("COUP DE GRIFFE")] public Vector3 clawEndPos0;
    [BoxGroup("COUP DE GRIFFE")] public Vector3 clawEndRotation0;
    [BoxGroup("COUP DE GRIFFE")] public float windUpSpeed0;
    [BoxGroup("COUP DE GRIFFE")] public float waitTime0;
    [BoxGroup("COUP DE GRIFFE")] public float attackSpeed0;
    
    
    [BoxGroup("MORSURE")] public Vector3 clawWindUpPos1;
    [BoxGroup("MORSURE")] public Vector3 clawWindUpRotation1;
    [BoxGroup("MORSURE")] public Vector3 headWindUpPos1;
    [BoxGroup("MORSURE")] public Vector3 clawEndPos1;
    [BoxGroup("MORSURE")] public Vector3 clawEndRotation1;
    [BoxGroup("MORSURE")] public Vector3 headEndPos1;
    [BoxGroup("MORSURE")] public float windUpSpeed1;
    [BoxGroup("MORSURE")] public float waitTime1;
    [BoxGroup("MORSURE")] public float attackSpeed1;
    
    
    public enum State
    {
        Idle,
        Exposed,
        Attacking
    }
    
    public enum Phase
    {
        Normal,
        Furious
    }
    public enum ZoneType
    {
        Center,
        Left,
        Right,
        Back,
    }
    
    // Start is called before the first frame update
    public virtual void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(instance);
        }

        instance = this;
    }

    private void Start()
    {
        player = CharacterController.instance;
        parts.AddRange(GetComponentsInChildren<PuppetHealth>());
    }

    // Update is called once per frame


    public void CheckPaws()
    {
        if (!parts[1].canAttack && !parts[2].canAttack)
        {
            parts[0].StunHead();
            parts[0].canAttack = false;
            parts[0].isInvincible = false;
        }
    }

    public IEnumerator StunReset()
    {
        yield return new WaitForSeconds(stunTime);
        parts[1].Reactivate();
        parts[2].Reactivate();
        parts[0].HeadReset();
        
    }
}
