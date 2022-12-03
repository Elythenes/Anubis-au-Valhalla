using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class GhostDash : MonoBehaviour
{
    public float ghostDelay;
    public float ghostDelaySeconds;
    public GameObject ghost;
    public Sprite ghostSprite;
    public bool activerEffet;
    public List<GameObject> tousLesSprites;
    public Vector3 lastPlayerPos;
    public float frequency;

    public void OnEnable()
    {
        ghostDelaySeconds = ghostDelay;
    }
    
    void Update()
    {
        ghostDelaySeconds -= Time.deltaTime;
        if (!CharacterController.instance.isAttacking)
        {
            if (Vector3.Distance(CharacterController.instance.transform.position,lastPlayerPos) >= frequency) // Créer un effet fantôme derrière le player pendant le dash
            {
                GameObject currentGhost = Instantiate(ghost, transform.position,transform.rotation);
                tousLesSprites.Add(currentGhost);
                //currentGhost.transform.localScale = this.transform.localScale;
                currentGhost.GetComponent<SpriteRenderer>().sprite = ghostSprite;
                StartCoroutine(Destroyghost(currentGhost));
                lastPlayerPos = CharacterController.instance.transform.position;
            } 
        }

        if (ghostDelaySeconds < 0)
        {
            ghostDelaySeconds = ghostDelay;
            enabled = false;
        }
    }

    public IEnumerator Destroyghost(GameObject ghost)
    {
        Destroy(ghost, 1f);
        yield return new WaitForSeconds(1f);
        tousLesSprites.Remove(ghost);
    }
}
