using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devlike.Characters
{
    /// <summary>
    /// A DialogueCollection is a list of YarnScripts
    /// </summary>
    public class DialogueCollection : ScriptableObject
    {
        public List<string> dialogueNames;
        public DialogueType dialogueType;
        public float empathyNeeded;
    }
}
