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

        public string Seed { get => seed; }
        public GameState CurrentState { get => gameState; }

        public void UpdateGameState(GameState state)
        {
            gameState = state;
        }

        public override void ResetValues()
        {
            gameState = startState;
        }
    }
}
