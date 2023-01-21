using System;
using System.Collections;
using System.Collections.Generic;
using GenPro;
using TMPro;
using UnityEngine;

public class TimerChallenge : MonoBehaviour
{
    public float internalTimer = 60f;
    public float timerLimit = 30f;

    public SalleGenerator sg;
    public int tempsAdditionnel;
    public int multiplicateurTemps;
    
    private int seconds;
    
    

    public TextMeshProUGUI timer;
    // Start is called before the first frame update

    void OnEnable()
    {
        CalculTempsAddi();
        internalTimer = timerLimit + tempsAdditionnel;
    }

    // Update is called once per frame
    void Update()
    {
        if (SalleGenerator.Instance.currentRoom.roomDone) return;
        if (internalTimer >= 0)
        {
            internalTimer -= Time.deltaTime;
            seconds = Mathf.RoundToInt(internalTimer);
            timer.text = seconds.ToString();
        }
    }

    void CalculTempsAddi()
    {
        int temps = sg.roomsDone * multiplicateurTemps;
        if (sg.zone2)
        {
            temps = (sg.roomsDone + 8) * multiplicateurTemps;
        }
        //Debug.Log(temps);
        switch (temps)
        {
            case <10:
                temps = 0;
                break;
            
            case >10:
                if (temps < 20)
                {
                    temps = 10;
                }
                else
                {
                    if (temps < 30)
                    {
                        temps = 20;
                    }
                    else
                    {
                        temps = 30;
                    }
                }
                break;
        }
        tempsAdditionnel = temps;
    }
    
}
