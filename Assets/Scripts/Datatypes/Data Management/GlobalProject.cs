using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devlike
{
    [CreateAssetMenu(fileName = "Project", menuName = "Devlike/Properties/Project")]
    public class GlobalProject : GlobalObject
    {
        [Header("TASKS")]
        [SerializeField]
        private int maxTaskPoints; // = 5;
        [SerializeField]
        private int baseTaskPointsPerDay; // = 7f;
        [SerializeField]
        private float bugChance; // = 0.1f;

        [SerializeField]
        private Color artTaskColor, designTaskColor, engineeringTaskColor, taskBorderColor, bugBorderColor;

        public int MaxTaskPoints { get => maxTaskPoints; }
        public int BaseTaskPointsPerDay { get => baseTaskPointsPerDay; }
        public float BaseBugChance { get => bugChance; }

        public Color ArtTask { get => artTaskColor; }
        public Color DesignTask { get => designTaskColor; }
        public Color EngineeringTask { get => engineeringTaskColor; }
        public Color TaskBorder { get => taskBorderColor; }
        public Color BugBorder { get => bugBorderColor; }
    }
}