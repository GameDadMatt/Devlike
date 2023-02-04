using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public float tickLength = 1f;
    public int DayTickLength { get; private set; } = 120;
    public int DayStartTick { get; private set; } = 3;
    public int DayEndTick { get; private set; } = 80;
    public int CurrentTick { get; private set; } = 0;
    private float seconds = 0f;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        seconds += Time.deltaTime;
        if (seconds >= tickLength)
        {
            seconds -= tickLength;
            Tick();
        }
    }

    public event Action OnTick;
    public void Tick()
    {
        CurrentTick++;
        if(CurrentTick > DayTickLength)
        {
            CurrentTick = 0;
        }
        OnTick?.Invoke();
    }
}
