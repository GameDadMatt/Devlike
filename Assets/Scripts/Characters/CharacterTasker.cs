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

        private Character thisCharacter;

        private TaskList tasks = new TaskList();
        private float alignment = 0;
        private float crunchPressure = 0;
        private float velocityMod = 0;
        private float bugMod = 0;

        //Character
        //How easy it is for this character to be overwhelmed, based on confidence
        public int WorkloadPoints { get { return (gProject.BaseTaskPointsPerDay + TaskEstimateAdjustment) * gStudio.WorkDays; } }

        //How accurate this characters estimations are, based on confidence and experience
        public int TaskEstimateAdjustment { get { return Mathf.RoundToInt(((float)thisCharacter.Profile.Confidence + (float)thisCharacter.Profile.Experience) / 2); } }

        //Tasks
        public TaskList Tasks { get => tasks; }
        public int NumTasks { get { return tasks.TotalCount; } }
        public TaskType CurrentTask { get; private set; }
        public MoodletType CurrentEmotion { get; private set; }

        //Days
        private int workStartTweak = 0;
        private int workEndTweak = 0;
        public int WorkStart { get { return gTime.WorkStartTick + thisCharacter.Profile.WorkStartMod + workStartTweak; } }
        public int WorkEnd { get { return gTime.WorkEndTick + thisCharacter.Profile.WorkEndMod + workEndTweak; } }
        public int WorkTicks { get { return WorkEnd - WorkStart; } }

        //Alignment
        public float Alignment { get { return alignment; } }

        //Crunch
        public float CrunchPressure { get { return crunchPressure; } }

        private void OnEnable()
        {
            thisCharacter = GetComponent<Character>();
        }

        public void SetTasks(TaskList tasks)
        {
            this.tasks = tasks;
        }

        public void DoTasks()
        {
            Tasks.DoTask(Velocity, BugChance);
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

        public float Velocity
        {
            get
            {
                return (((gProject.BaseTaskPointsPerDay / WorkTicks) * thisCharacter.Profile.VelocityMultiplier) * thisCharacter.CappedMoodImpact) + velocityMod;
            }            
        }

        public float BugChance
        {
            get
            {
                return ((gProject.BaseBugChance * thisCharacter.Profile.BugChanceMultiplier) * thisCharacter.InverseCappedMoodImpact) + bugMod;
            }            
        }

        public void AlignmentDrift()
        {
            float drift = (gCharacter.AlignmentDriftPerDay * thisCharacter.Profile.AlignDriftMultiplier) / WorkTicks;
            if (alignment > thisCharacter.Profile.NaturalAlignment)
            {
                alignment -= drift;
            }
            else
            {
                alignment -= drift / 2; //Alignment drops slower below the character's base alignment
            }
        }

        public void RestoreAlignment(float bonus)
        {
            alignment = thisCharacter.Profile.NaturalAlignment + bonus;
        }

        public void SetCrunchPressure()
        {

        }

        public void UpdateCrunchPressure()
        {

        }

        public void ReduceCrunchPressure(float value)
        {
            crunchPressure -= value;
        }
    }
}