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
        [SerializeField]
        private int charactersPerPage = 4;

        private int curPage = 0;
        private float columnWidth = 980f / 4f;
        private List<TaskColumn> allColumns;

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
            characterColumnArea.sizeDelta = new Vector2(columnWidth * gStudio.Characters.Count, characterColumnArea.rect.height);
            allColumns = backlogColumns;

            for (int i = 0; i < backlogColumns.Count; i++)
            {
                backlogColumns[i].Tasks = gProject.TaskLists[i];
            }

            for (int i = 0; i < gStudio.Characters.Count; i++)
            {
                //If this character exists
                if (gStudio.Characters.Count > i)
                {
                    GameObject columnObj = Instantiate(characterColumnPrefab, characterColumnArea);
                    TaskColumn taskColumn = columnObj.GetComponent<TaskColumn>();
                    Character owner = gStudio.Characters[i];
                    taskColumn.CharacterColumn(owner); //Assign the character and their tasks to this column
                    allColumns.Add(taskColumn);
                }
            }

            DragTaskManager.instance.SetContainerAreas(allColumns);
        }

        private void GenerateTasks(TaskList list, Transform parent)
        {
            foreach(TaskContainer task in list.Tasks)
            {
                GameObject obj = Instantiate(taskPrefab, parent);
                obj.GetComponent<TaskUIObject>().Setup(task);
            }
        }

        private void DrawColumnContents()
        {
            foreach(TaskColumn column in allColumns)
            {
                Debug.Log("Creating " + column.Tasks.Total + " tasks");
                GenerateTasks(column.Tasks, column.Area.transform);
            }
        }

        private void ClearColumnContents()
        {
            foreach (TaskColumn column in allColumns)
            {
                //Update the column task list based on its current state
                column.UpdateTaskList();
                //Clear the contents of the column
                for (int i = column.Area.childCount - 1; i >= 0; i--)
                {
                    Destroy(column.Area.GetChild(i).gameObject);
                }
            }
        }

        private void DisplayScreen(Player.ActionType type)
        {
            if (type == Player.ActionType.TaskManagement)
            {
                DrawColumnContents();
                canvas.enabled = true;
            }
        }

        private void CloseScreen()
        {
            if (canvas.enabled)
            {
                canvas.enabled = false;
                ClearColumnContents();
            }
        }
    }
}
