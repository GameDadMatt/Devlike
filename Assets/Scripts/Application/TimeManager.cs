using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using Devlike.UI;

namespace Devlike.Timing
{
    public class TimeManager : ExecutableBehaviour
    {
        [SerializeField]
        private Light DirectionalLight;
        [SerializeField]
        private List<Light> CeilingLights = new List<Light>();
        [SerializeField]
        private LightingPreset Preset;

        private float seconds = 0f;
        [SerializeField]
        private GameState state = GameState.Paused;
        private Day CurrentDay { get { return (Day)GameValues.CurrentDay; } }

        protected override void OnStart()
        {
            GameValues.CurrentDayInt = 1;
            EventManager.instance.OnChangeGameState += UpdateSpeed;
            GameManager.instance.State = state;
        }

        // Update is called once per frame
        void Update()
        {
            if (state != GameState.Paused && state != GameState.Interacting)
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
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((TimePercent * 360f) - 90f, 96f, 0f));
            foreach(Light light in CeilingLights)
            {
                light.color = Preset.InteriorColour.Evaluate(TimePercent);
            }
        }

        private void UpdateSpeed(GameState state)
        {
            if(state != this.state)
            {
                this.state = state;
                GameManager.instance.State = state; //Update the Game Manager state
            }            
        }

        public void Tick()
        {
            BehaviorManager.instance.Tick();

            currentTick++;

            GameplayUI.instance.SetTime(CurrentDay.ToString(), DisplayTime);
            UpdateLighting();

            //Change the speed if characters are or are not active
            if (!StudioManager.instance.CharactersActive && state == GameState.Normal)
            {
                EventManager.instance.ChangeGameState(GameState.Fast);
            }
            else if(StudioManager.instance.CharactersActive && state == GameState.Fast)
            {
                EventManager.instance.ChangeGameState(GameState.Normal);
            }

            //Check if the day has ended
            if (currentTick > StartingValues.value.DayEndTick)
            {
                currentTick = 0;
                NextDay();
            }

            //Send a Tick
            EventManager.instance.Tick();
        }

        private void NextDay()
        {
            currentDay++;
            if(currentDay > 6)
            {
                currentWeek++;
                GameplayUI.instance.SetWeek(currentWeek);
                currentDay = 0;
            }
        }

        private float TickLength
        {
            get
            {
                if(state == GameState.Normal)
                {
                    return StartingValues.value.TickLength;
                }
                else if (state == GameState.Fast)
                {
                    return StartingValues.value.IdleTickLength;
                }
                else
                {
                    return 0f;
                }
            }
        }

        private float TimePercent
        {
            get
            {
                float percent = (float)currentTick / (float)StartingValues.value.DayEndTick;
                return percent;
            }
        }
    }
}
