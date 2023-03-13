using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Player;

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

        public event Action<ActionType, string> OnPlayerAction;
        public void PlayerAction(ActionType type, string id)
        {
            OnPlayerAction?.Invoke(type, id);
        }

        public event Action<GameState> OnChangeGameState;
        public void ChangeGameState(GameState state)
        {
            OnChangeGameState?.Invoke(state);
        }
    }
}
