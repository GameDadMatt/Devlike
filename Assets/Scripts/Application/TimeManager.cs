using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

namespace Devlike.Timing
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager instance;

        private float seconds = 0f;
        private int currentDay = 1;
        public int CurrentTick { get; private set; } = 0;
        public int CurrentWeek { get; private set; } = 0;
        public Day CurrentDay { get { return (Day)currentDay; } }

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void Start()
        {
            UI.WeekViewUI.instance.Setup(PreviousDay, CurrentDay, NextDay);
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

        private Day NextDay
        {
            get
            {
                if (currentDay < 6)
                {
                    return (Day)currentDay + 1;
                }
                else
                {
                    return (Day)0;
                }
            }
        }

        private Day PreviousDay
        {
            get
            {
                if (currentDay > 0)
                {
                    return (Day)currentDay - 1;
                }
                else
                {
                    return (Day)6;
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

        public float ProgressToNextDay
        {
            get
            {
                float percent = (float)CurrentTick / (float)GlobalVariables.value.DayEndTick;
                return percent;
            }
        }
    }
}
