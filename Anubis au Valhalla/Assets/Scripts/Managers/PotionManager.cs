using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PotionManager : MonoBehaviour
{
    
   /* [Header("GENERAL")] 
    public static PotionManager instance;
    public AnubisCurrentStats anubis;
    public GameObject targetUser;
    public KeyCode potionKey;
    public bool canUsePotion;
    
    [Header("POTION SLOT")]
    public SpellObject containerA;
    public float cooldownSlot;
    public float cooldownSlotTimer;

    [Header("SPELL HIDDEN VAR")] 
   // [SerializeField] public InstantEffectObject spellIEo;
    [SerializeField] public EffectOverTimeObject spellEOTo;
    public bool isPotionFill = false;

    
     //Fonctions Système *************************************************************************************************
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        canUsePotion = true;
    }

    private void Start()
    {
        containerA = null;
    }

    void Update()
    {

        if (isPotionFill)
        {
            SpellCooldownCalcul(ConvertSpellIndex(containerA));
            if (Input.GetKeyDown(potionKey))
            {
                Debug.Log("spell 1 input");
                UseSpellSlot1(ConvertSpellIndex(containerA));
            }
        }
    }

    
    //Spell System *****************************************************************************************************
    
    
    private enum PotionNumber
    {
       Heal = 0,
       AttackBuff = 1,
    }

    PotionNumber ConvertSpellIndex(PotionObject potionObject)
    {
        var x = potionObject.potionIndex;
        Debug.Log("utilisation du spell index "+ x);
        return (PotionNumber)x;
    }

    void UseSpellSlot1(PotionNumber potionNumber, ScriptableObject potionObject)
    {
        switch (potionNumber)
            {
                case PotionNumber.Heal:
                  // HealPotion(containerA);
                    break;
                
                case PotionNumber.AttackBuff:
                    AttackBuffPotion(containerA);
                    break;
            }
    }
    
    void SpellCooldownCalcul(PotionNumber potionNumber) //pour calculer les CD de chaque spell (la fonction DEVRAIT se lancer que si on a le spell en question)
    {
        switch (potionNumber)
        {
            case PotionNumber.Heal:
                spellIEo = containerA;
                cooldownSlot = spellIEo.cooldown;
                
                if (spellIEo.cooldownTimer < spellIEo.cooldown && !spellIEo.canCast) //cooldown de la Fireball
                {
                    spellIEo.cooldownTimer += Time.deltaTime;
                    cooldownSlotTimer = spellIEo.cooldownTimer;
                }
                else if (spellIEo.cooldownTimer > spellIEo.cooldown)
                {
                    spellIEo.canCast = true;
                    spellIEo.cooldownTimer = 0;
                    cooldownSlotTimer = 0;
                }
                break;
        }
    }
    
    
    //Script des spells ************************************************************************************************

    IEnumerator EffectDuration(EffectOverTimeObject spellEOTo)
    {
        yield return new WaitForSeconds(spellEOTo.buffDuration);
        anubis.comboDamage[1] -= spellEOTo.buffAmount;
        anubis.comboDamage[2] -= spellEOTo.buffAmount;
        anubis.comboDamage[3] -= spellEOTo.buffAmount;
    }
    
    void HealPotion(InstantEffectObject spellIEo)
    {
        if (spellIEo.canCast)
        {
            cooldownSpellBar.instance.SetCooldownMax1();
            spellIEo.canCast = false;
            anubis.vieActuelle += spellIEo.valeurRestored;
        }
    }
    
    void AttackBuffPotion(EffectOverTimeObject spellEOTo)
    {
        if (spellEOTo.canCast)
        {
            cooldownSpellBar.instance.SetCooldownMax1();
            spellEOTo.canCast = false;
            anubis.comboDamage[1] += spellEOTo.buffAmount;
            anubis.comboDamage[2] += spellEOTo.buffAmount;
            anubis.comboDamage[3] += spellEOTo.buffAmount;
            StartCoroutine(EffectDuration(spellEOTo));
        }
    }*/
}
