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

        // Update is called once per frame
        void Update()
        {
            if (gGame.CurrentState != GameState.Paused && gGame.CurrentState != GameState.Interacting)
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

        public void Tick()
        {
            BehaviorManager.instance.Tick();

            gTime.CurrentTick++;

            UpdateLighting();

            //Change the speed if characters are or are not active
            if (!gStudio.CharactersActive && gGame.CurrentSpeed == GameSpeed.Normal)
            {
                gGame.UpdateGameSpeed(GameSpeed.Fast);
            }
            else if(gStudio.CharactersActive && gGame.CurrentSpeed == GameSpeed.Fast)
            {
                gGame.UpdateGameSpeed(GameSpeed.Normal);
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
                if(gGame.CurrentSpeed == GameSpeed.Normal)
                {
                    return gTime.TickLength;
                }
                else if (gGame.CurrentSpeed == GameSpeed.Fast)
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
