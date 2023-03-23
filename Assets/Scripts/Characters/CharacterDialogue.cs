using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devlike.Characters
{
    public class CharacterDialogue : MonoBehaviour
    {
        //References
        [SerializeField]
        private GlobalDialogue gDialogue;

        //Dialogue
        private DialogueContainer currentDialogue;
        public DialogueContainer CurrentDialogue { get => currentDialogue; }

        public void SetDialogue(DialogueContainer dialogue)
        {
            currentDialogue = dialogue;
        }

        public void NewDialogue(Character character, DialogueContainer newDialogue)
        {
            //Apply the impacts of the current dialogue
            //character.Tasker.
        }
    }
}
