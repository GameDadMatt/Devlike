using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Tasks;

namespace Devlike.Project
{
    /// <summary>
    /// Sets up the project the studio will be undertaking, and tracks its completion
    /// </summary>
    public class StudioProject : ExecutableBehaviour
    {
        public static StudioProject instance;

        private GlobalStudio studio;
        private GlobalProject project;

        [SerializeField]
        private int DesiredProjectDays;

        public int ProjectScope { get; private set; }

        public TaskList ArtTasks { get; private set; }
        public TaskList DesTasks { get; private set; }
        public TaskList EngTasks { get; private set; }
        public TaskList[] TaskLists { get { return new TaskList[] { ArtTasks, DesTasks, EngTasks }; } }

        //GENERATE RANDOM TOTAL TASK NUMBERS AND TASK SIZE
        //REDUCE TOTAL BY NUMBER PULLED INTO BACKLOG

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        protected override void SetProperties()
        {
            studio = GameManager.instance.GetGlobal("Studio") as GlobalStudio;
            project = GameManager.instance.GetGlobal("Project") as GlobalProject;

            GenerateProjectScope();
        }

        private void GenerateProjectScope()
        {
            //Set the project scope
            ProjectScope = Mathf.CeilToInt((studio.StudioSize * project.BasePointsPerDay) * DesiredProjectDays);
            List<TaskList> projectScope = RandomGeneration.instance.RandomProjectScope(ProjectScope);
            ArtTasks = projectScope[0];
            ArtTasks.GenerateTaskFromPoints(TaskType.Art, 20, project.BaseBugChance);
            DesTasks = projectScope[1];
            DesTasks.GenerateTaskFromPoints(TaskType.Design, 20, project.BaseBugChance);
            EngTasks = projectScope[2];
            EngTasks.GenerateTaskFromPoints(TaskType.Engineering, 20, project.BaseBugChance);

            Debug.Log("Total Scope = Art " + ArtTasks.TotalPoints + " points, Design " + DesTasks.TotalPoints + " points, Engineering " + EngTasks.TotalPoints + "\n Current Scope = Art " + ArtTasks.RemainingPoints + " points in " + ArtTasks.ActiveCount + " tasks, Design " + DesTasks.RemainingPoints + " points in " + ArtTasks.ActiveCount + " tasks, Engineering " + EngTasks.RemainingPoints + " points in " + EngTasks.ActiveCount + " tasks.");
        }

        public void AddTask(TaskContainer task)
        {
            switch (task.Type)
            {
                case TaskType.Art:
                    ArtTasks.AddTask(task);
                    break;
                case TaskType.Engineering:
                    EngTasks.AddTask(task);
                    break;
                case TaskType.Design:
                    DesTasks.AddTask(task);
                    break;
            }
        }
    }
}
