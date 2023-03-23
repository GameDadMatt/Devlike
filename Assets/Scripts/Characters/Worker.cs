using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Tasks;

namespace Devlike.Characters
{
    public class Worker
    {
        //References
        private string name;
        private int workStart;
        private int workEnd;
        private int workStartTweak;
        private int workEndTweak;

        private float velocityBase;
        private float velocityMultiplier;
        private float bugBase;
        private float bugMultiplier;
        private float alignment;
        private float alignmentDrift;

        private TaskList tasks = new TaskList();

        //Tasks
        public TaskList Tasks { get => tasks; }
        public int NumTasks { get { return tasks.TotalCount; } }
        public TaskType CurrentTask { get; private set; }
        public MoodletType CurrentEmotion { get; private set; }

        //Days
        private int WorkStartTweak = 0;
        private int WorkEndTweak = 0;
        public int WorkStart { get { return workStart + workStartTweak; } }
        public int WorkEnd { get { return workEnd + workEndTweak; } }
        public int WorkTicks { get { return WorkEnd - WorkStart; } }

        //Alignment
        public float Alignment { get { return alignment; } }
        public float AlignmentDrift { get { return alignmentDrift * WorkTicks; } }

        //Crunch


        public Worker(Character character, GlobalCharacter gchar, GlobalTime gtime, GlobalProject gproj)
        {
            name = character.Profile.FullName;
            workStart = gtime.WorkStartTick + character.Profile.WorkStartMod;
            workEnd = gtime.WorkEndTick + character.Profile.WorkEndMod;

            velocityBase = gproj.BasePointsPerDay;
            velocityMultiplier = character.Profile.VelocityMultiplier;
            bugBase = gproj.BaseBugChance;
            bugMultiplier = character.Profile.BugChanceMultiplier;
            alignment = gchar.AlignmentBase;
            alignmentDrift = gchar.AlignmentDriftPerDay * character.Profile.AlignDropMultiplier;
        }

        public void SetTasks(TaskList tasks)
        {
            this.tasks = tasks;
        }

        public void DoTasks(float cappedMood, float moodImpactMax)
        {
            Tasks.DoTask(Velocity(cappedMood), BugChance(cappedMood, moodImpactMax));
        }

        public void SetCrunching(int ticksInHour, int dayStartHours, int dayEndHours)
        {
            workStartTweak = dayStartHours * ticksInHour;
            workEndTweak = dayEndHours * ticksInHour;
        }

        public float Velocity(float cappedMood)
        {
            return ((velocityBase / WorkTicks) * velocityMultiplier) * cappedMood;
        }

        public float BugChance(float cappedMood, float moodImpactMax)
        {
            return (bugBase * bugMultiplier) * (moodImpactMax - cappedMood);
        }
    }
}