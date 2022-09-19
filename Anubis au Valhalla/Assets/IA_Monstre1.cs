using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Monstre1 : MonoBehaviour
{
    public float vieMax;
    public float vieActuelle;
    public Animator animator;

    private void Start()
    {
        vieActuelle = vieMax;
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(AnimationDamaged());
        vieActuelle -= damage;

        if (vieActuelle <= 0)
        {
            Die();
        }
    }

    IEnumerator AnimationDamaged()
    {
        animator.SetBool("IsTouched", true);
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("IsTouched", false); 
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
