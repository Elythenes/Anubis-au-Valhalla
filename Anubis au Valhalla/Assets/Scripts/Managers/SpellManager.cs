
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes.Test;
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
    public SpriteRenderer indicDirection;
    
    

    [Header("SPELL SLOTS")] 
    //public GameObject spellCollectManager;
    //public List<SpellObject> containerSlot1 = new List<SpellObject>(2);
    //public List<SpellObject> containerSlot2 = new List<SpellObject>(2);
    public SpellObject containerA;
    public GameObject prefabA;
    public SpellObject containerB;
    public GameObject prefabB;
    public float cooldownSlot1;
    public float cooldownSlotTimer1;
    public float cooldownSlot2;
    public float cooldownSlotTimer2;

    [Header("SPELL HIDDEN VAR")] 
    [SerializeField] public PouvoirFeuObject spellPFo;
    [SerializeField] public PouvoirFeu spellPFData;
    [SerializeField] public PouvoirPlaieObject spellPPo;
    [SerializeField] public PouvoirPlaie spellPPData;
    
    [SerializeField] public SpellStaticAreaObject spellSAo;
    [SerializeField] public SpellFollowingAreaObject spellFAo;
    [SerializeField] public SpellThrowingObject spellTo;
    [SerializeField] public SpellDefenceObject spellDo;
    [SerializeField] public SpellSpawnEntityObject spellSEo;
    public bool isSpell1Fill = false;
    public bool isSpell2Fill = false;
    

    //Fonctions Système *************************************************************************************************
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        canCastSpells = true;
    }

    private void Start()
    {
        containerA = null;
        prefabA = null;
        containerB = null;
        prefabB = null;
    }

    void Update()
    {
        //SpellReplacement(containerSlot1);
        //SpellReplacement(containerSlot2);
      
        if (isSpell1Fill)
        {
            SpellCooldownCalcul(ConvertSpellIndex(containerA), 1);
            if (Input.GetKeyDown(spell1))
            {
                Debug.Log("spell 1 input");
                UseSpellSlot1(ConvertSpellIndex(containerA),1);
            }
        }

        if (isSpell2Fill)
        {
            SpellCooldownCalcul(ConvertSpellIndex(containerB), 2);
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
        FuryOfSand = 2,
        Embaumement = 3,
        Ânkh = 4,
        Akh = 5,
        PlumeMaat = 6,
        OiseauBa = 7,
        SandWall = 8,
        NilWave = 9,
        Sepulture = 10,
        PouvoirFeu = 11,
        PouvoirPlaie = 12
    }

    SpellNumber ConvertSpellIndex(SpellObject spellObject)
    {
        var x = spellObject.spellIndex;
        Debug.Log("utilisation du spell index "+ x);
        return (SpellNumber)x;
    }

    void UseSpellSlot1(SpellNumber spellNumber, int spellSlot)
    {
        if (spellSlot == 1)
        {

            switch (spellNumber)
            {
                case SpellNumber.PouvoirFeu:
                    PouvoirFeuObject pouvoirFeuData1 = prefabA.GetComponent<PouvoirFeu>().soPouvoirFeu;
                    PouvoirFeu(spellSlot,pouvoirFeuData1);
                    break;
                
                case SpellNumber.PouvoirPlaie:
                    PouvoirPlaieObject pouvoirPlaieData1 = prefabA.GetComponent<PouvoirPlaie>().soPouvoirPlaie;
                    PouvoirPlaie(spellSlot,pouvoirPlaieData1);
                    break;
                
              /*  case SpellNumber.Fireball:
                    SpellThrowingObject FireballObj1 = prefabA.GetComponent<Fireball>().sOFireball;
                    ThrowingSpell(prefabA,spellSlot,FireballObj1);
                    break;*/
                
                case SpellNumber.OiseauBa:
                    SpellThrowingObject OiseauBaObj1 = prefabA.GetComponent<OiseauBa>().soOiseauBa;
                    ThrowingSpell(prefabA,spellSlot,OiseauBaObj1);
                    break;
                
               /* case SpellNumber.Embaumement:
                    SpellThrowingObject EmbaumementObj1 = prefabA.GetComponent<Embaumement>().sOEmbaumement;
                    ThrowingSpell(prefabA,spellSlot,EmbaumementObj1);
                    break;*/
            
                /*case SpellNumber.FireArea:
                    SpellStaticAreaObject FireAreaObj1 = prefabA.GetComponent<FlameArea>().sOFlameArea;
                    TimeLimitedSpell(prefabA,spellSlot,FireAreaObj1);
                    break;*/
                
               /* case SpellNumber.SandWall:
                    SpellSpawnEntityObject SandWallObj1 = prefabA.GetComponent<SandWall>().soSandWall;
                    SpawnEntity(prefabA,spellSlot,SandWallObj1);
                    break;*/
                
                case SpellNumber.NilWave:
                    SpellSpawnEntityObject NilWaveObj1 = prefabA.GetComponent<NilWave>().soNilWave;
                    SpawnEntity(prefabA,spellSlot,NilWaveObj1);
                    break;
                
               /* case SpellNumber.Sepulture:
                    SpellSpawnEntityObject SarcophageObj1 = prefabA.GetComponent<SarcophageBehaviour>().soSarcophage;
                    SpawnEntity(prefabA,spellSlot,SarcophageObj1);
                    break;*/

            
                case SpellNumber.Akh:
                    SpellStaticAreaObject AkhObj1 = prefabA.GetComponent<Akh>().soAkh;
                    TimeLimitedSpell(prefabA,spellSlot,AkhObj1);
                    break;
                
                case SpellNumber.PlumeMaat:
                    SpellStaticAreaObject PlumeObj1 = prefabA.GetComponent<PlumeMaat>().soPlumeMaat;
                    TimeLimitedSpell(prefabA,spellSlot,PlumeObj1);
                    break;
                
                case SpellNumber.FuryOfSand:
                    FollowingSpell(prefabA, spellSlot);
                    break;
                
               /*case SpellNumber.Ânkh:
                    SpellDefenceObject shieldObj1 = prefabA.GetComponent<AnkhShield>().sOAnkhShield;
                    Shield(prefabA, spellSlot, shieldObj1);
                    break;*/
            }
        }

        if (spellSlot == 2)
        {

            switch (spellNumber)
            {
                case SpellNumber.PouvoirFeu:
                    PouvoirFeuObject pouvoirFeuData = prefabB.GetComponent<PouvoirFeu>().soPouvoirFeu;
                    PouvoirFeu(spellSlot,pouvoirFeuData);
                    break;
                
                case SpellNumber.PouvoirPlaie:
                    PouvoirPlaieObject pouvoirPlaieData2 = prefabB.GetComponent<PouvoirPlaie>().soPouvoirPlaie;
                    PouvoirPlaie(spellSlot,pouvoirPlaieData2);
                    break;
                
                /*case SpellNumber.Fireball:
                    Debug.Log("FIRE-BAAAAALL");
                    SpellThrowingObject FireballObj2 = prefabB.GetComponent<Fireball>().sOFireball;
                    ThrowingSpell(prefabB,spellSlot,FireballObj2);
                    break;*/
                
                case SpellNumber.OiseauBa:
                    SpellThrowingObject OiseauBaObj2 = prefabB.GetComponent<OiseauBa>().soOiseauBa;
                    ThrowingSpell(prefabB,spellSlot,OiseauBaObj2);
                    break;
                
               /* case SpellNumber.Embaumement:
                    SpellThrowingObject EmbaumementObj2 = prefabB.GetComponent<Embaumement>().sOEmbaumement;
                    ThrowingSpell(prefabB,spellSlot,EmbaumementObj2);
                    break;*/

                /*case SpellNumber.FireArea:
                    SpellStaticAreaObject FireAreaObj2 = prefabB.GetComponent<FlameArea>().sOFlameArea;
                    TimeLimitedSpell(prefabB,spellSlot,FireAreaObj2);
                    break;*/
                
              /*  case SpellNumber.SandWall:
                    SpellSpawnEntityObject SandWallObj2 = prefabB.GetComponent<SandWall>().soSandWall;
                    SpawnEntity(prefabB,spellSlot,SandWallObj2);
                    break;*/
                
                case SpellNumber.NilWave:
                    SpellSpawnEntityObject NilWaveObj2 = prefabB.GetComponent<NilWave>().soNilWave;
                    SpawnEntity(prefabB,spellSlot,NilWaveObj2);
                    break;
                
               /* case SpellNumber.Sepulture:
                    SpellSpawnEntityObject SarcophageObj2 = prefabB.GetComponent<SarcophageBehaviour>().soSarcophage;
                    SpawnEntity(prefabB,spellSlot,SarcophageObj2);
                    break;*/
                
                case SpellNumber.Akh:
                    SpellStaticAreaObject AkhObj2 = prefabB.GetComponent<Akh>().soAkh;
                    TimeLimitedSpell(prefabB,spellSlot,AkhObj2);
                    break;
                
                case SpellNumber.PlumeMaat:
                    SpellStaticAreaObject PlumeObj2 = prefabB.GetComponent<PlumeMaat>().soPlumeMaat;
                    TimeLimitedSpell(prefabB,spellSlot,PlumeObj2);
                    break;
            
                case SpellNumber.FuryOfSand:
                    Debug.Log("FURY OF SAAAAAAND");
                    FollowingSpell(prefabB, spellSlot);
                    break;
                
               /* case SpellNumber.Ânkh:
                    SpellDefenceObject shieldObj1 = prefabB.GetComponent<AnkhShield>().sOAnkhShield;
                    Shield(prefabA, spellSlot, shieldObj1);
                    break;*/
            }
        }
    }
    
    
    
    void SpellCooldownCalcul(SpellNumber spellNumber, int slotNumber) //pour calculer les CD de chaque spell (la fonction DEVRAIT se lancer que si on a le spell en question)
    {
        switch (spellNumber)
        {
           /* case SpellNumber.Fireball:
                if (slotNumber == 1)
                {
                    spellTo = prefabA.GetComponent<Fireball>().sOFireball;
                    cooldownSlot1 = spellTo.cooldown;
                }

                if (slotNumber == 2)
                {
                    spellTo = prefabB.GetComponent<Fireball>().sOFireball;
                    cooldownSlot2 = spellTo.cooldown;
                }
                if (spellTo.cooldownTimer < spellTo.cooldown && !spellTo.canCast) //cooldown de la Fireball
                {
                    spellTo.cooldownTimer += Time.deltaTime;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = spellTo.cooldownTimer;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = spellTo.cooldownTimer;
                    }
                }
                else if (spellTo.cooldownTimer > spellTo.cooldown)
                {
                    spellTo.canCast = true;
                    spellTo.cooldownTimer = 0;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = 0;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = 0;
                    }
                }
                break;*/
            
            case SpellNumber.PouvoirFeu:
                if (slotNumber == 1)
                {
                    spellPFData = UiManager.instance.currentSpell1Holder.GetComponent<PouvoirFeu>();
                    spellPFo = prefabA.GetComponent<PouvoirFeu>().soPouvoirFeu;
                    cooldownSlot1 = spellPFo.duration;
                }

                if (slotNumber == 2)
                {
                    spellPFData = UiManager.instance.currentSpell2Holder.GetComponent<PouvoirFeu>();
                    spellPFo = prefabB.GetComponent<PouvoirFeu>().soPouvoirFeu;
                    cooldownSlot2 = spellPFo.duration;
                }
                
                if (spellPFData.secondesRestantes > 0)
                {
                    spellPFo.canCast = true;

                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = spellPFData.secondesRestantes;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = spellPFData.secondesRestantes;
                    }
                }
                else
                {
                    spellPFo.canCast = false;

                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = 0;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = 0;
                    }
                }
                break;
            
           case SpellNumber.PouvoirPlaie:
               if (slotNumber == 1)
               {
                   spellPPData = UiManager.instance.currentSpell1Holder.GetComponent<PouvoirPlaie>();
                   spellPPo = prefabA.GetComponent<PouvoirPlaie>().soPouvoirPlaie;
                   cooldownSlot1 = spellPPo.duration;
               }

               if (slotNumber == 2)
               {
                   spellPPData = UiManager.instance.currentSpell2Holder.GetComponent<PouvoirPlaie>();
                   spellPPo = prefabB.GetComponent<PouvoirPlaie>().soPouvoirPlaie;
                   cooldownSlot2 = spellPPo.duration;
               }
                
               if (spellPPData.secondesRestantes > 0)
               {
                   spellPPo.canCast = true;

                   if (slotNumber == 1)
                   {
                       cooldownSlotTimer1 = spellPPData.secondesRestantes;
                   }

                   if (slotNumber == 2)
                   {
                       cooldownSlotTimer2 = spellPPData.secondesRestantes;
                   }
               }
               else
               {
                   spellPPo.canCast = false;

                   if (slotNumber == 1)
                   {
                       cooldownSlotTimer1 = 0;
                   }

                   if (slotNumber == 2)
                   {
                       cooldownSlotTimer2 = 0;
                   }
               }
               break;
            
            case SpellNumber.OiseauBa:
                if (slotNumber == 1)
                {
                    spellTo = prefabA.GetComponent<OiseauBa>().soOiseauBa;
                    cooldownSlot1 = spellTo.cooldown;
                }

                if (slotNumber == 2)
                {
                    spellTo = prefabB.GetComponent<OiseauBa>().soOiseauBa;
                    cooldownSlot2 = spellTo.cooldown;
                }
                if (spellTo.cooldownTimer < spellTo.cooldown && !spellTo.canCast) //cooldown de la Fireball
                {
                    spellTo.cooldownTimer += Time.deltaTime;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = spellTo.cooldownTimer;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = spellTo.cooldownTimer;
                    }
                }
                else if (spellTo.cooldownTimer > spellTo.cooldown)
                {
                    spellTo.canCast = true;
                    spellTo.cooldownTimer = 0;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = 0;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = 0;
                    }
                }
                break;
            
            /*case SpellNumber.FireArea:
                if (slotNumber == 1)
                {
                    spellSAo = prefabA.GetComponent<FlameArea>().sOFlameArea;
                    cooldownSlot1 = spellSAo.cooldown;
                }

                if (slotNumber == 2)
                {
                    spellSAo = prefabB.GetComponent<FlameArea>().sOFlameArea;
                    cooldownSlot2 = spellSAo.cooldown;
                }
                if (spellSAo.cooldownTimer < spellSAo.cooldown && !spellSAo.canCast)
                {
                    spellSAo.cooldownTimer += Time.deltaTime;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = spellSAo.cooldownTimer;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = spellSAo.cooldownTimer;
                    }
                    
                }
                else if (spellSAo.cooldownTimer > spellSAo.cooldown)
                {
                    spellSAo.canCast = true;
                    spellSAo.cooldownTimer = 0;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = 0;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = 0;
                    }
                }
                break;*/
            
            case SpellNumber.Akh:
                if (slotNumber == 1)
                {
                    spellSAo = prefabA.GetComponent<Akh>().soAkh;
                    cooldownSlot1 = spellSAo.cooldown;
                }

                if (slotNumber == 2)
                {
                    spellSAo = prefabB.GetComponent<Akh>().soAkh;
                    cooldownSlot2 = spellSAo.cooldown;
                }
                if (spellSAo.cooldownTimer < spellSAo.cooldown && !spellSAo.canCast)
                {
                    spellSAo.cooldownTimer += Time.deltaTime;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = spellSAo.cooldownTimer;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = spellSAo.cooldownTimer;
                    }
                    
                }
                else if (spellSAo.cooldownTimer > spellSAo.cooldown)
                {
                    spellSAo.canCast = true;
                    spellSAo.cooldownTimer = 0;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = 0;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = 0;
                    }
                }
                break;

            case SpellNumber.PlumeMaat:
                if (slotNumber == 1)
                {
                    spellSAo = prefabA.GetComponent<PlumeMaat>().soPlumeMaat;
                    cooldownSlot1 = spellSAo.cooldown;
                }

                if (slotNumber == 2)
                {
                    spellSAo = prefabB.GetComponent<PlumeMaat>().soPlumeMaat;
                    cooldownSlot2 = spellSAo.cooldown;
                }
                if (spellSAo.cooldownTimer < spellSAo.cooldown && !spellSAo.canCast)
                {
                    spellSAo.cooldownTimer += Time.deltaTime;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = spellSAo.cooldownTimer;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = spellSAo.cooldownTimer;
                    }
                    
                }
                else if (spellSAo.cooldownTimer > spellSAo.cooldown)
                {
                    spellSAo.canCast = true;
                    spellSAo.cooldownTimer = 0;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = 0;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = 0;
                    }
                }
                break;
            
            case SpellNumber.FuryOfSand:
                //Debug.Log("CD Fury of Sand");
                if (slotNumber == 1)
                {
                    spellFAo = prefabA.GetComponent<HitboxSandstorm>().sOSandstorm;
                    cooldownSlot1 = spellFAo.cooldown;
                }

                if (slotNumber == 2)
                {
                    spellFAo = prefabB.GetComponent<HitboxSandstorm>().sOSandstorm;
                    cooldownSlot2 = spellFAo.cooldown;
                }
                
                if (spellFAo.cooldownTimer < spellFAo.cooldown && !spellFAo.canCast) //cooldown du Sandstorm
                {
                    spellFAo.cooldownTimer += Time.deltaTime;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = spellFAo.cooldownTimer;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = spellFAo.cooldownTimer;
                    }
                }
                else if (spellFAo.cooldownTimer > spellFAo.cooldown)
                {
                    spellFAo.canCast = true;
                    spellFAo.cooldownTimer = 0;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = 0;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = 0;
                    }
                }
                break;
                
            /*case SpellNumber.Embaumement:
                if (slotNumber == 1)
                {
                    spellTo = prefabA.GetComponent<Embaumement>().sOEmbaumement;
                    cooldownSlot1 = spellTo.cooldown;
                }

                if (slotNumber == 2)
                {
                    spellTo = prefabB.GetComponent<Embaumement>().sOEmbaumement;
                    cooldownSlot2 = spellTo.cooldown;
                }
                if (spellTo.cooldownTimer < spellTo.cooldown && !spellTo.canCast) 
                {
                    spellTo.cooldownTimer += Time.deltaTime;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = spellTo.cooldownTimer;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = spellTo.cooldownTimer;
                    }
                }
                else if (spellTo.cooldownTimer > spellTo.cooldown)
                {
                    spellTo.canCast = true;
                    spellTo.cooldownTimer = 0;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = 0;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = 0;
                    }
                }
                break;*/
            
           /* case SpellNumber.Ânkh:
                if (slotNumber == 1)
                {
                    spellDo = prefabA.GetComponent<AnkhShield>().sOAnkhShield;
                    cooldownSlot1 = spellDo.secondesTotales;
                }

                if (slotNumber == 2)
                {
                    spellDo = prefabB.GetComponent<AnkhShield>().sOAnkhShield;
                    cooldownSlot2 = spellDo.secondesTotales;
                }
                if (shieldData.secondesRestantes > 0)
                {
                    spellDo.canCast = true;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = shieldData.secondesRestantes;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = shieldData.secondesRestantes;
                    }
                }
                else
                {
                    spellDo.canCast = false;
                    spellDo.cooldownTimer = 0;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = 0;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = 0;
                    }
                }
                break;*/
            
            /*case SpellNumber.SandWall:
                if (slotNumber == 1)
                {
                    spellSEo = prefabA.GetComponent<SandWall>().soSandWall;
                    cooldownSlot1 = spellSEo.cooldown;
                }

                if (slotNumber == 2)
                {
                    spellSEo = prefabB.GetComponent<SandWall>().soSandWall;
                    cooldownSlot2 = spellSEo.cooldown;
                }
                if (spellSEo.cooldownTimer < spellSEo.cooldown && !spellSEo.canCast)
                {
                    spellSEo.cooldownTimer += Time.deltaTime;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = spellSEo.cooldownTimer;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = spellSEo.cooldownTimer;
                    }
                    
                }
                else if (spellSEo.cooldownTimer > spellSEo.cooldown)
                {
                    spellSEo.canCast = true;
                    spellSEo.cooldownTimer = 0;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = 0;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = 0;
                    }
                }
                break;*/
            
            case SpellNumber.NilWave:
                if (slotNumber == 1)
                {
                    spellSEo = prefabA.GetComponent<NilWave>().soNilWave;
                    cooldownSlot1 = spellSEo.cooldown;
                }

                if (slotNumber == 2)
                {
                    spellSEo = prefabB.GetComponent<NilWave>().soNilWave;
                    cooldownSlot2 = spellSEo.cooldown;
                }
                if (spellSEo.cooldownTimer < spellSEo.cooldown && !spellSEo.canCast)
                {
                    spellSEo.cooldownTimer += Time.deltaTime;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = spellSEo.cooldownTimer;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = spellSEo.cooldownTimer;
                    }
                    
                }
                else if (spellSEo.cooldownTimer > spellSEo.cooldown)
                {
                    spellSEo.canCast = true;
                    spellSEo.cooldownTimer = 0;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = 0;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = 0;
                    }
                }
                break;
            
            /*case SpellNumber.Sepulture:
                if (slotNumber == 1)
                {
                    spellSEo = prefabA.GetComponent<SarcophageBehaviour>().soSarcophage;
                    cooldownSlot1 = spellSEo.cooldown;
                }

                if (slotNumber == 2)
                {
                    spellSEo = prefabB.GetComponent<SarcophageBehaviour>().soSarcophage;
                    cooldownSlot2 = spellSEo.cooldown;
                }
                if (spellSEo.cooldownTimer < spellSEo.cooldown && !spellSEo.canCast)
                {
                    spellSEo.cooldownTimer += Time.deltaTime;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = spellSEo.cooldownTimer;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = spellSEo.cooldownTimer;
                    }
                    
                }
                else if (spellSEo.cooldownTimer > spellSEo.cooldown)
                {
                    spellSEo.canCast = true;
                    spellSEo.cooldownTimer = 0;
                    
                    if (slotNumber == 1)
                    {
                        cooldownSlotTimer1 = 0;
                    }

                    if (slotNumber == 2)
                    {
                        cooldownSlotTimer2 = 0;
                    }
                }
                break;*/
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
    void TimeLimitedSpell(GameObject gb/*, float timerReload*/, int slot,SpellStaticAreaObject spellSAo)
    {
        if (spellSAo.canCast)
        {
            if (slot == 1)
            {
                cooldownSpellBar.instance.SetCooldownMax1();
            }
            if (slot == 2)
            {
                cooldownSpellBar2.instance.SetCooldownMax2();
            }
            spellSAo.canCast = false;
            var gbInstance = Instantiate(gb, new Vector3(targetUser.transform.position.x, targetUser.transform.position.y/*-(targetUser.transform.localScale.y/2)*/, 0), Quaternion.identity);
            Debug.Log("Spell1 used");
            StartCoroutine(TimeLimitedGb(gbInstance, spellSAo.duration));
        }
    }
    
    void PouvoirFeu(int slot,PouvoirFeuObject spellPFo)
    {
        if (!spellPFData.isActive && !spellPFData.lockCast)
        {
            if (slot == 1)
            {
                cooldownSpellBar.instance.SetCooldownMax1();
            }
            if (slot == 2)
            {
                cooldownSpellBar2.instance.SetCooldownMax2();
            }
            //indicDirection.color = Color.red;
            spellPFData.isActive = true;
            spellPFo.canCast = true;
        }
        
        if(!spellPFData.isActive && !spellPFData.lockCast)
        {
            //indicDirection.color = Color.white;
            spellPFData.isActive = false;
            spellPFo.canCast = false;
        }
    }
    
    void PouvoirPlaie(int slot,PouvoirPlaieObject spellPPo)
    {
        if (!spellPPData.isActive && !spellPPData.lockCast)
        {
            if (slot == 1)
            {
                cooldownSpellBar.instance.SetCooldownMax1();
            }
            if (slot == 2)
            {
                cooldownSpellBar2.instance.SetCooldownMax2();
            }
            //indicDirection.color = Color.red;
            spellPPData.isActive = true;
            spellPPo.canCast = true;
        }
        
        if(!spellPPData.isActive && !spellPPData.lockCast)
        {
            //indicDirection.color = Color.white;
            spellPPData.isActive = false;
            spellPPo.canCast = false;
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

    
    
    void ThrowingSpell(GameObject gb, int slot, SpellThrowingObject spellTo)
    {
        if (spellTo.canCast)
        {
            if (slot == 1)
            {
                cooldownSpellBar.instance.SetCooldownMax1();
            }
            if (slot == 2)
            {
                cooldownSpellBar2.instance.SetCooldownMax2();
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

    /*public void Shield(GameObject gb, int slot, SpellDefenceObject spellDo)
    {
        if (spellDo.canCast && !shieldData.isActive)
        {
            shield.SetActive(true);
            shieldData.isActive = true;
            DamageManager.instance.isAnkh = true;
        }
        else if (spellDo.canCast && shieldData.isActive)
        {
            shield.SetActive(false);
            shieldData.isActive = false;
            DamageManager.instance.isAnkh = false;
        }
        
    }*/
    
    public void SpawnEntity(GameObject gb, int slot, SpellSpawnEntityObject spellSEo)
    {
        if (spellSEo.canCast)
        {
            if (slot == 1)
            {
                cooldownSpellBar.instance.SetCooldownMax1();
            }
            if (slot == 2)
            {
                cooldownSpellBar2.instance.SetCooldownMax2();
            }
            spellSEo.canCast = false;
            Vector2 mousePos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 charaPos = targetUser.transform.position;
            float angle = Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
            float distancePlayerFloat = Vector2.Distance(charaPos, mousePos);
            Vector2 vectorDirection = mousePos - charaPos;
            Vector2 vectorDirectionNormalized = vectorDirection.normalized;

            if (distancePlayerFloat >= spellSEo.maxDistanceSpawn)
            {
                var gbInstance = Instantiate(gb, (Vector2)targetUser.transform.position + vectorDirectionNormalized * spellSEo.maxDistanceSpawn, Quaternion.AngleAxis(angle, Vector3.forward));
                StartCoroutine(TimeLimitedGb(gbInstance, spellSEo.duration));
                Debug.Log("not in range");
            }
            else
            {
                var gbInstance = Instantiate(gb, mousePos, Quaternion.AngleAxis(angle, Vector3.forward));
                StartCoroutine(TimeLimitedGb(gbInstance, spellSEo.duration));
                Debug.Log("in range");
            }
         
        }
    }
}
