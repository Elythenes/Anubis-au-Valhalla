using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttaquesNormales : MonoBehaviour
{
    /*private InputManager controls;
    private AttaquesNormales instance;
    
    [Header ("Hitbox Attaque")]
    public LayerMask layerMonstres;
    public Transform pointAttaque;
    public float rangeAttaque;

    [Header ("Stat Attaque")]
    public int puissanceAttaque;
    public float vitesseAttaque;
    public GameObject effetVisuel;
    private int ComboActuel;
    
    [Header ("Customise Hitboxs")]
    public LayerMask mask;
    public bool useSphere = false;
    public Vector3 hitboxSize = Vector3.one;
    public float radius = 0.5f;
    public Color inactiveColor;
    public Color collisionOpenColor;
    public Color collidingColor;
   // private ColliderState _state;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        controls = new InputManager();
    }
    
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        Mouse mouse = InputSystem.GetDevice<Mouse>(); // Trouver input de la souris

        if (Time.time >= nextAttaque) // Cooldown de l'attaque
        {
            if (mouse.leftButton.wasPressedThisFrame)
            {
                StartCombo();
                nextAttaque = Time.time + vitesseAttaque;
            } 
        }
    }

  public void startCheckingCollision() {
      _state = ColliderState.Open;

  }

  public void stopCheckingCollision() {
      _state = ColliderState.Closed;

  }
  
  private void checkGizmoColor() {
      
      switch(_state) {

          case ColliderState.Closed:

              Gizmos.color = inactiveColor;

              break;

          case ColliderState.Open:

              Gizmos.color = collisionOpenColor;

              break;

          case ColliderState.Colliding:

              Gizmos.color = collidingColor;

              break;

      }

  }
  
    public void StartCombo() // Crée une hit box devant le perso et touche les ennemis
    {
        StartCoroutine(MovePersoC1());
        StartCoroutine(montrerEffet());
        Collider2D[] toucheMonstre = Physics2D.OverlapBoxAll(pointAttaque.position, rangeAttaque, layerMonstres);

        foreach (Collider2D monstre in toucheMonstre)
        {
            Debug.Log("touché");
            monstre.GetComponent<MonsterLifeManager>().TakeDamage(puissanceAttaque);
            monstre.GetComponent<MonsterLifeManager>().DamageText(puissanceAttaque);
        }
    }

    IEnumerator montrerEffet()
    {
        effetVisuel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        effetVisuel.SetActive(false);
    }
    
    IEnumerator MovePersoC1()
    {
        CharacterController.instance.speedX = vitessePersoXC1;
        CharacterController.instance.speedY = vitessePersoYC1;
        yield return new WaitForSeconds(0.5f);
        CharacterController.instance.speedX = vitessePersoNormaleX;
        CharacterController.instance.speedY = vitessePersoNormaleY;
    }

    private void OnDrawGizmos() // Permet de voir la hitbox du coup dans l'éditeur
    {
        if(pointAttaque == null)
            return;
        
        Gizmos.DrawWireSphere(pointAttaque.position, rangeAttaque);*/
    }

