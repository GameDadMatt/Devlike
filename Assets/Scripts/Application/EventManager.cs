using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Player;
using Devlike.Characters;

namespace Devlike
{
    public class EventManager : ExecutableBehaviour
    {
        public static EventManager instance;

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogWarning("Multiple copies of " + name + " detected");
            }
        }

        #region Player
        //Listen for and send events relacting to a player action
        public event Action<PlayerAction> OnPlayerAction;
        public void PlayerAction(PlayerAction action)
        {
            OnPlayerAction?.Invoke(action);
        }

        //Listen for and send events relating to the completion of a player action
        public event Action OnCompletePlayerAction;
        public void CompletePlayerAction()
        {
            OnCompletePlayerAction?.Invoke();
        }

        //Parse the output of a player action after it's been through PlayerManager
        public void ParsePlayerAction(ActionType type, object obj)
        {
            switch (type)
            {
                case ActionType.ScopeManagement:
                case ActionType.TaskManagement:
                case ActionType.CheckProgress:
                    OnDisplayUI?.Invoke(type);
                    break;
                case ActionType.TeamMeeting:
                    break;
                case ActionType.TalkTo:
                    OnCharacterInteract?.Invoke(obj as Character); //Forward this to the Character Interact event
                    break;
            }
            //OnPlayerAction?.Invoke(type, id);
        }

        public event Action<ActionType> OnDisplayUI;
        public void DisplayUI(ActionType type)
        {
            OnDisplayUI?.Invoke(type);
        }

        public event Action<Character> OnCharacterInteract;
        public void CharacterInteract(Character character)
        {
            OnCharacterInteract?.Invoke(character);
        }

        public event Action<Character> OnCharacterSelect;
        public void CharacterSelect(Character click)
        {
            OnCharacterSelect?.Invoke(click);
        }
        #endregion

        #region Game
        public event Action<GameState> OnChangeGameState;
        public void ChangeGameState(GameState state)
        {
            OnChangeGameState?.Invoke(state);
        }

        public event Action OnTick;
        public void Tick()
        {
            OnTick?.Invoke();
        }

        public event Action OnDayStart;
        public void DayStart()
        {
            OnDayStart?.Invoke();
        }

        public event Action OnWeekStart;
        public void WeekStart()
        {
            OnWeekStart?.Invoke();
        }

        public event Action OnSetCharacters;
        public void SetCharacters()
        {
            OnSetCharacters?.Invoke();
        }

        public event Action<string, bool, Sprite> OnSetCharacterMoodlet;
        public void SetCharacterMoodlet(string id, bool active, Sprite sprite)
        {
            OnSetCharacterMoodlet?.Invoke(id, active, sprite);
        }
        #endregion

        #region UI
        public event Action<object> OnRegisterButton;
        public void RegisterButton(object button)
        {
            OnRegisterButton?.Invoke(button);
        }
        #endregion
    }
}
