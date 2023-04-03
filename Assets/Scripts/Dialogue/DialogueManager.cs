using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Devlike.Characters
{
    public class DialogueManager : ExecutableBehaviour
    {
        [SerializeField]
        private GlobalGame gGame;
        [SerializeField]
        private GlobalDialogue gDialogue;

        private DialogueRunner dialogueRunner;

        public bool DialogueRunning { get; private set; } = false;
        private static Character activeCharacter;

        protected override void SetListeners()
        {
            EventManager.instance.OnCharacterInteract += StartConversation;
            dialogueRunner = GetComponent<DialogueRunner>();
            dialogueRunner.onDialogueComplete.AddListener(EndConversation);
        }

        private void StartConversation(Character character)
        {
            if(gGame.CurrentState != GameState.Interacting && gGame.CurrentState != GameState.Paused)
            {
                if (!DialogueRunning)
                {
                    activeCharacter = character;
                    Debug.Log("Started conversation with " + activeCharacter.Profile.FullName);
                    dialogueRunner.StartDialogue(activeCharacter.CharacterDialogue.CurrentDialogue.CurrentStartNode);
                    DialogueRunning = true;
                    gGame.UpdateGameState(GameState.Interacting);
                }
            }
        }

        private void EndConversation()
        {
            Debug.Log("Ended conversation with " + activeCharacter.Profile.FullName);
            DialogueRunning = false;
            //Complete the event
            activeCharacter.CharacterDialogue.ResolveDialogue();
            gGame.UpdateGameState(GameState.Ticking);
            EventManager.instance.CompletePlayerAction();
        }

        [YarnCommand("MoodImpact")]
        public void MoodImpact(float value)
        {
            activeCharacter.ChangeMoodImpact(value);
        }

        [YarnFunction("FullName")]
        public static string FullName()
        {
            return activeCharacter.Profile.FullName;
        }

        [YarnFunction("Nickname")]
        public static string Nickname()
        {
            return activeCharacter.Profile.Nickname;
        }

        [YarnFunction("FirstName")]
        public static string FirstName()
        {
            return activeCharacter.Profile.FirstName;
        }

        [YarnFunction("LastName")]
        public static string LastName()
        {
            return activeCharacter.Profile.LastName;
        }

        [YarnFunction("Profession")]
        public static string Profession()
        {
            return activeCharacter.Profile.Profession.name;
        }
    }
}
