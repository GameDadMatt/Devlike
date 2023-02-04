using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTypes;
using BehaviorDesigner.Runtime;

namespace Characters
{
    public class Character : MonoBehaviour
    {
        public List<Task> TaskList = new List<Task>();
        public TaskType CurrentTask { get; private set; }
        public EmotionType CurrentEmotion { get; private set; }
        public IncidentType RememberedIncident { get; private set; }

        public Need Rest { get; private set; } = new Need(NeedType.Rest, 1f, 0.3f);
        public Need Food { get; private set; } = new Need(NeedType.Food, 1f, 0.3f);
        public Need Insp { get; private set; } = new Need(NeedType.Inspiration, 1f, 0.3f);
        public Need Socl { get; private set; } = new Need(NeedType.Social, 1f, 0.3f);

        public float restBurnRate = 0.05f;
        public float foodBurnRate = 0.1f;
        public float inspBurnRate = 0.08f;
        public float soclBurnRate = 0.1f;

        public int dayStart { get; private set; } = 0;
        public int dayEnd { get; private set; } = 0;

        public void LowerNeeds()
        {
            Rest.curValue -= restBurnRate;
            Food.curValue -= foodBurnRate;
            Insp.curValue -= inspBurnRate;
            Socl.curValue -= soclBurnRate;
        }

        public void NeedTask(NeedType need)
        {
            throw new System.NotImplementedException();
        }

        public void RestoreNeed(NeedType need)
        {
            switch (need)
            {
                case NeedType.Rest:
                    Rest.curValue = 1f;
                    break;
                case NeedType.Food:
                    Food.curValue = 1f;
                    break;
                case NeedType.Inspiration:
                    Insp.curValue = 1f;
                    break;
                case NeedType.Social:
                    Socl.curValue = 1f;
                    break;
            }
        }

        public void Start()
        {
            TimeManager.instance.OnTick += Tick;
            dayStart = TimeManager.instance.DayStartTick;
            dayEnd = TimeManager.instance.DayEndTick;
        }

        private void Tick()
        {
            LowerNeeds();
            //BehaviorManager.instance.TIck
        }
    }
}
