using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Tasks;
using Devlike.Timing;
using BehaviorDesigner.Runtime;

namespace Devlike.Characters
{
    /// <summary>
    /// An individual character in the studio
    /// </summary>
    public class Character : MonoBehaviour
    {
        //Personality
        public Profile Profile { get; private set; }

        //Tasks
        public TaskList Tasks { get; private set; } = new TaskList();
        public TaskType CurrentTask { get; private set; }
        public EmotionType CurrentEmotion { get; private set; }
        public IncidentType RememberedIncident { get; private set; }

        //Days
        public int WorkStart { get { return GlobalVariables.value.WorkStartTick + Profile.WorkStartMod; } }
        public int WorkEnd { get { return GlobalVariables.value.WorkEndTick + Profile.WorkEndMod; } }
        public int CurrentTickRef { get { return TimeManager.instance.CurrentTick; } }
        public CharacterState CurrentState { get; set; }

        //Position
        public DoingType CurrentDoing { get; set; }
        public NPCInteractable Desk { get; private set; }
        public NPCInteractable Home { get; private set; }
        private NPCInteractable curInteract;

        //Needs
        public DoingTracker Rest { get; private set; } = new DoingTracker(DoingType.Rest, 1f, 0.3f);
        public DoingTracker Food { get; private set; } = new DoingTracker(DoingType.Food, 1f, 0.3f);
        public DoingTracker Insp { get; private set; } = new DoingTracker(DoingType.Inspiration, 1f, 0.3f);
        public DoingTracker Socl { get; private set; } = new DoingTracker(DoingType.Social, 1f, 0.3f);

        public float RestBurnRate { get { return GlobalVariables.value.BaseRestBurn * Profile.RestDropMultiplier; } }
        public float FoodBurnRate { get { return GlobalVariables.value.BaseFoodBurn * Profile.FoodDropMultiplier; } }
        public float InspBurnRate { get { return GlobalVariables.value.BaseInspBurn * Profile.InspDropMultiplier; } }
        public float SoclBurnRate { get { return GlobalVariables.value.BaseSoclBurn * Profile.SoclDropMultiplier; } }

        //Moods
        public float MoodImpact { get; private set; } = 0f;
        private float MoodImpactBurn { get { return GlobalVariables.value.MoodImpactBurn; } }

        //Velocity
        public float Velocity { get { return (GlobalVariables.value.BaseVelocity * Profile.VelocityMultiplier) * CappedMoodImpact; } }
        public float BugChance { get { return (GlobalVariables.value.BaseBugChance * Profile.BugChanceMultiplier) * (GlobalVariables.value.moodImpactMax - CappedMoodImpact); } }
        

        public void SetupCharacter()
        {
            TimeManager.instance.OnTick += Tick;
            Desk = InteractableManager.instance.ClaimWorkPosition();
            Home = InteractableManager.instance.Home;
            curInteract = Home;
        }

        public void StartWork()
        {
            Rest.curValue = Random.Range(0.7f, 1f);
            Food.curValue = Random.Range(0.8f, 1f);
            Insp.curValue = Random.Range(0.1f, 1f);
            Socl.curValue = Random.Range(0.1f, 1f);
        }

        public void Tick()
        {
            switch (CurrentState)
            {
                case CharacterState.Start:
                    StartWork();
                    break;
                case CharacterState.Active:
                    Rest.curValue -= RestBurnRate;
                    Food.curValue -= FoodBurnRate;
                    Insp.curValue -= InspBurnRate;
                    Socl.curValue -= SoclBurnRate;
                    MoodImpact -= MoodImpactBurn;
                    break;
                case CharacterState.End:
                    EndWork();
                    break;
                case CharacterState.Inactive:
                    break;
            }
        }

        public void EndWork()
        {

        }

        public void NewPosition(NPCInteractable i)
        {
            curInteract.inUse = false;
            curInteract = i;
            curInteract.inUse = true;
            CurrentDoing = i.type;
            transform.position = curInteract.thing.transform.position;
        }

        public float Mood
        {
            get
            {
                return (Profile.BaseMood * ((Rest.curValue + Food.curValue + Insp.curValue + Socl.curValue) / 4)) + MoodImpact;
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
