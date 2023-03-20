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
        [SerializeField]
        private Renderer spriteRenderer;
        [SerializeField]
        private Moodlet moodlet;
        private bool displayingMoodlet = false;
        private int moodletTicks = 0;

        //Personality
        public Profile Profile { get; private set; }

        //Dialogue
        public DialogueContainer CurrentDialogue { get; set; }

        //Tasks
        public TaskList Tasks { get; private set; } = new TaskList();
        public int NumTasks { get { return Tasks.TotalCount; } }
        public TaskType CurrentTask { get; private set; }
        public MoodletType CurrentEmotion { get; private set; }

        //Days
        public int WorkStart { get { return StartingValues.value.WorkStartTick + Profile.WorkStartMod; } }
        public int WorkEnd { get { return StartingValues.value.WorkEndTick + Profile.WorkEndMod; } }
        public int CurrentTickRef { get { return GameValues.CurrentTick; } }
        public CharacterState CurrentState { get; set; }

        //Position
        public DoingType CurrentDoing { get; set; }
        public NPCInteractable Desk { get; private set; }
        public NPCInteractable Home { get; private set; }
        private NPCInteractable curInteract;

        //Needs
        public DoingTracker Food { get; private set; }
        public DoingTracker Insp { get; private set; }
        public DoingTracker Socl { get; private set; }
        public DoingTracker Rest { get; private set; }

        public float RestBurnRate { get { return StartingValues.value.BaseRestBurn * Profile.RestDropMultiplier; } }
        public float FoodBurnRate { get { return StartingValues.value.BaseFoodBurn * Profile.FoodDropMultiplier; } }
        public float InspBurnRate { get { return StartingValues.value.BaseInspBurn * Profile.InspDropMultiplier; } }
        public float SoclBurnRate { get { return StartingValues.value.BaseSoclBurn * Profile.SoclDropMultiplier; } }

        public int RestoreTicks { get { return RandomGeneration.instance.RandomRestoreTime; } }

        //Moods
        public float MoodImpact { get; private set; } = 0f;
        private float MoodImpactBurn { get { return StartingValues.value.MoodImpactBurn; } }

        //Velocity
        public float Velocity { get { return (StartingValues.value.BaseVelocity * Profile.VelocityMultiplier) * CappedMoodImpact; } }
        public float BugChance { get { return (StartingValues.value.BaseBugChance * Profile.BugChanceMultiplier) * (StartingValues.value.moodImpactMax - CappedMoodImpact); } }

        private void Awake()
        {
            Food = new(DoingType.Food, 1f, StartingValues.value.NeedThreshold);
            Insp = new(DoingType.Inspiration, 1f, StartingValues.value.NeedThreshold);
            Socl = new(DoingType.Social, 1f, StartingValues.value.NeedThreshold);
            Rest = new(DoingType.Rest, 1f, StartingValues.value.NeedThreshold);
        }


        public void SetupCharacter(Profile profile)
        {
            Profile = profile;
            spriteRenderer.material.color = Profile.Color;
            EventManager.instance.OnTick += Tick;
        }

        public void SetPositions()
        {
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
                    DoTasks();
                    DisplayMoodlet();
                    break;
                case CharacterState.End:
                    EndWork();
                    break;
                case CharacterState.Inactive:
                    break;
            }
        }

        public void DoTasks()
        {
            if(CurrentDoing == DoingType.Work)
            {
                Tasks.DoTask(Velocity, BugChance);
                Debug.Log(Profile.FullName + " is working!");
            }
            else if(CurrentDoing == DoingType.Idle)
            {
                //Debug.Log(Profile.FullName + " is idle!");
            }
        }

        public void DisplayMoodlet()
        {
            /*if (!displayingMoodlet)
            {
                //DOING TYPE MOODLETS
                List<DoingTracker> needs = new List<DoingTracker>();
                foreach(DoingTracker tracker in doingTrackers)
                {
                    if (tracker.NearThreshold)
                    {
                        Debug.Log("Tracker " + tracker.type + " is at " + tracker.curValue);
                        needs.Add(tracker);
                    }
                }
                if(needs.Count > 0)
                {
                    DoingTracker randomTracker = needs[Random.Range(0, needs.Count)];
                    CharacterMoodlet m = GlobalVariables.value.GetMoodlet(randomTracker.type); //Get a moodlet based on the doing type
                    moodlet.DisplayMoodlet(m.sprite);
                    displayingMoodlet = true;
                }
            }
            else
            {
                moodletTicks++;
                if(moodletTicks > GlobalVariables.value.MoodletDisplayTicks)
                {
                    displayingMoodlet = false;
                    moodletTicks = 0;
                    moodlet.HideMoodlet();
                }
            }*/
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
                if(Mood > StartingValues.value.moodImpactMax)
                {
                    return StartingValues.value.moodImpactMax;
                }
                else if(Mood < StartingValues.value.moodImpactMin)
                {
                    return StartingValues.value.moodImpactMin;
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
