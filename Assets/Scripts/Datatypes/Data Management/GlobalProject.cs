using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Tasks;

namespace Devlike
{
    [CreateAssetMenu(fileName = "Project", menuName = "Devlike/Properties/Project")]
    public class GlobalProject : GlobalObject
    {
        [Header("PROJECT")]
        [SerializeField]
        private int desiredProjectDays;

        [Header("TASKS")]
        [SerializeField]
        private int maxTaskPoints; // = 5;
        [SerializeField]
        private int baseTaskPointsPerDay; // = 7f;
        [SerializeField]
        private float bugChance; // = 0.1f;

        [SerializeField]
        private Color artTaskColor, designTaskColor, engineeringTaskColor, taskBorderColor, bugBorderColor;

        public int DesiredProjectDays { get => desiredProjectDays; }
        public int MaxTaskPoints { get => maxTaskPoints; }
        public int BaseTaskPointsPerDay { get => baseTaskPointsPerDay; }
        public float BaseBugChance { get => bugChance; }

        public Color ArtTask { get => artTaskColor; }
        public Color DesignTask { get => designTaskColor; }
        public Color EngineeringTask { get => engineeringTaskColor; }
        public Color TaskBorder { get => taskBorderColor; }
        public Color BugBorder { get => bugBorderColor; }

        public int ProjectScope { get; private set; }

        public TaskList ArtTasks { get; private set; }
        public TaskList DesTasks { get; private set; }
        public TaskList EngTasks { get; private set; }
        public TaskList[] TaskLists { get { return new TaskList[] { ArtTasks, DesTasks, EngTasks }; } }

        public void GenerateProjectScope(int studioSize)
        {
            ProjectScope = Mathf.CeilToInt((studioSize * BaseTaskPointsPerDay) * DesiredProjectDays);
            List<TaskList> projectScope = RandomGeneration.instance.RandomProjectScope(ProjectScope);
            ArtTasks = projectScope[0];
            ArtTasks.GenerateTaskFromPoints(TaskType.Art, 20, BaseBugChance);
            DesTasks = projectScope[1];
            DesTasks.GenerateTaskFromPoints(TaskType.Design, 20, BaseBugChance);
            EngTasks = projectScope[2];
            EngTasks.GenerateTaskFromPoints(TaskType.Engineering, 20, BaseBugChance);

            Debug.Log("Total Scope = Art " + ArtTasks.TotalPoints + " points, Design " + DesTasks.TotalPoints + " points, Engineering " + EngTasks.TotalPoints + "\n Current Scope = Art " + ArtTasks.RemainingPoints + " points in " + ArtTasks.Total + " tasks, Design " + DesTasks.RemainingPoints + " points in " + ArtTasks.Total + " tasks, Engineering " + EngTasks.RemainingPoints + " points in " + EngTasks.Total + " tasks.");
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

        public override void ResetValues()
        {
            ProjectScope = 0;
            ArtTasks = new TaskList();
            DesTasks = new TaskList();
            EngTasks = new TaskList();
        }
    }
}