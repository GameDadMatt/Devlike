using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.UI;

namespace Devlike.Tasks
{
    public class ContainerArea
    {
        public TaskContainer Container { get; private set; }
        public DragDropArea Area { get; private set; }

        public ContainerArea(TaskContainer container, DragDropArea area)
        {
            Container = container;
            Area = area;
        }
    }

    public class TaskContainer
    {
        private List<Task> tasks;
        public List<Task> Tasks { get { return tasks; } }

        public TaskContainer(List<Task> tasks)
        {
            this.tasks = tasks;
        }

        public TaskContainer()
        {
            tasks = new List<Task>();
        }

        public void AddTask(Task task)
        {
            tasks.Add(task);
        }

        public void RemoveTask(Task task)
        {
            tasks.Remove(task);
        }
    }
}