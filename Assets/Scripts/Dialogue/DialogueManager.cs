using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Devlike.Characters
{
    public class DialogueManager : ExecutableBehaviour
    {
        private DialogueRunner dialogueRunner;

        public bool DialogueRunning { get; private set; } = false;
        private static Character activeCharacter;
        private GameState lastState = GameState.Normal;

        protected override void SetListeners()
        {
            EventManager.instance.OnCharacterInteract += StartConversation;
            dialogueRunner = GetComponent<DialogueRunner>();
            dialogueRunner.onDialogueComplete.AddListener(EndConversation);
        }

        private void StartConversation(Character character)
        {
            if(GameValues.CurrentState != GameState.Interacting && GameValues.CurrentState != GameState.Paused)
            {
                if (!DialogueRunning)
                {
                    activeCharacter = character;
                    Debug.Log("Started conversation with " + activeCharacter.Profile.FullName);
                    dialogueRunner.StartDialogue(activeCharacter.CurrentDialogue.CurrentStartNode);
                    DialogueRunning = true;
                    lastState = GameValues.CurrentState;
                    EventManager.instance.ChangeGameState(GameState.Interacting);
                }
            }
        }

        private void EndConversation()
        {
            Debug.Log("Ended conversation with " + activeCharacter.Profile.FullName);
            DialogueRunning = false;
            //Return to the previous game state
            EventManager.instance.ChangeGameState(lastState);
            //Complete the event
            EventManager.instance.CompletePlayerAction();
        }

        public DialogueContainer DefaultDialogue
        {
            get
            {
                return new DialogueContainer(StartingValues.value.allDialogues[0]); //Dialogue[0] should always be the default dialogue
            }
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
