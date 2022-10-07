using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttaquesNormales : MonoBehaviour
{

    [Header("Customise Hitboxs")] 
    public LayerMask layerMonstres;
    private HitboxState ColliderState;
    private InputManager controls;
    private AttaquesNormales instance;
    public enum HitboxState { Opened, Closed, Colliding };

    [Header("Stats Attaque 1")] 
    public int puissanceAttaqueC1;
    public float dureeHitbox1;
    public float dureeHitboxTimer1;
    public float cooldownAbandonCombo1;
    public float stunAftercombo1;
    public Vector2 rangeAttaque1;
    
    [Header("Stats Attaque 2")] 
    public int puissanceAttaqueC2;
    public float dureeHitbox2;
    public float dureeHitboxTimer2;
    public float cooldownAbandonCombo2;
    public float stunAftercombo2;
    public Vector2 rangeAttaque2;
    
    [Header("Stats Attaque 3")] 
    public int puissanceAttaqueC3;
    public float dureeHitbox3;
    public float dureeHitboxTimer3;
    public float cooldownAbandonComb3;
    public float stunAftercombo3;
    public Vector2 rangeAttaque3;

    [Header("Général")] 
    public GameObject attackPoint;
    public float radius;
    public Vector2 rotation;
    public bool canAttack;
    public GameObject effetVisuel;
    public int comboActuel;
    private List<Vector2> rangeAttackList = new List<Vector2>();
    public Vector2 hitboxPosition;





    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        controls = new InputManager();
    }

    private void Start()
    {
        rangeAttackList.Add(rangeAttaque1);
        rangeAttackList.Add(rangeAttaque2);
        rangeAttackList.Add(rangeAttaque3);
        ColliderState = HitboxState.Closed;
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

        if (mouse.leftButton.wasPressedThisFrame && canAttack)
        {
            Debug.Log("ouui");
            switch (comboActuel)
            {
                case 0:
                   // comboActuel++;
                    if (canAttack)
                    {
                        Combo1();
                    }
                    break;
                
                case 1:
                    comboActuel++;
                    break;
                
                case 2:
                    ;
                    break;
                    
            }
        }
      
    }

  
    private void checkGizmoColor()
    {

        switch (ColliderState)
        {

            case HitboxState.Closed:

                Gizmos.color = Color.red;

                break;

            case HitboxState.Opened:

                Gizmos.color = Color.blue;

                break;

            case HitboxState.Colliding:

                Gizmos.color = Color.green;

                break;

        }

    }

    public void Combo1() // Crée une hit box devant le perso et touche les ennemis
    {
        ColliderState = HitboxState.Opened;
        checkGizmoColor();
        StartCoroutine(montrerEffet(dureeHitbox1));
        Collider2D[] toucheMonstre = Physics2D.OverlapBoxAll(new Vector2(attackPoint.transform.position.x,attackPoint.transform.position.y), rangeAttaque1, layerMonstres);

        foreach (Collider2D monstre in toucheMonstre)
        {
            Debug.Log("touché");
           // monstre.GetComponent<MonsterLifeManager>().TakeDamage(puissanceAttaqueC1);
            //monstre.GetComponent<MonsterLifeManager>().DamageText(puissanceAttaqueC1);
        }
    }

    IEnumerator montrerEffet(float duréeHitbox)
    {
        effetVisuel.transform.position = hitboxPosition;
        effetVisuel.transform.localScale = rangeAttackList[comboActuel];
        effetVisuel.SetActive(true);
        yield return new WaitForSeconds(duréeHitbox);
        effetVisuel.SetActive(false);
    }


    private void OnDrawGizmos() // Permet de voir la hitbox du coup dans l'éditeur
    {
        if (rangeAttackList != null)
        {
            Gizmos.DrawCube(hitboxPosition, rangeAttaque1);
        }
        
        if (rangeAttackList != null)
        {
            Gizmos.DrawCube(hitboxPosition, rangeAttaque2);
        }
        
        if (rangeAttackList != null)
        {
            Gizmos.DrawCube(hitboxPosition, rangeAttaque3);
        }
    }
}

