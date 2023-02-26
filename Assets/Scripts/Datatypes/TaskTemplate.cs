using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devlike.Tasks
{
    //The base Task class can work to satify Needs
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