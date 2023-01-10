using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellThrowingProjectile : MonoBehaviour
{
    public GameObject targetUser;
    
    public GameObject fireBall;
    public float launchVelocity = 100f;
    public int defaultTimer = 2;
    
    //Pour un Spell qui apparaît (et disparaît après avoir touché qqc (ennemi ou mur))
    public void SpellThrowing(GameObject gb)
    {
        var gbInstance = Instantiate(gb, new Vector3(targetUser.transform.position.x, targetUser.transform.position.y+targetUser.transform.localScale.y/2, 0), Quaternion.identity);
        gbInstance.GetComponent<Rigidbody2D>().AddForce(Camera.main.ScreenToWorldPoint(Input.mousePosition)*launchVelocity);
        StartCoroutine(TimeLimitedGb(gbInstance, defaultTimer));
    }
    
    //Coroutine pour les spells qui doivent disparaître
    IEnumerator TimeLimitedGb(GameObject gbInstance, int timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gbInstance);
        Debug.Log("destroyed");
    }
}
