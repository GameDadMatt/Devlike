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
        private CharacterTasker characterTasker;

        //Dialogue
        private DialogueContainer noDialogue; //We store NoDialogue for whenever the character has no interaction
        private DialogueContainer currentDialogue;
        public DialogueContainer CurrentDialogue { get => currentDialogue; }

        private void OnEnable()
        {
            character = GetComponent<Character>();
        }

        public void SetDefault(DialogueContainer dialogue)
        {
            noDialogue = dialogue;
            currentDialogue = dialogue;
        }

        public void NewDialogue(DialogueContainer dialogue)
        {
            //Apply the impacts of the current dialogue
            ClearDialogue();

            //Set the new dialogue
            currentDialogue = dialogue;
        }

        public void DialogueFromMood(MoodletType type)
        {
            NewDialogue(gDialogue.GetDialogueOfMoodlet(type));
        }

        public void ResolveDialogue()
        {
            if (currentDialogue != null)
            {
                character.ImproveMood(currentDialogue.raiseMood);
                character.AlignmentRestore(currentDialogue.raiseAlignment);
                character.ReduceCrunchPressure(currentDialogue.lowerCrunch);
                Debug.Log("Applying dialogue outcome, mood: " + currentDialogue.raiseMood + ", alignment: " + currentDialogue.raiseAlignment + ", crunch: " + currentDialogue.lowerCrunch);

                currentDialogue = noDialogue;
            }
        }

        private void ClearDialogue()
        {
            if (currentDialogue != null)
            {
                character.LowerMood(currentDialogue.lowerMood); //This is a negative outcome
                character.AlignmentImpact(currentDialogue.lowerAlignment);
                character.SetCrunchPressure(currentDialogue.raiseCrunch);
                Debug.Log("Applying dialogue outcome, mood: " + currentDialogue.lowerMood + ", alignment: " + currentDialogue.lowerAlignment + ", crunch: " + currentDialogue.raiseCrunch);

                currentDialogue = noDialogue;
            }
        }

        public bool HasDrama
        {
            get
            {
                return currentDialogue.dramaType != DialogueType.None;
            }
        }

        public DialogueType DramaType
        {
            get
            {
                return currentDialogue.dramaType;
            }
        }
    }
}
