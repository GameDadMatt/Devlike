using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Tasks;

namespace Devlike.UI
{
    public enum TaskState { Idle, InProgress, Complete }

    /// <summary>
    /// TaskContainer holds the values set by a TaskTemplate to be a unique tracker for an individual task
    /// </summary>
    public class TaskContainer
    {
        public string Name { get; private set; }
        public Vector3 Position { get; set; }
        public TaskType Type { get; private set; }
        public TaskImportance Importance { get; private set; }
        public int MinTicks { get; private set; }
        public int MaxTicks { get; private set; }
        public float BugChance { get; private set; }

        public TaskState State { get; private set; } = TaskState.Idle;
        public float CompleteChance { get; private set; } = 0.95f;
        public int DoneTicks { get; private set; } = 0;

        public TaskContainer(TaskTemplate template)
        {
            Name = template.name;
            Type = template.type;
            Importance = template.importance;
            MinTicks = template.minTicks;
            MaxTicks = template.maxTicks;
            BugChance = template.bugChance;
        }

        public void StartTask()
        {
            State = TaskState.InProgress;
            Debug.LogWarning("Started task " + Name + " ID " + ID);
        }

        public void DoTask(float bugChance)
        {
            DoneTicks++;
            if (DoneTicks > MinTicks && DoneTicks < MaxTicks)
            {
                float complete = Random.Range(0f, 1f);
                if (complete >= CompleteChance)
                {
                    CompleteTask();
                }
            }
            else if (DoneTicks >= MaxTicks)
            {
                CompleteTask();
            }

            //Risk generating a bug every tick this task is incomplete
            if (Importance != TaskImportance.Bug)
            {
                float bug = Random.Range(0f, 1f);
                if (bug <= bugChance)
                {
                    GenerateBug();
                }
            }
        }

        private void GenerateBug()
        {
            Debug.LogWarning("Bug generated in " + Name + " ID " + ID);
        }

        public void CompleteTask()
        {
            State = TaskState.Complete;
            Debug.LogWarning("Completed task " + Name + " ID " + ID);
        }

        public string ID
        {
            get
            {
                var hash = new Hash128();
                hash.Append(Name);
                return hash.ToString();
            }
        }
    }
}

