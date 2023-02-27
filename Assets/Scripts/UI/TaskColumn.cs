using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Tasks;

namespace Devlike.UI
{
    public class TaskColumn : MonoBehaviour
    {
        public TaskList Tasks;
        public RectTransform Area;
        public bool onlyOneType = false;
        public TaskType type;
    }
}
