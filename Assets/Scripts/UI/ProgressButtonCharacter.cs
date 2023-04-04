using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Devlike.Player;
using Devlike.Timing;
using Devlike.Characters;

namespace Devlike.UI
{
    public class ProgressButtonCharacter : ProgressButton, IPointerClickHandler
    {
        public Image characterSprite;
        [HideInInspector]
        public Character character;
        private CharacterState curState = CharacterState.Start;

        public override void GenerateButton()
        {
            //Because we're doing this manually, we have to call SetListeners ourself
            SetListeners();
            buttonText.SetText(character.Profile.FirstName);
            GetComponent<MoodletDisplay>().RegisterMoodlet(character.ID);
            characterSprite.color = character.Profile.Color;
            action = new PlayerAction(gTime, this, character.Profile.FullName, actionContainer.type, character, actionContainer.randomCompleteTime, actionContainer.minHoursToComplete, actionContainer.maxHoursToComplete);
        }

        private void Update()
        {
            if(character != null)
            {
                //Update this button internally based upon this logic
                if (curState != character.CurrentState)
                {
                    curState = character.CurrentState;
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

        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                EventManager.instance.PlayerAction(action);
            }
            else if(eventData.button == PointerEventData.InputButton.Right)
            {
                EventManager.instance.CharacterSelect(character);
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