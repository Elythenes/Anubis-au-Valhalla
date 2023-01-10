using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellStaticArea : MonoBehaviour
{
    public GameObject targetUser;

    public GameObject flameArea;
    public int timerSpell1 = 2;

    //Pour un Spell qui apparaît et disparaît après une durée
    //(ici int déclarée au début "timerSpell1")
    public void TimeLimitedSpell(GameObject gb, int timer) 
    {
        var gbInstance = Instantiate(gb, new Vector3(targetUser.transform.position.x, targetUser.transform.position.y/*-(targetUser.transform.localScale.y/2)*/, 0), Quaternion.identity);
        Debug.Log("Spell1 used");
        StartCoroutine(TimeLimitedGb(gbInstance, timerSpell1));
    }
    
    //Coroutine pour les spells qui doivent disparaître
    IEnumerator TimeLimitedGb(GameObject gbInstance, int timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gbInstance);
        Debug.Log("destroyed");
    }
}
