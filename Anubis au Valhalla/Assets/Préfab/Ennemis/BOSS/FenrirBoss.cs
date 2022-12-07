using System;
using System.Collections;
using System.Collections.Generic;
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
    void Update()
    {
        
    }
}
