using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Devlike.Tasks;
using Devlike.Characters;

namespace Devlike.UI
{
    /// <summary>
    /// The tracking data for a column containing tasks in the UI
    /// </summary>
    public class TaskColumn : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI nameText;
        [SerializeField]
        private TextMeshProUGUI profText;

        private Character Owner;
        public TaskList Tasks;
        public RectTransform Area;
        public bool onlyOneType = false;
        public TaskType type;

        public void CharacterColumn(Character owner)
        {
            Owner = owner;
            Tasks = owner.Tasker.Tasks;
            nameText.SetText(Owner.Profile.FirstName + " " + Owner.Profile.LastName);
            profText.SetText(Owner.Profile.Profession.name);
        }

        public void UpdateTaskList()
        {
            List<TaskContainer> tlist = new List<TaskContainer>();
            foreach(TaskUIObject obj in transform.GetComponentsInChildren<TaskUIObject>())
            {
                tlist.Add(obj.Task);
            }
            Tasks.UpdateTaskList(tlist);
        }
    }
}
