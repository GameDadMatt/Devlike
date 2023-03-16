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
        private GameState lastState = GameState.Paused;

        protected override void SetListeners()
        {
            GameValues.CurrentDayInt = 1;
            EventManager.instance.OnChangeGameState += UpdateSpeed;
            GameValues.CurrentState = lastState; //Set the starting state
        }

        // Update is called once per frame
        void Update()
        {
            if (lastState != GameState.Paused && lastState != GameState.Interacting)
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
            if(state != this.lastState)
            {
                this.lastState = state;
                GameValues.CurrentState = state; //Update the Game Manager state
            }            
        }

        public void Tick()
        {
            BehaviorManager.instance.Tick();

            GameValues.CurrentTick++;

            UpdateLighting();

            //Change the speed if characters are or are not active
            if (!GameValues.CharactersActive && lastState == GameState.Normal)
            {
                EventManager.instance.ChangeGameState(GameState.Fast);
            }
            else if(GameValues.CharactersActive && lastState == GameState.Fast)
            {
                EventManager.instance.ChangeGameState(GameState.Normal);
            }

            //Check if the day has ended
            if (GameValues.CurrentTick > StartingValues.value.DayEndTick)
            {
                GameValues.CurrentTick = 0;
                NextDay();
            }

            //Send a Tick
            EventManager.instance.Tick();
        }

        private void NextDay()
        {
            GameValues.CurrentDayInt++;
            if(GameValues.CurrentDayInt > 6)
            {
                GameValues.CurrentWeek++;
                GameValues.CurrentDayInt = 0;
            }
        }

        private float TickLength
        {
            get
            {
                if(lastState == GameState.Normal)
                {
                    return StartingValues.value.TickLength;
                }
                else if (lastState == GameState.Fast)
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
                float percent = (float)GameValues.CurrentTick / (float)StartingValues.value.DayEndTick;
                return percent;
            }
        }
    }
}
