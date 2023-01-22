using System;
using System.Collections;
using DG.Tweening;
using GenPro;
using Pathfinding;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class CharacterController : MonoBehaviour
{
  [Header("Déplacements")] public bool isCinematic;
  public bool allowMovements;
  public Animator anim;
  public InputManager controls;
  public static CharacterController instance; //jai besion de l'instance pour bouger le joueur au changements de salles
  [NaughtyAttributes.ReadOnly] public float speedX;
  [NaughtyAttributes.ReadOnly] public float speedY;
  public bool isAttacking;
  public LookingAt facing;

  [Header("Interactions")] 
  public bool isInteracting;
  public bool canInteract;
  public bool canInteractTuto;
  public Collider2D currentDoor;
  public GameObject CanvasInteraction;
  public Canvas CanvasInteractionCanvas;
  public Vector3 offset;
  public TextMeshProUGUI TextInteraction;

  [Header("Audio")]
  public AudioSource audioSource;
  public AudioClip[] audioClipArray;
  public float timeBetweenSteps;
  public float timeBetweenStepsTimer;
  public bool HurtOnce;
  public bool isHiting;
  public bool isHitingSoundOn;
  public float HittingSoundLenght;
  public float HittingSoundLenghtTimer;
  
  [Header("Valeurs tracking pour les pouvoirs")] 
  public bool debutDash;
  public bool finDash;
  private int bossIndex;
  
  public enum LookingAt { Nord,NordEst,Est,SudEst,Sud,SudOuest,Ouest,NordOuest }

  [Header("Dash")] 
  public bool stopDash;
  public float dashSpeed;
  public float timerDash;
  public float dashDuration;
  public float timerdashCooldown;
  public float diagonalSpeedMultiplier;
  public bool canBuffer = false;
  public bool canBoost = false;
  [NaughtyAttributes.ReadOnly] public float dashCooldown;
  public bool isDashing;
  public bool canDash;
  public GhostDash ghost;
  public GameObject FXDash;

  [HideInInspector]public Rigidbody2D rb; // ca aussi
  public Vector2 movement;

  [Header("Utilitaires")]
  public bool isHub;
  public KeyCode interaction;
  public GameObject indicationDirection;
  public TrailRenderer trail;
  public Collider2D collisionPathfinding;
  public GameObject doorInteractUI;
  public RectTransform doorUITransform;
  public GameObject dashTracker;
  [HideInInspector] public BoxCollider2D playerCol;
  //public TilemapRenderer ground;


  private void Awake()
  {
    Time.timeScale = 1;
    if (instance == null)
    {
      instance = this;
    }
    rb = gameObject.GetComponent<Rigidbody2D>();
    playerCol = GetComponent<BoxCollider2D>();
    controls = new InputManager();
    PivotTo(transform.position);
    //doorUITransform = doorInteractUI.GetComponent<RectTransform>();
    dashTracker.SetActive(false);
    CanvasInteraction = GameObject.FindWithTag("CanvasInteraction");
    TextInteraction = GameObject.Find("TexteAction").GetComponent<TextMeshProUGUI>();
    CanvasInteractionCanvas = CanvasInteraction.GetComponent<Canvas>();

   
  }

  private void OnEnable()
  {
    controls.Enable();
  }
  private void OnDisable()
  {
    controls.Disable();
  }
  
  public void PivotTo(Vector3 position)
  {
    Vector3 offset = transform.position - position;
    foreach (Transform child in transform)
      child.transform.position += offset;
    transform.position = position;
  }

  private void FixedUpdate()
  {
    if (isDashing == false && !isAttacking) // Déplacments hors dash.
    {
      rb.AddForce(new Vector2(movement.x * speedX, movement.y * speedY));
    }
  }

  private void Update()
  {
    /*if (!isHub)  // A réactiver pour un pathfinding qui s'update en temps réel
    {
      var guo = new GraphUpdateObject(collisionPathfinding.bounds);
      guo.updatePhysics = true;
      AstarPath.active.UpdateGraphs (guo);
    }*/
    
   // trail.sortingOrder = 
    
    if (Time.timeScale != 0)
    {
      Flip();
    }
   
    Keyboard kb = InputSystem.GetDevice<Keyboard>();

    if (isCinematic)
    {
      movement = Vector2.zero;
      rb.velocity = Vector2.zero;
      rb.constraints = RigidbodyConstraints2D.FreezePosition;
      anim.SetBool("isIdle",true);
      anim.SetBool("isWalking",false);
    }
    
    if (allowMovements &&  Time.timeScale != 0)
    {
      rb.constraints = RigidbodyConstraints2D.FreezeRotation;
      Vector2 directionIndic = Camera.main.ScreenToWorldPoint(Input.mousePosition) - indicationDirection.transform.position;
      float angleIndic = Mathf.Atan2(directionIndic.y, directionIndic.x) * Mathf.Rad2Deg;
      Quaternion rotationIndic = Quaternion.AngleAxis(angleIndic, Vector3.forward);
      indicationDirection.transform.rotation = rotationIndic;
    }
   

    if (DamageManager.instance.stun == false && allowMovements && isDashing == false)
    {
      movement = controls.Player.Movement.ReadValue<Vector2>(); // Read les input de déplacement
    }
    
    if (isDashing == false)
    {
      if (movement.magnitude != 0)
      {
        anim.SetBool("isIdle", false);
        anim.SetBool("isWalking", true);
        timeBetweenSteps += Time.deltaTime;
        if (timeBetweenSteps >= timeBetweenStepsTimer)
        {
          //audioSource.pitch = Random.Range(0.8f,1.2f);
          audioSource.PlayOneShot(audioClipArray[Random.Range(1, 3)],0.5f);
          timeBetweenSteps = 0;
        }
      }
      else if (movement == Vector2.zero)
      {
        anim.SetBool("isIdle", true);
        anim.SetBool("isWalking", false);
      }
    }

    if (isHiting && isHitingSoundOn)
    {
      audioSource.pitch = Random.Range(0.8f,1.2f);
      audioSource.PlayOneShot(audioClipArray[4]);
      isHitingSoundOn = false;
    }

    if (DamageManager.instance.isHurt && HurtOnce)
    {
      audioSource.pitch = 1;
      audioSource.PlayOneShot(audioClipArray[5],0.8f);
      HurtOnce = false;
      StartCoroutine(ResetTracking());
    }

    if (!isHitingSoundOn)
    {
      HittingSoundLenghtTimer += Time.deltaTime;
      if (HittingSoundLenghtTimer >= HittingSoundLenght)
      {
        isHitingSoundOn = true;
        HittingSoundLenghtTimer = 0;
      }
    }

   if (debutDash)
   {
     audioSource.pitch = 1;
     audioSource.PlayOneShot(audioClipArray[0]);
   }
   
    if (kb.spaceKey.wasPressedThisFrame && isDashing == false && canDash && allowMovements && !AttaquesNormales.instance.attaqueSpe2 && Time.timeScale != 0)
    {
      timerDash = 0;
      dashTracker.SetActive(true);
      stopDash = false;
      allowMovements = false;
      debutDash = true;
      StartCoroutine(ResetTracking());
      ghost.lastPlayerPos = transform.position;
      AttaquesNormales.instance.canAttack = false;
      ghost.enabled = true;
      isDashing = true;
    }
    
    if (isDashing && !stopDash) // Déplacement lors du dash selon la direction du regard du perso
    {
      Dashing();
    }


    if (timerDash > dashDuration) // A la fin du dash...
    {
      
      isDashing = false;
      if (!canBoost && !canBuffer)
      {
        QuitDash();
      }
    }

    if (canDash == false)
    {
      timerdashCooldown += Time.deltaTime;
    }
    
    if (timerdashCooldown >= dashCooldown) // Cooldown dash
    {
      canDash = true;
      timerdashCooldown = 0;
    }
    
    
    // Interaction avec la porte
    if (canInteract)
    {
      CanvasInteractionCanvas.enabled = true;
      CanvasInteraction.SetActive(true); 
      CanvasInteraction.transform.position = transform.position + offset;
      CanvasInteraction.transform.DOScale(new Vector3(1, 1, CanvasInteraction.transform.localScale.z),0.25f);
      TextInteraction.SetText("Continuer");
      if (Input.GetKeyDown(KeyCode.F) && currentDoor is not null)
      {
        var guo = new GraphUpdateObject(collisionPathfinding.bounds);
        guo.updatePhysics = true;
        AstarPath.active.UpdateGraphs (guo);
        InteractWithDoor(currentDoor);
      }
    }
    
    if (canInteractTuto)
    {
      CanvasInteractionCanvas.enabled = true;
      CanvasInteraction.SetActive(true); 
      CanvasInteraction.transform.position = transform.position + offset;
      CanvasInteraction.transform.DOScale(new Vector3(1, 1, CanvasInteraction.transform.localScale.z),0.25f);
      TextInteraction.SetText("Continuer");
      if (Input.GetKeyDown(KeyCode.F) && currentDoor is not null)
      {
        InteractWithDoorTuto(currentDoor);
      }
    }
    
  }

  public void Dashing()
  {
    timerDash += Time.deltaTime;
    {
      anim.SetBool("isDashing",true);
      switch (facing)
      {
       
        case LookingAt.Nord:
          rb.velocity = (new Vector2(0,1) * dashSpeed);
         /* GameObject fxOBJ1 =Instantiate(FXDash, transform.position, Quaternion.identity);
          fxOBJ1.transform.rotation = new Quaternion(-90,90,-90,0);*/
          break;
          
        case LookingAt.Sud:
          rb.velocity = (new Vector2(0,-1) * dashSpeed);
         /* GameObject fxOBJ2 =Instantiate(FXDash, transform.position, Quaternion.identity);
          fxOBJ2.transform.rotation = new Quaternion(90,90,-90,0);*/
          break;
          
        case LookingAt.Est:
          rb.velocity = (new Vector2(1,0) * dashSpeed);
          /*GameObject fxOBJ3 =Instantiate(FXDash, transform.position, Quaternion.identity);
          fxOBJ3.transform.rotation = Quaternion.identity;*/
          break;
          
        case LookingAt.Ouest:
          rb.velocity = (new Vector2(-1,0) * dashSpeed);
        /*  GameObject fxOBJ4 =Instantiate(FXDash, transform.position, Quaternion.identity);
          fxOBJ4.transform.rotation = Quaternion.identity;*/
          break;
          
        case LookingAt.NordEst:
          rb.velocity = (new Vector2(0.5f,0.5f) * (dashSpeed * diagonalSpeedMultiplier));
         /* GameObject fxOBJ5 =Instantiate(FXDash, transform.position, Quaternion.identity);
          fxOBJ5.transform.rotation = Quaternion.identity;*/
          break;
          
        case LookingAt.NordOuest:
          rb.velocity = (new Vector2(-0.5f,0.5f) * (dashSpeed * diagonalSpeedMultiplier));
        /*  GameObject fxOBJ6 =Instantiate(FXDash, transform.position, Quaternion.identity);
          fxOBJ6.transform.rotation = Quaternion.identity;*/
          break;

        case LookingAt.SudEst:
          rb.velocity = (new Vector2(0.5f, -0.5f) * (dashSpeed * diagonalSpeedMultiplier));
         /* GameObject fxOBJ7 =Instantiate(FXDash, transform.position, Quaternion.identity);
          fxOBJ7.transform.rotation = Quaternion.identity;*/
          break;
          
        case LookingAt.SudOuest:
          rb.velocity = (new Vector2(-0.5f,-0.5f) * (dashSpeed * diagonalSpeedMultiplier));
         /* GameObject fxOBJ8 =Instantiate(FXDash, transform.position, Quaternion.identity);
          fxOBJ8.transform.rotation = Quaternion.identity;*/
          break;
      }
    }
  }
  
  #region Gestion De l'Orientation
  void Flip()
  {
    bool isEST;
    bool isNORD;
    bool isSUD;
    bool isOUEST;

    if (Input.GetKeyDown(KeyCode.Q) && !AttaquesNormales.instance.blockFlip && !isDashing)
    {
      facing = LookingAt.Ouest;
    }
    if (Input.GetKeyDown(KeyCode.D) && !AttaquesNormales.instance.blockFlip && !isDashing)
    {
      facing = LookingAt.Est;
    }
    if (Input.GetKeyDown(KeyCode.Z) && !AttaquesNormales.instance.blockFlip && !isDashing)
    {
      facing = LookingAt.Nord;
    }
    if (Input.GetKeyDown(KeyCode.S) && !AttaquesNormales.instance.blockFlip && !isDashing)
    {
      facing = LookingAt.Sud;
    }
    if (Input.GetKeyDown(KeyCode.S) && Input.GetKeyDown(KeyCode.Q) &&!AttaquesNormales.instance.blockFlip && !isDashing)
    {
      facing = LookingAt.SudOuest;
    }
    if (Input.GetKeyDown(KeyCode.S) && Input.GetKeyDown(KeyCode.D) && !AttaquesNormales.instance.blockFlip && !isDashing)
    {
      facing = LookingAt.SudEst;
    }
    if (Input.GetKeyDown(KeyCode.Z) && Input.GetKeyDown(KeyCode.Q) && !AttaquesNormales.instance.blockFlip && !isDashing)
    {
      facing = LookingAt.NordOuest;
    }
    if (Input.GetKeyDown(KeyCode.Z) && Input.GetKeyDown(KeyCode.D) && !AttaquesNormales.instance.blockFlip && !isDashing)
    {
      facing = LookingAt.NordEst;
    }
    
      if (movement.x > 0 && !isAttacking /*&& AttaquesNormales.instance.blockFlip*/) // Le personnage s'oriente vers la direction où il marche. 
      {
        isEST = true;
        facing = LookingAt.Est;
        if (AttaquesNormales.instance.blockFlip)
        {
          transform.localRotation = Quaternion.Euler(0, 0,0);
          //doorUITransform.rotation = new Quaternion(0, 0, 0, 0);
        }
       
      }
      else
      {
        isEST = false;

      }

      if (movement.x < 0 && !isAttacking /*&& AttaquesNormales.instance.blockFlip*/)
      {
        isOUEST= true;
        facing = LookingAt.Ouest;
        if (AttaquesNormales.instance.blockFlip)
        {
          transform.localRotation = Quaternion.Euler(0, 180,0);
          //doorUITransform.rotation = new Quaternion(0,180,0, 0);
        }
      }
      else
      {
        isOUEST = false;
      }
    
      if (movement.y < 0 && !isAttacking)
      {
        isSUD = true;
        facing = LookingAt.Sud;
        float face = transform.localScale.x;
        face = 1;
      }
      else
      {
        isSUD = false;
      }
    
      if (movement.y > 0 && !isAttacking)
      {
        isNORD = true;
        facing = LookingAt.Nord;
        float face = transform.localScale.x;
        face = 1;
      }
      else
      {
        isNORD = false;
      }

      if (movement.y < 0 && movement.x > 0 && !isAttacking && isSUD && isEST)
      {
        facing = LookingAt.SudEst;
      }
      if (movement.y > 0 && movement.x > 0 && !isAttacking && isNORD && isEST)
      {
        facing = LookingAt.NordEst;
      }
      if (movement.y < 0 && movement.x < 0 && !isAttacking && isSUD && isOUEST)
      {
        facing = LookingAt.SudOuest;
      }
      if (movement.y > 0 && movement.x < 0 && !isAttacking && isNORD && isOUEST)
      {
        facing = LookingAt.NordOuest;
      }
  }
    
  #endregion
  
  // ---TRUC POUR GENERER LA PROCHAINE SALLE---


  private void OnTriggerEnter2D(Collider2D col)
  {
    if (col.CompareTag("Door"))
    {
      CanvasInteraction.transform.localScale = new Vector3(0,0,CanvasInteraction.transform.localScale.z);
      canInteract = true;
      currentDoor = col;
    }

    if (col.CompareTag("DoorTuto"))
    {
      CanvasInteraction.transform.localScale = new Vector3(0,0,CanvasInteraction.transform.localScale.z);
      canInteractTuto = true;
      currentDoor = col;
    }
  }
  
  private void OnTriggerExit2D(Collider2D col)
  {
    if (col.gameObject.CompareTag("Door"))
    {
      if (CanvasInteraction is not null)
      {
        CanvasInteractionCanvas.enabled = false;
        //CanvasInteraction.SetActive(false);
      }
      canInteract = false;
      currentDoor = null;
    }
    if (col.gameObject.CompareTag("DoorTuto"))
    {
      if (CanvasInteraction is not null)
      {
        CanvasInteractionCanvas.enabled = false;
      }
      canInteractTuto = false;
      currentDoor = null;
    }
  }
  private void InteractWithDoor(Collider2D col)
  {
    if (SoundManager.instance.isShop)
    {
      if (SalleGenerator.Instance.zone2)
      {
        SoundManager.instance.ChangeToZone2();
        SoundManager.instance.PlayZone2();
      }
      else
      {
        SoundManager.instance.ChangeToZone1();
        SoundManager.instance.PlayZone1();
      }
    }
    allowMovements = true;
    ghost.activerEffet = false;
    isDashing = false;
    canDash = true;
    timerdashCooldown = 0;
    GlyphManager.Instance.enterNewRoom = true;
    var hitDoor = col.GetComponent<Door>();
    SalleGenerator.Instance.spawnDoor = col.gameObject.GetComponent<Door>().doorOrientation;
    SalleGenerator.Instance.SwapDoorType(SalleGenerator.Instance.sDoors[(int)SalleGenerator.Instance.fromDoor]);
    if (SalleGenerator.Instance.roomsDone > SalleGenerator.Instance.dungeonSize || hitDoor.currentDoorType == Door.DoorType.Transition)
    {
      Debug.Log("auuuuugh");
      SalleGenerator.Instance.NewZone(hitDoor.doorOrientation, true, hitDoor);
      hitDoor.willChooseSpecial = false;
      for (int i = 0; i < (int)SalleGenerator.DoorOrientation.West + 1; i++)
      {
        SalleGenerator.Instance.OpenDoors((SalleGenerator.DoorOrientation)i,false);
      }
      return;
    }
    if (hitDoor.willChooseSpecial)
    {
      SalleGenerator.Instance.challengeChooser = Random.Range(1, 6);
      SalleGenerator.Instance.challengeChooser = 3;
    }
    else
    {
      SalleGenerator.Instance.challengeChooser = 0;
    }

    if (hitDoor.currentDoorType == Door.DoorType.ToBoss && !SalleGenerator.Instance.zone2)
    {
      bossIndex++;
      if (bossIndex == 1)
      {
        SoundManager.instance.ChangeToBoss();
        SoundManager.instance.PlayBoss();
        SalleGenerator.Instance.morbinTime = true;
      }
      else if(bossIndex == 2)
      {
        SoundManager.instance.audioSource.Stop();
      }

    }
    if (hitDoor.currentDoorType == Door.DoorType.ToShop)
    {
      SoundManager.instance.ChangeToShop();
      SoundManager.instance.PlayShop();
      SalleGenerator.Instance.shopsVisited++;
      if (SalleGenerator.Instance.roomsDone < SalleGenerator.Instance.dungeonSize - 2)
      { 
        SalleGenerator.Instance.dungeonSize++;
      }
      SalleGenerator.Instance.TransitionToNextRoom(hitDoor.doorOrientation, true,
        hitDoor);
    }
    else if (hitDoor.currentDoorType != Door.DoorType.Normal)
    {
      SalleGenerator.Instance.TransitionToNextRoom(hitDoor.doorOrientation, true,
        hitDoor);
    }
    else
    {
      SalleGenerator.Instance.TransitionToNextRoom(hitDoor.doorOrientation, false,
        hitDoor);
    }
    hitDoor.willChooseSpecial = false;
    for (int i = 0; i < (int)SalleGenerator.DoorOrientation.West + 1; i++)
    {
      SalleGenerator.Instance.OpenDoors((SalleGenerator.DoorOrientation)i,false);
    }
  }
  
  private void InteractWithDoorTuto(Collider2D col)
  {
    allowMovements = true;
    ghost.activerEffet = false;
    isDashing = false;
    canDash = true;
    timerdashCooldown = 0;
    var hitDoor = col.GetComponent<DoorTuto>();
    hitDoor.NextRoom();
  }
  
  
  public IEnumerator ResetTracking()
  {
    yield return null;
    HurtOnce = true;
    debutDash = false;
    finDash = false;
  }

  public void PlayerHitboxTimer(float timer)
  {
    if (timer >= 0)
    {
      timer -= Time.deltaTime;
      Debug.Log(timer);
      return;
    }
    playerCol.enabled = true;
  }

  public void QuitDash()
  {
    isDashing = false;
    playerCol.enabled = true;
    timerDash = 0;
    anim.SetBool("isDashing",false);
    anim.SetBool("isWalking",true);
    dashTracker.SetActive(false);
    allowMovements = true;
    finDash = true;
    StartCoroutine(ResetTracking());
    rb.velocity *= 0.85f;
    AttaquesNormales.instance.canAttack = true;
    canDash = false;
    if (AttaquesNormales.instance.buffer)
    {
      AttaquesNormales.instance.buffer = false;
      AttaquesNormales.instance.ExecuteAttack();
    }

    if (AttaquesNormales.instance.buffer2)
    {
      AttaquesNormales.instance.buffer2 = false;
      AttaquesNormales.instance.SpecialAttack();
    }
  }
}
