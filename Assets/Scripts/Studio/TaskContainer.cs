using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.UI;
using Devlike.Characters;

namespace Devlike.Tasks
{
    public class ContainerArea
    {
        public string ID { get; set; }
        public TaskContainer Container { get; private set; }
        public UI.RectTransform Area { get; private set; }

        public ContainerArea(TaskContainer container, UI.RectTransform area)
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

        public void ReorganiseTasks(string id, int position)
        {
            Task task = null;
            foreach(Task t in tasks)
            {
                if (t.ID == id)
                {
                    task = t;
                    Tasks.Remove(t);
                    break;
                }
            }

            if (task == null)
            {
                Debug.LogError("Could not find specified task");
                return;
            }
            else
            {
                tasks.Insert(position, task);
            }
        }
    }
}