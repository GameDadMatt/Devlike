using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devlike.Characters
{
    /// <summary>
    /// A DialogueCollection is a list of YarnScripts
    /// </summary>
    [CreateAssetMenu(fileName = "Dialogue Collection", menuName = "Devlike/Dialogue Collection")]
    public class DialogueCollection : ScriptableObject
    {
        public List<string> startNodes;
        public DramaType dramaType;
        [Range(0, 10)]
        public int empathyNeeded;
        [Range(0f, 5f)]
        public float moodImpact;
        [Range(0f, 1f)]
        public float crunchPressure;
        [Range(0f, 1f)]
        public float alignmentPressure;
        public TriggerEvent trigger;
    }
}
