using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class AmphoreManager : MonoBehaviour
{
    public ParticleSystem partculesAmphores;
    public SpriteRenderer sr;
    public BoxCollider2D box;
    public GameObject ombre;
    public GameObject miniMap;
    public float timeToKill;
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip audioClipBreak;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("AttaqueNormale"))
        {
            audioSource.PlayOneShot(audioClipBreak);
            Souls.instance.CreateSouls(transform.position, Random.Range(0,4));
            Instantiate(partculesAmphores, transform.position,Quaternion.identity);
            Destroy(ombre);
            Destroy(miniMap);
            StartCoroutine(WaitKill());
            sr.enabled = false;
            box.enabled = false;
        }
    }

    /*private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            Souls.instance.CreateSouls(transform.position, Random.Range(0,4));
            Instantiate(partculesAmphores, new Vector3(transform.position.x,transform.position.y,10),Quaternion.identity);
            Destroy(gameObject);
        }
    }*/

    IEnumerator WaitKill()
    {
        yield return new WaitForSeconds(timeToKill);
        Destroy(gameObject);
    }
}

