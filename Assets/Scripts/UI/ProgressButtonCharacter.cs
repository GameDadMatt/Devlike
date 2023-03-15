using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Devlike.Player;
using Devlike.Timing;
using Devlike.Characters;

namespace Devlike.UI
{
    public class ProgressButtonCharacter : ProgressButton
    {
        public Image characterSprite;
        [HideInInspector]
        public Character character;
        private CharacterState curState = CharacterState.Start;

        public override void GenerateButton()
        {
            buttonText.SetText(character.Profile.FirstName);
            characterSprite.color = character.Profile.Color;
            action = new PlayerAction(this, character.Profile.FullName, actionContainer.type, character, actionContainer.randomCompleteTime, actionContainer.minHoursToComplete, actionContainer.maxHoursToComplete);
        }

        private void Update()
        {
            //Update this button internally based upon this logic
            if(curState != character.CurrentState)
            {
                curState = character.CurrentState;
                if(curState == CharacterState.Inactive && Interactable)
                {
                    Interactable = false;
                }
                else if (curState != CharacterState.Inactive && !Interactable)
                {
                    Interactable = true;
                }
            }
        }

        public override void PressButton()
        {
            if(character.CurrentState != CharacterState.Inactive)
            {
                EventManager.instance.PlayerAction(action);
            }            
        }

        public override void ResetButton()
        {
            progressBar.fillAmount = 0f;
            button.interactable = true;
            if (curState == CharacterState.Inactive && Interactable)
            {
                Interactable = false;
            }
            else if (curState != CharacterState.Inactive && !Interactable)
            {
                Interactable = true;
            }
        }
    }
}