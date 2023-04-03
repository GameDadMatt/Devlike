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
        public DialogueType dialogueType;
        public MoodletType moodletType;
        [Range(0, 10)]
        public int empathyBase;
        [Header("IF RESOLVED")]
        [Range(0f, 5f)]
        public float increaseMood;
        [Range(-1f, 0f)]
        public float decreaseCrunch;
        [Range(0f, 1f)]
        public float increaseAlignment;
        [Header("IF UNRESOLVED")]
        [Range(-5f, 0f)]
        public float decreaseMood;
        [Range(0f, 1f)]
        public float increaseCrunch;
        [Range(-1f, 0f)]
        public float decreaseAlignment;
        public TriggerEvent triggerEvent;
    }
}
