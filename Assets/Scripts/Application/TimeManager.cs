using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

namespace Devlike.Timing
{
    public class TimeManager : ExecutableBehaviour
    {
        private GlobalGame game;
        private GlobalTime time;
        private GlobalStudio studio;

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
            time = GameManager.instance.GetGlobal("Time") as GlobalTime;
            game = GameManager.instance.GetGlobal("Game") as GlobalGame;
            studio = GameManager.instance.GetGlobal("Studio") as GlobalStudio;

            lastState = game.CurrentState;
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
                game.UpdateGameState(state); //Update the Game Manager state
            }            
        }

        public void Tick()
        {
            BehaviorManager.instance.Tick();

            time.CurrentTick++;

            UpdateLighting();

            //Change the speed if characters are or are not active
            if (!studio.CharactersActive && lastState == GameState.Normal)
            {
                EventManager.instance.ChangeGameState(GameState.Fast);
            }
            else if(studio.CharactersActive && lastState == GameState.Fast)
            {
                EventManager.instance.ChangeGameState(GameState.Normal);
            }

            //Check if the day has ended
            if (time.CurrentTick > time.DayEndTick)
            {
                time.CurrentTick = 0;
                NextDay();
            }

            //Send a Tick
            EventManager.instance.Tick();
        }

        private void NextDay()
        {
            time.CurrentDayInt++;
            if(time.CurrentDayInt > 6)
            {
                time.CurrentWeek++;
                time.CurrentDayInt = 0;
            }
        }

        private float TickLength
        {
            get
            {
                if(lastState == GameState.Normal)
                {
                    return time.TickLength;
                }
                else if (lastState == GameState.Fast)
                {
                    return time.FastTickLength;
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
                float percent = (float)time.CurrentTick / (float)time.DayEndTick;
                return percent;
            }
        }
    }
}
