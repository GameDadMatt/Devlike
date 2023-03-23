using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

namespace Devlike.Timing
{
    public class TimeManager : ExecutableBehaviour
    {
        [SerializeField]
        private GlobalGame gGame;
        [SerializeField]
        private GlobalTime gTime;
        [SerializeField]
        private GlobalStudio gStudio;

        [SerializeField]
        private Light DirectionalLight;
        [SerializeField]
        private List<Light> CeilingLights = new List<Light>();
        [SerializeField]
        private LightingPreset Preset;

        private float seconds = 0f;
        [SerializeField]
        private GameState lastState;

        protected override void SetProperties()
        {
            lastState = gGame.CurrentState;
        }

        protected override void SetListeners()
        {
            EventManager.instance.OnChangeGameState += UpdateSpeed;
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
                gGame.UpdateGameState(state); //Update the Game Manager state
            }            
        }

        public void Tick()
        {
            BehaviorManager.instance.Tick();

            gTime.CurrentTick++;

            UpdateLighting();

            //Change the speed if characters are or are not active
            if (!gStudio.CharactersActive && lastState == GameState.Normal)
            {
                EventManager.instance.ChangeGameState(GameState.Fast);
            }
            else if(gStudio.CharactersActive && lastState == GameState.Fast)
            {
                EventManager.instance.ChangeGameState(GameState.Normal);
            }

            //Check if the day has ended
            if (gTime.CurrentTick > gTime.DayEndTick)
            {
                gTime.CurrentTick = 0;
                NextDay();
            }

            //Send a Tick
            EventManager.instance.Tick();
        }

        private void NextDay()
        {
            gTime.CurrentDayInt++;
            if(gTime.CurrentDayInt > 6)
            {
                gTime.CurrentWeek++;
                gTime.CurrentDayInt = 0;
                EventManager.instance.WeekStart();
            }
            EventManager.instance.DayStart();
        }

        private float TickLength
        {
            get
            {
                if(lastState == GameState.Normal)
                {
                    return gTime.TickLength;
                }
                else if (lastState == GameState.Fast)
                {
                    return gTime.FastTickLength;
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
                float percent = (float)gTime.CurrentTick / (float)gTime.DayEndTick;
                return percent;
            }
        }
    }
}
