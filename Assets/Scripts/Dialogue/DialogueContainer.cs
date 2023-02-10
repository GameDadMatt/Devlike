using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    //DialogueContainer holds a DialogueCollection's values for tracking
    public class DialogueContainer
    {
        public bool DialogueComplete { get; private set; } = false;
        public string name;
        public List<string> dialogueNames;
        public float empathyNeeded;
        public int stepInList = 0;

        public DialogueContainer(DialogueCollection collection)
        {
            name = collection.name;
            dialogueNames = collection.dialogueNames;
            empathyNeeded = collection.empathyNeeded;
            DialogueComplete = false;
        }

        public void EndDialogue()
        {
            DialogueComplete = false;
        }
    }
}