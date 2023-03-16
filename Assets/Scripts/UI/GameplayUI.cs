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
        public static GameplayUI instance;

        [SerializeField]
        private TextMeshProUGUI week;
        [SerializeField]
        private TextMeshProUGUI dayHour;

        private List<Button> otherButtons = new List<Button>();
        private List<ProgressButton> topButtons = new List<ProgressButton>();
        private List<ProgressButtonCharacter> characterButtons = new List<ProgressButtonCharacter>();

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
        }

        protected override void OnStart()
        {
            TimeManager.instance.OnTick += GameStateChange;
            EventManager.instance.OnCompletePlayerAction += ResetActionButtons;
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
            for(int i = 0; i < StudioManager.instance.Characters.Count; i++)
            {
                characterButtons[i].character = StudioManager.instance.Characters[i];
                characterButtons[i].GenerateButton();
            }
        }

        private void GameStateChange()
        {
            //Change the interactivity off by default if we're interacting or paused
            if(GameManager.instance.State == GameState.Interacting || GameManager.instance.State == GameState.Paused)
            {
                ChangeOtherInteractivity(false);
                ChangePlayerInteractivity(false);
                ChangeCharacterInteractivity(false);
            }
            else if(GameManager.instance.State == GameState.Fast)
            {
                ChangeOtherInteractivity(true);
                //Only allow the player buttons at the top to be interactable if there are characters active
                if (StudioManager.instance.CharactersActive)
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