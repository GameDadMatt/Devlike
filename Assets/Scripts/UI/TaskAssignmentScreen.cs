using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Tasks;
using Devlike.Characters;

namespace Devlike.UI
{
    /// <summary>
    /// The screen that allows tasks to be assigned to characters or the backlog
    /// </summary>
    public class TaskAssignmentScreen : ExecutableBehaviour
    {
        public static TaskAssignmentScreen instance;

        [SerializeField]
        private GlobalProject gProject;
        [SerializeField]
        private GlobalStudio gStudio;

        private Canvas canvas;
        [SerializeField]
        private GameObject taskPrefab;
        [SerializeField]
        private GameObject characterColumnPrefab;
        [SerializeField]
        private List<TaskColumn> backlogColumns;
        [SerializeField]
        private RectTransform characterColumnArea;
        [SerializeField]
        private Transform backlogColumnArea;

        private int curPage = 0;
        private float columnWidth = 980f / 4f;
        [SerializeField]
        private int charactersPerPage = 4;

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            //Make sure the GameObject is hidden
            canvas = GetComponent<Canvas>();
        }

        protected override void SetListeners()
        {
            EventManager.instance.OnDisplayUI += DisplayScreen;
            EventManager.instance.OnCloseUI += CloseScreen;
        }

        protected override void Launch()
        {
            GenerateScreen();
            canvas.enabled = false;
        }

        private void GenerateScreen()
        {
            //BACKLOG
            for(int i = 0; i < 3; i++)
            {
                backlogColumns[i].Tasks = gProject.TaskLists[i];
                GenerateTasks(gProject.TaskLists[i], backlogColumns[i].Area);
            }

            DrawCharacterColumns();
        }

        private void DrawCharacterColumns()
        {
            characterColumnArea.sizeDelta = new Vector2(columnWidth * gStudio.Characters.Count, characterColumnArea.rect.height);
            List<TaskColumn> containers = backlogColumns;            

            for (int i = 0; i < gStudio.Characters.Count; i++)
            {
                //If this character exists
                if(gStudio.Characters.Count > i)
                {
                    GameObject columnObj = Instantiate(characterColumnPrefab, characterColumnArea);
                    TaskColumn taskColumn = columnObj.GetComponent<TaskColumn>();
                    Character owner = gStudio.Characters[i];
                    taskColumn.CharacterColumn(owner);
                    containers.Add(taskColumn);
                    //SPAWN IN ALL THE TASKS FROM THIS CHARACTER
                    GenerateTasks(taskColumn.Tasks, taskColumn.Area);
                }           
            }

            DragTaskManager.instance.SetContainerAreas(containers);
        }

        private void GenerateTasks(TaskList list, Transform parent)
        {
            foreach(TaskContainer task in list.Tasks)
            {
                GameObject obj = Instantiate(taskPrefab, parent);
                obj.GetComponent<TaskUIObject>().Setup(task);
            }
        }

        private void DisplayScreen(Player.ActionType type)
        {
            if(type == Player.ActionType.TaskManagement)
            {
                canvas.enabled = true;
            }
        }

        private void CloseScreen()
        {
            if (canvas.enabled)
            {
                canvas.enabled = false;
            }
        }
    }
}
