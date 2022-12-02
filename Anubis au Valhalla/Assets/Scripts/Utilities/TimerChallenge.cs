using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerChallenge : MonoBehaviour
{
    private float internalTimer = 60f;

    private int minutes;

    private int seconds;

    public TextMeshProUGUI timer;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        internalTimer -= Time.deltaTime;
        seconds = Mathf.RoundToInt(internalTimer);
        timer.text = seconds.ToString();

    }
}
