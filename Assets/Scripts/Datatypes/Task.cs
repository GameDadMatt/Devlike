using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devlike.Tasks
{
    public enum TaskState { Idle, InProgress, Complete }

    //The base Task class can work to satify Needs
    [CreateAssetMenu(fileName = "TaskType", menuName = "Devlike/Task")]
    public class Task : ScriptableObject
    {
        public TaskType type;
        public TaskState state = TaskState.Idle;
        public TaskImportance importance = TaskImportance.None;
        public int minTicks;
        public int maxTicks;
        public float bugChance = 0.05f;


        public float completeChance = 0.95f;
        public int doneTicks;

        public void StartTask()
        {
            state = TaskState.InProgress;
        }

        public void DoTask(float bugChance)
        {
            doneTicks++;
            if(doneTicks > minTicks && doneTicks < maxTicks)
            {
                float complete = Random.Range(0f, 1f);
                if (complete >= completeChance)
                {
                    CompleteTask();
                }
            }
            else if(doneTicks >= maxTicks)
            {
                CompleteTask();
            }

            //Risk generating a bug every tick this task is incomplete
            if (importance != TaskImportance.Bug)
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
            Debug.LogWarning("Bug generated in " + name);
        }

        public void CompleteTask()
        {
            state = TaskState.Complete;
        }
    }
}