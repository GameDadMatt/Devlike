using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTypes;
using BehaviorDesigner.Runtime;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    
    public int CurrentTick { get; private set; } = 0;
    private float seconds = 0f;

    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        Tick();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.State != GameState.Paused)
        {
            seconds += Time.deltaTime;
            if (seconds >= GlobalVariables.value.TickLength)
            {
                seconds -= GlobalVariables.value.TickLength;
                Tick();
            }
        }        
    }

    public event Action OnTick;
    public event Action OnDayStart;
    public void Tick()
    {
        BehaviorManager.instance.Tick();

        CurrentTick++;

        if (CurrentTick > GlobalVariables.value.DayEndTick)
        {
            CurrentTick = 0;
            OnDayStart?.Invoke();
        }

        OnTick?.Invoke();
    }
}
