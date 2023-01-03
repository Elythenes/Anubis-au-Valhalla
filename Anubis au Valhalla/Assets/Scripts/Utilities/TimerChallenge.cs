using System;
using System.Collections;
using System.Collections.Generic;
using GenPro;
using TMPro;
using UnityEngine;

public class TimerChallenge : MonoBehaviour
{
    public float internalTimer = 60f;
    public float timerLimit;

    private int seconds;

    public TextMeshProUGUI timer;
    // Start is called before the first frame update

    void OnEnable()
    {
        internalTimer = timerLimit;
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
}
