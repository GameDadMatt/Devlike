using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTypes;
using BehaviorDesigner.Runtime;

namespace Characters
{
    public class Character : MonoBehaviour
    {
        //Personality
        public Profile profile = new Profile();

        //Tasks
        public List<Task> TaskList = new List<Task>();
        public TaskType CurrentTask { get; private set; }
        public EmotionType CurrentEmotion { get; private set; }
        public IncidentType RememberedIncident { get; private set; }

        //Days
        public int WorkStart { get { return GlobalVariables.value.WorkStartTick + profile.WorkStartMod; } }
        public int WorkEnd { get { return GlobalVariables.value.WorkEndTick + profile.WorkEndMod; } }
        public bool AtWork { get; set; }

        //Needs
        public Need Rest { get; private set; } = new Need(NeedType.Rest, 1f, 0.3f);
        public Need Food { get; private set; } = new Need(NeedType.Food, 1f, 0.3f);
        public Need Insp { get; private set; } = new Need(NeedType.Inspiration, 1f, 0.3f);
        public Need Socl { get; private set; } = new Need(NeedType.Social, 1f, 0.3f);

        public float RestBurnRate { get { return GlobalVariables.value.BaseRestBurn * profile.RestDropMultiplier; } }
        public float FoodBurnRate { get { return GlobalVariables.value.BaseFoodBurn * profile.FoodDropMultiplier; } }
        public float InspBurnRate { get { return GlobalVariables.value.BaseInspBurn * profile.InspDropMultiplier; } }
        public float SoclBurnRate { get { return GlobalVariables.value.BaseSoclBurn * profile.SoclDropMultiplier; } }

        //Moods
        public float MoodImpact { get; set; } = 0f; //MoodImpact can be modified externally to have an effect on the current mood
        private float MoodImpactBurn { get { return GlobalVariables.value.MoodImpactBurn; } }

        //Velocity
        public float Velocity { get { return (GlobalVariables.value.BaseVelocity * profile.VelocityMultiplier) * Mood; } }
        public float BugChance { get { return (GlobalVariables.value.BaseBugChance * profile.BugChanceMultiplier) * (1f - Mood); } }
        

        public void Start()
        {
            TimeManager.instance.OnTick += LowerNeeds;
            TimeManager.instance.OnDayStart += SetNeeds;
            profile.SetupProfile();
        }

        public void SetNeeds()
        {
            Rest.curValue = Random.Range(0.7f, 1f);
            Food.curValue = Random.Range(0.8f, 1f);
            Insp.curValue = Random.Range(0.1f, 1f);
            Socl.curValue = Random.Range(0.1f, 1f);
        }

        public void LowerNeeds()
        {
            if (AtWork)
            {
                Rest.curValue -= RestBurnRate;
                Food.curValue -= FoodBurnRate;
                Insp.curValue -= InspBurnRate;
                Socl.curValue -= SoclBurnRate;
                MoodImpact -= MoodImpactBurn;
            }            
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

        public float Mood
        {
            get
            {
                return (profile.BaseMood * ((Rest.curValue + Food.curValue + Insp.curValue + Socl.curValue) / 4)) + MoodImpact;
            }
        }
    }
}
