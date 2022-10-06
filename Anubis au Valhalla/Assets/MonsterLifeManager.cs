using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterLifeManager : MonoBehaviour
{
    public int vieMax;
    public int vieActuelle;
    public Animator animator;
    public int soulValue = 4;
    public GameObject textDamage;
    
    
    private void Start()
    {
        vieActuelle = vieMax;
    }
    
    public void TakeDamage(int damage)
    {
        StartCoroutine(AnimationDamaged());
        vieActuelle -= damage;
        HealthBarMonstre.instance.SetHealth(vieActuelle);

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
    
    public void DamageText(int damageAmount)
    {
        textDamage.GetComponentInChildren<TextMeshPro>().SetText(damageAmount.ToString());
        Instantiate(textDamage, new Vector3(transform.position.x,transform.position.y + 1,-5), Quaternion.identity);
    }
    
    void Die()
    {
        Souls.instance.CreateSouls(gameObject.transform.position, soulValue);
        Destroy(gameObject);
    }
}
