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
    private CircleCollider2D collider2D;
    public GameObject child;
    public GameObject vFXChild;
    public SpriteRenderer sr;
    public SpriteMask mask;

    private void Awake()
    {
        collider2D = GetComponent<CircleCollider2D>();
        child.transform.DOScale(child.transform.localScale * 40, 20f);
        vFXChild.transform.DOScale(Vector3.one * 40, 20f);
    }

    // Start is called before the first frame update
    void Start()
    {
        ia = IA_Valkyrie.instance;
        currentExpansionAmount = ia.expansionAmount;
        currentRate = ia.ExpansionRate;
        Destroy(gameObject,20f);
        /*StartCoroutine(Expand());

        StartCoroutine(Increase());*/
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

    IEnumerator Increase()
    {
        yield return new WaitForSeconds(ia.ExpansionSpeedUpRate);
        currentExpansionAmount += 0.001f;
        StartCoroutine(Increase());
    }
}
