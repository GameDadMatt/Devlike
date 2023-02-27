using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Tasks;
using Devlike.Characters;

namespace Devlike.UI
{
    public class TaskAssignmentScreen : MonoBehaviour
    {
        public static TaskAssignmentScreen instance;

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

        private TaskList backlog;

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void OnEnable()
        {
            GenerateScreen();
        }

        public void SetBacklog(TaskList backlog)
        {
            this.backlog = backlog;
        }

        private void GenerateScreen()
        {
            int charListStart = curPage * charactersPerPage;
            int charListEnd = charListStart + charactersPerPage;
            List<TaskColumn> containers = backlogColumns;

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
                dtc.Tasks = StudioManager.instance.Characters[i].tasks;
                charColumns.Add(column);
                containers.Add(dtc);
                //SPAWN IN ALL THE TASKS FROM THIS CHARACTER
                //GenerateTasks(characters[i].tasks);
            }

            DragTaskManager.instance.SetContainerAreas(containers);
        }

        private void GenerateTasks(TaskList container)
        {

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
