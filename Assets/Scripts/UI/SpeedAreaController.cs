using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Devlike.UI
{
    public class SpeedAreaController : ExecutableBehaviour
    {
        [SerializeField]
        private GlobalGame gGame;

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

        private GameState lastState;
        private GameSpeed displayedSpeed;

        protected override void SetProperties()
        {
            lastState = gGame.CurrentState;
            displayedSpeed = gGame.CurrentSpeed;
        }

        protected override void SetListeners()
        {
            gGame.OnGameSpeedChange += SpeedChange;
            gGame.OnGameStateChange += StateChange;
            EventManager.instance.RegisterButton(pauseButton);
            EventManager.instance.RegisterButton(playButton);
            EventManager.instance.RegisterButton(fastButton);
        }

        private void StateChange()
        {
            if(lastState != gGame.CurrentState)
            {
                lastState = gGame.CurrentState;
                UpdateButtons();
            }
        }

        private void SpeedChange()
        {
            if(displayedSpeed != gGame.CurrentSpeed || lastState != gGame.CurrentState)
            {
                lastState = gGame.CurrentState;
                displayedSpeed = gGame.CurrentSpeed;
                UpdateButtons();
            }
        }

        private void UpdateButtons()
        {
            switch (gGame.CurrentState)
            {
                case GameState.Paused:
                    pauseButton.interactable = false;
                    playButton.interactable = true;
                    fastButton.interactable = true;
                    ChangeDisplayedSpeed(pausedSprite);
                    break;
                case GameState.Ticking:
                    switch (gGame.CurrentSpeed)
                    {
                        case GameSpeed.Normal:
                            pauseButton.interactable = true;
                            playButton.interactable = false;
                            fastButton.interactable = true;
                            ChangeDisplayedSpeed(playSprite);
                            break;
                        case GameSpeed.Fast:
                            pauseButton.interactable = true;
                            playButton.interactable = true;
                            fastButton.interactable = false;
                            ChangeDisplayedSpeed(fastSprite);
                            break;
                    }
                    break;
                case GameState.Interacting:
                    pauseButton.interactable = false;
                    playButton.interactable = false;
                    fastButton.interactable = false;
                    ChangeDisplayedSpeed(pausedSprite);
                    break;
            }
        }

        public void ChangeDisplayedSpeed(Sprite sprite)
        {
            speedDisplay.sprite = sprite;
        }

        public void PauseButton()
        {
            gGame.UpdateGameState(GameState.Paused);
        }

        public void NormalSpeedButton()
        {
            gGame.UpdateGameState(GameState.Ticking);
            gGame.UpdateGameSpeed(GameSpeed.Normal);
        }

        public void FastSpeedButton()
        {
            gGame.UpdateGameState(GameState.Ticking);
            gGame.UpdateGameSpeed(GameSpeed.Fast);
        }
    }
}