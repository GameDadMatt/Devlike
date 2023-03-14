using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devlike.Characters
{
    //DialogueContainer holds a DialogueCollection's values for tracking
    public class DialogueContainer
    {
        public string name;
        public List<string> startNodes;
        public DramaType dramaType;
        public float empathyNeeded;
        public int stepInList = 0;

        public DialogueContainer(DialogueCollection collection)
        {
            name = collection.name;
            startNodes = collection.startNodes;
            dramaType = collection.dramaType;
            empathyNeeded = collection.empathyNeeded;
        }

        public void NextDialogueStep()
        {
            stepInList++;
        }

        public string CurrentStartNode
        {
            get
            {
                return startNodes[stepInList];
            }
        }

        public bool DialogueComplete
        {
            get
            {
                if (startNodes.Count == 0)
                {
                    return true;
                }
                else if (stepInList >= startNodes.Count)
                {
                    return true;
                }

                return false;
            }
        }
    }
}