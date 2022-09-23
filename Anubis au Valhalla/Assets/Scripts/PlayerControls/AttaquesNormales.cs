using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttaquesNormales : MonoBehaviour
{
    private InputManager controls;
    private AttaquesNormales instance;
    
    [Header ("Hitbox Attaque")]
    public LayerMask layerMonstres;
    public Transform pointAttaque;
    public float rangeAttaque;

    [Header ("Stat Attaque")]
    public int puissanceAttaque;
    public float vitesseAttaque;
    public GameObject effetVisuel;
    private float nextAttaque;
    public float vitessePersoXC1;
    public float vitessePersoYC1;
    public float vitessePersoNormaleX;
    public float vitessePersoNormaleY;
    
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

    public void StartCombo() // Crée une hit box devant le perso et touche les ennemis
    {
        StartCoroutine(ralentirPersoC1());
        StartCoroutine(montrerEffet());
        Collider2D[] toucheMonstre = Physics2D.OverlapCircleAll(pointAttaque.position, rangeAttaque, layerMonstres);

        foreach (Collider2D monstre in toucheMonstre)
        {
            Debug.Log("touché");
            monstre.GetComponent<IA_Monstre1>().TakeDamage(puissanceAttaque);
            monstre.GetComponent<IA_Monstre1>().DamageText(puissanceAttaque);
        }
    }

    IEnumerator montrerEffet()
    {
        effetVisuel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        effetVisuel.SetActive(false);
    }
    
    IEnumerator ralentirPersoC1()
    {
        CharacterController.instance.speedX = vitessePersoXC1;
        CharacterController.instance.speedY = vitessePersoYC1;
        yield return new WaitForSeconds(0.5f);
        CharacterController.instance.speedX = vitessePersoNormaleX;
        CharacterController.instance.speedY = vitessePersoNormaleY;
    }

    private void OnDrawGizmosSelected() // Permet de voir la hitbox du coup dans l'éditeur
    {
        if(pointAttaque == null)
            return;
        
        Gizmos.DrawWireSphere(pointAttaque.position, rangeAttaque);
    }
}
