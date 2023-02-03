using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTypes;
using BehaviorDesigner.Runtime;

namespace Characters
{
    public interface ICharacter
    {
        void Tick();
    }

    public class Character : MonoBehaviour, ICharacter
    {
        public List<Task> TaskList = new List<Task>();
        public TaskType CurrentTask { get; private set; }
        public EmotionType CurrentEmotion { get; private set; }
        public IncidentType RememberedIncident { get; private set; }

        public Need Rest { get; private set; } = new Need(1f, 0.3f);
        public Need Food { get; private set; } = new Need(1f, 0.3f);
        public Need Insp { get; private set; } = new Need(1f, 0.3f);

        public void LowerNeeds(float rest, float food, float insp)
        {
            Rest.curValue -= rest;
            Food.curValue -= food;
            Insp.curValue -= insp;
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
            }
        }

        public void Start()
        {
            GameManager.instance.OnTick += Tick;
        }

        public void Tick()
        {
            LowerNeeds(0.05f, 0.05f, 0.05f);
            //BehaviorManager.instance.TIck
        }

        public void Update()
        {
            
        }
    }
}
