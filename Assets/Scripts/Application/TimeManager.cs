using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

namespace Devlike.Timing
{
    public enum Day { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }

    public class TimeManager : MonoBehaviour
    {
        public static TimeManager instance;

        private float seconds = 0f;
        public int CurrentTick { get; private set; } = 0;
        public int CurrentWeek { get; private set; } = 0;
        public int CurrentDay { get; private set; } = 0;

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
            if (GameManager.instance.State != GameState.Paused)
            {
                seconds += Time.deltaTime;
                if (seconds >= TickLength)
                {
                    seconds -= TickLength;
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

        private float TickLength
        {
            get
            {
                if (StudioManager.instance.CharactersActive)
                {
                    return GlobalVariables.value.TickLength;
                }
                else
                {
                    return GlobalVariables.value.IdleTickLength;
                }
            }
        }
    }
}
