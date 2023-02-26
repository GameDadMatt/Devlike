using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.UI;
using Devlike.Characters;

namespace Devlike.Tasks
{
    public class TaskList
    {
        private List<TaskContainer> tasks;
        public List<TaskContainer> Tasks { get { return tasks; } }

        public TaskList(List<TaskContainer> tasks)
        {
            this.tasks = tasks;
        }

        public TaskList()
        {
            tasks = new List<TaskContainer>();
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
    }
}