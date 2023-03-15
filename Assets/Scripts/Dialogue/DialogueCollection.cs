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
        public int empathyNeeded;
    }
}
