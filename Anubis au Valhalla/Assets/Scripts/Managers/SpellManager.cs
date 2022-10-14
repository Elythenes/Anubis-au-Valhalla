using System.Collections;
using System.Collections.Generic;
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
        if (Input.GetKeyDown(spell1))
        {
            //UseSpellSlot1(DetectSpellNumberInList(containerSlot1));
            UseSpellSlot1(ConvertSpellIndex(containerA));
        }
    }

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

    void UseSpellSlot1(SpellNumber spellNumber)
    {
        switch (spellNumber)
        {
            case SpellNumber.Fireball:
                //Debug.Log("FIRE-BAAAAALL");
                break;
            
            case SpellNumber.FireArea:
                //Debug.Log("FIRE-AREAAAAA");
                TimeLimitedSpell(prefabA,prefabA.GetComponent<FlameArea>().sOFlameArea);
                break;
            
            case SpellNumber.FuryOfSand:
                //Debug.Log("FURY OF SAAAAAAND");
                break;
                
        }
    }
    
    //Coroutine pour les spells qui doivent disparaître
    IEnumerator TimeLimitedGb(GameObject gbInstance, int timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gbInstance);
        Debug.Log("destroyed");
    }
    
    //Pour un Spell qui apparaît et disparaît après une durée timerReload
    void TimeLimitedSpell(GameObject gb,SpellStaticAreaObject spellSAo/*, float timerReload*/)
    {
        spellSAo.canCast = false;
        var gbInstance = Instantiate(gb, new Vector3(targetUser.transform.position.x, targetUser.transform.position.y/*-(targetUser.transform.localScale.y/2)*/, 0), Quaternion.identity);
        Debug.Log("Spell1 used");
        StartCoroutine(TimeLimitedGb(gbInstance, spellSAo.duration));
    }
}
