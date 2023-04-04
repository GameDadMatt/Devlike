using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Tasks;

namespace Devlike.Characters
{
    public class CharacterTasker : MonoBehaviour
    {
        //References
        [SerializeField]
        private GlobalTime gTime;
        [SerializeField]
        private GlobalProject gProject;
        [SerializeField]
        private GlobalStudio gStudio;
        [SerializeField]
        private GlobalCharacter gCharacter;

        private Character character;

        private TaskList tasks = new TaskList();

        //Character
        //How easy it is for this character to be overwhelmed, based on confidence
        public int WorkloadPoints { get { return (gProject.BaseTaskPointsPerDay + TaskEstimateAdjustment) * gStudio.WorkWeekDays; } }

        //How accurate this characters estimations are, based on confidence and experience
        public int TaskEstimateAdjustment { get { return Mathf.RoundToInt(((float)character.Profile.Confidence + (float)character.Profile.Experience) / 2); } }

        //Tasks
        public TaskList Tasks { get => tasks; }
        public int NumTasks { get { return tasks.TotalCount; } }
        public TaskType CurrentTask { get; private set; }
        public MoodletType CurrentEmotion { get; private set; }

        //Days
        private int workStartTweak = 0;
        private int workEndTweak = 0;
        public int WorkStart { get { return gTime.WorkStartTick + character.Profile.WorkStartMod + workStartTweak; } }
        public int WorkEnd { get { return gTime.WorkEndTick + character.Profile.WorkEndMod + workEndTweak; } }
        public int WorkTicks { get { return WorkEnd - WorkStart; } }

        //Alignment
        private float alignment = 0;
        public float Alignment { get { return alignment; } }

        //Crunch
        private float crunchDesire = 0;
        private float crunchPressure = 0;
        public float CrunchPressure { get { return crunchPressure; } }

        //Modifiers
        private float velocityMod = 0;
        private float bugMod = 0;

        private void OnEnable()
        {
            character = GetComponent<Character>();
            Tasks.OnBugCreated += GenerateBug;
            Tasks.OnTaskComplete += CompleteTask;
        }

        public void SetTasks(TaskList tasks)
        {
            this.tasks = tasks;
        }

        public void DoTasks()
        {
            Tasks.DoTask(character.Profile.FullName, Velocity, BugChance);
        }

        public void GenerateBug()
        {
            Debug.Log(character.Profile.FullName + " has generated a bug");
            character.TempMoodlet(MoodletType.GeneratedBug);
        }

        public void CompleteTask()
        {
            Debug.Log(character.Profile.FullName + " has completed a task");
            character.TempMoodlet(MoodletType.CompletedTask);
        }

        public void UpdateDrift()
        {
            AlignmentDrift();
            CrunchDrift();
        }

        private void AlignmentDrift()
        {
            float drift = (gCharacter.AlignmentDriftPerDay * character.Profile.AlignDriftMultiplier) / WorkTicks;
            if (alignment > character.Profile.NaturalAlignment)
            {
                alignment -= drift;
            }
            else
            {
                alignment -= drift / 2; //Alignment drops slower below the character's base alignment
            }
        }

        private void CrunchDrift()
        {
            //CrunchPressure cannot reduce past the CrunchThreshold without direct intervention
            //CrunchDrift moves CrunchDesire towards the current CrunchPressure

            //Debug.Log("Add Crunch Drift");

            /*if (crunchPressure > thisCharacter.Profile.CrunchThreshold)
            {
                crunchPressure -= gCharacter.CrunchDriftPerDay / WorkTicks;
            }*/
        }

        public void ImpactAlignment(float value)
        {
            alignment += value;
        }

        public void RestoreAlignment(float bonus)
        {
            alignment = character.Profile.NaturalAlignment + bonus;
        }

        public void SetCrunchPressure(float pressure)
        {
            crunchPressure = pressure;
        }

        public void ReduceCrunchPressure(float value)
        {
            crunchPressure -= value;
        }

        public void TweakHours(int ticksInHour, int dayStartHours, int dayEndHours)
        {
            workStartTweak = dayStartHours * ticksInHour;
            workEndTweak = dayEndHours * ticksInHour;
        }

        public void SetVelocityMod(float velocityMod)
        {
            this.velocityMod = velocityMod;
        }

        public void SetBugMod(float bugMod)
        {
            this.bugMod = bugMod;
        }

        public float ExpectedVelocity
        {
            get
            {
                return (gProject.BaseTaskPointsPerDay / (float)WorkTicks) * character.Profile.VelocityMultiplier;
            }
        }

        public float Velocity
        {
            get
            {
                return (((gProject.BaseTaskPointsPerDay / (float)WorkTicks) * character.Profile.VelocityMultiplier) * character.CappedMoodImpact) + velocityMod;
            }            
        }

        public float NeededVelocity
        {
            get
            {
                return ((float)Tasks.RemainingPoints / (float)gStudio.WorkWeekDays) / (float)WorkTicks;
            }
        }

        public float BugChance
        {
            get
            {
                return ((gProject.BaseBugChance * character.Profile.BugChanceMultiplier) * character.InverseCappedMoodImpact) + bugMod;
            }            
        }

        public bool Crunching
        {
            get
            {
                return character.CrunchThreshold.OverThreshold;
            }
        }
    }
}