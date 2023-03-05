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
    public class TaskAssignmentScreen : MonoBehaviour
    {
        public static TaskAssignmentScreen instance;

        [SerializeField]
        private GameObject taskPrefab;
        [SerializeField]
        private GameObject characterColumnPrefab;
        [SerializeField]
        private List<TaskColumn> backlogColumns;
        [SerializeField]
        private Transform characterColumnArea;
        [SerializeField]
        private Transform backlogColumnArea;

        private int curPage = 0;
        private int TotalPages { get { return Mathf.CeilToInt(StudioManager.instance.Characters.Count / charactersPerPage); } }
        [SerializeField]
        private int charactersPerPage = 4;
        private List<GameObject> charColumns = new List<GameObject>();

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            //Make sure the GameObject is hidden
            gameObject.SetActive(false);
        }

        public void OnEnable()
        {
            GenerateScreen();
        }

        private void GenerateScreen()
        {
            int charListStart = curPage * charactersPerPage;
            int charListEnd = charListStart + charactersPerPage;
            List<TaskColumn> containers = backlogColumns; //All containers

            //BACKLOG
            for(int i = 0; i < 3; i++)
            {
                backlogColumns[i].Tasks = StudioProject.instance.TaskLists[i];
                GenerateTasks(StudioProject.instance.TaskLists[i], backlogColumns[i].Area);
            }

            //CHARACTERS
            //Iterate backwards over the list as we'll be removing contents
            for (int i = charColumns.Count; i > 0; i--)
            {
                Destroy(charColumns[i]);
                charColumns.RemoveAt(i);
            }

            for (int i = charListStart; i < charListEnd; i++)
            {
                GameObject column = Instantiate(characterColumnPrefab, characterColumnArea);
                TaskColumn dtc = column.GetComponent<TaskColumn>();
                dtc.Tasks = StudioManager.instance.Characters[i].Tasks;
                charColumns.Add(column);
                containers.Add(dtc);
                //SPAWN IN ALL THE TASKS FROM THIS CHARACTER
                GenerateTasks(dtc.Tasks, dtc.Area);
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

        public void NextPage()
        {
            if (curPage < TotalPages)
            {
                curPage++;
                GenerateScreen();
            }
        }

        public void PreviousPage()
        {
            if (curPage > 0)
            {
                curPage--;
                GenerateScreen();
            }
        }
    }
}
