
using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class UINewGlyphManager : MonoBehaviour
{
    public CanvasGroup myCanvas;
    public float alphaGained;
    public float textDuration;
    public GameObject popUp;
    public bool fadeIn;
    public static UINewGlyphManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        transform.position = CharacterController.instance.transform.position + new Vector3(0, 2, 0);
        if (fadeIn)
        {
            if (myCanvas.alpha < 1)
            {
                myCanvas.alpha += alphaGained;
            }
        }
        else
        {
            if (myCanvas.alpha > 0)
            {
                myCanvas.alpha -= alphaGained;
            }
        }
    }

    public void NewGlyph()
    {
        StopAllCoroutines();
        StartCoroutine(ActivateDesactivate());
        popUp.GetComponentInChildren<Image>().sprite = CollectThings.instance.collectableGlyph.GetComponent<SpriteRenderer>().sprite;
        GameObject textNewGlyph = Instantiate(popUp, transform.position + new Vector3(0,1,0),quaternion.identity);
        Destroy(textNewGlyph,1f);
    }


    IEnumerator ActivateDesactivate()
    {
        myCanvas.alpha = 0;
        fadeIn = true;
        yield return new WaitForSeconds(textDuration);
        fadeIn = false;
        yield return new WaitForSeconds(textDuration/3);
        gameObject.SetActive(false);
    }
}
