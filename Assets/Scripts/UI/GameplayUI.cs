using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Devlike.Timing;

namespace Devlike.UI
{
    public class GameplayUI : ExecutableBehaviour
    {
        private GlobalGame game;
        private GlobalTime time;
        private GlobalStudio studio;

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
            game = GameManager.instance.GetGlobal("Game") as GlobalGame;
            time = GameManager.instance.GetGlobal("Time") as GlobalTime;
            studio = GameManager.instance.GetGlobal("Studio") as GlobalStudio;
            lastState = game.CurrentState;
        }

        protected override void SetListeners()
        {
            EventManager.instance.OnTick += GameTick;
            EventManager.instance.OnCompletePlayerAction += ResetActionButtons;
            EventManager.instance.OnSetCharacters += GenerateCharacterButtons;
            EventManager.instance.OnRegisterButton += RegisterButton;
        }

        protected override void Launch()
        {
            SetWeek(time.CurrentWeek);
            SetTime(time.CurrentDay.ToString(), time.CurrentTime);
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
            for(int i = 0; i < studio.Characters.Count; i++)
            {
                characterButtons[i].character = studio.Characters[i];
                characterButtons[i].GenerateButton();
            }
        }

        private void GameTick()
        {
            if(game.CurrentState != lastState)
            {
                lastState = game.CurrentState;

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
                    if (studio.CharactersActive)
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
                SetWeek(time.CurrentWeek);
                SetTime(time.CurrentDay.ToString(), time.CurrentTime);
            }
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