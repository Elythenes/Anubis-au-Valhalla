
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
    public TextMeshProUGUI textAmelio;
    public GameObject popUp;
    public bool fadeIn;
    public static UINewGlyphManager instance;
    public Sprite iconePower1;
    public Sprite iconePower2;
    public GameObject VFXAme;
    public GameObject VFXSable;

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
        textAmelio.text = "Nouveau Glyphe obtenu \n Consutez l'inventaire";
        popUp.GetComponentInChildren<Image>().sprite = CollectThings.instance.collectableGlyph.GetComponent<SpriteRenderer>().sprite;
        GameObject textNewGlyph = Instantiate(popUp, transform.position+ new Vector3(0,1,0) ,Quaternion.identity);
        Destroy(textNewGlyph,1f);
    }
    
    public void NewPower()
    {
        StopAllCoroutines();
        StartCoroutine(ActivateDesactivate());
      
        if (CollectThings.instance.collectablePower.GetComponent<NewPowerRepository>().newPowerType == NewPowerType.Power1)
        {
            GameObject VFXames = Instantiate(VFXAme, DamageManager.instance.transform.position- new Vector3(0,1,0), Quaternion.identity);
            VFXames.transform.parent = DamageManager.instance.transform;
            textAmelio.text = "Pouvoir d'âmes amélioré \n Consultez l'inventaire";
            popUp.GetComponentInChildren<Image>().sprite = iconePower1;
        }
        else if(CollectThings.instance.collectablePower.GetComponent<NewPowerRepository>().newPowerType == NewPowerType.Power2)
        {
            GameObject VFXsableOBJ = Instantiate(VFXSable, DamageManager.instance.transform.position- new Vector3(0,1,0), Quaternion.identity);
            VFXsableOBJ.transform.parent = DamageManager.instance.transform;
            textAmelio.text = "Pouvoir de sable amélioré \n Consultez l'inventaire";
            popUp.GetComponentInChildren<Image>().sprite = iconePower2;
        }
        GameObject textNewGlyph = Instantiate(popUp, transform.position + new Vector3(0,1,0),Quaternion.identity);
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
