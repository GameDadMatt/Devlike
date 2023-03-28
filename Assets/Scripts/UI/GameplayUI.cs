using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Devlike.Timing;
using Devlike.Player;

namespace Devlike.UI
{
    public class GameplayUI : ExecutableBehaviour
    {
        [SerializeField]
        private GlobalGame gGame;
        [SerializeField]
        private GlobalTime gTime;
        [SerializeField]
        private GlobalStudio gStudio;

        private Canvas canvas;
        [SerializeField]
        private TextMeshProUGUI week;
        [SerializeField]
        private TextMeshProUGUI dayHour;

        private List<Button> otherButtons = new List<Button>();
        private List<ProgressButton> topButtons = new List<ProgressButton>();
        private List<ProgressButtonCharacter> characterButtons = new List<ProgressButtonCharacter>();

        private GameState lastState;

        protected override void SetProperties()
        {
            lastState = gGame.CurrentState;
            canvas = GetComponent<Canvas>();
        }

        protected override void SetListeners()
        {
            EventManager.instance.OnTick += GameTick;
            EventManager.instance.OnCompletePlayerAction += ResetActionButtons;
            EventManager.instance.OnSetCharacters += GenerateCharacterButtons;
            EventManager.instance.OnRegisterButton += RegisterButton;
            EventManager.instance.OnDisplayUI += DisplayUI;
        }

        protected override void Launch()
        {
            SetWeek(gTime.CurrentWeek);
            SetTime(gTime.CurrentDay.ToString(), gTime.CurrentTime);
        }

        public void RegisterButton(object button)
        {
            if(button is ProgressButtonCharacter)
            {
                characterButtons.Add(button as ProgressButtonCharacter);
            }
            else if(button is ProgressButton)
            {
                ProgressButton pg = button as ProgressButton;
                topButtons.Add(pg);
                pg.GenerateButton(); //The top buttons need no additional work
            }
            else if(button is Button)
            {
                otherButtons.Add(button as Button);
            }
            else
            {
                Debug.LogError("Failed to add " + button);
            }
        }

        public void GenerateCharacterButtons()
        {
            Debug.Log(characterButtons.Count + " Buttons");
            for(int i = 0; i < gStudio.Characters.Count; i++)
            {
                characterButtons[i].character = gStudio.Characters[i];
                characterButtons[i].GenerateButton();
            }
        }

        private void GameTick()
        {
            if(gGame.CurrentState != lastState)
            {
                lastState = gGame.CurrentState;

                //Change the interactivity off by default if we're interacting or paused
                if (lastState == GameState.Interacting || lastState == GameState.Paused)
                {
                    ChangeOtherInteractivity(false);
                    ChangePlayerInteractivity(false);
                    ChangeCharacterInteractivity(false);
                }
                else if (lastState == GameState.Fast)
                {
                    ChangeOtherInteractivity(true);
                    //Only allow the player buttons at the top to be interactable if there are characters active
                    if (gStudio.CharactersActive)
                    {
                        ChangePlayerInteractivity(true);
                    }
                    else
                    {
                        ChangePlayerInteractivity(false);
                    }
                }
                else
                {
                    //The game is running at normal speed, so make buttons interactive
                    //We let characters change themselves as appropriate in ProgressButtonCharacter
                    ChangeOtherInteractivity(true);
                    ChangePlayerInteractivity(true);
                }
            }
            
            if(lastState != GameState.Interacting && lastState != GameState.Paused)
            {
                SetWeek(gTime.CurrentWeek);
                SetTime(gTime.CurrentDay.ToString(), gTime.CurrentTime);
            }
        }

        public void DisplayUI(ActionType type)
        {
            EventManager.instance.ChangeGameState(GameState.Interacting);
            canvas.enabled = false;
            ResetActionButtons();
        }

        public void ChangeOtherInteractivity(bool state)
        {
            foreach (Button b in otherButtons)
            {
                b.interactable = state;
            }
        }

        public void ChangePlayerInteractivity(bool state)
        {
            foreach (ProgressButton pb in topButtons)
            {
                pb.Interactable = state;
            }
        }

        public void ChangeCharacterInteractivity(bool state)
        {
            foreach (ProgressButtonCharacter pbc in characterButtons)
            {
                pbc.Interactable = state;
            }
        }

        public void ResetActionButtons()
        {
            foreach(ProgressButton pb in topButtons)
            {
                if (!pb.InProgress)
                {
                    pb.ResetButton();
                }
            }
            foreach(ProgressButtonCharacter pbc in characterButtons)
            {
                if (!pbc.InProgress)
                {
                    pbc.ResetButton();
                }
            }
        }

        public void SetWeek(int num)
        {
            week.SetText("Week " + num);
        }

        public void SetTime(string day, string hour)
        {
            dayHour.SetText(day + " " + hour);
        }
    }
}