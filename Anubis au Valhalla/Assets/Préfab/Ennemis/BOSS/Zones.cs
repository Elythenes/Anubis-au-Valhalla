using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zones : MonoBehaviour
{
    public FenrirBoss.ZoneType zone;
    public FenrirBoss main;

    public void Start()
    {
        main = FenrirBoss.instance;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (zone)
            {
                case FenrirBoss.ZoneType.Center:
                    main.center = true;
                    break;
                case FenrirBoss.ZoneType.Left:
                    main.left = true;
                    break;
                case FenrirBoss.ZoneType.Right:
                    main.right = true;
                    break;
                case FenrirBoss.ZoneType.Back:
                    main.back = true;
                    break;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (zone)
            {
                case FenrirBoss.ZoneType.Center:
                    main.center = false;
                    break;
                case FenrirBoss.ZoneType.Left:
                    main.left = false;
                    break;
                case FenrirBoss.ZoneType.Right:
                    main.right = false;
                    break;
                case FenrirBoss.ZoneType.Back:
                    main.back = false;
                    break;
            }
        }
    }
}
