using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Devlike.UI
{
    public class SpeedAreaController : ExecutableBehaviour
    {
        private GlobalGame game;

        [SerializeField]
        private Image speedDisplay;
        [SerializeField]
        private Button pauseButton;
        [SerializeField]
        private Button playButton;
        [SerializeField]
        private Button fastButton;
        [SerializeField]
        private Sprite pausedSprite;
        [SerializeField]
        private Sprite playSprite;
        [SerializeField]
        private Sprite fastSprite;

        private bool started = false;
        private GameState lastState;

        protected override void SetProperties()
        {
            game = GameManager.instance.GetGlobal("Game") as GlobalGame;
            lastState = game.CurrentState;
        }

        protected override void SetListeners()
        {
            EventManager.instance.RegisterButton(pauseButton);
            EventManager.instance.RegisterButton(playButton);
            EventManager.instance.RegisterButton(fastButton);
        }

        protected override void AfterDelay()
        {
            started = true;
        }

        public void Update()
        {
            if(started && lastState != game.CurrentState)
            {
                lastState = game.CurrentState;
                switch (lastState)
                {
                    case GameState.Paused:
                        pauseButton.interactable = false;
                        playButton.interactable = true;
                        fastButton.interactable = true;
                        ChangeSpeedDisplay(pausedSprite);
                        break;
                    case GameState.Interacting:
                        pauseButton.interactable = false;
                        playButton.interactable = false ;
                        fastButton.interactable = false;
                        ChangeSpeedDisplay(pausedSprite);
                        break;
                    case GameState.Normal:
                        pauseButton.interactable = true;
                        playButton.interactable = false;
                        fastButton.interactable = true;
                        ChangeSpeedDisplay(playSprite);
                        break;
                    case GameState.Fast:
                        pauseButton.interactable = true;
                        playButton.interactable = true;
                        fastButton.interactable = false;
                        ChangeSpeedDisplay(fastSprite);
                        break;
                }
            }
        }

        public void ChangeSpeedDisplay(Sprite sprite)
        {
            speedDisplay.sprite = sprite;
        }

        public void PauseButton()
        {
            EventManager.instance.ChangeGameState(GameState.Paused);
        }

        public void NormalSpeedButton()
        {
            EventManager.instance.ChangeGameState(GameState.Normal);
        }

        public void FastSpeedButton()
        {
            EventManager.instance.ChangeGameState(GameState.Fast);
        }
    }
}