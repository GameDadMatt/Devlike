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
        public CharacterState CurrentState { get; set; }
        public DoingType CurrentDoing { get; set; }

        //Needs
        public DoingTracker Rest { get; private set; } = new DoingTracker(DoingType.Rest, 1f, 0.3f);
        public DoingTracker Food { get; private set; } = new DoingTracker(DoingType.Food, 1f, 0.3f);
        public DoingTracker Insp { get; private set; } = new DoingTracker(DoingType.Inspiration, 1f, 0.3f);
        public DoingTracker Socl { get; private set; } = new DoingTracker(DoingType.Social, 1f, 0.3f);

        public float RestBurnRate { get { return GlobalVariables.value.BaseRestBurn * profile.RestDropMultiplier; } }
        public float FoodBurnRate { get { return GlobalVariables.value.BaseFoodBurn * profile.FoodDropMultiplier; } }
        public float InspBurnRate { get { return GlobalVariables.value.BaseInspBurn * profile.InspDropMultiplier; } }
        public float SoclBurnRate { get { return GlobalVariables.value.BaseSoclBurn * profile.SoclDropMultiplier; } }

        //Moods
        public float MoodImpact { get; private set; } = 0f;
        private float MoodImpactBurn { get { return GlobalVariables.value.MoodImpactBurn; } }

        //Velocity
        public float Velocity { get { return (GlobalVariables.value.BaseVelocity * profile.VelocityMultiplier) * CappedMoodImpact; } }
        public float BugChance { get { return (GlobalVariables.value.BaseBugChance * profile.BugChanceMultiplier) * (GlobalVariables.value.moodImpactMax - CappedMoodImpact); } }
        

        public void Start()
        {
            TimeManager.instance.OnTick += Tick;
            TimeManager.instance.OnDayStart += SetNeeds;
            profile.SetupProfile();
        }

        public void StartDay()
        {

        }

        public void SetNeeds()
        {
            Rest.curValue = Random.Range(0.7f, 1f);
            Food.curValue = Random.Range(0.8f, 1f);
            Insp.curValue = Random.Range(0.1f, 1f);
            Socl.curValue = Random.Range(0.1f, 1f);
        }

        public void Tick()
        {
            if (CurrentState == CharacterState.Start)
            {

            }
            if (CurrentState == CharacterState.Active)
            {
                Rest.curValue -= RestBurnRate;
                Food.curValue -= FoodBurnRate;
                Insp.curValue -= InspBurnRate;
                Socl.curValue -= SoclBurnRate;
                MoodImpact -= MoodImpactBurn;
            }
        }

        public float Mood
        {
            get
            {
                return (profile.BaseMood * ((Rest.curValue + Food.curValue + Insp.curValue + Socl.curValue) / 4)) + MoodImpact;
            }
        }

        public float CappedMoodImpact
        {
            get
            {
                if(Mood > GlobalVariables.value.moodImpactMax)
                {
                    return GlobalVariables.value.moodImpactMax;
                }
                else if(Mood < GlobalVariables.value.moodImpactMin)
                {
                    return GlobalVariables.value.moodImpactMin;
                }
                else
                {
                    return Mood;
                }
            }
        }

        public void ChangeMoodImpact(float impact)
        {
            MoodImpact += impact;
        }

        public void ChangeBugChanceMod(float mod)
        {

        }
    }
}
