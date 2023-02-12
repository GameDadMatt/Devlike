using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using DataTypes;

namespace Characters
{
    public class CharacterInteraction : MonoBehaviour
    {
        private string conversationStartNode = "farts";
        private Character character;
        private DialogueRunner dialogueRunner;
        private bool interactable = true;
        private bool isCurrentConversation = false;

        public void OnEnable()
        {
            character = GetComponent<Character>();
            dialogueRunner = FindObjectOfType<DialogueRunner>();
            dialogueRunner.onDialogueComplete.AddListener(EndConversation);
        }

        public void OnMouseOver()
        {
            //Left click
            if (Input.GetMouseButtonDown(0))
            {
                if (interactable && !dialogueRunner.IsDialogueRunning)
                {
                    StartConversation();
                }
            }
            //Right click
            else if (Input.GetMouseButtonDown(1))
            {
                GameManager.instance.CharacterSelect(character);
            }
        }

        private void StartConversation()
        {
            Debug.Log($"Started conversation with {name}.");
            isCurrentConversation = true;
            dialogueRunner.StartDialogue(conversationStartNode.ToString());
        }

        private void EndConversation()
        {
            if (isCurrentConversation)
            {
                isCurrentConversation = false;
                Debug.Log($"Started conversation with {name}.");
            }
        }

        public void DisableConversation()
        {
            interactable = false;
        }

        [YarnCommand("MoodImpact")]
        public void MoodImpact(float value)
        {
            character.ChangeMoodImpact(value);
        }
    }
}