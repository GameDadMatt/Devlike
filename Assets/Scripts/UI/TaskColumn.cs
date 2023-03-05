using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Tasks;

namespace Devlike.UI
{
    /// <summary>
    /// The tracking data for a column containing tasks in the UI
    /// </summary>
    public class TaskColumn : MonoBehaviour
    {
        public TaskList Tasks;
        public RectTransform Area;
        public bool onlyOneType = false;
        public TaskType type;
    }
}
