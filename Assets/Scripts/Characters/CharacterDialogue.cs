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

        private Character character;

        //Dialogue
        private DialogueContainer currentDialogue;
        public DialogueContainer CurrentDialogue { get => currentDialogue; }

        private void OnEnable()
        {
            character = GetComponent<Character>();
        }

        public void SetDialogue(DialogueContainer dialogue)
        {
            currentDialogue = dialogue;
        }

        public void NewDialogue(DialogueContainer newDialogue)
        {
            //Apply the impacts of the current dialogue
            //character.Tasker.
        }

        public void ResolveDialogue()
        {

        }

        private void UnresolvedDialogue()
        {

        }

        public bool HasDrama
        {
            get
            {
                return currentDialogue.dramaType != DramaType.None;
            }
        }

        public DramaType DramaType
        {
            get
            {
                return currentDialogue.dramaType;
            }
        }
    }
}
