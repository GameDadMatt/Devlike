using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Player;
using Devlike.Characters;

namespace Devlike
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager instance;

        public void OnEnable()
        {
            if (instance == null)
            {
                instance = this;
            }
        }
        
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
                    break;
                case ActionType.TaskManagement:
                    break;
                case ActionType.TeamMeeting:
                    break;
                case ActionType.CheckProgress:
                    break;
                case ActionType.TalkTo:
                    OnCharacterInteract?.Invoke(obj as Character); //Forward this to the Character Interact event
                    break;
            }
            //OnPlayerAction?.Invoke(type, id);
        }

        public event Action<Character> OnCharacterInteract;
        public void CharacterInteract(Character character)
        {
            OnCharacterInteract?.Invoke(character);
        }

        public event Action<GameState> OnChangeGameState;
        public void ChangeGameState(GameState state)
        {
            OnChangeGameState?.Invoke(state);
        }
    }
}
