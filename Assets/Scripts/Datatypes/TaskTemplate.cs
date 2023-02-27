using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devlike.Tasks
{
    /// <summary>
    /// Allows the creation of unique tasks that can then be loaded into unique TaskContainers for storage during gameplay
    /// </summary>
    [CreateAssetMenu(fileName = "TaskType", menuName = "Devlike/Task")]
    public class TaskTemplate : ScriptableObject
    {
        public TaskType type;
        public TaskImportance importance = TaskImportance.None;
        public int minTicks;
        public int maxTicks;
        public float bugChance = 0.05f;
    }
}