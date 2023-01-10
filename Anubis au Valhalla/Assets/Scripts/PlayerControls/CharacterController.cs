using System;
using System.Collections;
using DG.Tweening;
using GenPro;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class CharacterController : MonoBehaviour
{
  [Header("Déplacements")] 
  public bool allowMovements;
  public Animator anim;
  public InputManager controls;
  public static CharacterController instance; //jai besion de l'instance pour bouger le joueur au changements de salles
  [NaughtyAttributes.ReadOnly] public float speedX;
  [NaughtyAttributes.ReadOnly] public float speedY;
  public bool isAttacking;
  public LookingAt facing;

  [Header("Interactions")] 
  public bool canInteract;
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
  public bool isHiting;
  public bool isHitingSoundOn;
  public float HittingSoundLenght;
  public float HittingSoundLenghtTimer;
  
  [Header("Valeurs tracking pour les pouvoirs")] 
  public bool debutDash;
  public bool finDash;
  
  public enum LookingAt { Nord,NordEst,Est,SudEst,Sud,SudOuest,Ouest,NordOuest }

  [Header("Dash")] 
  public bool stopDash;
  public float dashSpeed;
  public float timerDash;
  public float dashDuration;
  private float timerdashCooldown;
  public float diagonalSpeedMultiplier;
  public float dashBuffer;
  public bool canBuffer = false;
  [NaughtyAttributes.ReadOnly] public float dashCooldown;
  public bool isDashing;
  public bool canDash;
  public GhostDash ghost;

  [HideInInspector]public Rigidbody2D rb; // ca aussi
  public Vector2 movement;
  public float astarPathTimer = 0f;
  public float astarPathTimerMax = 1f;

  [Header("Utilitaires")] 
  public KeyCode interaction;
  public GameObject indicationDirection;
  public TrailRenderer trail;

  public GameObject doorInteractUI;

  public RectTransform doorUITransform;

  public GameObject dashTracker;

  [HideInInspector] public BoxCollider2D playerCol;
  //public TilemapRenderer ground;


  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }

    rb = gameObject.GetComponent<Rigidbody2D>();
    playerCol = GetComponent<BoxCollider2D>();
    controls = new InputManager();
    PivotTo(transform.position);
    doorUITransform = doorInteractUI.GetComponent<RectTransform>();
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
    Flip();
    Keyboard kb = InputSystem.GetDevice<Keyboard>();

    if (allowMovements)
    {
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
          audioSource.pitch = Random.Range(0.8f,1.2f);
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

   if (!isHitingSoundOn)
    {
      HittingSoundLenghtTimer += Time.deltaTime;
      if (HittingSoundLenghtTimer >= HittingSoundLenght)
      {
        isHitingSoundOn = true;
        HittingSoundLenghtTimer = 0;
      }
    }

    if (kb.spaceKey.wasPressedThisFrame && isDashing == false && canDash && allowMovements)
    {
      audioSource.pitch = 1;
      audioSource.PlayOneShot(audioClipArray[0]);
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
      anim.SetBool("isDashing",false);
      anim.SetBool("isWalking",true);
      dashTracker.SetActive(false);
      allowMovements = true;
      finDash = true;
      StartCoroutine(ResetTracking());
      rb.velocity *= 0.85f;
      AttaquesNormales.instance.canAttack = true;
      isDashing = false;
      timerDash = 0;
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
        InteractWithDoor(currentDoor);
      }
    }

    if (canBuffer)
    {
      dashBuffer -= Time.deltaTime;
      if (dashBuffer < 0)
      {
        Debug.Log("enabled");
        canBuffer = false;
        playerCol.enabled = true;
      }
    }
  }

  void Dashing()
  {
    timerDash += Time.deltaTime;
    {
      anim.SetBool("isDashing",true);
      switch (facing)
      {
       
        case LookingAt.Nord:
          rb.velocity = (new Vector2(0,1) * dashSpeed);
          break;
          
        case LookingAt.Sud:
          rb.velocity = (new Vector2(0,-1) * dashSpeed);
          break;
          
        case LookingAt.Est:
          rb.velocity = (new Vector2(1,0) * dashSpeed);
          break;
          
        case LookingAt.Ouest:
          rb.velocity = (new Vector2(-1,0) * dashSpeed);
          break;
          
        case LookingAt.NordEst:
          rb.velocity = (new Vector2(0.5f,0.5f) * (dashSpeed * diagonalSpeedMultiplier));
          break;
          
        case LookingAt.NordOuest:
          rb.velocity = (new Vector2(-0.5f,0.5f) * (dashSpeed * diagonalSpeedMultiplier));
          break;

        case LookingAt.SudEst:
          rb.velocity = (new Vector2(0.5f, -0.5f) * (dashSpeed * diagonalSpeedMultiplier));
          break;
          
        case LookingAt.SudOuest:
          rb.velocity = (new Vector2(-0.5f,-0.5f) * (dashSpeed * diagonalSpeedMultiplier));
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
    
    
    if (movement.x > 0 && !isAttacking) // Le personnage s'oriente vers la direction où il marche. 
    {
      isEST = true;
      facing = LookingAt.Est;
      transform.localRotation = Quaternion.Euler(0, 0,0);
      doorUITransform.rotation = new Quaternion(0, 0, 0, 0);
    }
    else
    {
      isEST = false;

    }

    if (movement.x < 0 && !isAttacking)
    {
      
      isOUEST= true;
      facing = LookingAt.Ouest;
      transform.localRotation = Quaternion.Euler(0, 180,0);
      doorUITransform.rotation = new Quaternion(0,180,0, 0);
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

    if (isEST && isSUD)
    {
      facing = LookingAt.SudEst;
    }
    if (isEST && isNORD)
    {
      facing = LookingAt.NordEst;
    }
    if (isOUEST && isSUD)
    {
      facing = LookingAt.SudOuest;
    }
    if (isOUEST && isNORD)
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
  }
  private void InteractWithDoor(Collider2D col)
  {
    allowMovements = true;
    ghost.activerEffet = false;
    isDashing = false;
    canDash = true;
    timerdashCooldown = 0;
    var hitDoor = col.GetComponent<Door>();
    SalleGenerator.Instance.spawnDoor = col.gameObject.GetComponent<Door>().doorOrientation;
    if (SalleGenerator.Instance.roomsDone == SalleGenerator.Instance.dungeonSize +1)
    {
      Debug.Log("auuuuugh");
      SalleGenerator.Instance.NewZone(hitDoor.doorOrientation, true, hitDoor);
    }
    if (hitDoor.willChooseSpecial)
    {
      SalleGenerator.Instance.challengeChooser = Random.Range(1, 6);
    }
    else
    {
      SalleGenerator.Instance.challengeChooser = 0;
    }

    if (hitDoor.currentDoorType == Door.DoorType.ToShop)
    {
      SalleGenerator.Instance.shopsVisited++;
      SalleGenerator.Instance.dungeonSize++;
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
  }
  
  
  IEnumerator ResetTracking()
  {
    yield return null;
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
}
