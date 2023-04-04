using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.UI;
using Devlike.Characters;

namespace Devlike.Tasks
{
    /// <summary>
    /// Keeps track of a list of tasks
    /// </summary>
    public class TaskList
    {
        public int TotalPoints { get; private set; }
        private Queue<int> taskPoints; //This is only used for the Backlogs
        private List<TaskContainer> tasks;
        public List<TaskContainer> Tasks { get { return tasks; } }
        private TaskContainer doingTask;

        public TaskList()
        {
            TotalPoints = 0;
            taskPoints = new Queue<int>();
            tasks = new List<TaskContainer>();
        }

        /// <summary>
        /// This initializer is used for generating quick and easy lists of tasks
        /// </summary>
        /// <param name="totalPoints"></param>
        /// <param name="taskPoints"></param>
        public TaskList(int totalPoints, Queue<int> taskPoints)
        {
            TotalPoints = totalPoints;
            this.taskPoints = taskPoints;
            tasks = new List<TaskContainer>();
        }

        /// <summary>
        /// This is used to generate starting tasks
        /// </summary>
        /// <param name="type"></param>
        /// <param name="total"></param>
        public void GenerateTaskFromPoints(TaskType type, int total, float baseBugChance)
        {
            if(taskPoints.Count > 0)
            {
                while (tasks.Count < total && taskPoints.Count != 0)
                {
                    int points = taskPoints.Dequeue();
                    tasks.Add(new TaskContainer(type, TaskImportance.None, RandomGeneration.instance.RandomTier, baseBugChance, points));
                }
            }
        }

        public void AddTask(TaskContainer task)
        {
            tasks.Add(task);
        }

        public void RemoveTask(TaskContainer task)
        {
            tasks.Remove(task);
        }

        public void UpdateTaskList(List<TaskContainer> list)
        {
            tasks = list;
        }

        //Actions
        public event Action OnTaskComplete;
        public event Action OnBugCreated;
        public void DoTask(string cname, float velocity, float bugChance)
        {
            if(doingTask == null)
            {
                doingTask = tasks[0];
                tasks.RemoveAt(0);
            }

            doingTask.DoTask(velocity, bugChance);

            if (doingTask.GeneratedBug)
            {
                OnBugCreated?.Invoke();
            }

            if (doingTask.State == TaskState.Complete)
            {
                //Task done, load the next task
                doingTask = tasks[0];
                tasks.RemoveAt(0);
                OnTaskComplete?.Invoke();
            }
        }

        public float RemainingPoints
        {
            get
            {
                if (tasks.Count > 0)
                {
                    float total = 0;
                    foreach (TaskContainer task in tasks)
                    {
                        total += task.Points;
                        total -= task.DonePoints;
                    }
                    return total;
                }
                else
                {
                    return TotalPoints;
                }
            }
        }

        public int ActiveCount
        {
            get
            {
                return tasks.Count;
            }
        }

        public int TotalCount
        {
            get
            {
                return tasks.Count + taskPoints.Count;
            }
        }

        public bool HasTasks
        {
            get
            {
                return ActiveCount > 0;
            }
        }
    }
}