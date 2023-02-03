using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataTypes
{
    public enum TaskState { Idle, InProgress, Complete }

    public interface ITask
    {
        void StartTask();
        void DoTask();
        void CompleteTask();
    }

    //The base Task class can work to satify Needs
    [CreateAssetMenu(fileName = "TaskType", menuName = "Devlike/Task")]
    public class Task : ScriptableObject, ITask
    {
        public TaskType type;
        public TaskState state = TaskState.Idle;
        public int minTicks;
        public int maxTicks;

        
        public float completeChance = 0.95f;
        public int doneTicks;

        public void StartTask()
        {
            state = TaskState.InProgress;
        }

        public void DoTask()
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
        }

        public void CompleteTask()
        {
            state = TaskState.Complete;
        }
    }
}