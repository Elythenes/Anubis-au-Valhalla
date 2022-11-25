using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.VFX;


[CreateAssetMenu(fileName = "Potion" ,menuName = "System/Potion System/PotionObject")]
public class PotionObject : ScriptableObject
{
    [Header("GENERAL")] 
    public string nom;
    [TextArea(6, 20)] public string description;
    [TextArea(3, 20)] public string citation;
    public int index;

    [Header("GAME DESIGN")] 
    public int prix;
    
    [BoxGroup("BUFFS")] public int damage;
    [BoxGroup("BUFFS")] public int attackSpeed;
    [BoxGroup("BUFFS")] public int heal;
    [BoxGroup("BUFFS")] public int armor;
    [BoxGroup("BUFFS")] public int speed;
    [BoxGroup("BUFFS")] public int magicForce;
    
    [BoxGroup("DEBUFFS")] public int w_armor;
    [BoxGroup("DEBUFFS")] public int w_magicForce;
    
    [Header("GRAPH")]
    public Texture sprite;
    //public VFX vfx;


    /*[Header("GENERAL")]
    public PotionType type;
    public string nom;
    public int potionIndex;
    [TextArea(10,20)] public string description;
    public int buffAmount;
    public int buffDuration;
    public float cooldownTimer;
    public float cooldown;
    public bool canCast;
    public int valeurRestored;
    
    
    
    public enum PotionType
{
    InstantEffect,
    EffectOverTime
}
    
    */
}
