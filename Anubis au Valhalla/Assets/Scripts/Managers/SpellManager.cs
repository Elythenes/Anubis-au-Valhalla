using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using Unity.VisualScripting;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    [Header("GENERAL")] 
    public static SpellManager instance; //singleton
    public GameObject targetUser;
    public KeyCode spell1;
    public KeyCode spell2;
    public LayerMask layerMonstres;
    public bool canCastSpells;

    [Header("SPELL SLOTS")] 
    public GameObject spellCollectManager;
    public List<SpellObject> containerSlot1 = new List<SpellObject>(2);
    public List<SpellObject> containerSlot2 = new List<SpellObject>(2);
    public SpellObject containerA;
    public GameObject prefabA;
    public SpellObject containerB;
    public GameObject prefabB;

    [Header("SPELL HIDDEN VAR")] 
    [SerializeField] public SpellStaticAreaObject spellSAo;
    [SerializeField] public SpellFollowingAreaObject spellFAo;
    [SerializeField] public SpellThrowingObject spellTo;
    public bool isSpell1fill = false;
    public bool isSpell2fill = false;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        canCastSpells = true;
    }
    
    void Update()
    {
        //SpellReplacement(containerSlot1);
        //SpellReplacement(containerSlot2);
        if (isSpell1fill)
        {
            SpellCooldownCalcul(ConvertSpellIndex(containerA));
            if (Input.GetKeyDown(spell1))
            {
                UseSpellSlot1(ConvertSpellIndex(containerA),1);
            }
        }

        if (isSpell2fill)
        {
            SpellCooldownCalcul(ConvertSpellIndex(containerB));
            if (Input.GetKeyDown(spell2))
            {
                Debug.Log("spell 2 inpuuut");
                UseSpellSlot1(ConvertSpellIndex(containerB),2);
            }
        }
        
    }

    
    //Spell System *****************************************************************************************************
    
    
    private enum SpellNumber
    {
        Fireball = 0,
        FireArea = 1,
        FuryOfSand = 2
    }
    
    void SpellReplacement(List<SpellObject> list)
    {
        if (list.Count >= 2)
        {
            list.RemoveRange(0,1);
        }
    }
    
    SpellNumber DetectSpellNumberInList(List<SpellObject> spellObjects) //récupère, à partir d'une liste d'un slot, l'index du spell (dans son SO) pour l'utiliser dans le switch UseSpellSlot1
    {
        var x = spellObjects[0].spellIndex;
        Debug.Log("utilisation du spell index "+ x);
        return (SpellNumber)x;
    }

    SpellNumber ConvertSpellIndex(SpellObject spellObject)
    {
        var x = spellObject.spellIndex;
        Debug.Log("utilisation du spell index "+ x);
        return (SpellNumber)x;
    }

    void UseSpellSlot1(SpellNumber spellNumber, int spellSlot)
    {
        switch (spellNumber)
        {
            case SpellNumber.Fireball:
                //Debug.Log("FIRE-BAAAAALL");
                ThrowingSpell(prefabA,spellSlot);
                break;
            
            case SpellNumber.FireArea:
                Debug.Log("FIRE-AREAAAAA");
                TimeLimitedSpell(prefabA,spellSlot);
                break;
            
            case SpellNumber.FuryOfSand:
                //Debug.Log("FURY OF SAAAAAAND");
                FollowingSpell(prefabA, spellSlot);
                break;
                
        }
    }
    
    
    
    void SpellCooldownCalcul(SpellNumber spellNumber) //pour calculer les CD de chaque spell (la fonction DEVRAIT se lancer que si on a le spell en question)
    {
        switch (spellNumber)
        {
            case SpellNumber.Fireball:
                //Debug.Log("CD Fireball");
                spellTo = prefabA.GetComponent<Fireball>().sOFireball;
                if (spellTo.cooldownTimer < spellTo.cooldown && !spellTo.canCast) //cooldown de la Fireball
                {
                    spellTo.cooldownTimer += Time.deltaTime;
                }
                else if (spellTo.cooldownTimer > spellTo.cooldown)
                {
                    spellTo.canCast = true;
                    spellTo.cooldownTimer = 0;
                }
                break;
            
            case SpellNumber.FireArea:
                //Debug.Log("CD Fire Area");
                spellSAo = prefabA.GetComponent<FlameArea>().sOFlameArea;
                if (spellSAo.cooldownTimer < spellSAo.cooldown && !spellSAo.canCast)
                {
                    spellSAo.cooldownTimer += Time.deltaTime;
                }
                else if (spellSAo.cooldownTimer > spellSAo.cooldown)
                {
                    spellSAo.canCast = true;
                    spellSAo.cooldownTimer = 0;
                }
                break;
            
            case SpellNumber.FuryOfSand:
                //Debug.Log("CD Fury of Sand");
                spellFAo = prefabA.GetComponent<HitboxSandstorm>().sOSandstorm;
                if (spellFAo.cooldownTimer < spellFAo.cooldown && !spellFAo.canCast) //cooldown du Sandstorm
                {
                    spellFAo.cooldownTimer += Time.deltaTime;
                }
                else if (spellFAo.cooldownTimer > spellFAo.cooldown)
                {
                    spellFAo.canCast = true;
                    spellFAo.cooldownTimer = 0;
                }
                break;
                
        }
    }
    
    //Script des spells ************************************************************************************************
    
    //Coroutine pour les spells qui doivent disparaître
    IEnumerator TimeLimitedGb(GameObject gbInstance, int timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gbInstance);
        Debug.Log("destroyed");
    }
    
    
    
    //Pour un Spell qui apparaît (et disparaît après une durée timerReload)
    void TimeLimitedSpell(GameObject gb/*, float timerReload*/, int slot)
    {
        if (slot == 1)
        {
            spellSAo = prefabA.GetComponent<FlameArea>().sOFlameArea;
        }
        if (slot == 2)
        {
            Debug.Log("test slot 2");
            spellSAo = prefabB.GetComponent<FlameArea>().sOFlameArea;
        }
        else
        {
            Debug.Log("erreur dans la fonction TimeLimitedSpell");
            
        }

        if (spellSAo.canCast)
        {
            spellSAo.canCast = false;
            var gbInstance = Instantiate(gb, new Vector3(targetUser.transform.position.x, targetUser.transform.position.y/*-(targetUser.transform.localScale.y/2)*/, 0), Quaternion.identity);
            Debug.Log("Spell1 used");
            StartCoroutine(TimeLimitedGb(gbInstance, spellSAo.duration));
        }
    }
    
    
    
    //Pour un Spell qui suit le joueur en permanence
    void FollowingSpell(GameObject gb, int slot)
    {
        if (slot == 1)
        {
            spellFAo = prefabA.GetComponent<HitboxSandstorm>().sOSandstorm;
        }
        else if (slot == 2)
        {
            spellFAo = prefabB.GetComponent<HitboxSandstorm>().sOSandstorm;
        }
        else
        {
            Debug.Log("erreur dans la fonction FollowingSpell");
        }

        if (spellFAo.canCast)
        {
            spellFAo.canCast = false;
            var gbInstance = Instantiate(gb,new Vector3(targetUser.transform.position.x, targetUser.transform.position.y/*-(targetUser.transform.localScale.y/2)*/, 0), Quaternion.identity, targetUser.transform);
            Debug.Log("Spell2 used");
            StartCoroutine(TimeLimitedGb(gbInstance, spellFAo.duration));
        }
        
    }

    
    
    void ThrowingSpell(GameObject gb, int slot)
    {
        if (slot == 1)
        {
            spellTo = prefabA.GetComponent<Fireball>().sOFireball;
        }
        else if (slot == 2)
        {
            spellTo = prefabB.GetComponent<Fireball>().sOFireball;
        }
        else
        {
            Debug.Log("erreur dans la fonction Throwing Spell");
        }
        spellTo.canCast = false;
        Vector2 mousePos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 charaPos = CharacterController.instance.transform.position;
        float angle = Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
        
        var gbInstance = Instantiate(gb, new Vector3(targetUser.transform.position.x,
            targetUser.transform.position.y+targetUser.transform.localScale.y/2, 0), Quaternion.AngleAxis(angle, Vector3.forward));
        StartCoroutine(TimeLimitedGb(gbInstance, spellTo.duration));
    }
}
