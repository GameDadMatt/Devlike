using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devlike
{
    [CreateAssetMenu(fileName = "Game", menuName = "Devlike/Properties/Game")]
    public class GlobalGame : GlobalObject
    {
        [SerializeField]
        private string seed;
        [SerializeField]
        private GameState gameState, startState;
        [SerializeField]
        private GameSpeed gameSpeed, startSpeed;

        public string Seed { get => seed; }
        public GameState CurrentState { get => gameState; }
        public GameSpeed CurrentSpeed { get => gameSpeed; }

        public event Action OnGameStateChange;
        public void UpdateGameState(GameState state)
        {
            if (gameState != state)
            {
                gameState = state;
                OnGameStateChange?.Invoke();
            }
        }

        public event Action OnGameSpeedChange;
        public void UpdateGameSpeed(GameSpeed speed)
        {
            if(gameSpeed != speed)
            {
                gameSpeed = speed;
                OnGameSpeedChange?.Invoke();
            }            
        }

        public override void ResetValues()
        {
            gameState = startState;
            gameSpeed = startSpeed;
        }
    }
}
