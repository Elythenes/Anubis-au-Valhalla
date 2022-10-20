using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostDash : MonoBehaviour
{
    public float ghostDelay;
    public float ghostDelaySeconds;
    public GameObject ghost;
    public bool activerEffet;
    public List<GameObject> tousLesSprites;
    public Vector3 lastPlayerPos;
    public float frequency;

    public void OnEnable()
    {
        ghostDelaySeconds = ghostDelay;
        Debug.Log(ghostDelaySeconds);
    }
    
    void Update()
    {
        ghostDelaySeconds -= Time.deltaTime;
        if (!CharacterController.instance.isAttacking)
        {
            if (Vector3.Distance(CharacterController.instance.transform.position,lastPlayerPos) >= frequency) // Créer un effet fantôme derrière le player pendant le dash
            {
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                tousLesSprites.Add(currentGhost);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentGhost.transform.localScale = this.transform.localScale;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                StartCoroutine(Destroyghost(currentGhost));
                lastPlayerPos = CharacterController.instance.transform.position;
            } 
        }

        if (ghostDelaySeconds < 0)
        {
            Debug.Log("stop");
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
