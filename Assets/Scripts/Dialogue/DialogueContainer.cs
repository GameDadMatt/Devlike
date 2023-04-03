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
        public DialogueType dramaType;
        public float empathyBase;
        public int stepInList = 0;
        //Positive
        public float raiseMood;
        public float lowerCrunch;
        public float raiseAlignment;
        //Negative
        public float lowerMood;
        public float raiseCrunch;
        public float lowerAlignment;
        public TriggerEvent trigger;

        public DialogueContainer(DialogueCollection collection)
        {
            name = collection.name;
            startNodes = collection.startNodes;
            dramaType = collection.dialogueType;
            empathyBase = collection.empathyBase;
            //Positive
            raiseMood = collection.increaseMood;
            lowerCrunch = collection.decreaseCrunch;
            raiseAlignment = collection.increaseAlignment;
            //Negative
            lowerMood = collection.decreaseMood;
            raiseCrunch = collection.increaseCrunch;
            lowerAlignment = collection.decreaseAlignment;
            trigger = collection.triggerEvent;
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