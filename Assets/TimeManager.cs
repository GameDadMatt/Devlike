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
    private float curTickLength = 0f;
    private float seconds = 0f;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        UIEvents.instance.onChangeSpeed += ChangeSpeed;

        curTickLength = GlobalVariables.value.normalTickLength;
        CurrentTick = GlobalVariables.value.DayTickLength; //Force us to start at the beginning of the day
        Tick();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GlobalVariables.value.paused)
        {
            seconds += Time.deltaTime;
            if (seconds >= curTickLength)
            {
                seconds -= curTickLength;
                Tick();
            }
        }
    }

    private void ChangeSpeed(GameSpeed speed)
    {
        Debug.Log(speed);

        switch (speed)
        {
            case GameSpeed.Slow:
                curTickLength = GlobalVariables.value.slowTickLength;
                break;
            case GameSpeed.Normal:
                curTickLength = GlobalVariables.value.normalTickLength;
                break;
            case GameSpeed.Fast:
                curTickLength = GlobalVariables.value.fastTickLength;
                break;
        }

        Debug.Log("Tick Length is now " + curTickLength);
    }

    public event Action OnTick;
    public event Action OnDayStart;
    public void Tick()
    {
        BehaviorManager.instance.Tick();

        CurrentTick++;

        if (CurrentTick > GlobalVariables.value.DayTickLength)
        {
            CurrentTick = 0;
            OnDayStart?.Invoke();
        }

        OnTick?.Invoke();
    }
}
