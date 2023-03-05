using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devlike.Tasks
{
    public enum TaskState { Idle, InProgress, Complete }

    /// <summary>
    /// TaskContainer holds the values set by a TaskTemplate to be a unique tracker for an individual task
    /// </summary>
    public class TaskContainer
    {
        public Vector3 Position { get; set; }
        public TaskType Type { get; private set; }
        public TaskImportance Importance { get; private set; }
        public int Points { get; private set; }
        public float BugChance { get; private set; }

        public TaskState State { get; private set; } = TaskState.Idle;
        public float DonePoints { get; private set; } = 0f;

        public TaskContainer(TaskType type, TaskImportance importance, Tier bugChance, int points)
        {
            Type = type;
            Importance = importance;
            BugChance = BugChanceFromTier(bugChance);
            Points = points;
        }

        private float BugChanceFromTier(Tier tier)
        {
            float bugChance = GlobalVariables.value.BaseBugChance;
            return bugChance * (int)tier;
        }

        public void StartTask()
        {
            State = TaskState.InProgress;
            Debug.LogWarning("Started " + Type + ", ID " + ID);
        }

        public void DoTask(float velocity, float cBugChance)
        {
            DonePoints += velocity;
            if (DonePoints >= Points)
            {
                CompleteTask();
            }

            //Risk generating a bug every tick
            if (Importance != TaskImportance.Bug)
            {
                RandomGeneration.instance.RandomGenerateBug(BugChance, cBugChance);
            }
        }

        public void CompleteTask()
        {
            State = TaskState.Complete;
            Debug.LogWarning("Completed " + Type + " ID " + ID);
        }

        public string ID
        {
            get
            {
                var hash = new Hash128();
                hash.Append(Type.ToString());
                return hash.ToString();
            }
        }
    }
}

