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
        private Transform characterColumnArea;
        [SerializeField]
        private Transform backlogColumnArea;

        private int curPage = 0;
        private int TotalPages { get { return Mathf.CeilToInt(characters.Count / charactersPerPage); } }
        [SerializeField]
        private int charactersPerPage = 4;
        private List<GameObject> charColumns = new List<GameObject>();

        private TaskContainer backlog;
        private List<Character> characters;

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void SetBacklog(TaskContainer backlog)
        {
            this.backlog = backlog;
        }

        public void RegisterCharacter(Character character)
        {
            characters.Add(character);
        }

        private void GenerateScreen()
        {
            int charListStart = curPage * charactersPerPage;
            int charListEnd = charListStart + charactersPerPage;

            //Iterate backwards over the list as we'll be removing contents
            for (int i = charColumns.Count; i > 0; i--)
            {
                Destroy(charColumns[i]);
                charColumns.RemoveAt(i);
            }

            for (int i = charListStart; i < charListEnd; i++)
            {
                GameObject column = Instantiate(characterColumnPrefab, characterColumnArea);
                column.GetComponent<DragTaskContainer>().TaskContainer = characters[i].tasks;
                charColumns.Add(column);
                //SPAWN IN ALL THE TASKS FROM THIS CHARACTER
                //GenerateTasks(characters[i].tasks);
            }
        }

        private void GenerateTasks(TaskContainer container)
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
