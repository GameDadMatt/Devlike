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
        public CharacterTasker CharacterTasker { get; private set; }
        public CharacterDialogue CharacterDialogue { get; private set; }
        public CharacterMoodlet CharacterMoodlet { get; private set; }
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

        public float RestBurnRate { get { return (gCharacter.RestBreaksPerDay / CharacterTasker.WorkTicks) * Profile.RestDropMultiplier; } }
        public float FoodBurnRate { get { return (gCharacter.FoodBreaksPerDay / CharacterTasker.WorkTicks) * Profile.FoodDropMultiplier; } }
        public float InspBurnRate { get { return (gCharacter.InspBreaksPerDay / CharacterTasker.WorkTicks) * Profile.InspDropMultiplier; } }
        public float SoclBurnRate { get { return (gCharacter.SoclBreaksPerDay / CharacterTasker.WorkTicks) * Profile.SoclDropMultiplier; } }

        //Moods
        private MoodletType currentMoodlet = MoodletType.None;
        private float moodImpact = 0;
        private float MoodImpactBurn { get { return (gCharacter.MoodImpactDuration * Profile.MoodImpactMultiplier) / CharacterTasker.WorkTicks; } }

        //THRESHOLDS
        public Threshold CrunchThreshold { get; private set; }
        public Threshold BadMoodThreshold { get; private set; }
        public Threshold GoodMoodThreadhold { get; private set; }
        public Threshold LowVelocityThreshold { get; private set; } //Low velocity threshold is a percentage of expected output
        public Threshold OverwhelmedThreshold { get; private set; }

        //Behaviour Designer References
        public int NumTasks { get => CharacterTasker.NumTasks; }
        public int WorkStart { get => CharacterTasker.WorkStart; }
        public int WorkEnd { get => CharacterTasker.WorkEnd; }
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

            CharacterTasker = GetComponent<CharacterTasker>();
            CharacterDialogue = GetComponent<CharacterDialogue>();
            CharacterMoodlet = GetComponent<CharacterMoodlet>();
            CharacterMoodlet.OnMoodletCompleted += EndMoodlet;

            Food = new(DoingType.Food, 1f, gCharacter.NeedThreshold);
            Insp = new(DoingType.Inspiration, 1f, gCharacter.NeedThreshold);
            Socl = new(DoingType.Social, 1f, gCharacter.NeedThreshold);
            Rest = new(DoingType.Rest, 1f, gCharacter.NeedThreshold);
        }

        public void SetupCharacter(Profile profile)
        {
            Profile = profile;
            characterSprite.material.color = Profile.Color;
            CharacterMoodlet.RegisterCharacter(ID);
            moodletDisplay.RegisterMoodlet(ID);

            //Setup thresholds
            CrunchThreshold = new Threshold(Profile.CrunchPoint);
            BadMoodThreshold = new Threshold(Profile.BadMoodPoint);
            GoodMoodThreadhold = new Threshold(Profile.GoodMoodPoint);
            LowVelocityThreshold = new Threshold(Profile.LowVelocityPoint);
            LowVelocityThreshold.SetScale(gProject.BaseTaskPointsPerDay);
            OverwhelmedThreshold = new Threshold(Profile.OverwhelmedPoint);
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
            moodImpact += Random.Range(-1f, 1f);
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
                    OnActiveTick();
                    break;
                case CharacterState.End:
                    EndWork();
                    CharacterMoodlet.ClearMoodlet();
                    break;
                case CharacterState.Inactive:
                    break;
            }
        }

        private void OnActiveTick()
        {
            //Update the mood
            UpdateMood();
            //Update the character's drift
            CharacterTasker.UpdateDrift();

            //Update the moodlets
            if (CharacterMoodlet.Ticking)
            {
                CharacterMoodlet.Tick();
            }

            //Do work if there is any. Otherwise, display a moodlet
            if (CurrentDoing == DoingType.Work)
            {
                CharacterTasker.DoTasks();
            }
            else if (CurrentDoing == DoingType.Idle)
            {
                TempMoodlet(MoodletType.NoTask);
            }
        }

        private void UpdateMood()
        {
            if (moodImpact < 0)
            {
                moodImpact += MoodImpactBurn;
                if (moodImpact > 0)
                {
                    moodImpact = 0;
                }
            }
            else if (moodImpact > 0)
            {
                moodImpact -= MoodImpactBurn;
                if (moodImpact < 0)
                {
                    moodImpact = 0;
                }
            }

            //Do a mood check and set appropriate moodlet based on current stats
            NaturalMoodCheck();
        }

        private void NaturalMoodCheck()
        {
            //If we have no current mood, set one
            if(currentMoodlet == MoodletType.None)
            {
                List<MoodletType> possibleMoods = new List<MoodletType>();

                //BAD MOOD
                if (BadMoodThreshold.UnderThreshold)
                {
                    possibleMoods.Add(MoodletType.BadMood);
                }

                //GOOD MOOD
                if (GoodMoodThreadhold.OverThreshold)
                {
                    possibleMoods.Add(MoodletType.GoodMood);
                }

                //LOW VELOCITY
                if (LowVelocityThreshold.UnderThreshold)
                {
                    possibleMoods.Add(MoodletType.LowVelocity);
                }

                //HIGH VELOCITY
                //To be implemented

                //OVERWHELMED
                if (OverwhelmedThreshold.OverThreshold)
                {
                    possibleMoods.Add(MoodletType.Overwhelmed);
                }

                //CRUNCHING
                if (CrunchThreshold.OverThreshold)
                {
                    possibleMoods.Add(MoodletType.HasDrama);
                }

                //Set everything
                if(possibleMoods.Count > 0)
                {
                    int random = Random.Range(0, possibleMoods.Count);
                    SetupMoodlet(possibleMoods[random]);
                    CharacterDialogue.DialogueFromMood(possibleMoods[random]);
                }
            }
        }

        public void TempMoodlet(MoodletType type)
        {
            if(type != MoodletType.None && CharacterMoodlet.TempDisplayReady)
            {
                Sprite sprite = gCharacter.GetMoodletSprite(type);
                int displayTicks = Mathf.RoundToInt(gCharacter.MoodletDisplayHours * gTime.TicksPerHour);
                int cooldownTicks = 0; //Mathf.RoundToInt(gCharacter.MoodletCooldownHours * gTime.TicksPerHour);
                CharacterMoodlet.NewTempMoodlet(sprite, displayTicks, cooldownTicks);
            }
        }

        private void SetupMoodlet(MoodletType type)
        {
            CharacterMoodlet.ClearMoodlet(); //Clear the moodlet first, just in case

            if (type != MoodletType.None)
            {
                Debug.Log("Setting up moodlet of type " + type);
                currentMoodlet = type;

                Sprite sprite = gCharacter.GetMoodletSprite(currentMoodlet);
                int delayTicks = Mathf.RoundToInt(Random.Range(0, gCharacter.MoodletDelayHours) * gTime.TicksPerHour);
                int displayTicks = Mathf.RoundToInt(gCharacter.MoodletDisplayHours * gTime.TicksPerHour);
                int cooldownTicks = Mathf.RoundToInt(gCharacter.MoodletCooldownHours * gTime.TicksPerHour);
                CharacterMoodlet.NewMoodlet(sprite, delayTicks, displayTicks, cooldownTicks);
            }
        }

        private void EndMoodlet()
        {
            currentMoodlet = MoodletType.None;
        }

        public void SetMoodAndDialogue(MoodletType mood, DialogueContainer dialogue)
        {
            currentMoodlet = mood;
            CharacterDialogue.NewDialogue(dialogue);
            SetupMoodlet(mood);
        }

        public void ClearMoodlet()
        {

        }

        public void ResolveMood()
        {
            currentMoodlet = MoodletType.None;
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

        public void ImproveMood(float impact)
        {
            moodImpact += (impact * Profile.MoodImpactMultiplier);
        }

        public void LowerMood(float impact)
        {
            moodImpact -= (impact * Profile.MoodImpactMultiplier);
        }

        public void AlignmentImpact(float impact)
        {
            CharacterTasker.ImpactAlignment(impact);
        }

        public void AlignmentRestore(float impact)
        {
            CharacterTasker.RestoreAlignment(impact);
        }

        public void SetCrunchPressure(float pressure)
        {
            CharacterTasker.SetCrunchPressure(pressure);
        }

        public void ReduceCrunchPressure(float pressure)
        {
            CharacterTasker.ReduceCrunchPressure(pressure);
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
