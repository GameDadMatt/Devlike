using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using Devlike.UI;

namespace Devlike.Timing
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager instance;

        [SerializeField]
        private Light DirectionalLight;
        [SerializeField]
        private LightingPreset Preset;

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
            GameplayUI.instance.SetWeek(CurrentWeek);
            GameplayUI.instance.SetTime(CurrentDay.ToString(), DisplayTime);
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

        private void UpdateLighting()
        {
            RenderSettings.ambientLight = Preset.AmbientColour.Evaluate(TimePercent);
            DirectionalLight.color = Preset.DirectionalColour.Evaluate(TimePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((TimePercent * 360f) - 90f, 90f, 0f));
        }

        public event Action OnTick;
        public event Action OnDayStart;
        public void Tick()
        {
            BehaviorManager.instance.Tick();

            CurrentTick++;

            GameplayUI.instance.SetTime(CurrentDay.ToString(), DisplayTime);
            UpdateLighting();

            if (CurrentTick > GlobalVariables.value.DayEndTick)
            {
                CurrentTick = 0;
                NextDay();
                OnDayStart?.Invoke();
            }

            OnTick?.Invoke();
        }

        private void NextDay()
        {
            currentDay++;
            if(currentDay > 6)
            {
                CurrentWeek++;
                GameplayUI.instance.SetWeek(CurrentWeek);
                currentDay = 0;
            }
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

        private float HoursMinutes
        {
            get
            {
                float time = (float)CurrentTick / (float)GlobalVariables.value.TicksPerHour;
                return time;
            }
        }

        private string DisplayTime
        {
            get
            {
                string ampm = "am";
                float h = Mathf.FloorToInt(HoursMinutes);
                float m = HoursMinutes % 1;
                m = 60 * m;
                if(h > 12)
                {
                    h -= 12;
                    ampm = "pm";
                }
                string hours = h.ToString();
                string minutes = m.ToString();
                if(hours.Length < 2)
                {
                    hours = "0" + hours;
                }
                if(minutes.Length < 2)
                {
                    minutes = "0" + minutes;
                }
                return hours + ":" + minutes + ampm;
            }
        }

        private float TimePercent
        {
            get
            {
                float percent = (float)CurrentTick / (float)GlobalVariables.value.DayEndTick;
                return percent;
            }
        }
    }
}
