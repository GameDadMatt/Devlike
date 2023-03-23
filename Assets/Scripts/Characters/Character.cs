using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Tasks;
using BehaviorDesigner.Runtime;

namespace Devlike.Characters
{
    /// <summary>
    /// An individual character in the studio
    /// </summary>
    public class Character : MonoBehaviour
    {
        private GlobalCharacter character;
        private GlobalStudio studio;
        private GlobalTime time;
        private GlobalProject project;

        [SerializeField]
        private SpriteRenderer characterSprite;
        [SerializeField]
        private Moodlet moodletDisplay;
        private bool displayingMoodlet = false;
        private int moodletTicks = 0;

        //Refs
        public Profile Profile { get; private set; }
        public Worker Worker { get; private set; }
        public int CurrentTickRef { get { return time.CurrentTick; } }
        public CharacterState CurrentState { get; set; }

        //Dialogue
        public DialogueContainer CurrentDialogue { get; set; }

        //Positions
        public DoingType CurrentDoing { get; set; }
        public NPCInteractable Desk { get; private set; }
        public NPCInteractable Home { get; private set; }
        private NPCInteractable curInteract;

        //Needs
        public DoingTracker Food { get; private set; }
        public DoingTracker Insp { get; private set; }
        public DoingTracker Socl { get; private set; }
        public DoingTracker Rest { get; private set; }

        public float RestBurnRate { get { return (character.RestBreaksPerDay / Worker.WorkTicks) * Profile.RestDropMultiplier; } }
        public float FoodBurnRate { get { return (character.FoodBreaksPerDay / Worker.WorkTicks) * Profile.FoodDropMultiplier; } }
        public float InspBurnRate { get { return (character.InspBreaksPerDay / Worker.WorkTicks) * Profile.InspDropMultiplier; } }
        public float SoclBurnRate { get { return (character.SoclBreaksPerDay / Worker.WorkTicks) * Profile.SoclDropMultiplier; } }
        

        //Moods
        public float MoodImpact { get; private set; } = 0f;
        private float MoodImpactBurn { get { return (character.MoodImpactDuration / Worker.WorkTicks) * Profile.MoodImpactMultiplier; } }

        //Behaviour Designer References
        public int NumTasks { get => Worker.NumTasks; }
        public int WorkStart { get => Worker.WorkStart; }
        public int WorkEnd { get => Worker.WorkEnd; }
        public int RestoreTicks { get => RandomGeneration.instance.RandomRestoreTime; }

        private void OnEnable()
        {
            EventManager.instance.OnTick += Tick;

            character = GameManager.instance.GetGlobal("Character") as GlobalCharacter;
            studio = GameManager.instance.GetGlobal("Studio") as GlobalStudio;
            time = GameManager.instance.GetGlobal("Time") as GlobalTime;
            project = GameManager.instance.GetGlobal("Project") as GlobalProject;

            Food = new(DoingType.Food, 1f, character.NeedThreshold);
            Insp = new(DoingType.Inspiration, 1f, character.NeedThreshold);
            Socl = new(DoingType.Social, 1f, character.NeedThreshold);
            Rest = new(DoingType.Rest, 1f, character.NeedThreshold);
        }

        public void SetupCharacter(Profile profile)
        {
            Debug.Log("Sprite Renderer = " + characterSprite + ", Profile Colour = " + profile.Color);
            Profile = profile;
            Worker = new Worker(this, character, time, project);
            characterSprite.material.color = Profile.Color;
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
                    if(CurrentDoing == DoingType.Work)
                    {
                        Worker.DoTasks(CappedMoodImpact, character.MoodImpactMax);
                    }
                    DisplayMoodlet();
                    break;
                case CharacterState.End:
                    EndWork();
                    break;
                case CharacterState.Inactive:
                    break;
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
                if(Mood > character.MoodImpactMax)
                {
                    return character.MoodImpactMax;
                }
                else if(Mood < character.MoodImpactMin)
                {
                    return character.MoodImpactMin;
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
