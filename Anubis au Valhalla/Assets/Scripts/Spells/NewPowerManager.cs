using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;


public class NewPowerManager : MonoBehaviour
{
    public static NewPowerManager Instance;
    public GameObject targetUser;
    public KeyCode keyPower1;
    public KeyCode keyPower2;
    public LayerMask layerMonstres;
    
    public CharacterController cc;
    public AttaquesNormales an;
    
    [Header("SYSTEM")]
    [Range(1,10)] public int currentLevelPower1;
    [Range(1,10)] public int currentLevelPower2;

    public List<GameObject> powersCollected = new();

    public List<int> p1ComboConeDamages = new(10);
    public List<float> p1ComboConeReaches = new(10);
    public List<float> p1ComboConeDurations = new(10);
    public List<int> p1ThrustBallDamages = new(10);
    public List<float> p1ThrustBallVelocities = new(10);
    public List<int> p1DashContactDamages = new(10);
    public List<float> p1DashContactStaggers = new(10);

    public List<int> p2ComboWaveDamages = new(10);
    public List<float> p2ComboWaveRadiuses = new(10);
    public List<int> p2ThrustBandageDamages = new(10);
    public List<float> p2ThrustBandageSizes = new(10);
    public List<int> p2DashTrailDamagesPerTick = new(10);
    public List<float> p2DashTrailDurations = new(10);

    [Header("UTILISATION")]
    public float durationPower1 = 8f;
    public float cooldownPower1 = 12f;
    public float currentDurationPower1;
    public float currentCooldownPower1;
    private IEnumerator tempsP1;
    
    public float durationPower2 = 8f;
    public float cooldownPower2 = 12f;
    public float currentDurationPower2;
    public float currentCooldownPower2;
    private IEnumerator tempsP2;


    [Header("VFX")]
    public SpriteRenderer VFXAme;
    public float ameValue;
    public bool isAmeActive;
    public SpriteRenderer VFXSable;
    public float sableValue;
    public bool isSableActive;
    
    [Header("DEBUG / Test")] 
    public int startingLevelPower1 = 1;
    public int startingLevelPower2 = 1;

    public bool testCustomLevel;
    public List<GameObject> testPowersCollected = new();
    public KeyCode testCustomKey = KeyCode.K;

    [Header("DEBUG / Var")] 
    public bool canUsePowers;
    public bool canUsePower1;
    public bool canUsePower2;

    public bool isPower1Active;
    public bool isPower2Active;

    public bool earlyDisablePower1;
    public bool earlyDisablePower2;

    [Foldout("p1ComboCone")] public GameObject p1ComboConeHitbox;
    [Foldout("p1ComboCone")] public int p1ComboConeDamage;
    [Foldout("p1ComboCone")] public float p1ComboConeReach;
    [Foldout("p1ComboCone")] public float p1ComboConeDuration;
    [Foldout("p1ComboCone")] public bool p1ComboConeStagger;
    [Foldout("p1ComboCone")] public bool p1ComboConeHalfSphere;
    [Foldout("p1ComboCone")] public bool p1ComboConeInversedCone;
    
    [Foldout("p1ThrustBall")] public GameObject p1ThrusrBallHitbox;
    [Foldout("p1ThrustBall")] public int p1ThrustBallDamage;
    [Foldout("p1ThrustBall")] public float p1ThrustBallVelocity;
    [Foldout("p1ThrustBall")] public float p1ThrustSize1;
    [Foldout("p1ThrustBall")] public float p1ThrustSize2;
    [Foldout("p1ThrustBall")] public bool p1ThrustExplosionSize;
    [Foldout("p1ThrustBall")] public bool p1ThrustBallTriple;
    [Foldout("p1ThrustBall")] public bool p1ThrustBallExecute;
    
    [Foldout("p1DashContact")] public GameObject p1DashContactHitbox;
    [Foldout("p1DashContact")] public float p1DashContactDamage;
    [Foldout("p1DashContact")] public float p1DashContactSlowDuration;
    [Foldout("p1DashContact")] public bool p1DashContactSlowForce;
    [Foldout("p1DashContact")] public bool p1DashContactStagger;
    [Foldout("p1DashContact")] public bool p1DashContactPowerExtend;
    
    [Foldout("p2ComboWave")] public GameObject p2ComboWaveHitbox;
    [Foldout("p2ComboWave")] public float p2ComboWaveDuration;
    [Foldout("p2ComboWave")] public float p2ComboWaveDamage;
    [Foldout("p2ComboWave")] public float p2ComboWaveRadius;
    [Foldout("p2ComboWave")] public bool p2ComboWaveSoul;
    [Foldout("p2ComboWave")] public bool p2ComboWaveDeathExplosion;
    [Foldout("p2ComboWave")] public bool p2ComboWaveDouble;
    
    [Foldout("p2ThrustBandage")] public GameObject p2ThrustBandageHitbox;
    [Foldout("p2ThrustBandage")] public float p2ThrustBandageDamage;
    [Foldout("p2ThrustBandage")] public float p2ThrustBandageSpeed;
    [Foldout("p2ThrustBandage")] public float p2ThrustBandageSize;
    [Foldout("p2ThrustBandage")] public int p2ThrustBandageMaxHit = 1;
    [Foldout("p2ThrustBandage")] public bool p2ThrustBandageStunUp;
    
    [Foldout("p2DashTrail")] public GameObject p2DashTrailHitbox;
    [Foldout("p2DashTrail")] public float timerSpawn;
    [Foldout("p2DashTrail")] public float timerSpawnMax;
    [Foldout("p2DashTrail")] public float p2DashTrailDamagePerTick;
    [Foldout("p2DashTrail")] public float p2DashTrailDuration;
    [Foldout("p2DashTrail")] public float p2DashTrailEspacementDoT;
    [Foldout("p2DashTrail")] public float p2DashTrailSize;
    [Foldout("p2DashTrail")] public bool p2DashTrailMiniStagger;
    [Foldout("p2DashTrail")] public bool p2DashTrailInfection;
    
    
    [Header("TEXTS")] 
    [Foldout("NEW POWER 1 TEXT")] public string nomPower1;
    [Foldout("NEW POWER 1 TEXT")] [TextArea(10,20)] public string descriptionGlobalPower1;
    [Foldout("NEW POWER 1 TEXT")] [TextArea(3, 10)] public string citationPower1;
    [Foldout("NEW POWER 1 TEXT")] public string nomComboPower1;
    [Foldout("NEW POWER 1 TEXT")] [TextArea(6,20)] public string descriptionComboPower1;
    [Foldout("NEW POWER 1 TEXT")] public string nomThrustPower1;
    [Foldout("NEW POWER 1 TEXT")] [TextArea(6,20)] public string descriptionThrustPower1;
    [Foldout("NEW POWER 1 TEXT")] public string nomDashPower1;
    [Foldout("NEW POWER 1 TEXT")] [TextArea(6,20)] public string descriptionDashPower1;
    
    [Foldout("NEW POWER 2 TEXT")] public string nomPower2;
    [Foldout("NEW POWER 2 TEXT")] [TextArea(10,20)] public string descriptionGlobalPower2;
    [Foldout("NEW POWER 2 TEXT")] [TextArea(3, 10)] public string citationPower2;
    [Foldout("NEW POWER 2 TEXT")] public string nomComboPower2;
    [Foldout("NEW POWER 2 TEXT")] [TextArea(6,20)] public string descriptionComboPower2;
    [Foldout("NEW POWER 2 TEXT")] public string nomThrustPower2;
    [Foldout("NEW POWER 2 TEXT")] [TextArea(6,20)] public string descriptionThrustPower2;
    [Foldout("NEW POWER 2 TEXT")] public string nomDashPower2;
    [Foldout("NEW POWER 2 TEXT")] [TextArea(6,20)] public string descriptionDashPower2;
    
    
    //Fonctions système ************************************************************************************************

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        cc = CharacterController.instance; 
        an = AttaquesNormales.instance;
        
        canUsePowers = true;
        canUsePower1 = true;
        canUsePower2 = true;

        currentLevelPower1 = startingLevelPower1;
        currentLevelPower2 = startingLevelPower2;
    }
    
    
    void Update()
    {
        if (isAmeActive && ameValue < 1)
        {
            ameValue += 0.003f;
            VFXAme.material.SetFloat("_ALPHA",ameValue);
        }
        else if (!isAmeActive && ameValue > 0)
        {
            ameValue -= 0.003f;
            VFXAme.material.SetFloat("_ALPHA",ameValue);
        }
        
        if (isSableActive && sableValue < 1)
        {
            sableValue += 0.003f;
            VFXSable.material.SetFloat("_ALPHA",sableValue);
        }
        else if (!isSableActive && sableValue > 0)
        {
            sableValue -= 0.003f;
            VFXSable.material.SetFloat("_ALPHA",sableValue);
        }
        
        if (testCustomLevel && Input.GetKeyDown(testCustomKey))
        {
            foreach (var gb in testPowersCollected)
            {
                Instantiate(gb);
            }
        }
        ActivePower();
        CheckPower();
    }


    //Fonctions Powers *************************************************************************************************

    public void PowerLevelUp(GameObject powerRepo)
    {
        switch(powerRepo.GetComponent<NewPowerRepository>().newPowerType)
        {
            case NewPowerType.None:
                Debug.Log("pas de level up si PowerType = None");
                break;
            
            case NewPowerType.Power1:
                currentLevelPower1++;
                break;
            
            case NewPowerType.Power2:
                currentLevelPower2++;
                break;
        }
        PowerLevelUnlock();
        
    }

    void PowerLevelUnlock()
    {
        switch (currentLevelPower1)
        {
            /*case 1:
                UiThotManager.Instance.generalAmorce = "Pouvoir du Sable Amélioré \n \nAugmente les dégats du pouvoir.";
                break;
            case 2:
                UiThotManager.Instance.generalAmorce = "Pouvoir du Sable Amélioré \n \nAugmente les dégats du pouvoir.";
                break;*/
            case 3:
                //UiThotManager.Instance.generalAmorce = "Pouvoir du Sable Amélioré \n \nEffets supplémentaires, consulter l'inventaire.";
                p1ThrustExplosionSize = true;
                p1ComboConeStagger = true;
                p1DashContactSlowForce = true;
                break;
         /*   case 4:
                UiThotManager.Instance.generalAmorce = "Pouvoir du Sable Amélioré \n \nAugmente la portée du pouvoir.";
                break;*/
            case 5:
                //UiThotManager.Instance.generalAmorce = "Pouvoir du Sable Amélioré \n \nNouveaux effets spéciaux, consulter l'inventaire.";
                p1ComboConeHalfSphere = true;
                p1ThrustBallTriple = true;
                p1DashContactStagger = true;
                break;
           /* case 6:
                UiThotManager.Instance.generalAmorce = "Pouvoir du Sable Amélioré \n \nAugmente les dégats du pouvoir.";
                break;
            case 7:
                UiThotManager.Instance.generalAmorce = "Pouvoir du Sable Amélioré \n \nAugmente la portée du pouvoir.";
                break;*/
            case 8:
                //UiThotManager.Instance.generalAmorce = "Pouvoir du Sable Amélioré \n \nNouveaux effets spéciaux, consulter l'inventaire.";
                p1ComboConeInversedCone = true;
                p1ThrustBallExecute = true;
                p1DashContactPowerExtend = true;
                break;
           /*case 9:
                UiThotManager.Instance.generalAmorce = "Pouvoir du Sable Amélioré \n \nAugmente les dégats du pouvoir.";
                break;
            case 10:
                UiThotManager.Instance.generalAmorce = "Pouvoir du Sable Amélioré \n \nAugmente les dégats et la portée du pouvoir.";
                break;*/
        }
        switch (currentLevelPower2)
        {
           /* case 1:
                UiThotManager.Instance.generalAmorce = "Pouvoir des Ames Amélioré \n \nAugmente les dégats du pouvoir.";
                break;
            case 2:
                UiThotManager.Instance.generalAmorce = "Pouvoir des Ames Amélioré \n \nAugmente les dégats du pouvoir.";
                break;*/
            case 3:
               // UiThotManager.Instance.generalAmorce = "Pouvoir des Ames Amélioré \n \nNouveaux effets spéciaux, consulter l'inventaire.";
                p2ComboWaveSoul = true;
                p2ThrustBandageMaxHit = 2;
                p2DashTrailSize *= 1.3f;
                break;
           /* case 4:
                UiThotManager.Instance.generalAmorce = "Pouvoir des Ames Amélioré \n \nAugmente la portée du pouvoir.";
                break;*/
            case 5:
               // UiThotManager.Instance.generalAmorce = "Pouvoir des Ames Amélioré \n \nNouveaux effets spéciaux, consulter l'inventaire.";
                p2ComboWaveDeathExplosion = true;
                p2ThrustBandageMaxHit = 100;
                p2DashTrailMiniStagger = true;
                break;
           /* case 6:
                UiThotManager.Instance.generalAmorce = "Pouvoir des Ames Amélioré \n \nAugmente les dégats du pouvoir.";
                break;
            case 7:
                UiThotManager.Instance.generalAmorce = "Pouvoir des Ames Amélioré \n \nAugmente la portée du pouvoir.";
                break;*/
            case 8:
                //UiThotManager.Instance.generalAmorce = "Pouvoir des Ames Amélioré \n \nNouveaux effets spéciaux, consulter l'inventaire.";
                p2ComboWaveDouble = true;
                p2ThrustBandageStunUp = true;
                p2DashTrailInfection = true;
                break;
            /*case 9:
                UiThotManager.Instance.generalAmorce = "Pouvoir des Ames Amélioré \n \nAugmente les dégats du pouvoir.";
                break;
            case 10:
                UiThotManager.Instance.generalAmorce = "Pouvoir des Ames Amélioré \n \nAugmente les dégats et la portée du pouvoir.";
                break;*/
        }
    }

    void ActivePower() //sert de base au système de CD, à savoir : activer un power, le désactiver, ou switch un avec l'autre
    {
        if (Input.GetKeyDown(keyPower1))
        {
            if (canUsePower1)
            {
                if (isPower2Active) //si on switch à p1 alors que p2 était actif
                {
                    earlyDisablePower2 = false;
                    //Debug.Log("Early Stop p2 via switch p1");
                    StopCoroutine(tempsP2);
                    isPower2Active = false;
                    isSableActive = false;
                    StartCoroutine(CooldownPower(2, cooldownPower2 * currentDurationPower2 / durationPower2));
                    currentDurationPower2 = 0f;
                }
                
                canUsePower1 = false;
                isPower1Active = true;
                isAmeActive = true;
                //Debug.Log("P1 Actif");
                
                StartCoroutine(CoroutineTime(1,.3f)); //set earlyDisablePower à false à la fin des .3 secondes
                tempsP1 = DurationPower(1);
                
                if (currentDurationPower1 < durationPower1)
                {
                    StartCoroutine(tempsP1);
                }
            }
            
            if (!canUsePower1 && earlyDisablePower1)
            {
                earlyDisablePower1 = false;
                //Debug.Log("Early Stop p1");
                StopCoroutine(tempsP1);
                isPower1Active = false;
                isAmeActive = false;
                StartCoroutine(CooldownPower(1, cooldownPower1 * currentDurationPower1 / durationPower1));
                currentDurationPower1 = 0f;
            }
        }
        
        if (Input.GetKeyDown(keyPower2))
        {
            if (canUsePower2)
            {
                if (isPower1Active)
                {
                    earlyDisablePower1 = false;
                    //Debug.Log("Early Stop p1 via switch p2");
                    StopCoroutine(tempsP1);
                    isPower1Active = false;
                    isAmeActive = false;
                    StartCoroutine(CooldownPower(1, cooldownPower1 * currentDurationPower1 / durationPower1));
                    currentDurationPower1 = 0f;
                }
                
                canUsePower2 = false;
                isPower2Active = true;
                isSableActive = true;
                //Debug.Log("P2 Actif");
                
                StartCoroutine(CoroutineTime(2,.3f)); //set earlyDisablePower à false à la fin des .3 secondes
                tempsP2 = DurationPower(2);
                
                if (currentDurationPower2 < durationPower2)
                {
                    StartCoroutine(tempsP2);
                }
            }
            
            if (!canUsePower2 && earlyDisablePower2)
            {
                earlyDisablePower2 = false;
                //Debug.Log("Early Stop p2");
                StopCoroutine(tempsP2);
                isPower2Active = false;
                isSableActive = false;
                StartCoroutine(CooldownPower(2, cooldownPower2 * currentDurationPower2 / durationPower2));
                currentDurationPower2 = 0f;
            }
        }
        
    }

    
    void CheckPower() //sert à regarder si un pouvoir est actif et lancer les spells si oui
    {
        if (isPower1Active)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 charaPos = CharacterController.instance.transform.position;
            float angle = Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
            if (an.attaque3) //Attaque smash
            {
                Vector3 moveDirection = (mousePos - charaPos);
                moveDirection.z = 0;
                moveDirection.Normalize();
                float angleInversé = Mathf.Atan2(charaPos.y - mousePos.y ,charaPos.x - mousePos.x ) * Mathf.Rad2Deg;
                Instantiate(p1ComboConeHitbox, cc.transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
                
                if (p1ComboConeInversedCone)
                {
                    Instantiate(p1ComboConeHitbox, cc.transform.position, Quaternion.AngleAxis(angleInversé, Vector3.forward));
                }
            }

            if (an.attaqueSpeSpell) // Attaque puissante
            {

                if (!p1ThrustBallTriple)
                {
                    Instantiate(p1ThrusrBallHitbox,cc.transform.position,Quaternion.AngleAxis(angle,Vector3.forward));     
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (i == 0)
                        {
                            angle -= 20;
                        }
                        if (i == 1)
                        {
                            angle += 20;
                        }
                        if (i == 2)
                        {
                            angle += 20;
                        }
                        Instantiate(p1ThrusrBallHitbox,cc.transform.position,Quaternion.AngleAxis(angle,Vector3.forward));
                    }
                }
            }

            if (cc.debutDash) // Attaque Dash
            {
                var direction = new Quaternion();
                switch (CharacterController.instance.facing)
                {
                    case CharacterController.LookingAt.Est:
                        direction = Quaternion.Euler(0,0,270);
                        break;
                    case CharacterController.LookingAt.NordEst:
                        direction = Quaternion.Euler(0,0,315);
                        break;
                    case CharacterController.LookingAt.NordOuest:
                        direction = Quaternion.Euler(0,0,45);
                        break;
                    case CharacterController.LookingAt.Ouest:
                        direction = Quaternion.Euler(0,0,90);              
                        break;
                    case CharacterController.LookingAt.SudEst:
                        direction = Quaternion.Euler(0,0,225);  
                        break;
                    case CharacterController.LookingAt.SudOuest:
                        direction = Quaternion.Euler(0,0,135); 
                        break;
                    case CharacterController.LookingAt.Nord:
                        direction = Quaternion.Euler(0,0,0);     
                        break;
                    case CharacterController.LookingAt.Sud:
                        Debug.Log("Mais fréro kestufous la");
                        direction = Quaternion.Euler(0, 0, 180);
                        break;
                }
                Debug.Log(direction);
                GameObject paralysieHitbox =
                    Instantiate(p1DashContactHitbox, cc.transform.position, direction, cc.transform);
                Destroy(paralysieHitbox, cc.dashCooldown);
            }
        }

        if (isPower2Active)
        {
            if (an.attaque3) //si attaque smash
            {
                Instantiate(p2ComboWaveHitbox, cc.transform.position, Quaternion.identity);

                if (p2ComboWaveDouble)
                {
                    StartCoroutine(doubleWave());
                }
            }
            if (an.attaqueSpeSpell)
            {

                Vector2 mousePos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 charaPos = CharacterController.instance.transform.position;
                    Vector3 moveDirection = (mousePos - charaPos);
                    moveDirection.z = 0;
                    moveDirection.Normalize();
                    float angle = Mathf.Atan2(mousePos.y - charaPos.y, mousePos.x - charaPos.x) * Mathf.Rad2Deg;
                    Instantiate(p2ThrustBandageHitbox,cc.transform.position + moveDirection,Quaternion.AngleAxis(angle,Vector3.forward));
            }
            if (!cc.canDash || cc.isDashing) //si dash
            {
                timerSpawn += Time.deltaTime;
                if (timerSpawn >= timerSpawnMax)
                {
                    GameObject fireZone = Instantiate(p2DashTrailHitbox, cc.transform.position + Vector3.down, Quaternion.identity);
                    timerSpawn = 0;
                }
            }
        }
    }
    
    
    private IEnumerator DurationPower(int power) //sert à calculer le temps actuel de pouvoir restant
    {
        switch (power)
        {
            case 1:
                while (currentDurationPower1 < durationPower1)
                {
                    yield return new WaitForSecondsRealtime(0.1f);
                    currentDurationPower1 += 0.1f;
                    //Debug.Log("dura p1 = " + currentDurationPower1);
                }
                //Debug.Log("full conso p1");
                currentDurationPower1 = 0f;
                isPower1Active = false;
                isAmeActive = false;
                StartCoroutine(CooldownPower(1, cooldownPower1));
                break;
            
            case 2:
                while (currentDurationPower2 < durationPower2)
                {
                    yield return new WaitForSecondsRealtime(0.1f);
                    currentDurationPower2 += 0.1f;
                    //Debug.Log("dura p2 = " + currentDurationPower2);
                }
                //Debug.Log("full conso p2");
                currentDurationPower2 = 0f;
                isSableActive = false;
                isPower2Active = false;
                StartCoroutine(CooldownPower(2, cooldownPower2));
                break;
            
            default:
                Debug.Log("non mec");
                break;
        }
        
    }
    
    private IEnumerator CooldownPower(int power, float max) //sert à calculer le temps actuel de recharge du pouvoir
    {
        switch (power)
        {
            case 1:
                earlyDisablePower1 = false;
                if (currentDurationPower1 != 0)
                {
                    CooldownPowerBar.Instance.durationBeforeCooldownPower1 = durationPower1 - currentDurationPower1;
                }
                else
                {
                    CooldownPowerBar.Instance.durationBeforeCooldownPower1 = currentDurationPower1;
                }
                CooldownPowerBar.Instance.sliderP1Max = max;
                CooldownPowerBar.Instance.p1OnCd = true;
                
                while (currentCooldownPower1 < max)
                {
                    yield return new WaitForSecondsRealtime(0.1f);
                    currentCooldownPower1 += 0.1f;
                    //Debug.Log("cd P1 = " + currentCooldownPower1);
                }
                //Debug.Log("p1 rechargé après " + currentCooldownPower1 + " sec.");
                currentCooldownPower1 = 0f;
                canUsePower1 = true;
                CooldownPowerBar.Instance.p1OnCd = false;
                break;
            
            case 2:
                earlyDisablePower2 = false;
                if (currentDurationPower2 != 0)
                {
                    CooldownPowerBar.Instance.durationBeforeCooldownPower2 = durationPower2 - currentDurationPower2;
                }
                else
                {
                    CooldownPowerBar.Instance.durationBeforeCooldownPower2 = currentDurationPower2;
                }
                CooldownPowerBar.Instance.sliderP2Max = max;
                CooldownPowerBar.Instance.p2OnCd = true;
                
                while (currentCooldownPower2 < max)
                {
                    yield return new WaitForSecondsRealtime(0.1f);
                    currentCooldownPower2 += 0.1f;
                    //Debug.Log("cd P2 = " + currentCooldownPower2);
                }
                //Debug.Log("p2 rechargé après " + currentCooldownPower2 + " sec.");
                currentCooldownPower2 = 0f;
                canUsePower2 = true;
                CooldownPowerBar.Instance.p2OnCd = false;
                break;
        }
    }
    
    private IEnumerator CoroutineTime(int power, float temps) //sert à empêcher les gens de spam les pouvoirs ou quoi (car il faut .3 sec après avoir appuyé sur un pouvoir pour pouvoir le désactiver)
    {
        switch (power)
        {
            case 1:
                float compteur1 = 0f;
                while (compteur1 < temps)
                {
                    yield return new WaitForSecondsRealtime(0.1f);
                    compteur1 += 0.1f;
                    //Debug.Log("temps 1 = " + compteur);
                }
                earlyDisablePower1 = true;
                break;
            
            case 2:
                float compteur2 = 0f;
                while (compteur2 < temps)
                {
                    yield return new WaitForSecondsRealtime(0.1f);
                    compteur2 += 0.1f;
                    //Debug.Log("temps 1 = " + compteur);
                }
                earlyDisablePower2 = true;
                break;
        }
        
        
    }

    IEnumerator doubleWave()
    {
        yield return new WaitForSeconds(0.25f);
        Instantiate(p2ComboWaveHitbox, cc.transform.position, Quaternion.identity);
    }
    
    
    
    
    
    
    //old Fonctions ****************************************************************************************************
    
    /*void ChangeLevelPower(int powerIndex, int currentLevel) //fonction à appeler quand on level Up; ou pour tester
    {
        if (powerIndex == 1)
        {
            currentLevelPower1 = currentLevel;
            switch (currentLevel)
            {
                case 1:
                    //effet du niveau
                    Debug.Log("niveau 1");
                    break;

                case <= 2:
                    //bonus du niveau
                    Debug.Log("niveau 2");
                    break;

                case <= 3:
                    //bonus du niveau
                    Debug.Log("niveau 3");
                    break;
                
                case <= 4:
                    //bonus du niveau
                    break;
                
                case <= 5:
                    //bonus du niveau
                    break;
                
                case <= 6: 
                    //bonus du niveau
                    break;
                
                case <= 7:
                    //bonus du niveau
                    break;
                
                case <= 8:
                    //bonus du niveau
                    break;
                
                case <= 9:
                    //bonus du niveau
                    break;
                
                case <= 10:
                    //bonus du niveau
                    break;
            }

        }
        else if (powerIndex == 2)
        {
            currentLevelPower2 = currentLevel;
            switch (currentLevel)
            {
                case <= 1:
                    //effet du niveau
                    break;

                case <= 2:
                    //bonus du niveau
                    break;

                case <= 3:
                    //bonus du niveau
                    break;
                
                case <= 4:
                    //bonus du niveau
                    break;
                
                case <= 5:
                    //bonus du niveau
                    break;
                
                case <= 6: 
                    //bonus du niveau
                    break;
                
                case <= 7:
                    //bonus du niveau
                    break;
                
                case <= 8:
                    //bonus du niveau
                    break;
                
                case <= 9:
                    //bonus du niveau
                    break;
                
                case <= 10:
                    //bonus du niveau
                    break;
            }
        }
        else
        {
            Debug.Log("Il n'y a que 2 pouvoirs, arrête d'essayer.");
        }
    }*/
}
