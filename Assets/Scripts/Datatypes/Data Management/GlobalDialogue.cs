using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Characters;

namespace Devlike
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Devlike/Properties/Dialogue")]
    public class GlobalDialogue : GlobalObject
    {
        [SerializeField]
        private float minWeeklyArtificialDramas;
        [SerializeField]
        private float maxWeeklyArtificialDramas;
        [SerializeField]
        private DialogueCollection defaultDialogue;
        [SerializeField]
        private List<DialogueCollection> dialogues = new List<DialogueCollection>();

        public float MinWeeklyArtificialDramas { get => minWeeklyArtificialDramas; }
        public float MaxWeeklyArtificialDramas { get => maxWeeklyArtificialDramas; }
        public DialogueContainer DefaultDialogue { get { return new DialogueContainer(defaultDialogue); } }
        public List<DialogueCollection> Dialogues { get => dialogues; }
    }
}
