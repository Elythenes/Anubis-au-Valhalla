using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostDash : MonoBehaviour
{
    public float ghotsDelay;
    private float ghotsDelaySeconds;
    public GameObject ghost;
    public bool activerEffet;
    public List<GameObject> tousLesSprites;

    public void Start()
    {
        ghotsDelay = ghotsDelaySeconds;
    }
    
    void Update()
    {
        if (activerEffet)
        {
            if (ghotsDelaySeconds > 0) // Créer un effet fantôme derrière le player pendant le dash
            {
                ghotsDelaySeconds -= Time.deltaTime;
            }
            else
            {
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                tousLesSprites.Add(currentGhost);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentGhost.transform.localScale = this.transform.localScale;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                ghotsDelaySeconds = ghotsDelay;
                Destroy(currentGhost, 1f);
            } 
        }
    }
}
