using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Devlike.UI
{
    public class GameplayUI : MonoBehaviour
    {
        public static GameplayUI instance;

        [SerializeField]
        private TextMeshProUGUI week;
        [SerializeField]
        private TextMeshProUGUI dayHour;

        private List<ProgressButton> topButtons = new List<ProgressButton>();
        private List<ProgressButtonCharacter> characterButtons = new List<ProgressButtonCharacter>();

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
        }

        public void RegisterButton(ProgressButton button)
        {
            if(button is ProgressButtonCharacter)
            {
                characterButtons.Add(button as ProgressButtonCharacter);
            }
            else
            {
                topButtons.Add(button);
                button.GenerateButton(); //The top buttons need no additional work
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