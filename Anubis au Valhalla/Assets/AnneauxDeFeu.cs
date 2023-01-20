using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AnneauxDeFeu : MonoBehaviour
{
    private IA_Valkyrie ia;
    private float currentExpansionAmount;
    private float currentRate;
    private CircleCollider2D col;
    public GameObject child;
    public SpriteRenderer sr;
    public SpriteMask mask;

    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        child.transform.DOScale(Vector3.one * 0.95f, 2);
    }

    // Start is called before the first frame update
    void Start()
    {
        ia = IA_Valkyrie.instance;
        currentExpansionAmount = ia.expansionAmount;
        currentRate = ia.ExpansionRate;
        StartCoroutine(Expand());
        Destroy(gameObject,3.5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    IEnumerator Expand()
    {
        yield return new WaitForSeconds(currentRate);
        transform.localScale += Vector3.one * currentExpansionAmount;
        StartCoroutine(Accelerate());
        StartCoroutine(Expand());

    }
    IEnumerator Accelerate()
    {
        yield return new WaitForSeconds(ia.ExpansionSpeedUpRate);
        currentRate -= ia.ExpansionSpeedUp;

    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            DamageManager.instance.TakeDamage(ia.ringDmg, gameObject);
            col.enabled = false;
        }
    }
}
