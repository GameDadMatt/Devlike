using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Devlike.Tasks;
using TMPro;

namespace Devlike.UI
{
    [RequireComponent(typeof(Image))]
    public class TaskUIObject : MonoBehaviour
    {
        public TaskContainer Task { get; private set; }
        [SerializeField]
        private Image frame;
        private Image background;
        [SerializeField]
        private TextMeshProUGUI tmpro;
        [SerializeField]
        private List<GameObject> points;

        public void Awake()
        {
            background = GetComponent<Image>();
            foreach(GameObject obj in points)
            {
                obj.SetActive(false);
            }
        }

        public void Setup(TaskContainer task)
        {
            GlobalProject project = GameManager.instance.GetGlobal("Project") as GlobalProject;

            Task = task;
            if(Task.Importance == TaskImportance.Bug)
            {
                frame.color = project.BugBorder;
            }
            else
            {
                frame.color = project.TaskBorder;
            }

            switch (Task.Type)
            {
                case TaskType.Art:
                    background.color = project.ArtTask;
                    tmpro.SetText("ART");
                    break;
                case TaskType.Engineering:
                    background.color = project.EngineeringTask;
                    tmpro.SetText("ENGINEERING");
                    break;
                case TaskType.Design:
                    background.color = project.DesignTask;
                    tmpro.SetText("DESIGN");
                    break;
            }

            for(int i = 0; i < Task.Points; i++)
            {
                points[i].SetActive(true);
            }

            //Setup the Drag Task
            GetComponent<DragTask>().taskType = Task.Type;
        }
    }
}

