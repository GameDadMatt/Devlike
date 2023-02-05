using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    
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
        if (seconds >= GlobalVariables.value.tickLength)
        {
            seconds -= GlobalVariables.value.tickLength;
            Tick();
        }
    }

    public event Action OnTick;
    public event Action OnDayStart;
    public void Tick()
    {
        CurrentTick++;
        if(CurrentTick > GlobalVariables.value.DayTickLength)
        {
            CurrentTick = 0;
            OnDayStart?.Invoke();
        }
        OnTick?.Invoke();
    }
}
