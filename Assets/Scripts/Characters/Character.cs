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
        [SerializeField]
        private GlobalCharacter gCharacter;
        [SerializeField]
        private GlobalStudio gStudio;
        [SerializeField]
        private GlobalTime gTime;
        [SerializeField]
        private GlobalProject gProject;

        [SerializeField]
        private SpriteRenderer characterSprite;
        [SerializeField]
        private UI.MoodletDisplay moodletDisplay;

        //Refs
        public Profile Profile { get; private set; }
        public CharacterTasker Tasker { get; private set; }
        public CharacterDialogue Dialogue { get; private set; }
        public CharacterMoodlet Moodlet { get; private set; }
        public int CurrentTickRef { get { return gTime.CurrentTick; } }
        public CharacterState CurrentState { get; set; }

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

        public float RestBurnRate { get { return (gCharacter.RestBreaksPerDay / Tasker.WorkTicks) * Profile.RestDropMultiplier; } }
        public float FoodBurnRate { get { return (gCharacter.FoodBreaksPerDay / Tasker.WorkTicks) * Profile.FoodDropMultiplier; } }
        public float InspBurnRate { get { return (gCharacter.InspBreaksPerDay / Tasker.WorkTicks) * Profile.InspDropMultiplier; } }
        public float SoclBurnRate { get { return (gCharacter.SoclBreaksPerDay / Tasker.WorkTicks) * Profile.SoclDropMultiplier; } }

        //Moods
        private float moodImpact = 0;
        private float MoodImpactBurn { get { return (gCharacter.MoodImpactDuration * Profile.MoodImpactMultiplier) / Tasker.WorkTicks; } }

        //Behaviour Designer References
        public int NumTasks { get => Tasker.NumTasks; }
        public int WorkStart { get => Tasker.WorkStart; }
        public int WorkEnd { get => Tasker.WorkEnd; }
        public int RestoreTicks { get => RandomGeneration.instance.RandomRestoreTime; }

        public string ID
        {
            get
            {
                var hash = new Hash128();
                hash.Append(Profile.FullNameAndAlias);
                return hash.ToString();
            }
        }

        private void OnEnable()
        {
            EventManager.instance.OnTick += Tick;

            Tasker = GetComponent<CharacterTasker>();
            Dialogue = GetComponent<CharacterDialogue>();
            Moodlet = GetComponent<CharacterMoodlet>();

            Food = new(DoingType.Food, 1f, gCharacter.NeedThreshold);
            Insp = new(DoingType.Inspiration, 1f, gCharacter.NeedThreshold);
            Socl = new(DoingType.Social, 1f, gCharacter.NeedThreshold);
            Rest = new(DoingType.Rest, 1f, gCharacter.NeedThreshold);
        }

        public void SetupCharacter(Profile profile)
        {
            Debug.Log("Sprite Renderer = " + characterSprite + ", Profile Colour = " + profile.Color);
            Profile = profile;
            characterSprite.material.color = Profile.Color;
            Moodlet.RegisterCharacter(ID);
            moodletDisplay.RegisterMoodlet(ID);
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
                    UpdateMood();
                    Tasker.UpdateDrift();
                    if(CurrentDoing == DoingType.Work)
                    {
                        Tasker.DoTasks();
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

        private void CheckMood()
        {

        }

        private void DisplayMoodlet()
        {
            if(Moodlet.Ready && Dialogue.HasDrama)
            {
                Sprite sprite = gCharacter.GetMoodletSprite(MoodletType.HasDrama);
                int delayTicks = Mathf.RoundToInt(Random.Range(0, gCharacter.MoodletDelayHours) * gTime.TicksPerHour);
                int displayTicks = Mathf.RoundToInt(gCharacter.MoodletDisplayHours * gTime.TicksPerHour);
                int cooldownTicks = Mathf.RoundToInt(gCharacter.MoodletCooldownHours * gTime.TicksPerHour);
                Moodlet.NewMoodlet(sprite, delayTicks, displayTicks, cooldownTicks);
            }
            else if (!Moodlet.Ready)
            {
                Debug.Log("Moodlet on " + Profile.FullName);
                Moodlet.Tick();
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

        public void ImpactMood(float impact)
        {
            moodImpact += (impact * Profile.MoodImpactMultiplier);
        }

        public void ImpactAlignment(float impact)
        {
            Tasker.ImpactAlignment(impact);
        }

        public void SetCrunchPressure(float pressure)
        {
            Tasker.SetCrunchPressure(pressure);
        }

        private void UpdateMood()
        {
            if (moodImpact < 0)
            {
                moodImpact += MoodImpactBurn;
                if(moodImpact > 0)
                {
                    moodImpact = 0;
                }
            }
            else if (moodImpact > 0)
            {
                moodImpact -= MoodImpactBurn;
                if(moodImpact < 0)
                {
                    moodImpact = 0;
                }
            }
        }

        public float Mood
        {
            get
            {
                return Profile.BaseMood + moodImpact;
            }
        }

        public float CappedMoodImpact
        {
            get
            {
                if(Mood > gCharacter.MoodImpactMax)
                {
                    return gCharacter.MoodImpactMax;
                }
                else if(Mood < gCharacter.MoodImpactMin)
                {
                    return gCharacter.MoodImpactMin;
                }
                else
                {
                    return Mood;
                }
            }
        }

        public float InverseCappedMoodImpact
        {
            get
            {
                return gCharacter.MoodImpactMax - CappedMoodImpact;
            }
        }

        public void ChangeMoodImpact(float impact)
        {
            moodImpact += impact;
        }
    }
}
